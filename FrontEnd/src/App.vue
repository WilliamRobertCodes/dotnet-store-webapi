<script setup>
import {RouterLink, RouterView} from "vue-router";
import {useAuthStore} from "@/stores/auth-store";
import Container from "@/components/layout/Container.vue";
import {useCartStore} from "@/stores/cart-store";
import StickyNotifications from "@/components/notifications/StickyNotifications.vue";

const authStore = useAuthStore();
const cartStore = useCartStore();

authStore.authenticate();

function logout() {
    authStore.logout();
}
</script>

<template>
    <StickyNotifications/>
    
    <header class="py-4 border-b border-grey">
        <Container class="flex justify-between items-center">
            <RouterLink :to="{ name: 'home' }" class="text-2xl font-semibold">
                Shop
            </RouterLink>

            <div class="text-sm">
                <div v-if="authStore.user" class="grid grid-flow-col items-center gap-2">
                    <div>Welcome, {{ authStore.user.userName }}</div>
                    <RouterLink :to="{ name: 'cart' }" class="relative z-10 w-6 cursor-pointer">
                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" class="stroke-slate-800" stroke-width="2" fill="none" stroke-linecap="round" stroke-linejoin="round">
                            <path stroke="none" d="M0 0h24v24H0z" fill="none"/>
                            <circle cx="6" cy="19" r="2" />
                            <circle cx="17" cy="19" r="2" />
                            <path d="M17 17h-11v-14h-2" />
                            <path d="M6 5l14 1l-1 7h-13" />
                        </svg>
                        <div v-if="cartStore.cart" class="absolute top-[-10px] right-[-10px] flex justify-center items-center min-w-[18px] h-[18px] rounded-full bg-blue-700 text-xs leading-[1] font-semibold text-white">
                            <div class="relative top-[0.5px]">{{ cartStore.numberOfItemsInCart }}</div>
                        </div>
                    </RouterLink>
                    <a @click.prevent="logout" href="#" class="ml-8">Log out</a>
                </div>
                <nav v-else class="grid grid-flow-col gap-4">
                    <RouterLink :to="{ name: 'accounts.login' }">Log in</RouterLink>
                    <RouterLink :to="{ name: 'accounts.register' }">Sign up</RouterLink>
                </nav>
            </div>
        </Container>
    </header>

    <main class="grow">
        <RouterView/>
    </main>

    <footer class="py-3 border-t border-grey">
        <Container>
            <RouterLink :to="{ name: 'home' }">Shop</RouterLink>
        </Container>
    </footer>
</template>
