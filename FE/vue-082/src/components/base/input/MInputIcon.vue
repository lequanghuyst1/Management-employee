<template>
  <div class="m-textfiled-icon m-textfiled-search">
    <input
      type="text"
      v-model="value"
      class="m-textfield"
      :placeholder="placeholder"
      :ref="refEl"
    />
    <div class="m-textfield-icon-search">
      <i class="fa-solid fa-magnifying-glass"></i>
    </div>
    <div v-if="isShowLoadingInput" class="icon-container">
      <i class="loader"></i>
    </div>
    <div @click="clearValueInput" v-if="isShowIconClose" class="icon-close">
      <i class="fa-solid fa-circle-xmark"></i>
    </div>
  </div>
</template>
<script>
export default {
  name: "MInputIcon",
  props: {
    placeholder: {
      typeof: String,
      required: false,
    },
    modelValue: {
      typeof: String,
      required: true,
    },
    refEl: {
      typeof: String,
      required: false,
    },
  },
  watch: {
    /**
     * Gán lại cho giá trị truyền vào
     * @param {string} newValue 
     * Author: (03/01/2024)
     */
    value(newValue) {
      this.isShowLoadingInput = true;
      this.isShowIconClose = false;
      clearTimeout(this.timeoutHandler);
      this.timeoutHandler = setTimeout(() => {
        this.$emit("update:modelValue", newValue.trim());
        this.isShowLoadingInput = false;
        if (newValue === null || newValue === "") {
          this.isShowIconClose = false;
        } else {
          this.isShowIconClose = true;
        }
      }, 500);
    },
  },
  methods: {
    /**
     * Xóa bỏ dữ liệu trên ô tìm kiếm
     * Author: Author: (03/01/2024)
     */
    clearValueInput() {
      this.value = "";
      this.isShowIconClose = false;
      this.$refs[this.refEl].focus();
    },
  },
  data() {
    return {
      value: null,
      timeoutHandler: null,
      isShowLoadingInput: false,
      isShowIconClose: false,
    };
  },
};
</script>
<style scoped>
@import url(./textfield.css);
.m-textfiled-search {
  width: 100%;
}
.m-textfiled-search .m-textfield {
  border-radius: 4px;
}
.m-textfield-icon-search {
  position: absolute;
  right: 5px;
  height: 100%;
  top: calc(50% - 10px);
  background-color: transparent;
  border-top-right-radius: 4px;
  border-bottom-right-radius: 4px;
  color: #aeadad;
}
.icon-container,
.icon-close {
  position: absolute;
  right: 35px;
  top: calc(50% - 11px);
}
.icon-close {
  color: #aeadad;
}
.loader {
  position: relative;
  height: 16px;
  width: 16px;
  display: inline-block;
  animation: around 5.4s infinite;
}

@keyframes around {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}

.loader::after,
.loader::before {
  content: "";
  background: white;
  position: absolute;
  display: inline-block;
  width: 100%;
  height: 100%;
  border-width: 2px;
  border-color: #50b83c #50b83c transparent transparent;
  border-style: solid;
  border-radius: 16px;
  box-sizing: border-box;
  top: 0;
  left: 0;
  animation: around 0.7s ease-in-out infinite;
}

.loader::after {
  animation: around 0.7s ease-in-out 0.1s infinite;
  background: transparent;
}
</style>
