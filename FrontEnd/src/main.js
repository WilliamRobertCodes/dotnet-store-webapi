import './assets/app.css'

import {createApp} from 'vue'
import {createPinia} from 'pinia'
import {router} from '@/routing/router'
import App from '@/App.vue'

createApp(App)
    .use(createPinia())
    .use(router)
    .mount('#app')
