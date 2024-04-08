<template>
  <TheHeader></TheHeader>
  <div class="container">
    <TheSidebar></TheSidebar>
    <TheContent></TheContent>
  </div>
</template>
<script>
import TheHeader from "../TheHeader.vue";
import TheContent from "../TheContent.vue";
import TheSidebar from "../TheSidebar.vue";
export default {
  name: "DefaultLayout",
  components: { TheHeader, TheContent, TheSidebar },

};
</script>
<style scoped>
</style>
