<template>
  <div class="m-pagination">
    <div class="m-pagination__left">
      <p>
        {{ this.$Resource[this.$languageCode].Text.Total }}:
        <b class="m-pagination__left-quantiy">{{ total }}</b>
        {{ this.$Resource[this.$languageCode].Text.Record.toLowerCase() }}
      </p>
    </div>
    <div class="m-pagination__right">
      <div class="m-pagination__selected-record">
        <p class="m-pagination__selected-text">
          {{ this.$Resource[this.$languageCode].Text.RecordPerPage }}:
        </p>
        <MDropdown :listItem="lstPageSize"> </MDropdown>
      </div>
      <div class="m-pagination__numbers-group">
        <span class="m-page__number-current">
          <b v-if="this.total === 0"
            >{{ "00" }}
            -
          </b>
          <b v-else
            >{{
              this.startIndex < 10 ? `0${this.startIndex}` : this.startIndex
            }}
            -
          </b></span
        >
        <span class="m-page__number-total"
          ><b v-if="this.total === 0"> {{ "00" }} </b>

          <b v-else> {{ this.endIndex }} </b></span
        >
        <span style="margin-left: 4px">
          {{
            this.$Resource[this.$languageCode].Text.Record.toLowerCase()
          }}</span
        >
      </div>
      <button
        ref="btnPrev"
        :style="{
          color:
            this.pageCurrent == 1 || this.total === 0 ? '#A8A8A8' : '#333333',
        }"
        class="btn-prev"
        @click="onClickPrevPage"
      >
        <i class="fa-solid fa-angle-left"></i>
      </button>
      <!-- <div
        v-for="number in pages"
        :key="number"
        class="m-page__number-option"
        :class="{ 'select-number': number == this.pageCurrent }"
        @click="onChangePageNumber(number)"
      >
        <span v-if="number !== '...'">
          {{ number }}
        </span>
        <span v-else>...</span>
      </div> -->
      <button
        ref="btnNext"
        :style="{
          color:
            this.pageCurrent == this.totalPage || this.total === 0
              ? '#A8A8A8'
              : '#333333',
        }"
        class="btn-next"
        @click="onClickNextPage"
      >
        <i class="fa-solid fa-angle-right"></i>
      </button>
    </div>
  </div>
</template>
<script>
export default {
  name: "MPagination",
  props: {
    total: {
      type: [Number],
      required: false,
    },
    totalPage: {
      type: [Number],
      required: false,
    },
    pageNumber: {
      type: [Number],
      required: false,
    },
    pageSize: {
      type: [Number],
      required: false,
    },
  },
  created() {
    this.pageCurrent = this.pageNumber;
    this.endIndex = this.pageSize;
  },

  mounted() {},
  watch: {
    /**
     * Theo dõi sự thay đổi của trang hiện tại và cập nhật
     * @param {int} newValue
     * Author: LQHUY(05/12/2023)
     */
    pageCurrent(newValue) {
      this.$emit("update:pageNumber", Number(newValue));
    },
    /**
     * Theo dõi sự thay đổi của số bản ghi trên 1 trang và cập nhật
     * @param {int} newValue
     * Author: LQHUY(05/12/2023)
     */
    pageSize(newValue) {
      this.startIndex = 1;
      this.endIndex = newValue;
      if (newValue) {
        this.pageCurrent = 1;
      }
      if (this.endIndex > this.total) {
        this.endIndex = this.total;
      }
    },
    pageNumber(newValue) {
      if (newValue === 1) {
        this.startIndex = 1;
        this.endIndex = newValue * this.pageSize;
      }
    },
  },
  computed: {
    /**
     * Xét paging dạng ...
     * Author : LQHUY
     */
    pages() {
      const middle = Math.floor(this.maxPages / 2);
      let start, end;
      if (this.totalPage <= this.maxPages) {
        start = 1;
        end = this.totalPage;
      } else if (this.pageCurrent <= middle) {
        start = 1;
        end = this.maxPages;
      } else if (this.pageCurrent >= this.totalPage - middle) {
        start = this.totalPage - this.maxPages + 1;
        end = this.totalPage;
      } else {
        start = this.pageCurrent - middle;
        end = this.pageCurrent + middle;
      }

      const pages = [];
      for (let i = start; i <= end; i++) {
        pages.push(i);
      }
      if (start > 1 && this.totalPage > this.maxPages) {
        pages.unshift("...");
        pages.splice(1, 1);
        pages.unshift(1);
      }
      if (end < this.totalPage) {
        pages.push("...");
        pages.splice(this.totalPage - 1, 1);
        pages.push(this.totalPage);
      }

      return pages;
    },
  },
  methods: {
    /**
     * Nhảy tới trang kế tiếp
     * Author: LQHUY(05/12/2023)
     */
    onClickNextPage() {
      try {
        this.pageCurrent++;
        if (this.pageCurrent > this.totalPage) {
          this.pageCurrent = this.totalPage;
          return;
        }
        this.startIndex = this.startIndex + this.pageSize;
        this.endIndex = this.startIndex + this.pageSize - 1;
        if (this.endIndex > this.total) {
          this.endIndex = this.total;
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Quay lại trang trước
     * Author: LQHUY(05/12/2023)
     */
    onClickPrevPage() {
      try {
        this.pageCurrent--;
        if (this.pageCurrent < 1) {
          this.pageCurrent = 1;
          return;
        }
        this.startIndex = this.startIndex - this.pageSize;
        this.endIndex = this.startIndex + this.pageSize - 1;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Chọn trang muốn tới
     * @param {int} number
     * Author: LQHUY(05/12/2023)
     */
    onChangePageNumber(number) {
      try {
        this.pageCurrent = number;
        this.startIndex = (this.pageCurrent - 1) * this.pageSize + 1;
        this.endIndex = this.pageCurrent * this.pageSize;
        if (this.endIndex > this.total) {
          this.endIndex = this.total;
        }
      } catch (error) {
        console.log(error);
      }
    },
  },
  data() {
    return {
      pageCurrent: null,
      lstPageSize: [10, 20, 30, 50, 100],
      startIndex: 1,
      endIndex: 0,
      maxPages: 4,
    };
  },
};
</script>
<style scoped>
@import url(./pagination.css);
</style>
