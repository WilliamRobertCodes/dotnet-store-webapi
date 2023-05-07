import {defineStore} from "pinia";
import {computed, ref} from "vue";
import {api} from "@/utils/api";
import {AuthEvents} from "@/stores/auth-store";

export const useCartStore = defineStore('cart', () => {
    const cart = ref(null);
    const addingToCart = ref(false);
    
    window.addEventListener(AuthEvents.LoggedIn, fetchCart);
    window.addEventListener(AuthEvents.SignedUp, fetchCart);
    window.addEventListener(AuthEvents.Authenticated, fetchCart);
    
    window.addEventListener(AuthEvents.LoggedOut, dumpCart);
    
    async function fetchCart()  {
        cart.value = await api.get('cart').json();
    }

    async function addToCart({ productId, quantity }) {
        addingToCart.value = true;
        
        try {
            const response = await api
                .post('cart/items', { json: { productId, quantity } })
                .json();
            
            cart.value = response;
            
            return response;
        } catch (e) {
            throw new e;
        } finally {
            addingToCart.value = false;
        }
    }

    async function updateCart({ productId, quantity }) {
        const response = await api
            .put('cart/items', { json: { productId, quantity } })
            .json();

        cart.value = response;

        return response;
    }
    
    async function removeFromCart({ productId }) {
        const response = await api.delete('cart/items', { searchParams: { productId }})
            .json();
        
        cart.value = response;
        
        return response;
    }
    
    const numberOfItemsInCart = computed(() => {
        return cart.value?.cartLineItems?.length || 0;
    });
    
    function dumpCart() {
        cart.value = null;
    }
    
    return { 
        cart,
        addingToCart,
        numberOfItemsInCart,
        fetchCart,
        addToCart,
        updateCart,
        removeFromCart,
        dumpCart,
    };
});
