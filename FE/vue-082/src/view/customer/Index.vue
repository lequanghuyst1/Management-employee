<template>
  <CustomerList></CustomerList>
</template>
<script>
import CustomerList from "./CustomerList.vue";
export default {
  name: "CustomerPage",
  components: {
    CustomerList,
  },
};
</script>
<style scoped>
@import url(../../css/pages/customer.css);
</style>
