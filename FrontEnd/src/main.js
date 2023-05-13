import './assets/app.css'

import {createApp} from 'vue'
import {createPinia} from 'pinia'
import {router} from '@/routing/router'
import App from '@/App.vue'
import {useAuthStore} from "@/stores/auth-store";

const app = createApp(App)
    .use(createPinia());

await useAuthStore().authenticate();

app
    .use(router)
    .mount('#app');
