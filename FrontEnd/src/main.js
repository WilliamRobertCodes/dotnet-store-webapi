import './assets/app.css'

import {createApp} from 'vue'
import {createPinia} from 'pinia'
import {router} from '@/routing/router'
import App from '@/App.vue'
import {useAuthStore} from "@/stores/auth-store";
import {useConfigStore} from "@/stores/config-store";

const app = createApp(App)
    .use(createPinia());

Promise.all([
    useConfigStore().fetchConfig(),
    useAuthStore().authenticate(),
]).then(() => {
    app
        .use(router)
        .mount('#app');
});
