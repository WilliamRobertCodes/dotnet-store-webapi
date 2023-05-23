<script setup>
import {nextTick, onMounted, reactive, ref} from "vue";
import {loadStripe} from '@stripe/stripe-js';
import {useCartStore} from "@/stores/cart-store";
import Container from "@/components/layout/Container.vue";
import AppButton from "@/components/AppButton.vue";
import {formatPriceInCents} from "@/utils/misc";
import {RouteNames} from "@/routing/router";
import {useConfigStore} from "@/stores/config-store";
import {api} from "@/utils/api";
import FormLabel from "@/components/forms/FormLabel.vue";
import FormSelect from "@/components/forms/FormSelect.vue";

const CheckoutSteps = Object.freeze({
    None: 'CheckoutSteps/None',
    AddressChoice: 'CheckoutSteps/AddressChoice',
    Payment: 'CheckoutSteps/Payment',
    Success: 'CheckoutSteps/Success',
});

const cartStore = useCartStore();
const configStore = useConfigStore();

const currentStep = ref(CheckoutSteps.None);

const addresses = ref(null);
const addressesForm = reactive({
    addressId: null,
});
const elements = ref(null);

const stripe = ref(null);

loadStripe(configStore.stripePublicKey).then((s) => {
    stripe.value = s;
});

onMounted(() => {
    api.get('accounts/addresses').json().then((response) => {
        addresses.value = response;
        addressesForm.addressId = addresses.value[0].id;
    });
});

async function createPaymentIntent() {
    const response = await api.post('checkout/create-payment-intent', {
        json: {
            addressId: parseInt(addressesForm.addressId),
        },
    }).json();

    currentStep.value = CheckoutSteps.Payment;

    elements.value = stripe.value.elements({
        appearance: {theme: 'stripe'},
        clientSecret: response.paymentIntentClientSecret,
    });

    const paymentElementOptions = {
        layout: "tabs",
    };

    nextTick().then(() => {
        const paymentElement = elements.value.create("payment", paymentElementOptions);
        paymentElement.mount("#payment-element");
    });
}

const onQuantityChange = ($event, productId) => {
    cartStore.updateCart({
        productId,
        quantity: $event.target.value,
    });
}

async function paymentFormSubmit(e) {
    e.preventDefault();
    setLoading(true);

    const result = await stripe.value.confirmPayment({
        elements: elements.value,
        redirect: "if_required",
        confirmParams: {
            // Make sure to change this to your payment completion page
            return_url: "http://localhost:4242/checkout.html",
        },
    });

    // This point will only be reached if there is an immediate error when
    // confirming the payment. Otherwise, your customer will be redirected to
    // your `return_url`. For some payment methods like iDEAL, your customer will
    // be redirected to an intermediate site first to authorize the payment, then
    // redirected to the `return_url`.
    if (result.error) {
        if (result.error.type === "card_error" || result.error.type === "validation_error") {
            showMessage(error.message);
        } else {
            showMessage("An unexpected error occurred.");
        }
    } else {
        currentStep.value = CheckoutSteps.Success;
    }

    setLoading(false);
}

// Fetches the payment intent status after payment submission
async function checkStatus() {
    const clientSecret = new URLSearchParams(window.location.search).get(
        "payment_intent_client_secret"
    );

    if (!clientSecret) {
        return;
    }

    const {paymentIntent} = await stripe.value.retrievePaymentIntent(clientSecret);

    switch (paymentIntent.status) {
        case "succeeded":
            showMessage("Payment succeeded!");
            break;
        case "processing":
            showMessage("Your payment is processing.");
            break;
        case "requires_payment_method":
            showMessage("Your payment was not successful, please try again.");
            break;
        default:
            showMessage("Something went wrong.");
            break;
    }
}

// ------- UI helpers -------

function showMessage(messageText) {
    const messageContainer = document.querySelector("#payment-message");

    messageContainer.classList.remove("hidden");
    messageContainer.textContent = messageText;

    setTimeout(function () {
        messageContainer.classList.add("hidden");
        messageText.textContent = "";
    }, 4000);
}

// Show a spinner on payment submission
function setLoading(isLoading) {
    if (isLoading) {
        // Disable the button and show a spinner
        document.querySelector("#submit").disabled = true;
        document.querySelector("#spinner").classList.remove("hidden");
        document.querySelector("#button-text").classList.add("hidden");
    } else {
        document.querySelector("#submit").disabled = false;
        document.querySelector("#spinner").classList.add("hidden");
        document.querySelector("#button-text").classList.remove("hidden");
    }
}
</script>

<template>
    <Container class="py-8">
        <h1 class="mb-4 text-2xl font-bold">Cart</h1>
        <div v-if="cartStore.cart">
            <div v-if="cartStore.cart.cartLineItems.length === 0" class="space-y-4 flex flex-col items-center py-4">
                <div class="text-lg text-center font-semibold">Your cart is empty :(</div>
                <div>
                    <RouterLink :to="{ name: RouteNames.Home }">
                        <AppButton>Go shopping !</AppButton>
                    </RouterLink>
                </div>
            </div>
            <div v-else class="grid grid-cols-[1fr_320px] items-start gap-8">
                <div class="overflow-hidden border rounded">
                    <article v-for="item in cartStore.cart.cartLineItems" :key="item.id"
                             class="flex justify-between p-4 border-b transition last:border-0 hover:bg-gray-50">
                        <div>
                            <h2 class="pb-1 align-baseline">
                                <RouterLink :to="{ name: RouteNames.Products_Single, params: { id: item.product.id } }"
                                            class="text-lg font-semibold hover:underline">{{ item.product.name }}
                                </RouterLink>&nbsp;<span>x{{ item.quantity }}</span>
                            </h2>
                            <div class="text-sm">
                                <p>Price per item: {{ formatPriceInCents(item.product.price) }}</p>
                                <p>Total for quantity: {{ formatPriceInCents(item.lineItemPriceInCents) }}</p>
                            </div>
                        </div>
                        <div class="flex items-end">
                            <div class="grid grid-flow-col gap-4">
                                <select @input="onQuantityChange($event, item.product.id)" v-model="item.quantity"
                                        class="h-[40px] w-[80px] px-2 border rounded cursor-pointer">
                                    <option v-for="i in 20" :value="i" :key="i">{{ i }}</option>
                                </select>
                                <AppButton @click="cartStore.removeFromCart({productId: item.product.id})"
                                           class="grid grid-flow-col gap-2">
                                    <svg xmlns="http://www.w3.org/2000/svg" class="w-[20px] h-[20px]"
                                         viewBox="0 0 24 24" stroke-width="1.5" stroke="#FFFFFF" fill="none"
                                         stroke-linecap="round" stroke-linejoin="round">
                                        <path stroke="none" d="M0 0h24v24H0z" fill="none"/>
                                        <line x1="4" y1="7" x2="20" y2="7"/>
                                        <line x1="10" y1="11" x2="10" y2="17"/>
                                        <line x1="14" y1="11" x2="14" y2="17"/>
                                        <path d="M5 7l1 12a2 2 0 0 0 2 2h8a2 2 0 0 0 2 -2l1 -12"/>
                                        <path d="M9 7v-3a1 1 0 0 1 1 -1h4a1 1 0 0 1 1 1v3"/>
                                    </svg>
                                    <span>Remove</span>
                                </AppButton>
                            </div>
                        </div>
                    </article>
                </div>
                <div class="py-4">
                    <div v-if="currentStep === CheckoutSteps.None" class="space-y-4 h-full">
                        <h3 class="mb-4 text-xl font-semibold">Order details</h3>
                        <div>Total price: {{ formatPriceInCents(cartStore.cart.totalPriceInCents) }}</div>
                        <div class="flex justify-end">
                            <AppButton @click="currentStep = CheckoutSteps.AddressChoice">Checkout</AppButton>
                        </div>
                    </div>
                    <div v-if="currentStep === CheckoutSteps.AddressChoice" class="space-y-4 h-full">
                        <div v-if="stripe && addresses.length">
                            <h3 class="mb-4 text-xl font-semibold">Check out</h3>
                            <form @submit.prevent="createPaymentIntent" class="space-y-4">
                                <div>
                                    <FormLabel>Address:</FormLabel>
                                    <FormSelect v-model="addressesForm.addressId">
                                        <option v-for="address in addresses" :value="address.id">
                                            {{ address.firstName }} {{ address.lastName }} - {{ address.street1 }}
                                            {{ address.city }}
                                        </option>
                                    </FormSelect>
                                </div>
                                <div class="flex justify-end">
                                    <AppButton type="submit">Go to payment</AppButton>
                                </div>
                            </form>
                        </div>
                        <div v-else>
                            <div>Loading...</div>
                        </div>
                    </div>
                    <div v-if="currentStep === CheckoutSteps.Payment">
                        <h3 class="mb-4 text-xl font-semibold">Payment</h3>
                        <form id="payment-form" @submit.prevent="paymentFormSubmit">
                            <div id="payment-element" class="mb-4"></div>
                            <div class="flex justify-end">
                                <AppButton id="submit">
                                    <div class="spinner hidden" id="spinner"></div>
                                    <span id="button-text">Pay now</span>
                                </AppButton>
                            </div>
                            <div id="payment-message" class="hidden"></div>
                        </form>
                    </div>
                    <div v-if="currentStep === CheckoutSteps.Success">
                        <h3 class="mb-4 text-xl font-semibold">Payment successful !</h3>
                        <p><a href="#">View my order</a></p>
                    </div>
                </div>
            </div>
        </div>
        <div v-else>
            <p>Loading...</p>
        </div>
    </Container>
</template>
