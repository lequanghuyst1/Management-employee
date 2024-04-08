<template>
  <div class="m-toast-box">
    <div class="m-toast" :class="className[type]">
      <div class="m-toast-icon">
        <i :class="icons[type]"></i>
      </div>
      <div class="m-toast-content">
        <p class="m-toast-status">
          {{ status }}
          <span class="m-toast-content">{{ message }}</span>
        </p>
      </div>
      <div class="m-toast-icon-close">
        <i class="fa-solid fa-xmark"></i>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: "MToastMessage",
  props: {
    message: {
      type: String,
      required: true,
      default: "",
    },
    type: {
      type: String,
      required: true,
      default: "",
    },
    status: {
      type: String,
      required: true,
      default: "",
    },
  },
  data() {
    return {
      icons: {
        success: "fa-solid fa-circle-check",
        error: "fa-solid fa-circle-xmark",
        warning: "fa-solid fa-triangle-exclamation",
        info: "fa-solid fa-circle-info",
      },
      className: {
        success: "m-toast-success",
        error: "m-toast-error",
        warning: "m-toast-warning",
        info: "m-toast-info",
      },
    };
  },
};
</script>
<style scoped>
@import url(./toast.css);
</style>
