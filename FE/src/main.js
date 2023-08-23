import { createApp } from 'vue'
import App from './App.vue'
import "bootstrap/dist/css/bootstrap.css"
import 'vue3-toastify/dist/index.css';
import 'vue-loading-overlay/dist/css/index.css';
import "bootstrap/dist/js/bootstrap.js"
import store from './store';

createApp(App).use(store).mount('#app')


