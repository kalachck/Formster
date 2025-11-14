<script setup>
import ContactForm from './components/ContactForm.vue'
import OrderForm from './components/OrderForm.vue';
import SubmissionList from './components/SubmissionList.vue';
</script>

<template>
  <SubmissionList />
  <OrderForm />
  <ContactForm />
</template>

<style scoped>
.logo {
  height: 6em;
  padding: 1.5em;
  will-change: filter;
  transition: filter 300ms;
}
.logo:hover {
  filter: drop-shadow(0 0 2em #646cffaa);
}
.logo.vue:hover {
  filter: drop-shadow(0 0 2em #42b883aa);
}
</style>
