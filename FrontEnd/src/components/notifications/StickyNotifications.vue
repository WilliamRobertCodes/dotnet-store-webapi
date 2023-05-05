<script setup>
import {useNotificationsStore} from "@/stores/notifications-store";
import Notification from "@/components/notifications/Notification.vue";

const notificationsStore = useNotificationsStore();
</script>

<template>
    <div class="pointer-events-none fixed top-[90px] w-full">
        <div class="pointer-events-auto grid gap-4 w-1/3 mx-auto select-none">
            <Notification v-for="(notification, i) in notificationsStore.notifications" :key="i" :notification="notification"/>
        </div>
    </div>
</template>
