<template lang="">
   <div class="container mt-2 border p-3">
    <div class="row text-center mb-2">
        <h1>Todo App</h1>
    </div>
    <div class="text-center ">
      <button @click="handleAddTodo" class="btn btn-primary mr-5">Thêm</button>  
    </div>
    <div class="text-center mt-4">
      <button class="btn btn-primary" @click="handleShowCompletedTasks">Công việc đã hoàn thành</button>
    </div>
    <div class="text-center p-3">
      <input class="rounded p-2" v-model="searchText" v-on:input="debouncedHandleSearch" placeholder="Search...">
    </div>
    <div class="row text-center">
        <table class="table table-striped">
            <thead>
                <tr>
                    <th>STT</th>
                    <th>Tên</th>
                    <th>Trạng thái</th>
                    <th></th>
                </tr>
            </thead>
             <tbody >
                <tr v-for="(item, index) in filteredTodoList" :key="item.id">
                    <td>{{index + 1}}</td>  
                    <td>{{item.content}}</td>
                    <td><input type="checkbox" v-model="item.status" @change="handleCompleteTask(item)"></td>
                    <td>
                        <button class="btn btn-secondary" @click="handleUpdatedTodo(item)" style="margin-right:1rem">
                            <i class="bx bxs-edit"></i> Sửa
                        </button>
                        <button class="btn btn-danger" @click="handleDeleteTodo(item)">
                            <i class="bx bx-trash"></i> Xóa
                        </button>
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="pagination text-end">
            <button class="btn" @click="prevPage" :disabled="currentPage === 1">Previous</button>
            <button class="btn" @click="nextPage" :disabled="currentPage === totalPages">Next</button>
        </div>
    </div>
    <modal-form v-if='showModal'
     :closeModal="handleShowModal" 
     :todoEdit="editingTodo"
     :onSave = "handleSave"/>
   </div>
</template>
<script>
import ModalForm from './ModalForm.vue';
import { toast } from 'vue3-toastify';
import _debounce from 'lodash/debounce';
import { mapState, mapGetters, mapActions, mapMutations } from 'vuex';
export default {
    data() {
        return {
            searchText: '',
        };
    },
    components: {
        ModalForm,
    },
    computed: {
        ...mapState(['todoList', 'editingTodo', 'showModal', 'showCompletedTasks', 'isLoading', 'currentPage', 'totalPages',]),
        ...mapGetters(['completedTodos']),
        filteredTodoList() {
            return this.showCompletedTasks ? this.completedTodos : this.todoList;
        },
        isLoading() {
            return this.todoList.length === 0;
        },
    },
    methods: {
        ...mapActions(['getAllTodo', 'searchAndFetch', 'setShowModal', 'setEditingTodo', 'addTodo', 'updateTodo', 'updateTodoStatus', 'deleteTodo', 'setShowCompletedTasks']),
        ...mapMutations(['SET_CURRENT_PAGE']),
        prevPage() {
            this.SET_CURRENT_PAGE(this.currentPage - 1);
            this.getAllTodo();
        },
        nextPage() {
            this.SET_CURRENT_PAGE(this.currentPage + 1);
            this.getAllTodo();
        },
        handleShowModal() {
            this.setShowModal(!this.showModal);
        },
        handleShowCompletedTasks() {
            this.setShowCompletedTasks(!this.showCompletedTasks)
        },
        handleAddTodo() {
            this.setEditingTodo(null);
            this.setShowModal(true);
        },
        handleUpdatedTodo(item) {
            this.setEditingTodo(item);
            this.setShowModal(true);
        },
        debouncedHandleSearch: _debounce(function () {
            if (this.searchText) {
                this.searchAndFetch(this.searchText);
            } else {
                this.getAllTodo();
            }
        }, 300),
        async handleDeleteTodo(item) {
            const confirmDelete = confirm("Bạn có chắc muốn xóa công việc này?");
            if (confirmDelete) {
                const error = await this.deleteTodo(item.id);
                if (!error) {
                    toast.success("Công việc đã được xóa!", { duration: 1500 });
                    this.getAllTodo();
                } else {
                    toast.error(error, { duration: 1500 });
                }
            }
        },
        async handleCompleteTask(item) {
            await this.updateTodoStatus(item.id);

        },
        async handleSave(item) {
            if (this.editingTodo) {
                const error = await this.updateTodo({ id: this.editingTodo.id, todo: { ...this.editingTodo, ...item } });
                if (error) {
                    toast.error(error, { duration: 1500 });
                } else {
                    toast.success('Cập nhật thành công!', { duration: 1500 });

                }
            } else {
                const newTodo = { ...item };
                const error = await this.addTodo(newTodo);
                if (error) {
                    toast.error(error, { duration: 1500 });
                } else {
                    toast.success('Thêm thành công!', { duration: 1500 });
                }
            }
            this.setShowModal(false);
            this.getAllTodo();
        },

        async fetchAllTodo() {
            await this.getAllTodo();
        },
    },
    created() {
        this.fetchAllTodo();
    }


}
</script>
<style lang="css"></style>