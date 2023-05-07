<script setup>
import {RouterView} from "vue-router";
import {useAuthStore} from "@/stores/auth-store";
import StickyNotifications from "@/components/notifications/StickyNotifications.vue";
import TheHeader from "@/components/layout/TheHeader.vue";
import TheFooter from "@/components/layout/TheFooter.vue";

const authStore = useAuthStore();

authStore.authenticate();
</script>

<template>
    <StickyNotifications/>
    <TheHeader/>
    <main class="grow">
        <RouterView/>
    </main>
    <TheFooter/>
</template>
