<script setup>
import {reactive, ref} from "vue";
import Container from "@/components/layout/Container.vue";
import FormLabel from "@/components/forms/FormLabel.vue";
import FormInput from "@/components/forms/FormInput.vue";
import AppButton from "@/components/AppButton.vue";
import FormErrors from "@/components/forms/FormErrors.vue";
import Card from "@/components/Card.vue";
import {useAuthStore} from "@/stores/auth-store";
import {useRouter} from "vue-router";
import {RouteNames} from "@/routing/router";

const authStore = useAuthStore();
const router = useRouter();

const form = reactive({
    email: '',
    password: '',
});

const formErrors = ref({});

const submit = async () => {
    formErrors.value = {};

    const result = await authStore.logIn(form);

    if (result.success) {
        router.push({ name: RouteNames.Home });
    } else {
        formErrors.value = result.errors;
    }
};
</script>

<template>
    <Container class="py-8">
        <Card class="max-w-[480px] mx-auto">
            <h1 class="mb-6 text-center text-xl font-bold">Login</h1>

            <form @submit.prevent="submit" class="space-y-4">
                <div>
                    <FormLabel for="login_email">Email</FormLabel>
                    <FormInput id="login_email" v-model="form.email" :has-error="formErrors?.email"/>
                    <FormErrors :errors="formErrors?.email"/>
                </div>
                <div>
                    <FormLabel for="login_password">Password</FormLabel>
                    <FormInput id="login_password" type="password" v-model="form.password"
                               :has-error="formErrors?.password"/>
                    <FormErrors :errors="formErrors?.password"/>
                </div>
                <div class="flex justify-end pt-3">
                    <AppButton>Log in</AppButton>
                </div>
            </form>

            <hr class="my-5">
            
            <div class="text-center">
                <p class="text-sm">
                    <span>No account ?&nbsp;</span>
                    <RouterLink :to="{ name: RouteNames.Accounts_Signup }" class="text-blue-700 hover:underline">Sign up</RouterLink>
                </p>
            </div>
        </Card>
    </Container>
</template>