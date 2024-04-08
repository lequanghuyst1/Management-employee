<template>
  <div id="import-excel">
    <div class="warrap-excel">
      <div class="import__header">
        <p v-if="this.nextPage === 0">
          {{ this.$Resource[this.$languageCode].ExcelImport.Step }}
          {{ this.$Resource[this.$languageCode].ExcelImport.StepOne }}
        </p>
        <p v-if="this.nextPage === 1">
          {{ this.$Resource[this.$languageCode].ExcelImport.Step }}
          {{ this.$Resource[this.$languageCode].ExcelImport.StepSecond }}
        </p>
        <p v-if="this.nextPage === 2">
          {{ this.$Resource[this.$languageCode].ExcelImport.Step }}
          {{ this.$Resource[this.$languageCode].ExcelImport.StepThird }}
        </p>
      </div>
      <div class="import__container">
        <div class="import__sidebar">
          <ul class="nav_list-import">
            <a
              @click="this.nextPage = 0"
              to=""
              :class="{
                'nav__list-item-import--selected': this.nextPage === 0,
              }"
              class="nav__list-item-import"
              >{{ this.$Resource[this.$languageCode].ExcelImport.StepOne }}</a
            >
            <a
              @click="checkDataImport"
              to=""
              class="nav__list-item-import"
              :class="{
                'nav__list-item-import--selected': this.nextPage === 1,
              }"
              >{{
                this.$Resource[this.$languageCode].ExcelImport.StepSecond
              }}</a
            >
            <a
              @click="resultImport"
              :class="{
                'nav__list-item-import--selected': this.nextPage === 2,
              }"
              to=""
              class="nav__list-item-import"
              >{{ this.$Resource[this.$languageCode].ExcelImport.StepThird }}</a
            >
          </ul>
        </div>
        <div class="import__content">
          <ImportSelectFile
            v-model="fileImport"
            v-if="this.nextPage == 0"
          ></ImportSelectFile>
          <ImportCheckData
            :columnsTable="columnsTable"
            :fileImport="fileImport"
            v-if="this.nextPage == 1"
            v-model:countImportSuccess="countImportSuccess"
            v-model:countImportFailure="countImportFailure"
            v-model:nextPage="nextPage"
            v-model:cacheKey="cacheKey"
            v-model:cacheKeyResult="cacheKeyResult"
          ></ImportCheckData>
          <ImportResult
            :countImportSuccess="countImportSuccess"
            :countImportFailure="countImportFailure"
            :cacheKeyResult="cacheKeyResult"
            :cacheKey="cacheKey"
            v-if="this.nextPage == 2"
          ></ImportResult>
        </div>
      </div>
      <div class="import__footer">
        <MButton
          v-tippy="{
            content: this.$Resource[this.$languageCode].ExcelImport.Help,
            placement: 'top',
          }"
          icon="fa-regular fa-circle-question"
          class="m-button-icon m-button--sub"
          color="#61AFFE"
          :text="this.$Resource[this.$languageCode].ExcelImport.Help"
          id="btn-import-help"
        ></MButton>
        <div v-if="this.nextPage == 0" class="import__footer-rigth">
          <MButton
            icon="fa-solid fa-left-long"
            class="m-button-icon m-button--sub disable"
            style="cursor: default"
            color="#61AFFE"
            :text="this.$Resource[this.$languageCode].ExcelImport.Prev"
            id="btn-import-prev"
            @click="onClickPrevPage"
          ></MButton>
          <MButton
            v-tippy="{
              content: this.$Resource[this.$languageCode].ExcelImport.Next,
              placement: 'top',
            }"
            icon="fa-solid fa-right-long"
            class="m-button-icon m-button--sub"
            color="#61AFFE"
            :text="this.$Resource[this.$languageCode].ExcelImport.Next"
            id="btn-import-next"
            iconRight="iconRight"
            @click="onClickNextPage"
          ></MButton>
          <MButton
            v-tippy="{
              content: this.$Resource[this.$languageCode].ExcelImport.Cancle,
              placement: 'top',
            }"
            icon="fa-solid fa-ban"
            class="m-button-icon m-button--sub"
            color="#F93E3E"
            :text="this.$Resource[this.$languageCode].ExcelImport.Cancle"
            id="btn-import-cancel"
            @click="onClickCancelImport"
          ></MButton>
        </div>

        <div v-if="this.nextPage == 1" class="import__footer-rigth">
          <MButton
            v-tippy="{
              content: this.$Resource[this.$languageCode].ExcelImport.Prev,
              placement: 'top',
            }"
            icon="fa-solid fa-left-long"
            class="m-button-icon m-button--sub"
            color="#61AFFE"
            :text="this.$Resource[this.$languageCode].ExcelImport.Prev"
            id="btn-import-prev"
            @click="onClickPrevPage"
          ></MButton>
          <MButton
            v-tippy="{
              content: this.$Resource[this.$languageCode].ExcelImport.Perform,
              placement: 'top',
            }"
            v-if="countImportSuccess != 0"
            icon="fa-solid fa-file-import"
            class="m-button-icon m-button--sub"
            color="#61AFFE"
            :text="this.$Resource[this.$languageCode].ExcelImport.Perform"
            id="btn-import-next"
            iconRight="iconRight"
            @click="onClickNextPage"
          ></MButton>
          <MButton
            v-if="countImportSuccess == 0"
            icon="fa-solid fa-file-import"
            class="m-button-icon m-button--sub disable"
            color="#61AFFE"
            text="Thực hiện"
            id="btn-import-next"
            iconRight="iconRight"
            style="cursor: default"
          ></MButton>
          <MButton
            v-tippy="{
              content: this.$Resource[this.$languageCode].ExcelImport.Cancle,
              placement: 'top',
            }"
            icon="fa-solid fa-ban"
            class="m-button-icon m-button--sub"
            color="#F93E3E"
            :text="this.$Resource[this.$languageCode].ExcelImport.Cancle"
            id="btn-import-cancel"
            @click="onClickCancelImport"
          ></MButton>
        </div>
        <div v-if="this.nextPage == 2" class="import__footer-rigth">
          <MButton
            v-tippy="{
              content: this.$Resource[this.$languageCode].ExcelImport.Close,
              placement: 'top',
            }"
            icon="fa-solid fa-ban"
            class="m-button-icon m-button--sub"
            color="#F93E3E"
            :text="this.$Resource[this.$languageCode].ExcelImport.Close"
            id="btn-import-cancel"
            @click="onClickCancelImport"
          ></MButton>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import ImportCheckData from "./ImportCheckData.vue";
import ImportResult from "./ImportResult.vue";
import ImportSelectFile from "./ImportSelectFile.vue";
import { columnsImport } from "@/js/data-table/employeeColumns";
export default {
  name: "MImportExcel",
  components: {
    ImportCheckData,
    ImportResult,
    ImportSelectFile,
  },
  props: {},
  emits: ["toggleShowImportExcel", "loadData"],
  created() {},
  methods: {
    /**
     * Nhảy về trang trước
     * Author: LQHUY(20/1/2024)
     */
    onClickPrevPage() {
      this.nextPage--;
    },
    /**
     * Nhảy đến trang sau
     * Author: LQHUY(20/1/2024)
     */
    onClickNextPage() {
      if (
        this.fileImport == null ||
        this.fileImport == "" ||
        this.fileImport == undefined
      ) {
        this.$emitter.emit(
          "toggleDialogNotice",
          false,
          true,
          this.$Resource[this.$languageCode].WanrningMessage,
          this.$Resource[this.$languageCode].ExcelImport.FileNotEmpty,
          this.$Resource[this.$languageCode].Dialog.Type.Warning
        );
      } else {
        this.nextPage++;
      }
    },
    checkDataImport() {
      if (this.checkFileImportIsEmpty()) {
        return;
      } else {
        this.nextPage = 1;
      }
    },
    resultImport() {
      if (this.checkFileImportIsEmpty()) {
        return;
      } else {
        this.nextPage = 2;
      }
    },
    checkFileImportIsEmpty() {
      if (
        this.fileImport == null ||
        this.fileImport == "" ||
        this.fileImport == undefined
      ) {
        this.$emitter.emit(
          "toggleDialogNotice",
          false,
          true,
          this.$Resource[this.$languageCode].WanrningMessage,
          this.$Resource[this.$languageCode].ExcelImport.FileNotEmpty,
          this.$Resource[this.$languageCode].Dialog.Type.Warning
        );
        return true;
      } else {
        return false;
      }
    },
    /**
     * Hủy khi hoàn tất hoặc bỏ
     * Author: LQHUY(20/1/2024)
     */
    onClickCancelImport() {
      this.$emit("toggleShowImportExcel", false);
      this.$emit("loadData");
    },
  },
  watch: {
    fileImport(value){
      console.log(value)
    }
  },
  data() {
    return {
      nextPage: 0,
      fileImport: null,
      columnsTable: columnsImport,
      countImportSuccess: null,
      countImportFailure: null,
      cacheKey: null,
      directionPage: false,
      cacheKeyResult: null,
    };
  },
};
</script>
<style scoped>
#import-excel {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  z-index: 10;
  background-color: rgba(0, 0, 0, 0.5);
}
.warrap-excel {
  padding: 0 16px;
  width: 80%;
  height: 80vh;
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  z-index: 10;
}
.import__header {
  height: 48px;
  line-height: 48px;
  font-size: 14px;
  font-weight: 600;
  font-size: 14px;
}
.import__container {
  display: flex;
  height: calc(100% - 48px - 48px);
}
.import__sidebar {
  width: 200px;
  background-color: #eeeff1;
  height: 100%;
  flex-shrink: 0;
  margin-right: 12px;
}
.import__sidebar .nav__list-item-import {
  list-style: none;
  text-decoration: none;
  display: block;
  padding: 0 16px;
  height: 38px;
  line-height: 38px;
  font-weight: 600;
  font-size: 14px;
  color: #000;
  border: 1px solid #e0e0e0;
}
.nav__list-item-import--selected {
  background-color: #98c1e3;
}
.nav__list-item-import.router-link-active {
  background-color: #98c1e3;
}
.import__container .import__content {
  border: 1px solid #e0e0e0;
  width: calc(100% - 200px);
  height: 100%;
  padding: 8px;
}
.import__footer {
  height: 48px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.import__footer-rigth {
  display: flex;
}
</style>
