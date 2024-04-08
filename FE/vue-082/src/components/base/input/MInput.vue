<template>
  <div class="form-group">
    <label :for="id" class="m-lable" :class="{ 'label--required': required }">{{
      label
    }}</label>
    <input
      v-tippy="{
        content: messageError,
        placement: 'bottom',
        theme: 'light',
        offset: [80, 10],
      }"
      class="m-textfield"
      v-model="value"
      :id="id"
      :type="type ? type : 'text'"
      :class="{ 'm-textfield-error': messageError }"
      :placeholder="placeholder"
      :style="{ 'text-align': textAlign || 'left' }"
      ref="input"
      @blur="onBulrInput"
    />
    <span class="m-error-message">{{ messageError }}</span>
  </div>
</template>
<script>
export default {
  created() {
    this.value = this.modelValue;
    this.messageError = this.message;
  },

  name: "MInput",
  props: {
    label: {
      type: String,
      required: false,
    },
    modelValue: {
      type: [String, Number, Boolean, Array],
      required: false,
    },
    required: {
      type: Boolean,
      default: false,
    },
    type: {
      type: String,
      required: false,
    },
    placeholder: {
      type: String,
      required: false,
      default: "",
    },
    message: {
      type: String,
      required: false,
      default: "",
    },
    id: {
      type: String,
      required: false,
    },
    textAlign: {
      type: String,
      required: false,
    },
    formatMoney: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      value: null,
      messageError: "",
    };
  },
  watch: {
    /**
     * Hàm theo dõi giá trị thay đổi của value
     * @param {string} newValue
     * Author: LQHUY(08/12/2023)
     */
    value: function (newValue) {
      if (newValue === null || newValue === "" || newValue === undefined) {
        this.messageError = this.message;
        //nếu giá trị mới là rỗng hoặc null thì cập nhật giá trị = null
        this.$emit("update:modelValue", null);
      } else {
        this.messageError = "";
        this.$emit("update:modelValue", newValue.trim());
      }
    },
    /**
     * Hàm theo dõi giá trị truyển vào cho value
     * @param {string} newValue
     * Author: LQHUY(08/12/2023)
     */
    modelValue(newValue) {
      this.value = newValue;
    },
    /**
     * Hàm theo dõi message lỗi truyển vào
     * @param {string} newValue
     * Author: LQHUY(08/12/2023)
     */
    message(newValue) {
      this.messageError = newValue;
    },
  },
  methods: {
    // onInputValue() {
    //   if (this.formatMoney) {
    //     // Xóa các ký tự không phải số và định dạng do có chữ d nên phải định dạng toàn số
    //     let valueFormat = this.$helper.formatMoney(
    //       this.value.replace(/[^0-9]/g, "")
    //     );
    //     this.value = valueFormat;
    //   }
    // },
    /**
     * Set focus vào ô input
     * Author: LQHUY(07/12/2002)
     */
    setFocus() {
      this.$refs["input"].focus();
    },
    /**
     * Set border màu đỏ cho ô input lỗi
     * Author: LQHUY(18/03/2024)
     */
    setBorderError() {
      this.$refs["input"].classList.add("m-textfield-error");
    },
    /**
     * Gỡ border màu đỏ cho ô input lỗi
     * Author: LQHUY(18/03/2024)
     */
    removeBorderError() {
      this.$refs["input"].classList.remove("m-textfield-error");
    },
    /**
     * Blur ra ngoài ô input validate
     * Author: LQHUY(07/12/2002)
     */
    onBulrInput() {
      if (this.required) {
        if (
          this.value === null ||
          this.value === "" ||
          this.value === undefined
        ) {
          this.messageError = this.$Resource[this.$languageCode].ErrorMessage(
            this.label
          );
          //nếu giá trị mới là rỗng hoặc null thì cập nhật giá trị = null
        } else {
          this.messageError = "";
        }
      }
    },
  },
};
</script>
<style scoped>
@import url(./textfield.css);
</style>
