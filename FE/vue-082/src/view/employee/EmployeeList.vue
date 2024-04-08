<template>
  <div class="content__header">
    <div class="content__header-title">
      <h3>{{ this.$Resource[this.$languageCode].Text.Employee }}</h3>
    </div>

    <div class="content__header-group-button">
      <tippy
        :content="this.$Resource[this.$languageCode].Text.AddNew"
        placement="bottom"
        animation="scale"
      >
        <MButton
          class="m-button-icon"
          icon="fa-solid fa-plus"
          @:click="onBtnAddEmployee"
          id="btn-add-employee"
          :text="this.$Resource[this.$languageCode].Text.AddNew"
          style="font-weight: 600"
        >
        </MButton>
      </tippy>
    </div>
  </div>

  <div class="content__body">
    <div class="content__toolbar">
      <div class="content__toolbar-left">
        <div
          v-show="isShowToolbarAction"
          class="content__toolbar-left--action"
          style="display: flex"
        >
          <p class="content__toolbar-left--selected">
            {{ this.$Resource[this.$languageCode].Text.Selected }}
            <b class="content__toolbar-left--selected-quantity">{{
              totalRecordSelected
            }}</b>
          </p>
          <MButton
            class="m-button--sub m-button-none"
            id="m-button-uncheked"
            :text="this.$Resource[this.$languageCode].Text.Removed"
            @click="btnRemoveRowSelected"
          >
          </MButton>
          <MButton
            icon="fa-solid fa-trash"
            class="m-button-icon m-button--sub m-button-none"
            id="btn-delete-all"
            :text="this.$Resource[this.$languageCode].Text.RemoveAll"
            @click="btnDeleteAllEmployee"
          >
          </MButton>
        </div>
      </div>
      <div class="content__toolbar-right">
        <div class="content__toolbar-search">
          <MInputIcon
            refEl="txtSearchString"
            v-model="searchString"
            :placeholder="this.$Resource[this.$languageCode].Text.SearchField"
          ></MInputIcon>
        </div>
        <MButtonNoText
          v-tippy="{
            content: this.$Resource[this.$languageCode].Text.Excel,
            placement: 'bottom',
          }"
          @click="
            (e) => {
              this.isShowActionListExcel = true;
              e.stopPropagation();
            }
          "
          icon="m-icon-ecxel"
        >
          <div class="warrap-excel" v-show="isShowActionListExcel">
            <ul class="excel__list">
              <a>
                <li @click="importAllRecord" class="excel__list-item">
                  {{ this.$Resource[this.$languageCode].Text.Import }}
                </li>
              </a>
              <a>
                <li @click="onClickExportAllRecord" class="excel__list-item">
                  {{ this.$Resource[this.$languageCode].Text.ExportAll }}
                </li>
              </a>
              <a>
                <li @click="onClickExportListRecord" class="excel__list-item">
                  {{ this.$Resource[this.$languageCode].Text.ExportList }}
                </li>
              </a>
            </ul>
          </div>
        </MButtonNoText>

        <MButtonNoText
          :class="{ active: this.isShowSetting }"
          v-tippy="{
            content: this.$Resource[this.$languageCode].Text.Setting,
            placement: 'bottom',
          }"
          icon="m-icon-setting"
          @click="
            (e) => {
              this.isShowSetting = true;
              e.stopPropagation();
            }
          "
        >
          <div v-if="isShowSetting" class="warrap-setting">
            <div class="setting__header">
              <div class="setting__header-tilte">
                {{ this.$Resource[this.$languageCode].Text.ChangeColumns }}
              </div>
              <div
                @click="
                  (e) => {
                    this.isShowSetting = false;
                    e.stopPropagation();
                  }
                "
                class="setting__header-close"
              >
                <i class="fa-solid fa-xmark"></i>
              </div>
            </div>
            <div class="setting__search-column">
              <MInputIcon
                refEl="txtSearchField"
                v-model="searchField"
                :placeholder="this.$Resource[this.$languageCode].Text.Search"
              ></MInputIcon>
            </div>
            <div class="setting__body">
              <div class="setting__list-field">
                <div
                  v-for="(col, index) in this.fieldsTable"
                  :key="index"
                  class="setting__list-field-item"
                  @click="onClickRowField(col, index)"
                >
                  <div class="setting__list-field-item--checbox">
                    <input
                      type="checkbox"
                      :value="col.nameField"
                      v-model="lstColumnDisplay"
                    />
                  </div>
                  <div class="setting__list-field-item--name">
                    {{ col.nameField }}
                  </div>
                  <div class="setting__list-field-item--pin">
                    <i class="fa-solid fa-thumbtack"></i>
                  </div>
                </div>
              </div>
            </div>
            <div class="setting__footer">
              <MButton
                @click="onClickResetField"
                class="m-button--sub"
                :text="this.$Resource[this.$languageCode].Text.DefaultSetting"
              ></MButton>
              <MButton @click="onSaveChangeColumns" text="Lưu"> </MButton>
            </div>
          </div>
        </MButtonNoText>
        <MButtonNoText
          v-tippy="{
            content: this.$Resource[this.$languageCode].Text.RefreshData,
            placement: 'bottom',
          }"
          @click="this.loadData"
          icon="m-icon-reload"
        >
        </MButtonNoText>
      </div>
    </div>

    <!-- Table -->

    <MTable
      id="tblEmployee"
      :columnsTable="columnsTable"
      idObject="EmployeeId"
      codeObject="EmployeeCode"
      :pageData="pageData"
      :selectAll="selectAll"
      @toggleShowForm="toggleShowForm"
      @updateItemId="updateItemId"
      @updateListItemId="updateListItemId"
      @updateItemIdClone="updateItemIdClone"
      @toggleShowToolbarAction="toggleShowToolbarAction"
      @updateTotalRecordSelected="updateTotalRecordSelected"
    >
    </MTable>

    <!-- Pagination -->
    <MPagination
      :total="totalRecord"
      :totalPage="totalPage"
      v-model:pageNumber="pageNumber"
      v-model:pageSize="pageSize"
    ></MPagination>
  </div>

  <EmployeeDetail
    v-if="showForm"
    :selectedEmployeeId="selectedEmployeeId"
    :selectedEmployeeIdClone="selectedEmployeeIdClone"
    @loadData="loadData"
  >
  </EmployeeDetail>

  <MImportExcel
    v-if="isShowImportExcel"
    @toggleShowImportExcel="toggleShowImportExcel"
    :importLink="importLink"
    @loadData="loadData"
  ></MImportExcel>
</template>
<script>
import EmployeeDetail from "./EmployeeDetail.vue";
import { columns } from "../../js/data-table/employeeColumns.js";
import { saveAs } from "file-saver";
import { checkExpiredTokenTime } from "../../utils/token.js";
export default {
  name: "EmployeeList",
  async created() {
    await this.loadData();
    this.fieldsTable = [...this.columnsTable];
    this.asignColumnsTable = [...this.columnsTable];
    localStorage.setItem(
      "columnsDefault",
      JSON.stringify([...this.columnsTable])
    );
    this.$emitter.on("deleteEmployee", this.deleteEmployee);
    this.$emitter.on("deleteEmployees", this.deleteEmployees);
    this.$emitter.on("toggleShowForm", this.toggleShowForm);
    this.$emitter.on("updatePageSize", this.updatePageSize);
  },
  mounted() {
    /**
     * Ẩn menu excel hoặc menu setting khi click không phải vào thành phần chính nó
     * Author: LQHUY(10/01/2024)
     */
    document.addEventListener("click", (e) => {
      if (!e.target.closest(".warrap-excel")) {
        this.isShowActionListExcel = false;
      }
      if (!e.target.closest(".warrap-setting")) {
        this.isShowSetting = false;
      }
    });
    this.$emitter.emit("toggleShowLoadingTable", true);
  },
  components: {
    EmployeeDetail,
  },
  beforeUnmount() {
    this.$emitter.off("deleteEmployee", this.deleteEmployee);
    this.$emitter.off("deleteEmployees", this.deleteEmployees);
    this.$emitter.off("toggleShowForm", this.toggleShowForm);
    this.$emitter.off("updatePageSize", this.updatePageSize);
  },
  computed: {
    /**
     * Lấy và set dữ liệu cho checkbox setting
     * Author: LQHUY(10/1/2024)
     */
    lstColumnDisplay: {
      /**
       * Lấy giá trị để hiển thị
       * Author: LQHUY(10/1/2024)
       */
      get: function () {
        return this.fieldsTable
          ? this.fieldsTable.map((item) => {
              return this.columnsTable
                .map((item) => item.nameField)
                .includes(item.nameField)
                ? item.nameField
                : "";
            })
          : [];
      },
      /**
       * Set lại giá trị
       * Author: LQHUY(10/1/2024)
       */
      set: function (value) {
        if (value && this.columnsTable != null) {
          //console.log(value)
        } else {
          console.log("value");
        }
      },
    },
  },

  methods: {
    /**
     * Lấy dữ liệu từ API
     * Author: LQHUY (5/12/2023)
     */
    async loadData() {
      await checkExpiredTokenTime(this.$emitter);
      const token = JSON.parse(localStorage.getItem("Token"));
      this.$emitter.emit("toggleShowLoadingTable", true);
      if (token) {
        try {
          let params = {};
          params = !this.searchString
            ? {
                pageSize: this.pageSize,
                pageNumber: this.pageNumber,
              }
            : {
                pageSize: this.pageSize,
                pageNumber: this.pageNumber,
                searchString: this.searchString,
              };
          const res = await this.$httpRequest.getAuthorization(
            "Employees/Filter",
            {
              params,
            }
          );
          switch (res.status) {
            case 200:
            case 201:
              this.pageData = res.data.Data;
              this.totalRecord = res.data.TotalRecord;
              this.totalPage = res.data.TotalPage;
              this.$emitter.emit("toggleShowLoadingTable", false);
              break;
            default:
              break;
          }
        } catch (error) {
          this.$emitter.emit("handleApiError", error);
          this.$emitter.emit("toggleShowLoading", false);
        }
      }
      return;
    },

    /**
     * Hàm click nút thêm mới
     * Author: LQHUY (8/12/2023)
     */
    onBtnAddEmployee() {
      this.selectedEmployeeId = null;
      this.selectedEmployeeIdClone = null;
      this.toggleShowForm(true);
    },

    /**
     * Hàm ẩn hiện form chi tiết khách hàng
     * @param {Boolean} isShow
     * Author: LQHUY (5/12/2023)
     */
    toggleShowForm(isShow) {
      this.showForm = isShow;
    },

    /**
     * Hàm ẩn hiện toolbar
     * @param {Boolean} isShow
     * Author: LQHUY (5/12/2023)
     */
    toggleShowToolbarAction(isShow) {
      this.isShowToolbarAction = isShow;
    },

    /**
     * Cập nhật lại tổng số lượng bản ghi đã chọn
     * Author: LQHUY (5/12/2023)
     */
    updateTotalRecordSelected(total) {
      this.totalRecordSelected = total;
    },

    /**
     * Hàm gỡ bỏ tất cả những dòng được chọn
     * Author: LQHUY (5/12/2023)
     */
    btnRemoveRowSelected() {
      this.selectAll = false;
      setTimeout(() => {
        this.selectAll = null;
      }, 500);
    },

    /**
     * Hàm thực hiện xóa một nhân viên
     * Author: LQHUY (7/12/2023)
     */
    async deleteEmployee() {
      try {
        if (this.selectedEmployeeId && this.lstIdDelete.length == 0) {
          const res = await this.$httpRequest.remove(
            `Employees/${this.selectedEmployeeId}`
          );
          switch (res.status) {
            case 200:
              this.$emitter.emit("toggleDialogNotice", false);
              this.$emitter.emit(
                "onShowToastMessage",
                this.$Resource[this.$languageCode].ToastMessage.Type.Success,
                this.$Resource[this.$languageCode].DeleteSuccess,
                this.$Resource[this.$languageCode].ToastMessage.Status.Success
              );
              this.pageNumber = 1;
              this.loadData();
              break;
            default:
              break;
          }
        }
      } catch (error) {
        this.$emitter.emit("handleApiError", error);
      }
    },

    /**
     * Hiển thị dialog xác nhận xóa tất cả
     * Author: LQHUY (12/12/2023)
     */
    btnDeleteAllEmployee() {
      this.$emitter.emit(
        "toggleDialogNotice",
        true,
        true,
        this.$Resource[this.$languageCode].DeleteConfirm,
        this.$Resource[this.$languageCode].ConfirmDeleteAll(
          this.$Resource[this.$languageCode].Employee
        ),
        this.$Resource[this.$languageCode].Dialog.Type.Question
      );
    },

    /**
     * Hàm xóa tất cả dữ liệu những dòng đã chọn
     * Author: LQHUY (12/12/2023)
     */
    async deleteEmployees() {
      try {
        if (this.lstIdDelete.length === 0) {
          return;
        }
        this.$emitter.emit("toggleDialogNotice", false);
        const res = await this.$httpRequest.remove(`Employees/DeleteMany`, {
          data: this.lstIdDelete,
        });
        switch (res.status) {
          case 200:
            this.pageNumber = 1;
            this.$emitter.emit("toggleDialogNotice", false);
            this.$emitter.emit(
              "onShowToastMessage",
              this.$Resource[this.$languageCode].ToastMessage.Type.Success,
              this.$Resource[this.$languageCode].DeleteSuccess,
              this.$Resource[this.$languageCode].ToastMessage.Status.Success
            );
            this.loadData();
            break;
          default:
            break;
        }
        this.btnRemoveRowSelected();
      } catch (error) {
        this.$emitter.emit("handleApiError", error);
      }
    },

    /**
     * Hàm cập nhật số lượng bản ghi trong 1 trang
     * @param {int} pageSize
     * Author: LQHUY (8/12/2023)
     */
    updatePageSize(pageSize) {
      this.pageSize = pageSize;
    },

    /**
     * Hàm cập nhật lại id được chon
     * @param {string} id
     * Author: LQHUY (10/12/2023)
     */
    updateItemId(id) {
      this.selectedEmployeeId = id;
    },

    /**
     * Hàm cập nhật lại id để nhân bản
     * @param {string} id
     * Author: LQHUY (10/12/2023)
     */
    updateItemIdClone(id) {
      this.selectedEmployeeIdClone = id;
    },

    /**
     * Hàm cập nhật lại danh sách các id được chon
     * @param {Array} id
     * Author: LQHUY (10/12/2023)
     */
    updateListItemId(ids) {
      this.lstIdDelete = ids;
    },

    /**
     * Hàm gỡ bỏ nhưng trường dữ liệu không muốn hiển thị trên table
     * @param {string} col
     * @param {int} index
     * Author: LQHUY(10/01/2024)
     */
    onClickRowField(col, index) {
      try {
        if (this.lstColumnDisplay.indexOf(col.nameField) === -1) {
          this.asignColumnsTable.splice(index, 0, col);
        } else {
          this.asignColumnsTable = this.asignColumnsTable.filter(
            (item) => item.nameField != col.nameField
          );
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Hàm lưu lại những trường dữ liệu không muốn hiển thị trên table
     * Author: LQHUY(10/01/2024)
     */
    onSaveChangeColumns() {
      this.columnsTable = [...this.asignColumnsTable];
      this.loadData();
      this.isShowSetting = false;
      event.stopPropagation();
    },

    /**
     * Đặt lại những trường dữ liệu như ban đầu
     * Author: LQHUY(10/01/2024)
     */
    onClickResetField() {
      const cols = JSON.parse(localStorage.getItem("columnsDefault"));
      this.columnsTable = [...cols];
      this.loadData();
      this.isShowSetting = false;
      event.stopPropagation();
    },

    /**
     * Hàm thực hiện Export tất cả các bản ghi vào file excel
     * Author: LQHUY(10/01/2024)
     */
    async onClickExportAllRecord() {
      try {
        this.$emitter.emit("toggleShowLoading", true);
        let res;

        res = await this.$httpRequest.getExcel("Employees/Export", []);
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

    /**
     * Hàm thực hiện Export danh sách các bản ghi vào file excel
     * Author: LQHUY(10/01/2024)
     */
    async onClickExportListRecord() {
      try {
        let res;
        if (this.lstIdDelete.length === 0) {
          this.$emitter.emit(
            "toggleDialogNotice",
            false,
            true,
            this.$Resource[this.$languageCode].Warning,
            this.$Resource[this.$languageCode].ListIdsDeleteEmpty,
            this.$Resource[this.$languageCode].Dialog.Type.Warning
          );
          return;
        } else {
          this.$emitter.emit("toggleShowLoading", true);
          res = await this.$httpRequest.getExcel(
            "Employees/Export",
            this.lstIdDelete
          );
        }

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

    /**
     * Import dữ liệu từ file excel
     * Author: LQHUY(16/01/2024)
     */
    importAllRecord() {
      this.isShowImportExcel = true;
    },

    /**
     * Hiển thị màn import
     * @param {Boolean} isShow
     * Author: LQHUY(16/01/2024)
     */
    toggleShowImportExcel(isShow) {
      this.isShowImportExcel = isShow;
      this.isShowActionListExcel = false;
    },
  },

  watch: {
    /**
     * Theo dõi số bản ghi trong 1 trang
     * @param {int} newValue
     * Author: LQHUY(07/01/2024)
     */
    pageSize(newValue) {
      if (newValue) {
        this.loadData();
        //localStorage.setItem("pageNumber", JSON.stringify(this.pageNumber));
      }
    },

    /**
     * Theo dõi số trang hiện tại
     * @param {int} newValue
     * Author: LQHUY(07/01/2024)
     */
    pageNumber(newValue) {
      if (newValue) {
        this.loadData();
        //localStorage.setItem("pageNumber", JSON.stringify(this.pageNumber));
      }
    },

    /**
     * Theo dõi chuỗi tìm kiếm các bản ghi
     * Author: LQHUY(07/01/2024)
     */
    searchString() {
      this.loadData();
    },
  },
  provide() {
    return {
      pageSize: 10,
      importLink: {
        urlImport: "Employees/Import",
        urlReadData: "Employees/ReadDataFromExcel",
        urlTemplateFile: "Employees/TemplateFile",
        urlFileImportError: "Employees/FileImportError",
        urlFileImportResult: "Employees/FileImportResult",
      },
    };
  },
  data() {
    return {
      /**Các biển ẩn hiện form, toolbar,setting,action excel */
      showForm: false,
      isShowToolbarAction: false,
      isShowSetting: false,
      isShowActionListExcel: false,
      isShowImportExcel: false,

      /**tổng số bản ghi được chon */
      totalRecordSelected: null,

      /**id dùng để edit và clone */
      selectedEmployeeId: null,
      selectedEmployeeIdClone: null,

      /**danh sách id để xóa */
      lstIdDelete: [],

      /**chuỗi tìm kiếm nhanh các bản ghi */
      searchString: null,

      /**biến dùng cho paging */
      totalRecord: null,
      totalPage: null,
      pageSize: 10,
      pageNumber: 1,

      /**biến dùng cho table */
      selectAll: null,
      columnsTable: columns,
      pageData: [],

      /**dùng cho setting */
      searchField: null,
      fieldsTable: [],
      asignColumnsTable: [],
    };
  },
};
</script>
<style scoped></style>
