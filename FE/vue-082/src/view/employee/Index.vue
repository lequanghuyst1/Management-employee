<template>
  <EmployeeList />
</template>
<script>
import EmployeeList from "./EmployeeList.vue";
export default {
  name: "EmployeePage",
  components: {
    EmployeeList,
  },
};
</script>
<style scoped>
@import url("../../css/pages/employee.css");
</style>
