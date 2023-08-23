<template lang="">
    <!-- The Modal -->
      <div class="modal">
        <div class="modal-dialog">
          <div class="modal-content">
            <!-- Modal Header -->
            <div class="modal-header">
              <h4 class="modal-title">Modal form</h4>
              <button type="button" @click="closeModal" class="btn-close"></button>
            </div>

            <!-- Modal body -->
            <div class="modal-body input-group">
              <input type="text" class="form-control" placeholder="Thêm công việc" v-model="contentName" >
            </div>

            <!-- Modal footer -->
            <div class="modal-footer">
              <button type="button" class="btn btn-primary" @click="saveTodo">{{ todoEdit !== null ? 'Lưu' : 'Thêm công việc' }}</button>
              <button type="button" class="btn btn-danger" @click="closeModal">Close</button>
            </div>
          </div>
        </div>
      </div>
</template>
<script>
import { toast } from 'vue3-toastify';

export default {
  props: {
    closeModal: {
      type: Boolean,
      required: true

    },
    todoEdit: {
      type: Object,
      required: true
    },
    onSave: {
      type: Function,
      required: true
    }

  },

  data() {
    return {
      contentName: "",
    }
  },
  methods: {
    saveTodo() {
      if (this.contentName.trim() === "") {
        toast.error("Bạn chưa nhập tên task!", { duration: 3000 });
        return;
      }
      const item = {
        content: this.contentName,
      };
      this.onSave(item);
    }
  },
  mounted() {
    if (this.todoEdit) {
      this.contentName = this.todoEdit.content;
    } else {
      this.contentName = "";
    }
  }
}
</script>
<style lang="css" scoped>
.modal {
  display: block !important;
}
</style>