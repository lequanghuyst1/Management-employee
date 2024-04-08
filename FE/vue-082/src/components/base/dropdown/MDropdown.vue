<template>
  <div class="m-dropdown" @click="onHideDropdown">
    {{ this.outputValue }}
    <i
      @click="
        (e) => {
          e.stopPropagation();
          onHideDropdown();
        }
      "
      v-if="isShowUp"
      class="fa-solid fa-angle-up"
    ></i>
    <i
      @click="
        (e) => {
          e.stopPropagation();
          onHideDropdown();
        }
      "
      v-if="isShowDown"
      class="fa-solid fa-angle-down"
    ></i>
    <ul v-show="isShowDropdown" class="m-dropdowm__list">
      <li
        v-for="(item, index) in listItem"
        :key="index"
        @click="onClickSelectItem(item)"
        class="m-dropdowm__list-item"
        :class="{ active: this.outputValue == item }"
      >
        <p class="m-dropdowm__list-item--value">{{ item }}</p>
        <div class="m-dropdowm__list-item--icon">
          <i class="fa-solid fa-check"></i>
        </div>
      </li>
    </ul>
  </div>
</template>
<script>
export default {
  name: "MDropdown",
  props: {
    listItem: {
      type: Array,
      required: true,
    },
  },
  inject: {
    pageSize: {
      type: [Number],
      required: false,
    },
  },
  created() {
    this.outputValue = this.pageSize;
  },
  mounted() {
    /* bắt sự kiện khi click ra ngoài màn hình */
    document.addEventListener("click", this.windowClose);
  },
  methods: {
    /**
     * Chọn 1 item
     * @param {int} item
     * @param {int} index
     * Author: LQHUY(27/12/2023)
     */
    onClickSelectItem(item) {
      this.isShowDropdown = false;
      this.outputValue = item;
      this.isShowDown = false;
      this.isShowUp = true;
      this.$emitter.emit("updatePageSize", item);
      event.stopPropagation();
    },

    /**
     * Ẩn dropdown
     * Author: LQHUY(27/12/2023)
     */
    onHideDropdown() {
      this.isShowDropdown = !this.isShowDropdown;
      this.isShowDown = !this.isShowDown;
      this.isShowUp = !this.isShowUp;
    },
    /**
     * Ẩn dropdown khi click ra ngoài chính nó
     * Author: LQHUY(27/12/2023)
     */
    windowClose(e) {
      if (!this.$el.contains(e.target)) {
        this.isShowDropdown = false;
      }
    },
  },
  data() {
    return {
      outputValue: null,
      pageCurrent: null,
      isShowDropdown: false,
      isShowUp: true,
      isShowDown: false,
    };
  },
};
</script>
<style scoped>
.m-dropdown {
  height: 36px;
  line-height: 36px;
  font-size: 14px;
  padding: 0 8px;
  width: 76px;
  position: relative;
  border: 1px solid #e0e0e0;
  border-radius: 4px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}
.m-dropdowm__list {
  position: absolute;
  bottom: calc(0% + 38px);
  background-color: #ffffff;
  width: 100%;
  left: 0;
  padding: 4px 8px;
  border-radius: 4px;
  box-shadow: 0 2px 8px rgba(23, 27, 42, 0.24);
}
.m-dropdowm__list-item {
  list-style: none;
  padding: 0 6px;
  height: 36px;
  display: flex;
  justify-content: space-between;
  border-radius: 4px;
}
.m-dropdowm__list-item.active,
.m-dropdowm__list-item.active .m-dropdowm__list-item--icon {
  color: #50b83c;
}
.m-dropdowm__list-item:hover {
  background-color: #c7efc0;
  color: #ffffff;
}
.m-dropdowm__list-item--icon {
  color: #ffffff;
  font-size: 16px;
}
</style>
