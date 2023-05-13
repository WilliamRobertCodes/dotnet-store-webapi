<script setup>
import {ref} from "vue";
import {api} from "@/utils/api";
import NewAddress from "@/views/accounts/user/addresses/NewAddress.vue";
import EditAddress from "@/views/accounts/user/addresses/EditAddress.vue";
import {NotificationTypes, useNotificationsStore} from "@/stores/notifications-store";

const notificationsStore = useNotificationsStore();

const comps = Object.freeze({
    None: 'None',
    NewAddress: 'NewAddress',
    EditAddress: 'EditAddress',
});

const comp = ref(comps.None);

const addresses = ref(null);
const countries = ref(null);
const loaded = ref(false);
const editAddress = ref(null);


Promise.all([
    fetchAddresses(),
    fetchCountries(),
]).then(() => {
    loaded.value = true;
});


async function fetchAddresses() {
    addresses.value = await api.get('accounts/addresses').json();
}

async function fetchCountries() {
    countries.value = await api.get('countries').json();
}

async function deleteAddress(addressId) {
    if (confirm("Delete this address ?")) {
        await api.delete(`accounts/addresses/${addressId}`).json();
        
        notificationsStore.addNotification({
            type: NotificationTypes.Success,
            message: 'Address deleted !',
        });
        
        await fetchAddresses();
    }
}

async function onAddressCreated() {
    notificationsStore.addNotification({
        type: NotificationTypes.Success,
        message: 'Address created !',
    });
    
    fetchAddresses();
    comp.value = comps.None;
}

async function editAddressClicked(address) {
    editAddress.value = {...address};
    comp.value = comps.EditAddress;
}

async function onAddressEdited() {
    notificationsStore.addNotification({
        type: NotificationTypes.Success,
        message: 'Address edited !',
    });
    fetchAddresses();
    comp.value = comps.None;
}
</script>

<template>
    <div class="grid grid-cols-[40%_60%] gap-8" v-if="loaded">
        <div>
            <div class="flex justify-between items-center mb-4">
                <h1 class="text-xl font-bold">Your addresses</h1>
            </div>
            <div class="space-y-4">
                <div v-for="address in addresses" :key="address.id" class="relative grid gap-2 p-4 border rounded text-sm group">
                    <button @click="editAddressClicked(address)" class="hidden group-hover:flex justify-center items-center absolute w-6 h-6 top-10 right-2 rounded-full bg-blue-600 transition hover:scale-105">
                        <svg xmlns="http://www.w3.org/2000/svg" class="stroke-white" width="16" height="16" viewBox="0 0 24 24" stroke-width="2" stroke="#2c3e50" fill="none" stroke-linecap="round" stroke-linejoin="round">
                            <path stroke="none" d="M0 0h24v24H0z" fill="none"/>
                            <path d="M7 7h-1a2 2 0 0 0 -2 2v9a2 2 0 0 0 2 2h9a2 2 0 0 0 2 -2v-1" />
                            <path d="M20.385 6.585a2.1 2.1 0 0 0 -2.97 -2.97l-8.415 8.385v3h3l8.385 -8.415z" />
                            <path d="M16 5l3 3" />
                        </svg>
                    </button>
                    <button @click="deleteAddress(address.id)" class="absolute top-2 right-2 hidden group-hover:flex justify-center items-center w-6 h-6 rounded-full bg-red-600 transition hover:scale-105">
                        <div class="relative top-[-1.5px] text-xl text-white">&times;</div>
                    </button>
                    <div>{{ address.firstName }} {{ address.lastName }}</div>
                    <div v-if="address.companyName">{{ address.companyName }}</div>
                    <div>{{ address.street1 }}</div>
                    <div v-if="address.street2">{{ address.street2 }}</div>
                    <div>{{ address.zipCode }} {{ address.city }}</div>
                    <div>{{ address.country.name }}</div>
                </div>
                <div v-if="addresses.length == 0" class="flex justify-center items-center py-3 px-4 border-2 rounded text-center">
                    <span class="text-sm text-gray-600 font-semibold">
                        You have no adresses... :( <br>
                        Use the button below to add one.
                    </span>
                </div>
                <button @click="comp = comps.NewAddress" class="flex justify-center items-center w-full py-3 px-4 border rounded transition group hover:bg-blue-100">
                    <div class="bg-blue-600 rounded-full transition group-hover:scale-110">
                        <svg xmlns="http://www.w3.org/2000/svg" class="stroke-white" width="24" height="24" viewBox="0 0 24 24" stroke-width="1.5" stroke="#2c3e50" fill="none" stroke-linecap="round" stroke-linejoin="round">
                            <path stroke="none" d="M0 0h24v24H0z" fill="none"/>
                            <path d="M12 5l0 14" />
                            <path d="M5 12l14 0" />
                        </svg>
                    </div>
                </button>
            </div>
        </div>
        <NewAddress
            v-if="comp === comps.NewAddress"
            @address-created="onAddressCreated"
            @cancel="comp = comps.None"
            :countries="countries"/>
        <EditAddress
            v-if="comp === comps.EditAddress"
            @address-edited="onAddressEdited"
            @cancel="comp = comps.None"
            :address="editAddress"
            :countries="countries"/>
    </div>
    <div v-else>Loading...</div>
</template>
