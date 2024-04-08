<template>
  <p style="font-size: 14px">
    {{ this.$Resource[this.$languageCode].Text.SelectValueToImport }}
  </p>
  <div class="inputfile-box">
    <input type="file" id="file" class="inputfile" @change="uploadFile" />
    <label for="file" class="text-file">
      <span id="file-name" class="file-box">{{
        this.$Resource[this.$languageCode].Text.SelectAFile
      }}</span>
      <span
        class="file-button"
        v-tippy="{
          content: this.$Resource[this.$languageCode].ExcelImport.SelectFile,
          placement: 'top',
        }"
        >{{ this.$Resource[this.$languageCode].Text.Select }}</span
      >
    </label>
  </div>
  <p style="font-size: 14px">
    {{ this.$Resource[this.$languageCode].Text.NoFileTemplate }}
    <a
      v-tippy="{
        content: this.$Resource[this.$languageCode].ExcelImport.GetFileTemplate,
        placement: 'bottom',
      }"
      @click="getTemplateFileExcel"
      class="link-example"
      href="#"
      >{{ this.$Resource[this.$languageCode].Text.Here }}</a
    >
  </p>
</template>
<script>
import { saveAs } from "file-saver";
export default {
  name: "ImportSelectFile",
  props: {
    modelValue: {
      type: [String, File],
      require: false,
    },
  },
  inject: {
    importLink: {
      typeof: Object,
    },
  },
  methods: {
    /**
     * Thay đổi file
     * @param {event} e
     * Author: LQHUY(18/01/2024)
     */
    async uploadFile(e) {
      document.getElementById("file-name").innerHTML = e.target.files[0]?.name;
      try {
        var formData = new FormData();
        formData.append("formFile", e.target.files[0]);
        var res = await this.$httpRequest.postExcel(
          this.importLink.urlReadData,
          formData
        );
        switch (res.status) {
          case 200:
            this.$emit("update:modelValue", e.target.files[0]);
            break;
        }
      } catch (error) {
        this.$emitter.emit("handleApiError", error);
      }
    },

    /**
     * Lấy ra file nhập khẩu mẫu
     * Author: LQHUY(22/02/2024)
     */
    async getTemplateFileExcel() {
      try {
        this.$emitter.emit("toggleShowLoading", true);
        let res;

        res = await this.$httpRequest.getExcel(
          `${this.importLink.urlTemplateFile}`
        );
        const blob = new Blob([res.data], {
          type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        });
        var fileName = this.$Resource[this.$languageCode].Text.FileNameExcel;
        if (res.status == 200) {
          this.$emitter.emit("toggleShowLoading", false);
          // Mở cửa sổ thoại mở thư mục và cho phép thay tên file
          saveAs(blob, fileName, { autoBom: false });
        }
        this.isShowActionListExcel = false;
      } catch (error) {
        this.$emitter.emit("handleApiError", error);
        this.$emitter.emit("toggleShowLoading", false);
      }
    },
  },
};
</script>
<style scoped>
.inputfile-box {
  position: relative;
}
.inputfile {
  display: none;
}

.container {
  display: inline-block;
  width: 100%;
}
.text-file {
  margin-top: 12px;
  margin-bottom: 12px;
  display: flex;
  column-gap: 20px;
}
.file-box {
  display: inline-block;
  width: 70%;
  border: 1px solid #a49d9d;
  border-radius: 4px;
  padding: 0 12px;
  line-height: 36px;
  height: 36px;
  box-sizing: border-box;
  font-size: 14px;
}
.file-name {
  font-size: 14px;
}
.file-button {
  min-width: 100px;
  height: 36px;
  line-height: 36px;
  background-color: #50b83c;
  color: #ffffff;
  font-size: 14px;
  font-weight: 500;
  border: unset;
  cursor: pointer;
  text-align: center;
  border-radius: 4px;
  color: #1f1f1f;
  border: 1px solid #a49d9d;
  background-color: #ffffff;
}
.link-example {
  text-decoration: none;
  color: #0f84d2;
}
</style>
