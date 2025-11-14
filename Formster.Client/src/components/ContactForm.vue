<script setup>
import { ref, computed } from 'vue';
import axios from 'axios';

const form = ref({
    fullName: '',
    inquiryType: '',
    preferredDate: '',
    contactMethod: '',
    newsletter: false,
});

const inquiryOptions = [
    { value: 'support', label: 'Technical Support' },
    { value: 'sales', label: 'Sales Inquiry' },
    { value: 'other', label: 'Other' }
];

const contactOptions = [
    { value: 'email', label: 'Email' },
    { value: 'phone', label: 'Phone' }
];

const isRequired = (value) => value !== null && value !== undefined && value !== '';

const validationErrors = computed(() => ({
    fullName: isRequired(form.value.fullName) ? '' : 'Full name is required.',
    inquiryType: isRequired(form.value.inquiryType) ? '' : 'Please select an inquiry type.',
    preferredDate: isRequired(form.value.preferredDate) ? '' : 'Preferred date is required.',
    contactMethod: isRequired(form.value.contactMethod) ? '' : 'Please select a contact method.',
}));

const isFormValid = computed(() => {
    return Object.values(validationErrors.value).every(error => error === '');
});

const handleSubmit = async () => {
    if (isFormValid.value) {
        const submissionData = {
            fullName: form.value.fullName,
            inquiryType: form.value.inquiryType,
            preferredDate: form.value.preferredDate,
            contactMethod: form.value.contactMethod,
            newsletter: form.value.newsletter,
        };

        console.log('Form is valid. Submission Data:', submissionData);
        alert('Form Submitted! Check console for data structure.');
        
        const body = {
            formName: 'Contact Form',
            jsonData: submissionData
        }

        try {
            const response = await axios.post('http://localhost:5032/formster/api/form-submission', body);
            
            console.log('Submission successful:', response.data);
            alert('Form submitted successfully!');
            form.value = {
                fullName: '',
                inquiryType: '',
                preferredDate: '',
                contactMethod: '',
                newsletter: false,
            } 

        } catch (error) {
            console.error('Submission failed:', error.response ? error.response.data : error.message);
            alert('Error submitting form. Please try again.');
        }

    } else {
        console.error('Form has errors.');
        alert('Please correct the highlighted errors.');
    }
};
</script>

<template>
    <div class="form-container">
        <h2>Contact Us</h2>
        <form @submit.prevent="handleSubmit">

            <div class="form-group">
                <label for="fullName">Full Name *</label>
                <input type="text" id="fullName" v-model="form.fullName" :class="{ 'input-error': validationErrors.fullName }" />
                <span v-if="validationErrors.fullName" class="error-message">{{ validationErrors.fullName }}</span>
            </div>
            
            <div class="form-group">
                <label for="inquiryType">Inquiry Type *</label>
                <select id="inquiryType" v-model="form.inquiryType" :class="{ 'input-error': validationErrors.inquiryType }">
                    <option value="" disabled>-- Select an option --</option>
                    <option v-for="option in inquiryOptions" :key="option.value" :value="option.value">
                        {{ option.label }}
                    </option>
                </select>
                <span v-if="validationErrors.inquiryType" class="error-message">{{ validationErrors.inquiryType }}</span>
            </div>

            <div class="form-group">
                <label for="preferredDate">Preferred Contact Date *</label>
                <input type="date" id="preferredDate" v-model="form.preferredDate" :class="{ 'input-error': validationErrors.preferredDate }" />
                <span v-if="validationErrors.preferredDate" class="error-message">{{ validationErrors.preferredDate }}</span>
            </div>

            <div class="form-group">
                <label>Preferred Contact Method *</label>
                <div class="radio-group">
                    <div v-for="option in contactOptions" :key="option.value" class="radio-option">
                        <input type="radio" :id="`contact-${option.value}`" :value="option.value" v-model="form.contactMethod" />
                        <label :for="`contact-${option.value}`">{{ option.label }}</label>
                    </div>
                </div>
                <span v-if="validationErrors.contactMethod" class="error-message">{{ validationErrors.contactMethod }}</span>
            </div>

            <div class="form-group checkbox-group">
                <input type="checkbox" id="newsletter" v-model="form.newsletter" />
                <label for="newsletter">Yes, subscribe me to the newsletter.</label>
            </div>

            <button type="submit" :disabled="!isFormValid">Submit Contact Form</button>

        </form>
    </div>
</template>