<script setup>
import {computed, reactive, ref} from "vue";
import {useRoute} from "vue-router";
import {api} from "@/utils/api";
import {useCartStore} from "@/stores/cart-store";
import AppButton from "@/components/AppButton.vue";
import Container from "@/components/layout/Container.vue";
import FormLabel from "@/components/forms/FormLabel.vue";
import FormInput from "@/components/forms/FormInput.vue";
import {formatPriceInCents} from "@/utils/misc";
import {useNotificationsStore, NotificationTypes} from "@/stores/notifications-store";
import Pill from "@/components/Pill.vue";

const route = useRoute();
const cartStore = useCartStore();
const notificationsStore = useNotificationsStore();

const product = ref(null);
const form = reactive({
    quantity: 1,
    productId: route.params.id,
});

api.get(`products/${route.params.id}`).json().then(res => {
    product.value = res;
});

const price = computed(() => {
    if (!product.value) {
        return null;
    }
    
    return formatPriceInCents(product.value.price);
});

const submit = () => {
    cartStore.addToCart(form);
    notificationsStore.addNotification({
        type: NotificationTypes.Success,
        message: `Product added to cart !`,
    });
};
</script>

<template>
    <Container class="py-8">
        <template v-if="product == null">
            <div>Loading...</div>
        </template>
        <div v-else class="w-1/2"> 
            <h1 class="mb-4 text-2xl font-bold">{{ product.name }}</h1>
            <div v-if="product.productCategories.length" class="flex gap-2 mb-4">
                <Pill v-for="category in product.productCategories" :key="category.id">
                    {{ category.name }}
                </Pill>
            </div>
            <p class="mb-4 text-lg font-bold">{{ price }}</p>
            <p class="mb-4">{{ product.description }}</p>
            <form class="space-y-4" @submit.prevent="submit">
                <div>
                    <FormLabel for="add_cart_quantity">Quantity</FormLabel>
                    <FormInput id="add_cart_quantity" type="number" max="20" min="1" v-model="form.quantity"/>
                </div>
                <input type="hidden" v-model="form.productId">
                <div>
                    <AppButton type="submit" :disabled="cartStore.addingToCart">Add to cart</AppButton>
                </div>
            </form>
        </div>
    </Container>
</template>
