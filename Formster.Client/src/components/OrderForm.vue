<script setup>
import { ref, computed } from 'vue';
import axios from 'axios';

// --- Form State ---
const orderForm = ref({
    customerEmail: '', 
    productSelection: '',
    deliveryDate: '', 
    paymentMethod: '', 
    isGift: false, 
    quantity: 1 
});

const productOptions = [
    { value: 'basic', label: 'Basic Package ($50)' },
    { value: 'premium', label: 'Premium Service ($150)' },
    { value: 'enterprise', label: 'Enterprise Custom Quote' }
];

const paymentOptions = [
    { value: 'card', label: 'Credit Card' },
    { value: 'invoice', label: 'Invoice' },
    { value: 'crypto', label: 'Crypto' }
];

const isRequired = (value) => value !== null && value !== undefined && value !== '';
const isValidEmail = (email) => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
const isPositiveNumber = (value) => value > 0;

const validationErrors = computed(() => ({
    customerEmail: isRequired(orderForm.value.customerEmail) 
        ? (isValidEmail(orderForm.value.customerEmail) ? '' : 'Please enter a valid email address.') 
        : 'Email is required.',
    productSelection: isRequired(orderForm.value.productSelection) ? '' : 'Product selection is required.',
    deliveryDate: isRequired(orderForm.value.deliveryDate) ? '' : 'Delivery date is required.',
    paymentMethod: isRequired(orderForm.value.paymentMethod) ? '' : 'Payment method is required.',
    quantity: isPositiveNumber(orderForm.value.quantity) ? '' : 'Quantity must be greater than zero.',
}));

const isFormValid = computed(() => {
    return Object.values(validationErrors.value).every(error => error === '');
});

const handleSubmit = async () => {
    if (isFormValid.value) {
        const submissionData = {
            email: orderForm.value.customerEmail,
            item: orderForm.value.productSelection,
            desiredDelivery: orderForm.value.deliveryDate,
            payment: orderForm.value.paymentMethod,
            isGiftWrap: orderForm.value.isGift,
            units: orderForm.value.quantity
        };

        console.log('Order Form is valid. Submission Data:', submissionData);
        
        const body = {
            formName: 'Order Form', 
            jsonData: submissionData
        }

        try {
            const response = await axios.post(import.meta.env.VITE_FORM_SUBMISSION_API_URL, body);
            
            console.log('Order Submission successful:', response.data);
            alert('Order submitted successfully!');
            
            orderForm.value = {
                customerEmail: '',
                productSelection: '',
                deliveryDate: '',
                paymentMethod: '',
                isGift: false,
                quantity: 1
            } 

        } catch (error) {
            console.error('Order Submission failed:', error.response ? error.response.data : error.message);
            alert('Error submitting order form. Please try again.');
        }

    } else {
        console.error('Order Form has errors.');
        alert('Please correct the highlighted errors.');
    }
};
</script>

<template>
    <div class="form-container">
        <h2>Place Your Order</h2>
        <form @submit.prevent="handleSubmit">

            <div class="form-group">
                <label for="customerEmail">Customer Email *</label>
                <input type="text" id="customerEmail" v-model="orderForm.customerEmail" :class="{ 'input-error': validationErrors.customerEmail }" />
                <span v-if="validationErrors.customerEmail" class="error-message">{{ validationErrors.customerEmail }}</span>
            </div>
            
            <div class="form-group">
                <label for="productSelection">Product/Service *</label>
                <select id="productSelection" v-model="orderForm.productSelection" :class="{ 'input-error': validationErrors.productSelection }">
                    <option value="" disabled>-- Select a product --</option>
                    <option v-for="option in productOptions" :key="option.value" :value="option.value">
                        {{ option.label }}
                    </option>
                </select>
                <span v-if="validationErrors.productSelection" class="error-message">{{ validationErrors.productSelection }}</span>
            </div>

            <div class="form-group">
                <label for="quantity">Quantity *</label>
                <input type="number" id="quantity" v-model.number="orderForm.quantity" min="1" :class="{ 'input-error': validationErrors.quantity }" />
                <span v-if="validationErrors.quantity" class="error-message">{{ validationErrors.quantity }}</span>
            </div>

            <div class="form-group">
                <label for="deliveryDate">Desired Delivery Date *</label>
                <input type="date" id="deliveryDate" v-model="orderForm.deliveryDate" :class="{ 'input-error': validationErrors.deliveryDate }" />
                <span v-if="validationErrors.deliveryDate" class="error-message">{{ validationErrors.deliveryDate }}</span>
            </div>

            <div class="form-group">
                <label>Payment Method *</label>
                <div class="radio-group">
                    <div v-for="option in paymentOptions" :key="option.value" class="radio-option">
                        <input type="radio" :id="`payment-${option.value}`" :value="option.value" v-model="orderForm.paymentMethod" />
                        <label :for="`payment-${option.value}`">{{ option.label }}</label>
                    </div>
                </div>
                <span v-if="validationErrors.paymentMethod" class="error-message">{{ validationErrors.paymentMethod }}</span>
            </div>

            <div class="form-group checkbox-group">
                <input type="checkbox" id="isGift" v-model="orderForm.isGift" />
                <label for="isGift">Mark as Gift (additional charge applies)</label>
            </div>

            <button type="submit" :disabled="!isFormValid">Submit Order</button>

        </form>
    </div>
</template>