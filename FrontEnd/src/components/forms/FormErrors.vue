<script setup>
const { errors } = defineProps(['errors']);
</script>

<template>
    <template v-if="errors">
        <p class="space-y-1 block pt-1.5 text-xs font-semibold text-red-500">
            <template v-if="Array.isArray(errors)">
                <span class="block" v-for="(error, i) in errors" :key="i">{{ error }}</span>
            </template>
            <span v-else>{{ errors }}</span>
        </p>
    </template>
</template>
