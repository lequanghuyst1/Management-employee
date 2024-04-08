<template>
  <div class="form-group">
    <label for="" class="m-lable">{{
      this.$Resource[this.$languageCode].Text.Gender
    }}</label>
    <label
      v-for="item in this.$Enum.Gender"
      :key="item"
      class="warrap__input-radio"
      >{{ this.$helper.formatGender(item) }}
      <input type="radio" name="gender" :value="item" v-model="genderValue" />
      <span class="checkmark"></span>
    </label>
    <span class="m-error-message"></span>
  </div>
</template>
<script>
export default {
  name: "MGenderRadio",
  props: {
    modelValue: {
      type: [String, Number],
      required: false,
    },
  },
  created() {
    this.genderValue = this.$Enum.Gender.Male;
  },
  watch: {
    /**
     * Theo dõi sự thay đổi giá trị truyền vào
     * @param {*} newValue
     */
    modelValue(newValue) {
      if (newValue === null || newValue === undefined || newValue == "") {
        this.genderValue = this.$Enum.Gender.Male;
      } else {
        this.genderValue = this.modelValue;
      }
    },

    /**
     * Cập nhật lại dữ liệu truyền vào
     * @param {enum} value
     * Author: LQHUY(10/01/2024)
     */
    genderValue(value) {
      this.$emit("update:modelValue", value);
    },
  },
  data() {
    return {
      genderValue: null,
    };
  },
};
</script>
<style scoped>
/* input radio */
.warrap__input-radio {
  display: inline-block;
  position: relative;
  cursor: pointer;
  user-select: none;
  padding-left: 22px;
  margin-top: 8px;
  font-size: 14px;
}
.warrap__input-radio + .warrap__input-radio {
  margin-left: 24px;
}
.warrap__input-radio input {
  position: absolute;
  opacity: 0;
  cursor: pointer;
}
.checkmark {
  position: absolute;
  top: 0;
  left: 0;
  width: 18px;
  height: 18px;
  background: #eee;
  border-radius: 50%;
}
.warrap__input-radio:hover .checkmark {
  background: #ccc;
}
.checkmark:after {
  content: "";
  position: absolute;
  display: none;
}
.warrap__input-radio .checkmark:after {
  top: 50%;
  left: 50%;
  width: 10px;
  height: 10px;
  border-radius: 50%;
  border: solid 2px white;
  transform: translate(-50%, -50%) rotate(45deg);
}
.warrap__input-radio input:checked ~ .checkmark {
  background: #50b83c;
}
.warrap__input-radio input:checked ~ .checkmark:after {
  display: block;
}
</style>
