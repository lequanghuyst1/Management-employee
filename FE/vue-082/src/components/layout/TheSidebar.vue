<template>
  <div class="sidebar" :style="{ width: isShowSideBar ? '200px' : '78px' }">
    <div class="sidebar__list">
      <router-link
        v-for="(item, index) in sidebarList"
        :key="index"
        :to="item.path"
        class="sidebar__item"
      >
        <div class="sidebar__item-icon">
          <div :class="item.icon"></div>
        </div>
        <p v-if="isShowTitle" class="sidebar__item-tilte">{{ item.title }}</p>
      </router-link>
      <!-- <router-link to="/employee" href="" class="sidebar__item">
        <div class="sidebar__item-icon">
          <div class="m-icon-employee"></div>
        </div>
        <p class="sidebar__item-tilte">Nhân viên</p>
      </router-link> -->
    </div>
    <div @click="toggleSidebar" class="sidbar__action">
      <div
        v-if="isShowTitle"
        class="sidebar__item sidbar__action-collsape"
      >
        <div class="sidebar__item-icon">
          <div class="m-icon-prev"></div>
        </div>
        <p v-if="isShowTitle" class="sidebar__item-tilte">Thu gọn</p>
      </div>
      <div
        v-if="isShowTitle == false"
        class="sidebar__item sidbar__action-expand"
      >
        <div class="sidebar__item-icon">
          <div class="m-icon-next"></div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import { menu } from "../../js/resource/menu";
export default {
  name: "TheSidebar",
  props: ["modelValue"],
  mounted() {},
  methods: {
    /**
     * Hàm thu gọn và mở rộng sidebar
     * Author LQHUY(20/01/2024)
     */
    toggleSidebar() {
      this.isShowSideBar = !this.isShowSideBar;
      if (this.isShowSideBar == false) {
        this.isShowTitle = !this.isShowTitle;
      } else {
        setTimeout(() => {
          this.isShowTitle = !this.isShowTitle;
        }, 300);
      }
      this.$emit("update:modelValue", this.isShowSideBar);
    },
  },
  data() {
    return {
      sidebarList: menu,
      isShowSideBar: true,
      isShowTitle: true,
    };
  },
};
</script>
<style scoped>
@import url(../../css/layout/sidebar.css);
</style>
