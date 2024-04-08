<template>
    <MDialog id="m-dialog__notice">
      <template #body>
        <div class="m-dialog__content-icon" :class="className[type]">
          <i :class="icons[type]"></i>
        </div>
        <p class="m-dialog__content-message">
          {{ message }}
        </p>
      </template>
      <template #footer>
        <MButton
          @click="onBtnCancel"
          class="m-button--sub"
          :text="this.$Resource[this.$languageCode].Text.No"
        ></MButton>
        <MButton
          @click="onBtnConfirm"
          :text="this.$Resource[this.$languageCode].Text.Have"
        ></MButton>
      </template>
    </MDialog>
  </template>
  <script>
  export default {
    name: "MDialogConfirm",
    props: {
      message: {
        type: String,
        required: true,
      },
      type: {
        type: String,
        required: true,
      },
    
    },
    methods: {
      /**
       * Hàm hủy xóa ẩn dialog
       * Author: LQHUY(06/12/2002)
       */
      onBtnCancel() {
        this.$emitter.emit("toggleShowForm", false);
        this.$emitter.emit("toggleDialogConfirm", false);
      },
      /**
       * Hàm xác nhận xóa khách hàng
       * Author: LQHUY(06/12/2002)
       */
      onBtnConfirm() {
        
      },
      /**
       * Hàm xác nhận thông báo và đóng dialog
       * Author: LQHUY(06/12/2002)
       */
      onBtnCorrect() {
        this.$emitter.emit("toggleDialogConfirm", false);
      },
    },
    data() {
      return {
        icons: {
          question: "fa-solid fa-circle-question",
          warning: "fa-solid fa-triangle-exclamation",
          info: "fa-solid fa-circle-info",
        },
        className: {
          question: "m-dialog__content-icon--question",
          warning: "m-dialog__content-icon--warning",
          info: "m-dialog__content-icon--info",
        },
      };
    },
  };
  </script>
  <style scoped>
  @import url(./dialog.css);
  </style>
  