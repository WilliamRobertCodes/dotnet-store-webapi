<script setup>
import {reactive, ref} from "vue";
import {useRouter} from "vue-router";
import Container from "@/components/layout/Container.vue";
import FormLabel from "@/components/forms/FormLabel.vue";
import FormInput from "@/components/forms/FormInput.vue";
import FormErrors from "@/components/forms/FormErrors.vue";
import AppButton from "@/components/AppButton.vue";
import Card from "@/components/Card.vue";
import {useAuthStore} from "@/stores/auth-store";
import {RouteNames} from "@/routing/router";

const authStore = useAuthStore();
const router = useRouter();

const form = reactive({
    email: '',
    userName: '',
    password: '',
});

const formErrors = ref({});

const submit = async () => {
    const result = await authStore.signUp(form);
    
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
            <h1 class="mb-6 text-center text-xl font-bold">Sign up</h1>

            <form @submit.prevent="submit" class="space-y-4">
                <div>
                    <FormLabel for="register_email">Email</FormLabel>
                    <FormInput id="register_email" v-model="form.email" :has-error="formErrors?.email"/>
                    <FormErrors :errors="formErrors.email"/>
                </div>
                <div>
                    <FormLabel for="register_user_name">Username</FormLabel>
                    <FormInput id="register_user_name" v-model="form.userName" :has-error="formErrors?.userName"/>
                    <FormErrors :errors="formErrors.userName"/>
                </div>
                <div>
                    <FormLabel for="register_password">Password</FormLabel>
                    <FormInput id="register_password" type="password" v-model="form.password" :has-error="formErrors?.password"/>
                    <FormErrors :errors="formErrors.password"/>
                </div>
                <div class="flex justify-end pt-3">
                    <AppButton>Sign Up</AppButton>
                </div>
            </form>

            <hr class="my-5">

            <div class="text-center">
                <p class="text-sm">
                    <span>Already have an account ?&nbsp;</span>
                    <RouterLink :to="{ name: RouteNames.Accounts_Login }" class="text-blue-700 hover:underline">Log in</RouterLink>
                </p>
            </div>
        </Card>
    </Container>
</template>
