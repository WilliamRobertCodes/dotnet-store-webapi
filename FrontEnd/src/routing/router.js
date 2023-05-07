import {createRouter, createWebHistory} from 'vue-router'
import HomeView from '@/views/HomeView.vue'
import {authenticatedGuard} from "@/routing/guards";

const RouteNames = Object.freeze({
    Home: 'home',
    Cart: 'cart',
    Accounts_Login: 'accounts.login',
    Accounts_Signup: 'accounts.signup',
    Products_Single: 'products.single',
});

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/',
            name: RouteNames.Home,
            component: HomeView
        },
        {
            path: '/cart',
            name: RouteNames.Cart,
            component: () => import('../views/carts/CartView.vue'),
            beforeEnter: [authenticatedGuard],
        },
        {
            path: '/accounts/login',
            name: RouteNames.Accounts_Login,
            component: () => import('../views/accounts/LoginView.vue'),
        },
        {
            path: '/accounts/signup',
            name: RouteNames.Accounts_Signup,
            component: () => import('../views/accounts/SignUpView.vue'),
        },
        {
            path: '/products/:id',
            name: RouteNames.Products_Single,
            component: () => import('../views/products/ProductView.vue'),
        },
    ],
})

export {
    RouteNames,
    router,
}
