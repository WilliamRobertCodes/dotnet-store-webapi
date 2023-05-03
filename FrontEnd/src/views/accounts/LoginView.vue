<script setup>
import {reactive, ref} from "vue";
import Container from "@/components/layout/Container.vue";
import FormLabel from "@/components/forms/FormLabel.vue";
import FormInput from "@/components/forms/FormInput.vue";
import AppButton from "@/components/AppButton.vue";
import FormErrors from "@/components/forms/FormErrors.vue";
import Card from "@/components/Card.vue";
import {useAuthStore} from "@/stores/auth-store";

const {user, logIn} = useAuthStore();

const form = reactive({
    email: '',
    password: '',
});

const formErrors = ref({});

const submit = async () => {
    formErrors.value = {};

    const result = await logIn(form);

    if (result.success) {
        console.log('Success!');
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
        </Card>
    </Container>
</template>