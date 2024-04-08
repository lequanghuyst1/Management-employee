<template>
  <div class="login">
    <div class="login__container">
      <div class="login__wrapper">
        <div class="login__header"></div>
        <div class="login__form">
          <div class="wrapper-input">
            <input
              class="login__form-input"
              :class="{ ' login__form-input-error': errorList.Email }"
              type="text"
              placeholder="Số điện thoại/email"
              @input="onChangeEmailValue"
              v-model="account.Email"
            />
            <span class="form-input-error-message">{{ errorList.Email }}</span>
          </div>
          <div class="wrapper-input input-password">
            <input
              class="login__form-input"
              :class="{ ' login__form-input-error': errorList.Password }"
              :type="showPass ? 'text' : 'password'"
              placeholder="Mật khẩu"
              @input="onChangePasswordValue"
              v-model="account.Password"
            />
            <div
              class="btn-show-pass"
              :class="{ active: showPass }"
              @click="this.showPass = !this.showPass"
            ></div>
            <span class="form-input-error-message">{{
              errorList.Password
            }}</span>
          </div>
        </div>
        <span style="margin-bottom: 16px" class="error-account-failure">{{
          this.errorMessageLogin
        }}</span>
        <a href="" class="forgot-password">{{
          this.$Resource[this.$languageCode].Text.ForgotPassword
        }}</a>

        <div @click="onClickLogin" type="" class="login__form-btn">
          {{ this.$Resource[this.$languageCode].Text.Login }}
        </div>
        <div class="login-method">
          <div class="login-with">
            <div class="login-with--line"></div>
            <div class="login-with--title">
              {{ this.$Resource[this.$languageCode].Text.LoginWith }}
            </div>
            <div class="login-with--line"></div>
          </div>
          <div class="login-method-list">
            <div class="login-method-item" method="Google"></div>
            <div class="login-method-item" method="Apple"></div>
            <div class="login-method-item" method="Microsoft"></div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: "LoginPage",
  created() {},
  mounted() {
    document.addEventListener("keydown", (e) => {
      switch (e.keyCode) {
        case 13:
          this.onClickLogin();
          break;
        default:
          break;
      }
    });
  },
  watch: {},
  methods: {
    /**
     * Hàm đăng nhập vào hệ thống
     * Author: LQHUY(22/02/2024)
     */
    onClickLogin() {
      this.validateFormLogin();
      if (this.isCheckLogin) {
        this.loginInSystem();
      }
    },
    /**
     * Hàm kiểm tra dữ liệu thông tin đăng nhập
     * Author: LQHUY(22/02/2024)
     */
    validateFormLogin() {
      if (
        this.account.Email === "" ||
        this.account.Email === null ||
        this.account.Email === undefined
      ) {
        this.errorList.Email =
          this.$Resource[this.$languageCode].Text.UsernameLoginNotEmpty;
        this.isCheckLogin = false;
      } else {
        this.errorList.Email = "";
        this.isCheckLogin = true;
      }
      if (
        this.account.Password === "" ||
        this.account.Password === null ||
        this.account.Password === undefined
      ) {
        this.errorList.Password =
          this.$Resource[this.$languageCode].Text.PasswordLoginNotEmpty;
        this.isCheckLogin = false;
      } else {
        this.errorList.Password = null;
        this.isCheckLogin = true;
      }
    },
    onChangeEmailValue(){
      if (
        this.account.Email === "" ||
        this.account.Email === null ||
        this.account.Email === undefined
      ) {
        this.errorList.Email =
          this.$Resource[this.$languageCode].Text.UsernameLoginNotEmpty;
        this.isCheckLogin = false;
      } else {
        this.errorList.Email = null;
        this.isCheckLogin = true;
      }
    },
    onChangePasswordValue(){
      if (
        this.account.Password === "" ||
        this.account.Password === null ||
        this.account.Password === undefined
      ) {
        this.errorList.Password =
          this.$Resource[this.$languageCode].Text.PasswordLoginNotEmpty;
        this.isCheckLogin = false;
      } else {
        this.errorList.Password = "";
        this.isCheckLogin = true;
      }
    },
  
    /**
     * Hàm gọi API lấy token phân quyền để có thể đăng nhập
     * Author: LQHUY(22/02/2024)
     */
    async loginInSystem() {
      try {
        const res = await this.$httpRequest.post(
          "Authentications/Login",
          this.account
        );
        switch (res.status) {
          case 200:
            location.reload();
            this.$router.push("/employee");
            localStorage.setItem("Token", JSON.stringify(res.data.Token));
            localStorage.setItem("User", JSON.stringify({ ...res.data.User }));
            break;
        }
      } catch (error) {
        this.errorMessageLogin = Object.values(error.response.data.errors).join(
          ""
        );
      }
    },
  },
  data() {
    return {
      account: {},
      errorList: {},
      isCheckLogin: true,
      errorMessageLogin: null,
      showPass: false,
    };
  },
};
</script>
<style scoped>
.login__container {
  width: 100%;
  min-height: 100vh;
  position: relative;
  background-image: url(../../assets/img/bg1.jpg);
  background-repeat: no-repeat;
  background-size: cover;
  background-position: center;
}
.login__container::before {
  content: "";
  display: block;
  position: absolute;
  z-index: 1;
  width: 100%;
  height: 100%;
  top: 0;
  left: 0;
  background: linear-gradient(
    rgba(0, 30, 61, 0.6) 0,
    rgba(0, 0, 0, 0.1) 41.42%,
    rgba(0, 0, 0, 0.3) 100%
  );
}

.login__wrapper {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 400px;
  border-radius: 8px;
  padding: 40px 48px 40px 48px;
  min-height: 340px;
  background: #fff;
  box-shadow: 0 12px 20px rgba(0, 0, 0, 0.12);
  z-index: 2;
}
.login__header {
  font-size: 60px;
  background: url(../../assets/img/login-icon.svg) center no-repeat;
  background-size: contain;
  display: block;
  display: -moz-box;
  display: -ms-flexbox;
  width: 196px;
  height: 36px;
  margin: 0 auto;
  margin-bottom: 40px;
}
.login__body {
}
.wrapper-input {
  margin-bottom: 12px;
}
.wrapper-input .login__form-input {
  font-size: 16px;
  color: #212121;
  line-height: 1.2;
  display: block;
  width: 100%;
  height: 48px;
  border: 1px solid #e0e5e9;
  background: 0 0;
  padding: 14px 16px 15px 16px;
  border-radius: 3px;
  outline: none;
}
.login__form-input:focus {
  border-color: #50b83c;
  transition: all 0.5s;
}
.login__form-input::placeholder {
  color: #a0a2a6;
}
.forgot-password {
  font-size: 14px;
  color: #  ;
  line-height: 17px;
  margin-top: 0;
  display: block;
  text-align: left;
  text-decoration: none;
  margin-bottom: 24px;
  margin-top: 4px;
}
.login__form-btn {
  width: 100%;
  font-size: 16px;
  color: #fff;
  line-height: 40px;
  font-weight: 500;
  text-align: center;
  cursor: pointer;
  height: 40px;
  border-radius: 3px;
  background: #0073e6;
  transition: all 0.4s;
}
.login__form-btn:hover {
  background-color: #384fd5;
  box-shadow: 0 2px 15px rgba(42, 126, 252, 0.25);
  transition: all 0.5s;
}
.login-with {
  display: flex;
  margin-top: 24px;
  align-items: center;
  margin-bottom: 16px;
}
.login-with--line {
  height: 1px;
  width: 70px;
  background-color: #9ea1a5;
}
.login-with--title {
  padding: 5px 10px;
  background-color: #fff;
  color: #9ea1a5;
  font-size: 14px;
}
.login-method-list {
  display: flex;
  justify-content: center;
}
.login-method-item {
  width: 40px;
  height: 40px;
  border-radius: 20px;
  margin: 0 4px;
  cursor: pointer;
  background-position: center;
  background-size: 40px;
  background-repeat: no-repeat;
}
.login-method-item[method="Google"] {
  background-image: url(../../assets/icons/icon-google.svg);
}
.login-method-item[method="Apple"] {
  background-image: url(../../assets/icons/icon-apple.svg);
}
.login-method-item[method="Microsoft"] {
  background-image: url(../../assets/icons/icon-microsoft.svg);
}
.form-input-error-message {
  font-size: 12px;
  color: #ff1d1d;
  height: 20px;
  line-height: 20px;
}
.login__form-input.login__form-input-error {
  border: 1px solid #ff1d1d;
}
.error-account-failure {
  color: #ff1d1d;
  font-size: 14px;
  margin-bottom: 16px;
  display: block;
}
.input-password {
  position: relative;
}
.btn-show-pass {
  display: block;
  position: absolute;
  right: 8px;
  top: 16px;
  width: 16px;
  height: 16px;
  background: url(../../assets/icons/icon-hide-pass.svg) center no-repeat;
}
.btn-show-pass.active {
  display: block;
  background: url(../../assets/icons/icon-show-pass.svg) center no-repeat;
}
</style>
