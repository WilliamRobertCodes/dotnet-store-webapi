<script setup>
import {ref, watch} from "vue";
import {api} from "@/utils/api";
import Container from "@/components/layout/Container.vue";
import AppButton from "@/components/AppButton.vue";

const products = ref(null);
const page = ref(1);

watch(page, fetchProducts, {immediate: true});

function fetchProducts() {
    const options = { 
        searchParams: {
            perPage: 12,
            page: page.value,
        },
    };
    
    api.get('products', options).json().then(response => {
        products.value = response;
    });
}
</script>

<template>
    <Container class="py-8">
        <h1 class="mb-6 text-2xl font-bold">Our products</h1>
        <div v-if="products == null">
            <div>Loading....</div>
        </div>
        <template v-else>
            <div class="grid grid-cols-3 gap-4">
                <div v-for="product in products.data" :key="product.id" class="space-y-3 flex flex-col p-4 border rounded shadow-sm">
                    <RouterLink :to="{ name: 'products.single', params: { id: product.id }}" class="text-lg font-semibold">{{ product.name }}</RouterLink>
                    <div class="grow text-sm">{{ product.description }}</div>
                    <div class="flex justify-end">
                        <RouterLink :to="{ name: 'products.single', params: { id: product.id }}" class="block">
                            <AppButton>View product</AppButton>
                        </RouterLink>
                    </div>
                </div>
            </div>
            <div v-if="products.pagination.numberOfPages > 1" class="grid grid-flow-col justify-center gap-1 mx-auto mt-8">
                <div 
                    @click="page = i"
                    v-for="i in products.pagination.numberOfPages"
                    :key="i"
                    :class="[
                        'flex justify-center items-center w-10 h-10 rounded hover:bg-blue-100 cursor-pointer transition',
                        i == page ? 'bg-blue-200' : '',
                    ]">
                    <div class="select-none">{{ i }}</div>
                </div>
            </div>
        </template>
    </Container>
</template>