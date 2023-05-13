<script setup>
import Container from "@/components/layout/Container.vue";
import {RouteNames} from "@/routing/router";
</script>

<template>
    <Container class="py-8 grid grid-cols-[140px_1fr] gap-4">
        <nav class="space-y-3 pr-4">
            <RouterLink 
                :to="{ name: RouteNames.Accounts_User_Profile }" 
                class="block font-semibold"
                active-class="text-blue-600">
                Profile
            </RouterLink>
            <RouterLink 
                :to="{ name: RouteNames.Accounts_User_Addresses }" 
                class="block font-semibold"
                active-class="text-blue-600">
                Addresses
            </RouterLink>
        </nav>
        <div>
            <RouterView/>
        </div>
    </Container>
</template>
