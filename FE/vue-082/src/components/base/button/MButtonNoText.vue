<template>
  <button class="m-button--no-text">
    <div :class="icon"></div>
    <slot></slot>
  </button>
</template>
<script>
export default {
  name: "MButtonNoText",
  props: {
    icon: {
      type: String,
      required: true,
    },

  },
};
</script>
<style scoped>
@import url(./button.css);
</style>
