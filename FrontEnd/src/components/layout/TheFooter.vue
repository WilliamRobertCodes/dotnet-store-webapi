<script setup>
import {RouterLink} from "vue-router";
import Container from "@/components/layout/Container.vue";
import {RouteNames} from "@/routing/router";
</script>

<template>
    <footer class="py-3 border-t border-grey">
        <Container>
            <RouterLink :to="{ name: RouteNames.Home }">Shop</RouterLink>
        </Container>
    </footer>
</template>
