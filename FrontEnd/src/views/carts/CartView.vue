<script setup>
import {useCartStore} from "@/stores/cart-store";
import Container from "@/components/layout/Container.vue";
import AppButton from "@/components/AppButton.vue";
import {formatPriceInCents} from "@/utils/misc";

const cartStore = useCartStore();

const onQuantityChange = ($event, productId) => {
    cartStore.updateCart({
        productId, 
        quantity: $event.target.value,
    });
}
</script>

<template>
    <Container class="py-8">
        <h1 class="mb-4 text-2xl font-bold">Cart</h1>
        <div v-if="cartStore.cart">
            <div v-if="cartStore.cart.cartLineItems.length === 0" class="space-y-4 flex flex-col items-center py-4">
                <div class="text-lg text-center font-semibold">Your cart is empty :(</div>
                <div>
                    <RouterLink :to="{ name: 'home' }">
                        <AppButton>Go shopping !</AppButton>
                    </RouterLink>
                </div>
            </div>
            <div v-else class="grid grid-cols-[1fr_320px] items-start gap-4">
                <div class="overflow-hidden border rounded">
                    <article v-for="item in cartStore.cart.cartLineItems" :key="item.id" class="flex justify-between p-4 border-b transition last:border-0 hover:bg-gray-50">
                        <div>
                            <h2 class="pb-2 align-baseline">
                                <span class="text-lg font-semibold">{{ item.product.name }}</span>&nbsp;<span>x{{ item.quantity }}</span>
                            </h2>
                            <div class="text-sm">
                                <p>Price per item: {{ formatPriceInCents(item.product.price) }}</p>
                                <p>Total for quantity: {{ formatPriceInCents(item.lineItemPriceInCents) }}</p>
                            </div>
                        </div>
                        <div class="flex items-end">
                            <div class="grid grid-flow-col gap-4">
                                <select @input="onQuantityChange($event, item.product.id)" v-model="item.quantity" class="h-[40px] w-[80px] px-2 border rounded cursor-pointer">
                                    <option v-for="i in 20" :value="i" :key="i">{{ i }}</option>
                                </select>
                                <AppButton @click="cartStore.removeFromCart({productId: item.product.id})" class="grid grid-flow-col gap-2">
                                    <svg xmlns="http://www.w3.org/2000/svg" class="w-[20px] h-[20px]" viewBox="0 0 24 24" stroke-width="1.5" stroke="#FFFFFF" fill="none" stroke-linecap="round" stroke-linejoin="round">
                                        <path stroke="none" d="M0 0h24v24H0z" fill="none"/>
                                        <line x1="4" y1="7" x2="20" y2="7" />
                                        <line x1="10" y1="11" x2="10" y2="17" />
                                        <line x1="14" y1="11" x2="14" y2="17" />
                                        <path d="M5 7l1 12a2 2 0 0 0 2 2h8a2 2 0 0 0 2 -2l1 -12" />
                                        <path d="M9 7v-3a1 1 0 0 1 1 -1h4a1 1 0 0 1 1 1v3" />
                                    </svg>
                                    <span>Remove</span>
                                </AppButton>
                            </div>
                        </div>
                    </article>
                </div>
                <div class="space-y-4 h-full p-4">
                    <h3 class="text-xl font-semibold">Order details</h3>
                    <div>Total price: {{ formatPriceInCents(cartStore.cart.totalPriceInCents) }}</div>
                    <AppButton>Checkout</AppButton>
                </div>
            </div>
        </div>
        <div v-else>
            <p>Loading...</p>
        </div>
    </Container>
</template>
