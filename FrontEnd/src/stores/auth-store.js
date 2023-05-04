import {defineStore} from "pinia";
import {computed, ref} from "vue";
import { api } from "@/utils/api";
import {useCartStore} from "@/stores/cart-store";

export const useAuthStore = defineStore("auth", () => {
    const cartStore = useCartStore();
    
    const user = ref(null);
    const authenticated = computed(() => !!user.value);

    async function authenticate() {
        const response = await api
            .get('accounts/me', {
                credentials: 'include',
            })
            .json()
        
        cartStore.fetchCart();

        if (response.user) {
            user.value = response.user;
        }

        return {
            success: true,
            data: response,
        };
    }
    
    async function logIn(credentials) {
        try {
            const response = await api
                .post('accounts/login', { json: credentials })
                .json();
            
            user.value = response.user;

            cartStore.fetchCart();
            
            return {
                success: true,
                data: response,
            };
        } catch (error) {
            const { status } = error.response ?? null;

            if (status >= 400 && status < 500) {
                const json = await error.response.json();

                return {
                    success: false,
                    errors: json.errors,
                };
            }
            
            throw error;
        }
    }
    
    async function signUp(credentials) {
        try {
            const response = await api
                .post('accounts/register', { json: credentials })
                .json();

            user.value = response.user;

            cartStore.fetchCart();

            return { 
                success: true,
                data: response,
            };
        } catch (error) {
            const { status } = error.response ?? null;

            if (status >= 400 && status < 500) {
                const json = await error.response.json();

                return {
                    success: false,
                    errors: json.errors,
                };
            }

            throw error;
        }
    }
    
    return { 
        user, 
        authenticated,
        authenticate,
        logIn, 
        signUp, 
    };
});
