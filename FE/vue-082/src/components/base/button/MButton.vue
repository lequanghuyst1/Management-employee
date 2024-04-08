<template>
  <button class="m-button">
    <i v-if="icon" :class="icon" :style="{color: color}"></i>
    {{text}}
  </button>
</template>
<script>
export default {
  name: "MButton",
  props: {
    text: {
      type: String,
      required: true,
    },
    icon: {
      type: String,
      required: false,
    },
    color:{
      type: String,
      required: false,
    },
    iconRight:{
      type: String,
      required: false,
    }
  },
};
</script>
<style scoped>
@import url(./button.css);
</style>
