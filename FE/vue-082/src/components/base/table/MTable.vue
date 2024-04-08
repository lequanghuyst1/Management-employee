<template>
  <div class="m-grid">
    <table class="m-table" :id="id">
      <thead>
        <tr>
          <th class="" style="max-width: 50px; text-align: center">
            <input type="checkbox" v-model="selectAllRecord" />
          </th>
          <th
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
        </tr>
      </thead>
      <tbody v-if="isShowLoadingTable == false">
        <tr
          v-for="data in pageData"
          :key="data.idObject"
          :class="{ selected: lstDelete.indexOf(data[idObject]) >= 0 }"
          class="m-table__row"
          @dblclick="
            (event) => {
              onDblClickRowTable(data, event);
            }
          "
          @mouseleave="this.isShowActionMenu = false"
        >
          <td
            style="text-align: center"
            :class="{ selected: lstDelete.indexOf(data[idObject]) >= 0 }"
          >
            <input
              :value="data[idObject]"
              @click="onClickCheckbox(data)"
              v-model="lstDelete"
              ref="inputChecked"
              type="checkbox"
              @dblclick="
                (e) => {
                  e.stopPropagation();
                }
              "
            />
          </td>
          <td
            v-for="(col, index) in this.columnsTable"
            :key="index"
            :style="{
              textAlign: col.textAlign || 'left',
            }"
          >
            <tippy :content="this.setValueData(data, col)" placement="top">
              {{ this.setValueData(data, col) }}
            </tippy>
          </td>
          <div class="m-table__action">
            <div
              v-tippy="{
                content: this.$Resource[this.$languageCode].Text.Edit,
                placement: 'bottom',
              }"
              @click="
                (e) => {
                  onClickEdit(data[this.idObject], e);
                }
              "
              :style="{
                right: isShowActionMenu ? '145px' : '55px',
              }"
              class="m-table__action-item m-table__action-item--edit"
            >
              <div class="m-icon-edit"></div>
            </div>

            <div
              v-tippy="{
                content: this.$Resource[this.$languageCode].Text.Open,
                placement: 'bottom',
              }"
              @click="
                (e) => {
                  onClickIconShowMenuAction(e);
                }
              "
              @dblclick="
                (e) => {
                  e.stopPropagation();
                }
              "
              class="m-table__action-item m-table__action-item--many"
            >
              <i class="fa-solid fa-ellipsis"></i>
            </div>
            <div
              v-if="isShowActionMenu"
              v-tippy="{
                content: this.$Resource[this.$languageCode].Text.Delete,
                placement: 'bottom',
              }"
              @click="
                (e) => {
                  onClickIconDelete(data, e);
                }
              "
              class="m-table__action-item m-table__action-item--delete"
            >
              <div class="m-icon-delete"></div>
            </div>
            <div
              v-if="isShowActionMenu"
              v-tippy="{
                content: this.$Resource[this.$languageCode].Text.Clone,
                placement: 'bottom',
              }"
              @click="
                (e) => {
                  onClickIconClone(data, e);
                }
              "
              class="m-table__action-item m-table__action-item--clone"
            >
              <div class="m-icon-clone"></div>
            </div>
          </div>
        </tr>
      </tbody>
    </table>
    <MLoadingData v-if="isShowLoadingTable"></MLoadingData>

    <div
      class="no-data-table"
      v-if="isShowNoData && this.pageData.length === 0"
    >
      {{ this.$Resource[this.$languageCode].Text.NoDataContent }}
    </div>
  </div>
</template>
<script>
export default {
  name: "MTable",
  props: {
    columnsTable: {
      typeof: Array,
      required: true,
    },
    pageData: {
      typeof: Array,
      required: true,
    },
    idObject: {
      typeof: String,
      required: true,
    },
    codeObject: {
      typeof: String,
      required: true,
    },
    id: {
      typeof: String,
      required: true,
    },
    selectAll: {
      typeof: Boolean,
      required: false,
    },
  },
  emit: [
    "toggleShowForm",
    "updateItemId",
    "updateListItemId",
    "updateItemIdClone",
    "toggleShowToolbarAction",
    "updateTotalRecordSelected",
  ],
  created() {
    this.$emitter.on("removeRowSelectedDelete", this.removeRowSelectedDelete);
    this.$emitter.on("toggleShowLoadingTable", this.toggleShowLoadingTable);
  },
  mounted() {
    this.columnsResizable();
  },
  beforeUnmount() {
    this.$emitter.off("removeRowSelectedDelete", this.removeRowSelectedDelete);
    this.$emitter.off("toggleShowLoadingTable", this.toggleShowLoadingTable);
  },
  watch: {
    /**
     * Gỡ bỏ chọn tất cả
     * @param {boolean} val
     * Author: LQHUY(06/12/2023)
     */
    selectAll(val) {
      if (val === false) {
        this.selectAllRecord = val;
      }
    },
    /**
     * Theo dõi danh sách id
     * Author: LQHUY(06/12/2023)
     */
    lstDelete() {
      this.$emit("updateListItemId", this.lstDelete);
    },

    /**
     * Hiện không thấy dữ liệu khi pageData không phù hợp
     * Author: LQHUY(16/01/2024)
     */
    pageData() {
      if (!this.isShowNoData) {
        this.isShowNoData = true;
      }
    },
  },
  computed: {
    selectAllRecord: {
      // Khi truy cập giá trị computed property
      get: function () {
        return this.pageData
          ? this.pageData
              .map((item) => item[this.idObject])
              .every((ele) => this.lstDelete.includes(ele)) &&
              this.lstDelete.length >= this.pageData.length &&
              this.lstDelete.length > 0
          : false;
      },
      // Khi thay đổi giá trị computed property
      set: function (value) {
        let lstDelete = [];
        if (value && this.pageData != null) {
          //duyệt dữ liệu push id vào mảng
          this.pageData.forEach((item) => {
            let id = item[this.idObject];
            //nếu trong lstDelete chưa tồn tại id thì mới push
            if (!this.lstDelete.map((item) => item).includes(id)) {
              lstDelete.push(item[this.idObject]);
            }
          });
          this.lstDelete = [...this.lstDelete, ...lstDelete];
          this.countSelectedRow();
        } else {
          if (this.selectAll === false) {
            this.lstDelete = [];
            this.countSelectedRow();
            return;
          }
          this.lstDelete = this.lstDelete.filter((ele) => {
            return !this.pageData
              .map((item) => item[this.idObject])
              .includes(ele);
          });
          this.countSelectedRow();
        }
      },
    },
  },
  methods: {
    /**
     * Set dữ liệu cho từng ô trên table
     * @param {object} data
     * @param {string} col
     * Author: LQHUY(06/12/2023)
     */
    setValueData(data, col) {
      try {
        if (col.type === "gender") {
          return this.$helper.formatGender(data[col.field]);
        } else if (col.type === "money") {
          return this.$helper.formatMoney(data[col.field]);
        } else if (col.type === "datetime") {
          return this.$helper.formatDate(data[col.field]);
        } else {
          return data[col.field];
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Hàm hiển form chi tiết thông tin khách hàng để cập nhật
     * @param {string} id
     * @param {event} e
     * Author: LQHUY(06/12/2023)
     */
    onClickEdit(id, e) {
      this.$emit("toggleShowForm", true);
      this.$emit("updateItemId", id);
      e.stopPropagation();
    },

    /**
     * Hàm chọn xóa một khách hàng và hiển thị dialog
     * Author: LQHUY (6/12/2023)
     * @param {int} id
     * @param {event} e
     */
    onClickIconDelete(data, e) {
      this.$emitter.emit(
        "toggleDialogNotice",
        true,
        true,
        this.$Resource[this.$languageCode].DeleteConfirm,
        this.$Resource[this.$languageCode].ConfirmText(
          `${this.$Resource[this.$languageCode].Employee} <${
            data[this.codeObject]
          }>`
        ),
        this.$Resource[this.$languageCode].Dialog.Type.Question
      );
      this.$emit("updateItemId", data[this.idObject]);
      e.stopPropagation();
    },

    /**
     * Nhấp đúp hiển thị form thông tin chi tiết khách hàng
     * @param {int} index
     * @param {object} data
     * Author: LQHUY (6/12/2023)
     */
    onDblClickRowTable(data, event) {
      event.stopPropagation();
      event.stopImmediatePropagation();
      if (this.lstDelete.indexOf(data[this.idObject]) === -1) {
        this.lstDelete.push(data[this.idObject]);
      } else {
        this.lstDelete = this.lstDelete.filter(
          (item) => item !== data[this.idObject]
        );
      }
      this.$emit("updateItemId", data[this.idObject]);
      this.$emit("toggleShowForm", true);
      this.countSelectedRow();
    },

    /**
     * Chọn và hủy chọn 1 dòng trên table
     * @param {object} data
     * Author: LQHUY (6/12/2023)
     */
    onClickCheckbox(data) {
      if (this.lstDelete.indexOf(data[this.idObject]) === -1) {
        this.lstDelete.push(data[this.idObject]);
      } else {
        this.lstDelete = this.lstDelete.filter(
          (item) => item !== data[this.idObject]
        );
      }
      this.$emit("updateListItemId", this.lstDelete);
      this.countSelectedRow();
    },

    /**
     * Hàm cập nhật số lượng các dòng được chọn
     * @returns số lượng dòng được chọn
     * Author: LQHUY (6/12/2023)
     */
    countSelectedRow() {
      try {
        const total = this.lstDelete.filter((item) => item).length;
        if (total === 0) {
          this.$emit("updateTotalRecordSelected", null);
          this.$emit("toggleShowToolbarAction", false);
          return;
        }
        this.$emit("updateTotalRecordSelected", total);
        this.$emit("toggleShowToolbarAction", true);
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm gỡ bỏ tất cả các dòng được chọn
     * Author: LQHUY (6/12/2023)
     */
    removeRowSelectedDelete() {
      this.$emit("updateTotalRecordSelected", null);
      this.$emit("toggleShowToolbarAction", false);
    },

    /**
     * Thực hiện điều chỉnh độ rộng của các cột
     * @author: LQHUY (11/01/2024)
     */
    columnsResizable() {
      let th;
      let startOffset;
      let startEnd;
      const thTable = document.querySelectorAll("table thead th");
      thTable.forEach((item) => {
        var cuisor = document.createElement("div");
        cuisor.style.top = 0;
        cuisor.style.right = "1px";
        cuisor.style.bottom = 0;
        cuisor.style.width = "6px";
        cuisor.style.position = "absolute";
        cuisor.style.cursor = "col-resize";
        cuisor.addEventListener("mousedown", function (e) {
          th = item;
          startOffset = item.offsetWidth;
          startEnd = e.pageX;
        });
        item.appendChild(cuisor);
      });

      document.addEventListener("mousemove", function (e) {
        if (th) {
          th.style.width = startOffset + e.pageX - startEnd + "px";
          th.style.minWidth = startOffset + e.pageX - startEnd + "px";
        }
      });

      document.addEventListener("mouseup", function () {
        th = undefined;
      });
    },

    /**
     * Hàm mở rộng thêm các chức năng
     * @param {event} e
     * Author: LQHUY (10/01/2024)
     */
    onClickIconShowMenuAction(e) {
      this.isShowActionMenu = !this.isShowActionMenu;
      // this.styleMenuAction.left =
      //   e.target.getBoundingClientRect().x - 100 + "px";
      // this.styleMenuAction.top = e.target.getBoundingClientRect().y + 10 + "px";
      e.stopPropagation();
    },

    /**
     * Hàm hiển thị form chứa thông tin nhân bản khác hàng
     * @param {object} data
     * @param {event} e
     * Author: LQHUY (18/01/2024)
     */
    onClickIconClone(data, e) {
      this.$emit("updateItemIdClone", data[this.idObject]);
      this.$emit("updateItemId", null);
      this.$emit("toggleShowForm", true);
      e.stopPropagation();
    },

    /**
     * Hàm ẩn hiện loading mỗi khi load data
     */
    toggleShowLoadingTable(isShow) {
      this.isShowLoadingTable = isShow;
    },
  },
  data() {
    return {
      /**biến chọn tất cả các bản ghi trong 1 trang */
      isCheckedAllRow: false,

      /**ẩn hiện loading table và menu ation */
      isShowActionMenu: false,
      isShowLoadingTable: false,

      /**danh sách id */
      lstDelete: [],

      /**ẩn hiện không có dữ liêu phuf hợp khi tìm kiếm */
      isShowNoData: false,
    };
  },
};
</script>
<style scoped>
@import url(./table.css);
</style>
