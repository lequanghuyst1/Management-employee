<template>
  <MForm
    id="m-dialog__info-employee"
    title="Thông tin nhân viên"
    isShowIconHelp="show"
    @isCheckData="isCheckData"
    :formMode="formMode"
    @compareValueObject="compareValueObject"
  >
    <template #form>
      <form action="" class="form" id="form-detal-employee">
        <div class="grid">
          <div class="row">
            <div class="col l-2">
              <MInput
                v-model="employee.EmployeeCode"
                :label="
                  this.$Resource[this.$languageCode].EmployeeField.EmployeeCode
                "
                :required="true"
                :message="lstErrorMessage.errEmployeeCode"
                id="txtEmployeeCode"
                ref="EmployeeCode"
              />
            </div>
            <div class="col l-4">
              <MInput
                v-model="employee.Fullname"
                :label="
                  this.$Resource[this.$languageCode].EmployeeField.EmployeeName
                "
                :required="true"
                :message="lstErrorMessage.errFullname"
                ref="Fullname"
              />
            </div>
            <div class="col l-3">
              <MInput
                type="date"
                :label="
                  this.$Resource[this.$languageCode].EmployeeField.DateOfBirth
                "
                :message="lstErrorMessage.errDateOfBirth"
                v-model="employee.DateOfBirth"
                ref="DateOfBirth"
                placeholder="dd/MM/yyyy"
              />
            </div>
            <div class="col l-3">
              <MGenderRadio v-model="employee.Gender"></MGenderRadio>
            </div>
          </div>

          <div class="row">
            <div class="col l-6">
              <MCombobox
                :label="
                  this.$Resource[this.$languageCode].EmployeeField
                    .DepartmentName
                "
                url="Departments"
                v-model="employee.DepartmentId"
                propValue="DepartmentId"
                propText="DepartmentName"
                ref="DepartmentId"
                :required="true"
                :message="lstErrorMessage.errDepartmentId"
                id="cbDepartment"
              ></MCombobox>
            </div>

            <div class="col l-3">
              <MInput
                :label="
                  this.$Resource[this.$languageCode].EmployeeField
                    .IdentityNumber
                "
                v-tippy="{
                  content:
                    this.$Resource[this.$languageCode].Text.IdentityNumber,
                  placement: 'top',
                }"
                v-model="employee.IdentityNumber"
              ></MInput>
            </div>
            <div class="col l-3">
              <MInput
                :label="
                  this.$Resource[this.$languageCode].EmployeeField.IdentityDate
                "
                type="date"
                v-model="employee.IdentityDate"
              ></MInput>
            </div>
          </div>
          <div class="row">
            <div class="col l-6">
              <MCombobox
                :label="
                  this.$Resource[this.$languageCode].EmployeeField
                    .EmployeePosition
                "
                url="Positions"
                v-model="employee.PositionId"
                propValue="PositionId"
                propText="PositionName"
                ref="Position"
                id="cbPosition"
              ></MCombobox>
            </div>
            <div class="col l-6">
              <MInput
                :label="
                  this.$Resource[this.$languageCode].EmployeeField.IdentityPlace
                "
                v-model="employee.IdentityPlace"
              ></MInput>
            </div>
          </div>
          <div class="row">
            <div class="col l-12">
              <MInput
                :label="
                  this.$Resource[this.$languageCode].EmployeeField.Address
                "
                v-model="employee.Address"
              ></MInput>
            </div>
          </div>
          <div class="row">
            <div class="col l-3">
              <MInput
                v-tippy="{
                  content:
                    this.$Resource[this.$languageCode].EmployeeField
                      .TelephoneNumberToolTip,
                  placement: 'top',
                }"
                :label="
                  this.$Resource[this.$languageCode].EmployeeField.PhoneNumber
                "
                v-model="employee.PhoneNumber"
              ></MInput>
            </div>
            <div class="col l-3">
              <MInput
                :label="
                  this.$Resource[this.$languageCode].EmployeeField
                    .PhoneNumberFixed
                "
                v-tippy="{
                  content:
                    this.$Resource[this.$languageCode].EmployeeField
                      .PhoneNumberFixedToolTip,
                  placement: 'top',
                }"
                v-model="employee.PhoneNumberFixed"
                placeholder="(000)-000000"
              ></MInput>
            </div>
            <div class="col l-3">
              <MInput
                ref="txtEmail"
                :label="this.$Resource[this.$languageCode].EmployeeField.Email"
                v-model="employee.Email"
                :message="lstErrorMessage.errEmail"
              ></MInput>
            </div>
          </div>
          <div class="row">
            <div class="col l-3">
              <MInput
                :label="
                  this.$Resource[this.$languageCode].EmployeeField.BankAccount
                "
                v-model="employee.BankAccount"
              ></MInput>
            </div>
            <div class="col l-3">
              <MInput
                :label="
                  this.$Resource[this.$languageCode].EmployeeField.BankName
                "
                v-model="employee.BankName"
              ></MInput>
            </div>
            <div class="col l-3">
              <MInput
                :label="
                  this.$Resource[this.$languageCode].EmployeeField.BankAddress
                "
                v-model="employee.BankAddress"
              ></MInput>
            </div>
          </div>
        </div>
      </form>
    </template>
  </MForm>
</template>
<script>
export default {
  name: "EmployeeDetail",
  props: {
    selectedEmployeeId: {
      type: String,
      required: false,
    },
    selectedEmployeeIdClone: {
      type: String,
      required: false,
    },
  },
  emits: ["loadData"],
  created() {
    if (this.formMode == this.$Enum.FormMode.Add) {
      this.getNewEmployeeCode();
    } else if (this.formMode == this.$Enum.FormMode.Clone) {
      this.getCloneInfoEmployee();
    } else {
      this.getEmployeeDetail();
    }
  },
  mounted() {
    //tự động focus vào ô input đầu tiên
    this.$refs["EmployeeCode"].setFocus();
    document.addEventListener("keydown", (e) => {
      switch (e.keyCode) {
        case 27:
          this.$emitter.emit("toggleShowForm", false);
          break;
        default:
          break;
      }
    });
  },
  updated() {},
  beforeUnmount() {
  },
  computed: {
    /**
     * Trạng thái của form (thêm, sửa, nhân bản)
     * Author: LQHUY (26/11/2023)
     */
    formMode() {
      if (this.selectedEmployeeId) {
        return this.$Enum.FormMode.Edit;
      } else if (this.selectedEmployeeIdClone) {
        return this.$Enum.FormMode.Clone;
      } else {
        return this.$Enum.FormMode.Add;
      }
    },
  },
  methods: {
    /**
     * Hàm lấy ra mã khách hàng mới
     * Author: LQHUY (26/11/2023)
     */
    async getNewEmployeeCode() {
      this.employee = {};
      this.$emitter.emit("toggleShowLoading", true);
      try {
        var res = await this.$httpRequest.get("Employees/NewCode");
        switch (res.status) {
          case 200:
            this.employee.EmployeeCode = res.data;
            this.$refs["EmployeeCode"].removeBorderError();
            this.$refs["EmployeeCode"].setFocus();
            setTimeout(() => {
              this.$emitter.emit("toggleShowLoading", false);
            }, 300);
            break;
          default:
            break;
        }
      } catch (error) {
        this.$emitter.emit("handleApiError", error);
        this.$emitter.emit("toggleShowLoading", false);
      }
    },

    /**
     * Hàm lấy ra thông tin chi tiết khách hàng
     * Author: LQHUY (26/11/2023)
     */
    async getEmployeeDetail() {
      this.$emitter.emit("toggleShowLoading", true);
      try {
        const res = await this.$httpRequest.get(
          `Employees/${this.selectedEmployeeId}`
        );
        switch (res.status) {
          case 200:
            this.employee = res.data;
            this.employeeCheck = Object.assign({}, this.employee);
            if (this.employee.DateOfBirth) {
              this.employee.DateOfBirth = this.$helper.formatDate(
                this.employee.DateOfBirth,
                true
              );
            }
            if (this.employee.IdentityDate) {
              this.employee.IdentityDate = this.$helper.formatDate(
                this.employee.IdentityDate,
                true
              );
            }
            if (this.employee.Salary) {
              this.employee.Salary = this.$helper.formatMoney(
                this.employee.Salary
              );
            }
            setTimeout(() => {
              this.$emitter.emit("toggleShowLoading", false);
            }, 500);
            break;
          default:
            break;
        }
      } catch (error) {
        this.$emitter.emit("handleApiError", error);
        this.$emitter.emit("toggleShowLoading", false);
      }
    },

    /**
     * Hàm kiểm tra các giá trị có thỏa mãn hay không và thêm mới khách hàng nếu show form là true thì
     * hiển thị lại form sau khi thêm
     * @param {Boolean} showForm
     * Author: LQHUY (26/11/2023)
     */
    isCheckData(showForm) {
      try {
        this.lstErorr = [];
        this.validateData();
        if (this.lstErorr.length > 0) {
          this.setFocusInputFirstError();
          return;
        }
        this.transferData();
        if (this.formMode === this.$Enum.FormMode.Add) {
          this.addNewEmployee(showForm);
        } else if (this.formMode === this.$Enum.FormMode.Clone) {
          this.addNewEmployee(showForm);
        } else {
          this.editEmployee();
        }
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm kiểm tra các giá trị nhập vào có hợp lệ không
     * Author: LQHUY (26/11/2023)
     */
    validateData() {
      try {
        this.setError("EmployeeCode", this.$refs.EmployeeCode.label);
        this.setError("Fullname", this.$refs.Fullname.label);
        this.setError("DepartmentId", this.$refs.DepartmentId.label);
        if (this.employee.DateOfBirth < this.$helper.formatDate(Date.now)) {
          this.$refs.DateOfBirth.removeBorderError();
        }
        // if (this.employee.Email) {
        //   this.validateEmail("Email");
        // } else {
        //   this.setError("Email", this.$refs.txtEmail.label);
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm xét message lỗi
     * @param {string} field
     * @param {string} title
     * Author: LQHUY (26/11/2023)
     */
    setError(field, title) {
      try {
        if (
          this.employee[field] === "" ||
          this.employee[field] === null ||
          this.employee[field] === undefined
        ) {
          this.lstErrorMessage[`err${field}`] =
            this.$Resource[this.$languageCode].ErrorMessage(title);
          this.lstErorr.push(field);
        } else {
          this.lstErorr.filter((item) => item !== field);
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
        if (!this.employee[field].match(/^[\w-.]+@([\w-]+\.)+[\w-]{2,4}$/)) {
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
     * Set vào ô input lỗi đầu tiên
     * Author: LQHUY(07/12/2002)
     */
    setFocusInputFirstError() {
      this.$refs[this.lstErorr[0]].setFocus();
    },

    /**
     * Hàm thực hiện chuyển lại dữ liệu gửi lên API
     * Author: LQHUY (10/12/2023)
     */
    transferData() {
      //Chuyển dữ liệu định dạng trên ô input loại bỏ dấu . chữ đ và khoảng cách
      try {
        if (this.employee.Salary) {
          this.employee.Salary = this.$helper.formatMoneySendApi(
            this.employee.Salary
          );
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Hàm thực hiện thêm khách hàng mới
     * Author: LQHUY (7/12/2023)
     */
    async addNewEmployee(showForm) {
      try {
        const res = await this.$httpRequest.post("Employees", this.employee);
        switch (res.status) {
          case 201:
            this.$emitter.emit("toggleShowForm", false);
            this.successResponse(this.$Resource[this.$languageCode].AddSuccess);
            if (showForm) {
              this.employee = {};
              this.lstErrorMessage = {};
              this.$emitter.emit("toggleShowForm", true);
              await this.getNewEmployeeCode();
            }
            break;
          default:
            break;
        }
      } catch (error) {
        this.$emitter.emit("handleApiError", error);
        this.$emitter.emit("toggleShowLoading", false);
        const keys = Object.keys(error.response.data.errors);
        this.$refs[keys[0]].setFocus();
        keys.forEach((key) => {
          this.$refs[key].setBorderError();
        });
      }
    },

    /**
     * Hàm thực hiện sửa thông tin khách hàng
     * Author: LQHUY (7/12/2023)
     */
    async editEmployee() {
      try {
        const res = await this.$httpRequest.put(
          `Employees/${this.selectedEmployeeId}`,
          this.employee
        );
        switch (res.status) {
          case 200:
            this.$emitter.emit("toggleShowForm", false);
            this.successResponse(
              this.$Resource[this.$languageCode].EditSuccess
            );
            break;
          default:
            break;
        }
      } catch (error) {
        this.$emitter.emit("handleApiError", error);
        this.$emitter.emit("toggleShowLoading", false);
        const keys = Object.keys(error.response.data.errors);
        this.$refs[keys[0]].setFocus();
        keys.forEach((key) => {
          this.$refs[key].setBorderError();
        });
      }
    },

    /**
     * Hàm lấy thông tin nhân bản 1 khách hàng
     * Author: LQHUY (18/01/2024)
     */
    async getCloneInfoEmployee() {
      this.$emitter.emit("toggleShowLoading", true);
      try {
        const res = await this.$httpRequest.get(
          `Employees/${this.selectedEmployeeIdClone}`
        );
        const resNewCode = await this.$httpRequest.get("Employees/NewCode");
        switch (res.status) {
          case 200:
            this.employee = res.data;
            this.employeeCheck = Object.assign({}, this.employee);
            if (this.employee.DateOfBirth) {
              this.employee.DateOfBirth = this.$helper.formatDate(
                this.employee.DateOfBirth,
                true
              );
            }
            if (this.employee.Salary) {
              this.employee.Salary = this.$helper.formatMoney(
                this.employee.Salary
              );
            }
            this.employee.EmployeeCode = resNewCode.data;
            setTimeout(() => {
              this.$emitter.emit("toggleShowLoading", false);
            }, 500);
            break;
          default:
            break;
        }
      } catch (error) {
        this.$emitter.emit("handleApiError", error);
        this.$emitter.emit("toggleShowLoading", false);
      }
    },
    /**
     * Hàm so sánh 2 đối tượng
     */
    compareValueObject() {
      console.log("message")
      const isALike = this.$helper.compareObjectEqual(
        this.employee,
        this.employeeCheck
      );
      return isALike;
    },
    /**
     * Hàm xử lí khi thêm hoặc sửa thông tin thành công
     * @param {string} message
     * Author: LQHUY
     */
    successResponse(message) {
      try {
        this.$emitter.emit("toggleShowForm", false);

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
      employee: {
        Gender: 0,
      },
      employeeCheck: {},
      isCheck: true,
      lstErrorMessage: {},
      lstErorr: [],
    };
  },
};
</script>
<style scoped></style>
