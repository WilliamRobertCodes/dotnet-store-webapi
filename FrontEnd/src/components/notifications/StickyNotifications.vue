<script setup>
import {useNotificationsStore} from "@/stores/notifications-store";
import Notification from "@/components/notifications/Notification.vue";
import Container from "@/components/layout/Container.vue";

const notificationsStore = useNotificationsStore();
</script>

<template>
    <div v-if="notificationsStore.notifications.length" iv class="pointer-events-none sticky top-[90px] w-full pb-4">
        <Container class="pointer-events-auto grid gap-4 select-none">
            <Notification v-for="(notification, i) in notificationsStore.notifications" :key="i" :notification="notification"/>
        </Container>
    </div>
</template>
