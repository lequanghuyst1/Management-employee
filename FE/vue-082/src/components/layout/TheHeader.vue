<template>
  <div class="header">
    <div class="header__logo">
      <div class="header__logo-icon m-icon-toggle"></div>
      <div class="header__logo-image">
        <div class="m-icon-logo"></div>
        <div class="m-icon-name-logo">Kế Toán</div>
      </div>
    </div>

    <div class="header__info">
      <div class="header__info-name">
        <div class="m-icon-menu"></div>
        <h4 class="header__info-company-name">
          CÔNG TY TNHH SẢN XUẤT - THƯƠNG MẠI - DỊCH VỤ QUY PHÚ
        </h4>
        <div class="m-icon-arrow-down"></div>
      </div>
      <div class="header__navbar-list">
        <div
          v-tippy="{ content: 'Thông báo', placement: 'bottom' }"
          class="header__navbar-item"
        >
          <i class="header__navbar-icon fa-regular fa-bell"></i>
        </div>
        <div
          v-tippy="{ content: 'Giúp đỡ', placement: 'bottom' }"
          class="header__navbar-item"
        >
          <i class="header__navbar-icon fa-regular fa-circle-question"></i>
        </div>
        <div
          @click="this.isShowActionUser = !this.isShowActionUser"
          class="header__navbar-item header__navbar-item-user"
        >
          <img
            src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRSbpqvNQMR7YBZ9eN_5qUeAzj6Vp-1ljDiuw&usqp=CAU"
            alt="Ảnh"
            class="header__navbar-item-avatar"
          />
          <span class="header__navbar-item-user-name">{{
            user ? user.Fullname : ""
          }}</span>
          <div class="m-icon-arrow-down" style="scale: calc(10 / 14)"></div>
          <ul v-show="isShowActionUser" class="user-action">
            <li class="user-action-item">Thông tin cá nhân</li>
            <li class="user-action-item" @click="logOutSystem">Đăng xuất</li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: "TheHeader",
  created() {
    this.user = JSON.parse(localStorage.getItem("User"));
  },
  methods: {
    logOutSystem() {
      localStorage.setItem("Token", JSON.stringify(null));
      localStorage.setItem("User", JSON.stringify(null));

      this.$router.push("/login").finally(() => {
        document.title = "Đăng nhập"; // Đặt lại tiêu đề sau khi chuyển hướng
        location.reload();
      });
    },
  },
  data() {
    return {
      isShowActionUser: false,
      user: null,
    };
  },
};
</script>
<style scoped>
@import url(../../css/layout/header.css);
</style>
