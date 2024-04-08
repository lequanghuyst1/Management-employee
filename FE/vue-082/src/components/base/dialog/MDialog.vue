<template>
  <div class="m-dialog">
    <div class="m-dialog__overlay"></div>
    <div class="m-dialog__container">
      <div class="m-dialog__header">
        <h3 class="m-dialog__header-title">{{ title }}</h3>

        <div class="m-dialog__header-action">
          <div
            v-show="isShowIconHelp"
            v-tippy="{
              content: this.$Resource[this.$languageCode].ToolTip.Help,
              placement: 'bottom',
            }"
            class="m-dialog__header-help"
          >
            <i class="fa-regular fa-circle-question"></i>
          </div>

          <div
            v-tippy="{
              content: this.$Resource[this.$languageCode].ToolTip.ESC,
              placement: 'bottom',
            }"
            class="m-dialog__header-close"
            @click="btnHiddenForm"
          >
            <i class="fa-solid fa-xmark"></i>
          </div>
        </div>
      </div>
      <div class="m-dialog__content">
        <slot name="body"></slot>
      </div>
      <div class="m-dialog__footer">
        <slot name="footer"></slot>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: "MDialog",
  props: {
    title: {
      type: String,
      required: true,
    },
    isShowIconHelp: {
      type: String,
      required: false,
    },
  },
  methods: {
    /**
     * Hàm ẩn form hoặc dialog
     * Author: LQHUY (06/12/2023)
     */
    btnHiddenForm() {
      this.$emitter.emit("toggleDialogNotice", false);

      this.$emitter.emit("toggleShowForm");
    },
  },
};
</script>
<style scoped>
@import url(./dialog.css);
</style>
