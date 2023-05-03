<script setup>
import {reactive, ref} from "vue";
import ky from "ky";
import Container from "@/components/layout/Container.vue";
import FormLabel from "@/components/forms/FormLabel.vue";
import FormInput from "@/components/forms/FormInput.vue";
import FormErrors from "@/components/forms/FormErrors.vue";
import AppButton from "@/components/AppButton.vue";
import Card from "@/components/Card.vue";

const form = reactive({
    email: '',
    password: '',
});

const formErrors = ref({});

const submit = async () => {
    try {
        const json = await ky
            .post('http://localhost:5115/api/accounts/register', { json: form })
            .json();
    } catch (error) {

    }
};
</script>

<template>
    <Container class="py-8">
        <Card class="max-w-[480px] mx-auto">
            <h1 class="mb-6 text-center text-xl font-bold">Sign up</h1>

            <form @submit.prevent="submit" class="space-y-4">
                <div>
                    <FormLabel for="login_email">Email</FormLabel>
                    <FormInput id="login_email" v-model="form.email"/>
                    <FormErrors v-if="formErrors.email">{{ formErrors.email }}</FormErrors>
                </div>
                <div>
                    <FormLabel for="login_password">Password</FormLabel>
                    <FormInput id="login_password" v-model="form.password"/>
                    <FormErrors v-if="formErrors.password">{{ formErrors.password }}</FormErrors>
                </div>
                <div class="flex justify-end pt-3">
                    <AppButton>Log in</AppButton>
                </div>
            </form>
        </Card>
    </Container>
</template>
