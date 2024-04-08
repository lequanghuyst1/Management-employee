<template>
  <div class="import-check__header">
    <p class="import-check__header--correct">
      {{ dataImport.length - this.lstError.length }}/{{ dataImport.length }}
      {{ this.$Resource[this.$languageCode].Text.CountValid }}
    </p>
    <p class="import-check__header--not-correct">
      {{ this.lstError.length }}/{{ dataImport.length }}
      {{ this.$Resource[this.$languageCode].Text.CountIsValid }}
    </p>
  </div>
  <div class="import-check__body">
    <div class="m-grid m-grid-import">
      <table class="m-table">
        <thead>
          <tr>
            <th
              class="m-table-row"
              v-for="(column, index) in columnsTable"
              :key="index"
              :style="{
                'text-align': column.textAlign || 'left',
                'min-Width': column.width ? column.width : null,
              }"
              :class="column.class"
              :ref="column.nameField"
            >
              {{ column.nameField }}
            </th>
            <th style="min-width: 400px">
              {{ this.$Resource[this.$languageCode].Text.Status }}
            </th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="(item, index) in dataImport"
            :key="index"
            class="m-table__row"
            style="min-height: 48px"
          >
            <td
              v-for="(col, index) in this.columnsTable"
              :key="index"
              :style="{
                textAlign: col.textAlign || 'left',
              }"
            >
              <tippy :content="this.setValueData(item, col)" placement="top">
                {{ this.setValueData(item, col) }}
              </tippy>
            </td>
            <td :class="[item.IsImport == false ? 'not-correct' : 'correct']">
              <p
                v-show="item.IsImport == false"
                v-for="(error, index) in setValueListError(item)"
                :key="index"
                style="text-wrap: wrap"
              >
                - {{ error.join("") }}
              </p>
              <p v-show="item.IsImport">
                {{ this.$Resource[this.$languageCode].Text.Correct }}
              </p>
            </td>
          </tr>
        </tbody>
      </table>
      <MLoadingData v-if="isShowLoadingImport"></MLoadingData>
    </div>
  </div>
  <div class="import-footer">
    <p @click="getFileImportError">
      {{ this.$Resource[this.$languageCode].Text.DownLoadFileImportErorr }}
      <a style="text-decoration: none; color: #0f84d2" href="#">{{
        this.$Resource[this.$languageCode].Text.Here
      }}</a>
    </p>
  </div>
</template>
<script>
import { saveAs } from "file-saver";
export default {
  name: "ImportCheckData",
  props: [
    "fileImport",
    "nextPage",
    "columnsTable",
    "countImportSuccess",
    "countImportFailure",
    "cacheKey",
  ],
  inject: {
    importLink: {
      typeof: Object,
    },
  },
  created() {
    this.loadDataImport();
  },
  watch: {
    lstError() {
      this.$emit(
        "update:countImportSuccess",
        this.dataImport.length - this.lstError.length
      );
      this.$emit("update:countImportFailure", this.lstError.length);
    },
  },
  methods: {
    /**
     * Gọi API đọc dữ liệu file excel
     * Author: LQHUY(18/01/2024)
     */
    async loadDataImport() {
      try {
        this.toggleShowLoadingImport();
        var formData = new FormData();
        formData.append("formFile", this.fileImport);
        var res = await this.$httpRequest.postExcel(
          this.importLink.urlReadData,
          formData
        );
        switch (res.status) {
          case 200:
            this.dataImport = res.data.ListInfoImport;
            this.lstError = this.dataImport.filter(
              (item) => item.IsImport === false
            );
            setTimeout(() => {
              this.$emitter.emit("toggleShowLoadingTable", false);
            }, 300);
            this.$emit("update:cacheKey", res.data.CacheKey),
              this.$emit(
                "update:cacheKeyResult",
                res.data.CacheKeyResultImport
              ),
              this.toggleShowLoadingImport();
            this.cacheKeyImportError = res.data.CacheKeyImportError;
            break;
        }
      } catch (error) {
        this.$emitter.emit("handleApiError", error);
        this.$emit("update:nextPage", 0);
        this.$emitter.emit("toggleShowLoadingTable", false);
      }
    },
    /**
     * Set dữ liệu cho từng ô trên table
     * @param {object} data
     * @param {string} col
     * Author: LQHUY(06/12/2023)
     */
    setValueData(data, col) {
      if (col.type === "gender") {
        return this.$helper.formatGender(data[col.field]);
      } else if (col.type === "money") {
        return this.$helper.formatMoney(data[col.field]);
      } else if (col.type === "datetime") {
        return this.$helper.formatDate(data[col.field]);
      } else {
        return data[col.field];
      }
    },
    /**
     * Xét các giá trị lỗi
     * @param {object} item
     * Author: LQHUY(18/01/2024)
     */
    setValueListError(item) {
      const lstErrorItem = item.ImportInvalidErrors;
      if (Object.values(lstErrorItem).join(",").trim() !== "") {
        return Object.values(lstErrorItem);
      } else {
        return [];
      }
    },
    /***
     * Ẩn hiện loading
     * Author: LQHUY(18/01/2024)
     */
    toggleShowLoadingImport() {
      this.isShowLoadingImport = !this.isShowLoadingImport;
    },
    /**
     * Lấy ra file chứa danh sách import bị lỗi
     * Author: LQHUY(05/03/2024)
     */
    async getFileImportError() {
      try {
        this.$emitter.emit("toggleShowLoading", true);
        let res;

        res = await this.$httpRequest.getExcel(
          `${this.importLink.urlFileImportError}`,
          this.cacheKeyImportError
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
  computed: {
    /**
     * Số dòng hợp lệ
     * Author: LQHUY(18/01/2024)
     */
  },
  data() {
    return {
      lstError: [],
      dataImport: [],
      isShowLoadingImport: false,
      cacheKeyImportError: "",
    };
  },
};
</script>
<style scoped>
@import url("../../components/base/table/table.css");
.m-grid-import {
  height: 100%;
}
.import-check__header {
  display: flex;
  column-gap: 160px;
  font-size: 14px;
  margin-bottom: 12px;
}
.import-check__body {
  height: calc(100% - 60px);
  border: 1px solid #e0e0e0;
  margin-bottom: 6px;
}
.import-footer {
  font-size: 14px;
}
.correct {
  color: #5ba6f1;
}
.not-correct {
  color: #f33b3b;
}

.m-grid-import .m-table th:nth-child(1),
.m-grid-import .m-table td:nth-child(1) {
  position: sticky;
  left: 0;
}

.m-grid-import .m-table th:last-child(1),
.m-grid-import .m-table td:last-child(1) {
  position: sticky;
  right: 0;
}
</style>
