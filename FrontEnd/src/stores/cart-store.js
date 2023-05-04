import {defineStore} from "pinia";
import {computed, ref} from "vue";
import {api} from "@/utils/api";

export const useCartStore = defineStore('cart', () => {
    const cart = ref(null);
    const addingToCart = ref(false);
    
    async function fetchCart()  {
        cart.value = await api.get('cart').json();
    }

    async function addToCart({ productId, quantity }) {
        addingToCart.value = true;
        
        try {
            const response = await api.post('cart/items', { json: { productId, quantity } })
                .json();
            
            addingToCart.value = false;
            cart.value = response;
            
            return response;
        } catch (e) {
            addingToCart.value = false;
            
            throw new e;
        }
    }

    async function updateCart({ productId, quantity }) {
        const response = await api.put('cart/items', { json: { productId, quantity } })
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
        if (!cart.value) {
            return 0;
        }
        
        return cart.value.cartLineItems.length;
    });
    
    return { 
        cart,
        addingToCart,
        numberOfItemsInCart,
        fetchCart,
        addToCart,
        updateCart,
        removeFromCart,
    };
});
