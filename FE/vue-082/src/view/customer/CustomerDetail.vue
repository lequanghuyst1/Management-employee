<template>
  <MForm
    id="m-dialog__info-customer"
    title="Thông tin nhân viên"
    @isCheckData="isCheckData"
    :formMode="formMode"
  >
    <template #form>
      <form action="" class="form" id="form-detal-customer">
        <div class="grid">
          <div class="row">
            <div class="col l-2">
              <MInput
                v-model="customer.CustomerCode"
                label="Mã khách hàng"
                :required="true"
                :message="lstErrorMessage.errCustomerCode"
                ref="txtCustomerCode"
                id="txtCustomerCode"
              />
            </div>
            <div class="col l-4">
              <MInput
                v-model="customer.FullName"
                label="Họ và tên"
                :required="true"
                :message="lstErrorMessage.errFullName"
                ref="txtFullName"
              />
            </div>
            <div class="col l-3">
              <MInput
                type="date"
                label="Ngày sinh"
                v-model="customer.DateOfBirth"
              />
            </div>
            <div class="col l-3">
              <MGenderRadio v-model="customer.Gender"></MGenderRadio>
            </div>
          </div>

          <div class="row">
            <div class="col l-6">
              <MInput
                type="text"
                label="Số điện thoại"
                v-model="customer.PhoneNumber"
                :required="true"
              ></MInput>
              <!-- <MCombobox
          url="CustomerGroups"
          v-model="searchString"
          propValue="CustomerGroupId"
          propText="CustomerGroupName"
          idInput="txtCompanyName"
        ></MCombobox> -->
            </div>

            <div class="col l-3">
              <MInput
                label="Số CMND"
                v-tippy="{ content: 'Số chứng minh nhân dân', placement: 'top' }"
                v-model="customer.MemberCardCode"
              ></MInput>
            </div>
            <div class="col l-3">
              <MInput
                label="Ngày cấp"
                type="date"
                v-model="customer.CreatedDate"
              ></MInput>
            </div>
          </div>
          <div class="row">
            <div class="col l-6">
              <MInput
                label="Email"
                placeholder="example@gmail.com"
                v-model="customer.Email"
                :message="lstErrorMessage.errEmail"
                :required="true"
                ref="txtEmail"
              ></MInput>
            </div>
            <div class="col l-6">
              <MInput label="Nơi cấp" v-model="customer.Address"></MInput>
            </div>
          </div>
          <div class="row">
            <div class="col l-3">
              <MInput
                label="Số tiền nợ"
                v-model="customer.DebitAmount"
                :formatMoney="true"
                textAlign="right"
              ></MInput>
            </div>
            <div>
              <MCombobox
                label="Nhóm khách hàng"
                url="CustomerGroups"
                v-model="customer.CustomerGroupId"
                refEl="txtCompanyName"
                propValue="CustomerGroupId"
                propText="CustomerGroupName"
                id="txtCompanyName"
              ></MCombobox>
            </div>
          </div>
          <div class="row">
            <div class="col l-12">
              <MInput label="Công ty" v-model="customer.CompanyName"></MInput>
            </div>
          </div>
          <div class="row">
            <div class="col l-12">
              <MInput label="Địa chỉ" v-model="customer.Address"></MInput>
            </div>
          </div>
        </div>
      </form>
    </template>
  </MForm>
</template>
<script>
import $ from "jquery";
export default {
  created() {
    this.customerId = this.selectedCustomerId;
    //this.loadCustomerGroups();
    if (this.formMode == this.$Enum.FormMode.Add) {
      this.getNewCustomerCode();
    } else {
      this.getCustomerDetail();
    }
  },
  mounted() {
    //tự động focus vào ô input đầu tiên
    $("#txtCustomerCode").focus();
    
  },
  beforeUpdate() {
    //tự động focus vào ô input lỗi đầu tiên
    if ($("input.m-textfield-error").length > 0) {
      $("input.m-textfield-error")[0].focus();
    }
  },
  name: "CustomerDetail",
  props: ["selectedCustomerId", "loadData"],
  computed: {
    formMode() {
      if (this.selectedCustomerId) {
        return this.$Enum.FormMode.Edit;
      }
      return this.$Enum.FormMode.Add;
    },
  },
  methods: {
    /**
     * Hàm lấy ra mã khách hàng mới
     * Author: LQHUY
     */
    async getNewCustomerCode() {
      this.customer = {};
      try {
        var res = await this.$httpRequest.get("Customers/NewCustomerCode");
        this.customer.CustomerCode = res.data;
      } catch (error) {
        this.$emitter.emit("handleApiError", error);
      }
    },
    /**
     * Hàm lấy ra thông tin chi tiết khách hàng
     * Author: LQHUY
     */
    async getCustomerDetail() {
      try {
        const res = await this.$httpRequest.get(
          `Customers/${this.selectedCustomerId}`
        );
        switch (res.status) {
          case 200:
            this.customer = res.data;
            if (this.customer.DateOfBirth) {
              this.customer.DateOfBirth = this.$helper.formatDate(
                this.customer.DateOfBirth,
                true
              );
            }
            if (this.customer.DebitAmount) {
              this.customer.DebitAmount = this.$helper.formatMoney(
                this.customer.DebitAmount
              );
            }
            if (this.customer.Gender) {
              this.displayGenderOnForm(this.customer.Gender);
            }
            break;
          default:
            break;
        }
      } catch (error) {
        this.$emitter.emit("handleApiError", error);
      }
    },

    /**
     * Hàm hiển thị giá trị giới tính lên form chi tiết
     * @param {int} gender giá trị giới tính
     * Author: LQHUY (25/11/2023)
     */
    displayGenderOnForm(gender) {
      try {
        if (gender !== null || gender !== undefined) {
          //lấy ra các input radio và so sánh với giá trị truyền vào
          $('#m-dialog__info-customer input[name="gender"]').each(function () {
            if ($(this).val() == gender) {
              $(this).prop("checked", true);
            }
          });
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * Hàm kiểm tra các giá trị có thỏa mãn hay không và thêm mới khách hàng
     * Author: LQHUY
     */
    isCheckData() {
      try {
        this.validateData();
        if (!this.isCheck) {
          return;
        }
        this.transferData();
        if (this.formMode === this.$Enum.FormMode.Add) {
          this.addNewCustomer();
        } else {
          this.editCustomer();
        }
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm kiểm tra các giá trị nhập vào có hợp lệ không
     * Author: LQHUY (7/12/2023)
     */
    validateData() {
      try {
        this.setError("CustomerCode", this.$refs.txtCustomerCode.label);
        this.setError("FullName", this.$refs.txtFullName.label);
        if (this.customer.Email) {
          this.validateEmail("Email");
        } else {
          this.setError("Email", this.$refs.txtEmail.label);
        }
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm xét message lỗi
     * @param {string} field
     * @param {string} title
     * Author: LQHUY
     */
    setError(field, title) {
      try {
        if (
          this.customer[field] === "" ||
          this.customer[field] === null ||
          this.customer[field] === undefined
        ) {
          this.isCheck = false;
          this.lstErrorMessage[`err${field}`] =
            this.$Resource[this.$languageCode].ErrorMessage(title);
        } else {
          this.isCheck = true;
        }
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm validate email đúng định dạng
     * @param {string} field
     * @param {string} title
     * Author: LQHUY (26/11/2023)
     * Update: LQHUY (11/12/2023)
     */
    validateEmail(field) {
      try {
        if (!this.customer[field].match(/^[\w-.]+@([\w-]+\.)+[\w-]{2,4}$/)) {
          this.isCheck = false;
          this.lstErrorMessage[`err${field}`] =
            this.$Resource[this.$languageCode].EmailNotValid;
        } else {
          this.isCheck = true;
        }
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm thực hiện chuyển lại dữ liệu gửi lên API
     * Author: LQHUY (10/12/2023)
     */
    transferData() {
      //Chuyển dữ liệu định dạng trên ô input loại bỏ dấu . chữ đ và khoảng cách
      if (this.customer.DebitAmount) {
        this.customer.DebitAmount = this.$helper.formatMoneySendApi(
          this.customer.DebitAmount
        );
      }
      if (this.customer.Gender) {
        this.customer.Gender = parseInt(this.customer.Gender);
      }
    },

    /**
     * Hàm thực hiện thêm khách hàng mới
     * Author: LQHUY (7/12/2023)
     */
    async addNewCustomer() {
      try {
        const res = await this.$httpRequest.post("Customers", this.customer);
        switch (res.status) {
          case 201:
            this.$emitter.emit("toggleShowForm");
            this.successResponse(this.$Resource[this.$languageCode].AddSuccess);
            break;
          default:
            break;
        }
      } catch (error) {
        this.$emitter.emit("handleApiError", error);
      }
    },

    /**
     * Hàm thực hiện sửa thông tin khách hàng
     * Author: LQHUY (7/12/2023)
     */
    async editCustomer() {
      try {
        const res = await this.$httpRequest.put(
          `Customers/${this.selectedCustomerId}`,
          this.customer
        );
        switch (res.status) {
          case 200:
            this.$emitter.emit("toggleShowForm");
            this.successResponse(
              this.$Resource[this.$languageCode].EditSuccess
            );
            break;
          default:
            break;
        }
      } catch (error) {
        this.customer.Gender = this.$helper.formatGender(this.customer.Gender);
        this.customer.DebitAmount = this.$helper.formatMoney(
          this.customer.DebitAmount
        );
        this.$emitter.emit("handleApiError", error);
      }
    },

    /**
     * Hàm xử lí khi thêm hoặc sửa thông tin thành công
     * @param {string} message
     * Author: LQHUY
     */
    successResponse(message) {
      try {
        this.$emitter.emit("toggleShowForm");

        this.$emitter.emit(
          "onShowToastMessage",
          this.$Resource[this.$languageCode].ToastMessage.Type.Success,
          message,
          this.$Resource[this.$languageCode].ToastMessage.Status.Success
        );
        this.$emit("loadData");
      } catch (error) {
        console.error(error);
      }
    },
  },

  data() {
    return {
      customer: {
        Gender: 0,
      },
      title:null,
      //customerGroups: [],
      isCheck: true,
      lstErrorMessage: {},
    };
  },
};
</script>
<style scoped></style>
