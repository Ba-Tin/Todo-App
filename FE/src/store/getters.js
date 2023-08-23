const getters = {
    completedTodos: (state) => state.todoList.filter((todo) => todo.status),
};

export default getters;