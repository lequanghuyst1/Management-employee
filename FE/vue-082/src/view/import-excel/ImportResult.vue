<template>
  <div class="import-result">
    <h3 class="import-result__header">
      {{ this.$Resource[this.$languageCode].Text.ResultImport }}
    </h3>
    <p>
      {{ this.$Resource[this.$languageCode].Text.DownLoadFile }}
      <a
        @click="getFileResultImport"
        style="text-decoration: none; color: #0f84d2"
        href="#"
        >{{ this.$Resource[this.$languageCode].Text.Here }}</a
      >
    </p>
    <p>
      + {{ this.$Resource[this.$languageCode].Text.CountRowImportSuccess }}:
      {{ this.countImportSuccess }}
    </p>
    <p>
      + {{ this.$Resource[this.$languageCode].Text.CountRowImportFailure }}:
      {{ this.countImportFailure }}
    </p>
  </div>
</template>
<script>
import { saveAs } from "file-saver";
export default {
  name: "ImportResult",
  props: {
    countImportSuccess: {
      type: Number,
      required: false,
    },
    countImportFailure: {
      type: Number,
      required: false,
    },
    cacheKeyResult: {
      typeof: String,
    },
    cacheKey: {
      typeof: String,
    },
  },
  inject: {
    importLink: {
      typeof: Object,
    },
  },
  created() {
    this.ImportData();
  },

  methods: {
    /**
     * Gọi API import
     * Author: LQHUY(18/01/2024)
     */
    async ImportData() {
      try {
        this.$emitter.emit("toggleShowLoading", true);
        var res = await this.$httpRequest.importExcel(
          this.importLink.urlImport,
          this.cacheKey
        );
        switch (res.status) {
          case 200:
            this.$emitter.emit("toggleShowLoading", false);
            break;
        }
      } catch (error) {
        this.$emitter.emit("toggleShowLoading", false);
        this.$emitter.emit("handleApiError", error);
      }
    },
    /**
     * Gọi API import
     * Author: LQHUY(18/01/2024)
     */
    async getFileResultImport() {
      try {
        this.$emitter.emit("toggleShowLoading", true);
        let res;

        res = await this.$httpRequest.getExcel(
          `${this.importLink.urlFileImportResult}`,
          this.cacheKeyResult
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
.import-result {
  font-size: 14px;
}
p + p {
  margin-top: 12px;
}
.import-result__header {
  font-size: 24px;
  margin-bottom: 12px;
}
</style>
