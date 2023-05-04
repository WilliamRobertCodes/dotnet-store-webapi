import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/cart',
      name: 'cart',
      component: () => import('../views/carts/CartView.vue'),
    },
    {
      path: '/accounts/login',
      name: 'accounts.login',
      component: () => import('../views/accounts/LoginView.vue'),
    },
    {
      path: '/accounts/login',
      name: 'accounts.register',
      component: () => import('../views/accounts/SignUpView.vue'),
    },
    {
      path: '/products/:id',
      name: 'products.single',
      component: () => import('../views/products/ProductView.vue'),
    },
  ],
})

export default router
