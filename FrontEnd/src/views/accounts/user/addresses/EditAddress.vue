<script setup>
import AppButton from "@/components/AppButton.vue";
import FormInput from "@/components/forms/FormInput.vue";
import FormLabel from "@/components/forms/FormLabel.vue";
import FormErrors from "@/components/forms/FormErrors.vue";
import FormSelect from "@/components/forms/FormSelect.vue";
import {reactive, ref} from "vue";
import {api} from "@/utils/api";

const { countries, showCancelButton, address } = defineProps({
    address: {
        required: true,
        type: Object,
    },
    countries: {
        required: true,
        type: Array,
    },
    showCancelButton: {
        required: false,
        type: Boolean,
        default: true,
    },
});
const emit = defineEmits(['addressEdited', 'cancel']);

const form = reactive({...address});
const formErrors = ref({});

async function editAddress() {
    try {
        await api.put(`accounts/addresses/${address.id}`, { json: form }).json();
        emit('addressEdited');
    } catch (error) {
        if (!error.response) {
            throw error;
        }
        
        const {status} = error.response;

        if (status >= 400 && status < 500) {
            const json = await error.response.json();

            formErrors.value = json.errors;
        }

        throw error;
    }
}
</script>

<template>
    <div>
        <h2 class="mb-4 text-xl font-bold">Edit address</h2>
        <form @submit.prevent="editAddress" v-if="countries" class="space-y-4">
            <div class="grid grid-cols-2 gap-4">
                <div>
                    <FormLabel for="create_address_first_name">First name</FormLabel>
                    <FormInput for="create_address_first_name" v-model="form.firstName" :has-error="formErrors?.firstName"/>
                    <FormErrors :errors="formErrors?.firstName"/>
                </div>
    
                <div>
                    <FormLabel for="create_address_last_name">Last name</FormLabel>
                    <FormInput for="create_address_last_name" v-model="form.lastName" :has-error="formErrors?.lastName"/>
                    <FormErrors :errors="formErrors?.lastName"/>
                </div>
            </div>
    
            <div>
                <FormLabel for="create_address_company_name">Company name</FormLabel>
                <FormInput for="create_address_company_name" v-model="form.companyName" :has-error="formErrors?.companyName"/>
                <FormErrors :errors="formErrors?.companyName"/>
            </div>
    
            <div>
                <FormLabel for="create_address_street1">Street 1</FormLabel>
                <FormInput for="create_address_street1" v-model="form.street1" :has-error="formErrors?.street1"/>
                <FormErrors :errors="formErrors?.street1"/>
            </div>
    
            <div>
                <FormLabel for="create_address_street2">Street 2</FormLabel>
                <FormInput for="create_address_street2" v-model="form.street2" :has-error="formErrors?.street2"/>
                <FormErrors :errors="formErrors?.street2"/>
            </div>
    
    
            <div class="grid grid-cols-[120px_1fr] gap-4">
                <div>
                    <FormLabel for="create_address_zip_code">Zip code</FormLabel>
                    <FormInput for="create_address_zip_code" v-model="form.zipCode" :has-error="formErrors?.zipCode"/>
                    <FormErrors :errors="formErrors?.zipCode"/>
                </div>
    
                <div>
                    <FormLabel for="create_address_city">City</FormLabel>
                    <FormInput for="create_address_city" v-model="form.city" :has-error="formErrors?.city"/>
                    <FormErrors :errors="formErrors?.city"/>
                </div>
            </div>
    
            <div>
                <FormLabel for="create_address_country_id">Country</FormLabel>
                <FormSelect name="country" id="create_address_country_id" v-model="form.countryId">
                    <option v-for="country in countries" :key="country.id" :value="country.id">
                        {{ country.name }}
                    </option>
                </FormSelect>
                <FormErrors :errors="formErrors?.countryId"/>
            </div>
    
            <div class="flex justify-end gap-4 pt-3">
                <button v-if="showCancelButton" @click="emit('cancel')" type="button" class="px-3 border-2 rounded font-semibold transition hover:bg-gray-200">Cancel</button>
                <AppButton type="submit">Edit address</AppButton>
            </div>
        </form>
    </div>
</template>

<style scoped>

</style>