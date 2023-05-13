<script setup>
import {RouterView} from "vue-router";
import StickyNotifications from "@/components/notifications/StickyNotifications.vue";
import TheHeader from "@/components/layout/TheHeader.vue";
import TheFooter from "@/components/layout/TheFooter.vue";
</script>

<template>
    <TheHeader/>
    <StickyNotifications/>
    <main class="grow">
        <RouterView/>
    </main>
    <TheFooter/>
</template>
