<script setup>
defineProps(['hasError', 'modelValue']);
defineEmits(['update:modelValue'])
</script>

<template>
    <input :class="[
        'w-full px-3.5 py-2.5 border rounded text-sm shadow-sm focus:outline-2 focus:outline-blue-600',
        hasError ? 'border-red-300 bg-red-100/25' : 'border-grey',
    ]"
           :value="modelValue"
           @input="$emit('update:modelValue', $event.target.value)">
</template>
