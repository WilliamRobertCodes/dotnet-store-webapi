import {defineStore} from "pinia";
import {ref} from "vue";
import {api} from "@/utils/api";

export const useConfigStore = defineStore('config', () => {
    const stripePublicKey = ref(null);
    
    async function fetchConfig() {
        const response = await api.get('config').json();
        
        stripePublicKey.value = response.stripePublicKey;
    }
    
    return {
        stripePublicKey,
        fetchConfig,
    }
});
