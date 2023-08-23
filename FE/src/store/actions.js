import axios from "axios";
import { BASE_URL } from "../contant/network";

export default {
    async getAllTodo({ commit, state }) {
        try {

            const response = await axios.get(`${BASE_URL}/api/Todo?page=${state.currentPage}&pageSize=${state.totalPages}`);
            commit('SET_TO_DO_LIST', response.data.data);
            commit('SET_TOTAL_PAGES', state.totalPages);

        } catch (error) {
            console.error('Error', error);
        }
    },
    async searchAndFetch({ commit, state }, searchText) {
        try {
            const response = await axios.get(`${BASE_URL}/api/Todo?page=${state.currentPage}&search=${searchText}`);
            commit('SET_TO_DO_LIST', response.data.data);
            commit('SET_TOTAL_PAGES', state.totalPages);
        } catch (error) {
            console.error('Error', error);
        }
    },
    setShowModal({ commit }, value) {
        commit('SET_SHOW_MODAL', value);
    },
    setShowCompletedTasks({ commit }, value) {
        commit('SET_SHOW_COMPLETE_TASK', value);
    },
    setEditingTodo({ commit }, todo) {
        commit('SET_EDITING_TODO', todo);
    },
    async addTodo({ commit }, todo) {
        try {
            const response = await axios.post(`${BASE_URL}/api/Todo/create`, todo)
            commit('ADD_TODO', response.data);
        }
        catch (error) {
            console.error("Error", error)
            return error.response.data;
        }
    },
    async updateTodo({ commit }, { id, todo }) {
        try {
            const response = await axios.put(`${BASE_URL}/api/Todo/update/${id}`, todo);
            commit('UPDATE_TODO', response.data);
            console.log(response.data);
        } catch (error) {
            console.error("Error", error);
            return error.response.data;
        }
    },
    async updateTodoStatus({ commit }, id) {
        try {
            const response = await axios.put(`${BASE_URL}/api/Todo/updateStatus/${id}`);
            commit('UPDATE_TODO_STATUS', { id, newStatus: response.data.data.status });
        } catch (error) {
            console.error('Error:', error);
        }
    },
    async deleteTodo({ commit }, id) {
        try {
            const response = await axios.delete(`${BASE_URL}/api/Todo/delete/${id}`);
            commit('DELETE_TODO', response);
        } catch (error) {
            console.error("Error", error);
            return error.response.data;
        }
    }
}