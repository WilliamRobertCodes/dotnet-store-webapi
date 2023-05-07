import {defineStore} from "pinia";
import {computed, ref} from "vue";
import { api } from "@/utils/api";
import {useCartStore} from "@/stores/cart-store";

const EVENTS_PREFIX = "AuthEvents";

export const AuthEvents = {
    Authenticated: `${EVENTS_PREFIX}/Authenticated`,
    LoggedIn: `${EVENTS_PREFIX}/LoggedIn`,
    SignedUp: `${EVENTS_PREFIX}/SignedUp`,
    LoggedOut: `${EVENTS_PREFIX}/LoggedOut`,
};

export const useAuthStore = defineStore("auth", () => {
    const cartStore = useCartStore();
    
    const user = ref(null);
    const authenticated = computed(() => !!user.value);

    async function authenticate() {
        const response = await api
            .get('accounts/me', {
                credentials: 'include',
            })
            .json();
        
        user.value = response.user;
        
        window.dispatchEvent(new CustomEvent(AuthEvents.Authenticated, {
            detail: { user: response.user },
        }));

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

            window.dispatchEvent(new CustomEvent(AuthEvents.LoggedIn, {
                detail: { user: response.user },
            }));
            
            return {
                success: true,
                data: response.user,
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

            window.dispatchEvent(new CustomEvent(AuthEvents.SignedUp, {
                detail: { user: response.user },
            }));

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
    
    async function logout() {
        await api.post('accounts/logout').json();
        
        user.value = null;
    }
    
    return { 
        user, 
        authenticated,
        authenticate,
        logIn, 
        signUp,
        logout,
    };
});
