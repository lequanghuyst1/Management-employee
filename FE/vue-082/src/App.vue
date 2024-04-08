<template>
  <router-view name="LayoutViewRouter"></router-view>
  <MToast
    v-if="showToastMessage"
    :type="typeToast"
    :message="messageToast"
    :status="statusToast"
  ></MToast>

  <MDialogNotice
    v-if="showDialogNotice"
    :title="titleDialog"
    :message="messageDialog"
    :type="typeDialog"
    :isBtnCancel="isBtnCancel"
  >
  </MDialogNotice>
  <MDialogConfirm
    v-if="showDialogConfirm"
    :title="titleDialog"
    :message="messageDialog"
    :type="typeDialog"
  >
  </MDialogConfirm>
  <MLoading v-show="showLoading"></MLoading>
</template>
<script>
export default {
  name: "App",
  created() {
    this.$emitter.on("onShowToastMessage", this.onShowToastMessage);
    this.$emitter.on("toggleDialogConfirm", this.toggleDialogConfirm);
    this.$emitter.on("toggleDialogNotice", this.toggleDialogNotice);
    this.$emitter.on("toggleShowLoading", this.toggleShowLoading);
    this.$emitter.on("handleApiError", this.handleApiError);
  },
  beforeUnmount() {
    this.$emitter.off("onShowToastMessage");
    this.$emitter.on("toggleDialogConfirm");
    this.$emitter.off("toggleDialogNotice");
    this.$emitter.off("toggleShowLoading");
    this.$emitter.off("handleApiError");
  },
  methods: {
    /**
     * Hàm hiện toast message và tự động gỡ bỏ
     * Author: LQHUY (6/12/2023)
     */
    onShowToastMessage(type, message, status) {
      this.showToastMessage = true;
      this.typeToast = type;
      this.messageToast = message;
      this.statusToast = status;
      setTimeout(() => {
        this.showToastMessage = false;
      }, 3000);
    },

    /**
     * Hàm hiển thị dialog thông báo
     * @param {string} title
     * @param {string} message
     * @param {string} type
     * @param {boolean} isBtnCancel
     * Author: LQHUY (5/12/2023)
     * Update: LQHUY (8/12/2023)
     */
    toggleDialogNotice(isBtnCancel, isShow, title, message, type) {
      this.isBtnCancel = isBtnCancel;
      this.showDialogNotice = isShow;
      this.titleDialog = title;
      this.messageDialog = message;
      this.typeDialog = type;
    },

    /**
     * Hàm hiển thị dialog thông báo
     * @param {string} title
     * @param {string} message
     * @param {string} type
     * Author: LQHUY (5/12/2023)
     * Update: LQHUY (8/12/2023)
     */
    toggleDialogConfirm(isShow, title, message, type) {
      this.showDialogConfirm = isShow;
      this.titleDialog = title;
      this.messageDialog = message;
      this.typeDialog = type;
    },

    /**
     * Hàm xử lí ẩn hiện loading
     * @param {object} req
     * Author: LQHUY(08/12/2023)
     */
    toggleShowLoading(isShow) {
      this.showLoading = isShow;
    },

    /**
     * Hàm xử lí các lỗi trả về từ API
     * @param {object} req
     * Author: LQHUY(15/12/2023)
     */
    handleApiError(req) {
      try {
        switch (req.response.status) {
          //Lỗi từ người dùng nhập thông tin không hợp lệ
          case 400:
            this.$emitter.emit(
              "toggleDialogNotice",
              false,
              true,
              this.$Resource[this.$languageCode].WanrningMessage,
              Object.values(req.response.data.errors).join(","),
              this.$Resource[this.$languageCode].Dialog.Type.Warning
            );
            break;
          //Lỗi khi tải khoản đăng nhập không đúng
          case 401:
            this.$emitter.emit("toggleShowLoading", false);
            this.$emitter.emit("toggleShowLoadingTable", false);
            this.$emitter.emit(
              "onShowToastMessage",
              this.$Resource[this.$languageCode].ToastMessage.Type.Error,
              this.$Resource[this.$languageCode].InCorrectAccount,
              this.$Resource[this.$languageCode].ToastMessage.Status.Error
            );
            break;
          //Lỗi khi không có quyền truy cập
          case 403:
            this.$emitter.emit("toggleShowLoading", false);
            this.$emitter.emit("toggleShowLoadingTable", false);
            this.$emitter.emit(
              "onShowToastMessage",
              this.$Resource[this.$languageCode].ToastMessage.Type.Error,
              this.$Resource[this.$languageCode].NotHaveAccess,
              this.$Resource[this.$languageCode].ToastMessage.Status.Error
            );
            break;
          //Lỗi khi đường dẫn gọi API lỗi
          case 404:
            this.$emitter.emit("toggleShowLoading", false);
            this.$emitter.emit("toggleShowLoadingTable", false);
            this.$emitter.emit(
              "onShowToastMessage",
              this.$Resource[this.$languageCode].ToastMessage.Type.Error,
              this.$Resource[this.$languageCode].InCorrectUrlLink,
              this.$Resource[this.$languageCode].ToastMessage.Status.Error
            );
            break;
          //Lỗi từ phía backend
          case 500:
            this.$emitter.emit("toggleShowLoading", false);
            this.$emitter.emit("toggleShowLoadingTable", false);
            this.$emitter.emit(
              "onShowToastMessage",
              this.$Resource[this.$languageCode].ToastMessage.Type.Error,
              req.response.data.UserMsg,
              this.$Resource[this.$languageCode].ToastMessage.Status.Error
            );
            break;
          default:
            break;
        }
      } catch (error) {
        this.$emitter.emit("toggleShowLoadingTable", false);
        this.$emitter.emit(
          "onShowToastMessage",
          this.$Resource[this.$languageCode].ToastMessage.Type.Error,
          this.$Resource[this.$languageCode].MessageHelp,
          this.$Resource[this.$languageCode].ToastMessage.Status.Error
        );
      }
    },
  },
  data() {
    return {
      /**data toast message */
      showToastMessage: false,
      typeToast: "",
      messageToast: "",
      statusToast: "",

      /**data dialog notice */
      showDialogNotice: false,
      isBtnCancel: false,
      titleDialog: "",
      messageDialog: "",
      typeDialog: "",
      showLoading: false,

      showDialogConfirm: false,
    };
  },
};
</script>

<style scoped>
@import url(./css/main.css);
</style>
