<template>
  <div class="content__header">
    <div class="content__header-title">
      <h3>Khách hàng</h3>
    </div>

    <div class="content__header-group-button">
      <tippy content="Thêm mới" placement="bottom" animation="scale">
        <MButton
          @:click="onBtnAddCustomer"
          class="m-button-icon"
          id="btn-add-customer"
          text="Thêm mới"
          icon="fa-solid fa-plus"
        >
        </MButton>
      </tippy>
      <MButtonNoText icon="fa-solid fa-ellipsis"> </MButtonNoText>
    </div>
  </div>

  <div class="content__body">
    <div class="content__toolbar">
      <div class="content__toolbar-left">
        <p class="content__toolbar-left--selected">
          Đã chọn
          <b class="content__toolbar-left--selected-quantity">{{
            this.totalRecordSelected
          }}</b>
        </p>

        <div
          v-show="isShowToolbarAction"
          class="content__toolbar-left--action"
          style="display: flex"
        >
          <MButton
            class="m-button--sub m-button-none"
            id="m-button-uncheked"
            text="Bỏ chọn"
            @click="btnRemoveRowSelected"
          >
          </MButton>
          <MButton
            icon="fa-solid fa-trash"
            class="m-button-icon m-button--sub m-button-none"
            id="btn-delete-all"
            text="Xóa tất cả"
            @click="btnDeleteAllCustomer"
          >
          </MButton>
        </div>
      </div>
      <div class="content__toolbar-right">
        <div class="content__toolbar-search">
          <MInputIcon
            refEl="txtSearchString"
            v-model="searchString"
            placeholder="Tìm theo mã, tên nhân viên"
          ></MInputIcon>
        </div>
        <tippy content="Xuất khẩu" placement="bottom">
          <MButtonNoText
            @click="
              (e) => {
                this.isShowActionListExcel = true;
                e.stopPropagation();
              }
            "
            icon="fa-solid fa-file-export"
          >
            <div class="warrap-excel" v-show="isShowActionListExcel">
              <ul class="excel__list">
                <a>
                  <li @click="importAllRecord" class="excel__list-item">
                    Nhập khẩu
                  </li>
                </a>
                <a>
                  <li @click="onClickExportAllRecord" class="excel__list-item">
                    Xuất khẩu toàn bộ
                  </li>
                </a>
                <a>
                  <li @click="onClickExportListRecord" class="excel__list-item">
                    Xuất khẩu theo danh sách
                  </li>
                </a>
              </ul>
            </div>
          </MButtonNoText>
        </tippy>
        <MButtonNoText
          :class="{ active: this.isShowSetting }"
          v-tippy="{ content: 'Cài đặt', placement: 'bottom' }"
          icon="fa-solid fa-gear"
          @click="onShowSetting"
        >
          <div
            v-if="isShowSetting"
            class="warrap-setting"
            @click="
              (e) => {
                e.stopPropagation();
              }
            "
          >
            <div class="setting__header">
              <div class="setting__header-tilte">Tùy chỉnh cột</div>
              <div @click="onHideSetting" class="setting__header-close">
                <i class="fa-solid fa-xmark"></i>
              </div>
            </div>
            <MInput
              class="setting__search"
              v-model="searchField"
              placeholder="Tìm kiếm"
            ></MInput>
            <div class="setting__body">
              <div class="setting__list-field">
                <div
                  v-for="(filed, index) in this.fileds"
                  :key="index"
                  class="setting__list-field-item"
                >
                  <div class="setting__list-field-item--checbox">
                    <input type="checkbox" v-model="isShowField[index]" />
                  </div>
                  <div class="setting__list-field-item--name">{{ filed }}</div>
                  <div
                    :ref="lstPinField[index]"
                    @click="onClickPinField(index)"
                    class="setting__list-field-item--pin"
                  >
                    <i class="fa-solid fa-thumbtack"></i>
                  </div>
                </div>
              </div>
            </div>
            <div class="setting__footer">
              <MButton
                @click="resetDisplayFieldOnTable"
                class="m-button--sub"
                text="Đặt lại mặc định"
              ></MButton>
              <MButton text="Lưu"> </MButton>
            </div>
          </div>
        </MButtonNoText>
        <tippy content="Load lại dữ liệu" placement="bottom">
          <MButtonNoText @click="this.loadData" icon="fa-solid fa-rotate">
          </MButtonNoText>
        </tippy>
      </div>
    </div>
    <!-- Table -->

    <MTable
      id="tblCustomer"
      :columnsTable="columnsTable"
      idObject="CustomerId"
      :pageData="pageData"
      :isShowToolbarAction="isShowToolbarAction"
      :selectAll="selectAll"
      @getItemId="getItemId"
      @toggleShowForm="toggleShowForm"
      @toggleShowToolbarAction="toggleShowToolbarAction"
      @updateTotalRecordSelected="updateTotalRecordSelected"
      @getListItemId="getListItemId"
    >
    </MTable>

    <!-- Pagination -->
    <MPagination
      :total="totalCustomer"
      :totalPage="totalPage"
      v-model:pageNumber="pageNumber"
      v-model:pageSize="pageSize"
    ></MPagination>
  </div>

  <CustomerDetail
    v-if="showForm"
    :selectedCustomerId="selectedCustomerId"
    @loadData="loadData"
  >
  </CustomerDetail>
</template>
<script>
import MInput from "@/components/base/input/MInput.vue";
import CustomerDetail from "./CustomerDetail.vue";
import { columns } from "../../js/data-table/customerColums.js";
export default {
  name: "CustomerList",
  created() {
    this.loadData();
    this.$emitter.on("deleteCustomer", this.deleteCustomer);
    this.$emitter.on("deleteCustomers", this.deleteCustomers);
    this.$emitter.on("toggleShowForm", this.toggleShowForm);
    this.$emitter.on("updatePageSize", this.updatePageSize);
    this.pageNumber = JSON.parse(localStorage.getItem("pageNumber"));
  },
  components: {
    CustomerDetail,
    MInput,
  },
  beforeUnmount() {
    this.$emitter.off("deleteCustomer", this.deleteCustomer);
    this.$emitter.off("deleteCustomers", this.deleteCustomers);
    this.$emitter.off("toggleShowForm", this.toggleShowForm);
    this.$emitter.off("updatePageSize", this.updatePageSize);
  },
  methods: {
    /**
     * Lấy dữ liệu từ API
     * Author: LQHUY (5/12/2023)
     */
    async loadData() {
      this.$emitter.emit("toggleShowLoading", true);
      try {
        let params = {};
        params =
          this.searchString == null || undefined || ""
            ? {
                pageSize: this.pageSize,
                pageNumber: this.pageNumber,
              }
            : {
                pageSize: this.pageSize,
                pageNumber: this.pageNumber,
                searchString: this.searchString,
              };
        const res = await this.$httpRequest.get("Customers/Filter", {
          params,
        });
        switch (res.status) {
          case 200:
          case 201:
            this.customers = res.data.Data;
            this.totalCustomer = res.data.TotalRecord;
            this.totalPage = res.data.TotalPage;
            this.pageData = this.customers;
            this.$emitter.emit("toggleShowLoading", false);
            break;
          default:
            break;
        }
      } catch (error) {
        console.log(error);
        this.$emitter.emit("handleApiError", error);
      }
    },
    /**
     * Hàm click nút thêm mới
     * Author: LQHUY (8/12/2023)
     */
    onBtnAddCustomer() {
      this.toggleShowForm(true);
    },
    toggleShowToolbarAction(isShow) {
      this.isShowToolbarAction = isShow;
    },
    updateTotalRecordSelected(total) {
      this.totalRecordSelected = total;
    },
    getListItemId(ids) {
      this.lstIdDelete = ids;
    },
    /**
     * Hàm ẩn hiện form chi tiết khách hàng
     * Author: LQHUY (5/12/2023)
     */
    toggleShowForm(isShow) {
      this.showForm = isShow;
      this.selectedCustomerId = null;
    },

    /**
     * Hàm thực hiện xóa một khách hàng
     * Author: LQHUY (7/12/2023)
     */
    async deleteCustomer() {
      if (this.selectedCustomerId) {
        const res = await this.$httpRequest.remove(
          `Customers/${this.selectedCustomerId}`
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
    },

    /**
     * Hàm click xóa tất cả hiển thị dialog
     * Author: LQHUY (12/12/2023)
     */
    btnDeleteAllCustomer() {
      this.$emitter.emit(
        "toggleDialogNotice",
        true,
        true,
        this.$Resource[this.$languageCode].DeleteConfirm,
        this.$Resource[this.$languageCode].ConfirmDeleteAll(
          this.$Resource[this.$languageCode].Customer
        ),
        this.$Resource[this.$languageCode].Dialog.Type.Question
      );
    },

    /**
     * Hàm xóa tất cả dữ liệu những dòng đã chọn
     * Author: LQHUY (12/12/2023)
     */
    async deleteCustomers() {
      if (this.lstIdDelete.length === 0) {
        return;
      }
      this.$emitter.emit("toggleDialogNotice", false);
      const res = await this.$httpRequest.remove(`Customers/DeleteMany`, {
        data: this.lstIdDelete,
      });
      switch (res.status) {
        case 200:
          this.$emitter.emit("toggleDialogNotice", false);
          this.$emitter.emit(
            "onShowToastMessage",
            this.$Resource[this.$languageCode].ToastMessage.Type.Success,
            this.$Resource[this.$languageCode].DeleteSuccess,
            this.$Resource[this.$languageCode].ToastMessage.Status.Success
          );
          this.loadData();
          this.pageNumber = 1;
          break;
        default:
          break;
      }
      this.btnRemoveRowSelected();
    },
    updatePageSize(pageSize) {
      this.pageSize = pageSize;
    },
    onShowSetting() {
      this.isShowSetting = true;
    },
    onHideSetting() {
      this.isShowSetting = false;
    },
    onClickPinField(index) {
      if (this.lstPinField[index] === true) {
        this.$refs["pinField"].style.color = "#50B83C";
      } else {
        this.$refs["pinField"].style.color = "#aeadad";
      }
    },
    resetDisplayFieldOnTable() {
      this.isShowField = JSON.parse(localStorage.getItem("lstFieldDislay"));
      this.isShowSetting = false;
      this.$emitter.emit("toggleShowLoading", true);
      setTimeout(() => {
        this.$emitter.emit("toggleShowLoading", false);
      }, 300);
    },
    importAllRecord() {
      this.$router.push("import/excel");
    },
    getItemId(id) {
      this.selectedCustomerId = id;
    },
    btnRemoveRowSelected() {
      this.selectAll = false;
      setTimeout(() => {
        this.selectAll = null;
      }, 500);
    },
  },

  watch: {
    /**
     * Theo dõi giá trị thay đổi của input checkbox table header và chọn tất cả
     * @param {boolean} value
     */
    // isCheckedAllRow(value) {
    //   //cập nhật các mảng sử dụng map để đặt tất cả các phần tử thành value
    //   this.lstCheckbox = this.customers.map(() => value);
    //   this.lstRowTable = this.customers.map(() => value);
    // },
    pageSize(newValue) {
      if (newValue) {
        this.loadData();
        this.pageNumber = 1;
        localStorage.setItem("pageNumber", JSON.stringify(this.pageNumber));
      }
    },
    pageNumber(newValue) {
      if (newValue) {
        this.loadData();
        localStorage.setItem("pageNumber", JSON.stringify(this.pageNumber));
      }
    },
    searchString() {
      setTimeout(() => {
        this.loadData();
      }, 1000);
    },
    updateTotalRecordSelected(total) {
      this.totalRecordSelected = total;
    },
  },
  provide() {
    return {
      pageSize: 10,
    };
  },
  data() {
    return {
      showForm: false,
      isShowToolbarAction: false,
      isShowSetting: false,
      isShowActionListExcel: false,

      totalCustomer: null,
      customers: [],
      customerGroups: [],
      totalRecordSelected: null,

      selectedCustomerId: null,
      lstIdDelete: [],

      totalPage: null,
      searchString: null,
      pageSize: 10,
      pageNumber: 1,

      columnsTable: columns,
      selectAll: null,
      pageData: [],

      searchField: null,
    };
  },
};
</script>
<style scoped></style>
