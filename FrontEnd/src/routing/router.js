import {createRouter, createWebHistory} from 'vue-router'
import HomeView from '@/views/HomeView.vue'
import {authenticatedGuard} from "@/routing/guards";
import ProfileView from "@/views/accounts/user/ProfileView.vue";
import AddressesView from "@/views/accounts/user/AddressesView.vue";

const RouteNames = Object.freeze({
    Home: 'home',
    Cart: 'cart',
    Accounts_Login: 'accounts.login',
    Accounts_Signup: 'accounts.signup',
    Accounts_User: 'accounts.user',
    Accounts_User_Profile: 'accounts.user.profile',
    Accounts_User_Addresses: 'accounts.user.addresses',
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
            path: '/accounts/user',
            name: RouteNames.Accounts_User,
            component: () => import('../views/accounts/UserView.vue'),
            redirect: { name: RouteNames.Accounts_User_Profile },
            beforeEnter: [authenticatedGuard],
            children: [
                {
                    path: 'profile',
                    name: RouteNames.Accounts_User_Profile,
                    component: ProfileView,
                },
                {
                    path: 'addresses',
                    name: RouteNames.Accounts_User_Addresses,
                    component: AddressesView,
                }
            ]
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
