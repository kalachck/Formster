<script setup>
import { ref } from 'vue';
import ContactForm from './components/ContactForm.vue'
import OrderForm from './components/OrderForm.vue';
import SubmissionList from './components/SubmissionList.vue';

const activeForm = ref('none');

const toggleForm = (formName) => {
  if (activeForm.value === formName) {
    activeForm.value = 'none';
  } else {
    activeForm.value = formName;
  }
}

</script>

<template>
  <SubmissionList />

  <hr>
  <div class="form-buttons">
    <button @click="toggleForm('order')">
      {{ activeForm === 'order' ? 'Order Form' : 'Order Form' }}
    </button>
    <button @click="toggleForm('contact')">
      {{ activeForm === 'contact' ? 'Contact Form' : 'Contact Form' }}
    </button>
  </div>

  <OrderForm v-if="activeForm === 'order'" />
  <ContactForm v-if="activeForm === 'contact'" />
</template>
