import app from "@/main.js";
const helper = {
  /**
   * Hàm định dạng ngày tháng
   * @param {date} date giá trị ngày tháng
   * @param {boolean} displayOnForm xác định xem hiển thị lên table hay form
   * @returns trả về định dạng để hiển thị lên form và table
   * Author: LQHUY
   */
  formatDate(date, displayOnForm) {
    try {
      if (date) {
        //chuyển về dũ liệu date trong js
        let dob = new Date(date);
        let day = dob.getDate() < 10 ? `0${dob.getDate()}` : dob.getDate();
        let month =
          dob.getMonth() + 1 < 10
            ? `0${dob.getMonth() + 1}`
            : dob.getMonth() + 1;
        let year = dob.getFullYear();
        if (displayOnForm) {
          return `${year}-${month}-${day}`;
        } else {
          return `${day}/${month}/${year}`;
        }
      } else {
        return "";
      }
    } catch (error) {
      console.error(error);
    }
  },

  /**
   * Hàm định dạng giới tính
   * @param {int} gender giá trị ngày tháng
   * @returns trả về định dạng để hiển thị lên form và table
   * Author: LQHUY
   */
  formatGender(gender) {
    const appConfig = app.config.globalProperties;
    try {
      if (gender !== null || gender !== undefined) {
        switch (gender) {
          case appConfig.$Enum.Gender.Male:
            return appConfig.$Resource[appConfig.$languageCode].Gender.Male;
          case appConfig.$Enum.Gender.Female:
            return appConfig.$Resource[appConfig.$languageCode].Gender.Female;
          case appConfig.$Enum.Gender.Other:
            return appConfig.$Resource[appConfig.$languageCode].Gender.Other;
          default:
            return "";
        }
      }
    } catch (error) {
      console.error(error);
    }
  },
  /**
   * Hàm định dạng tiền
   * @param {double} money
   * @returns định dạng muốn hiển thị (VNĐ)
   * Author: LQHUY (24/11/2023)
   */
  formatMoney(money) {
    try {
      if (money !== null && money !== undefined) {
        money = new Intl.NumberFormat("vi-VN", {
          style: "currency",
          currency: "VND",
        }).format(money);
        return money;
      } else {
        return "";
      }
    } catch (error) {
      console.error(error);
    }
  },
  /**
   * Hàm định dạng lại giá trị là tiển từ input gửi lên API
   * @param {string} value
   * @returns giá trị đúng
   * Author: LQHUY(13/12/2023)
   */
  formatMoneySendApi(value) {
    try {
      if (value) {
        return value
          .replace(/[^\dđ]/gi, "")
          .replace(/đ/gi, "d")
          .replace(/d/gi, "");
      }
      return "";
    } catch (error) {
      console.error(error);
    }
  },

  /**
   * Hàm chuyển chuỗi tiếng Việt thành k dấu và loại bỏ các ký tự đặc biệt
   * @param {string} str
   * @returns string
   * Author: LQHUY(26/01/2024)
   */
  removeVietnameseTones(str) {
    str = str.toLowerCase();
    str = str.replace(/[áàảãạăắằẳẵặâấầẩẫậ]/g, "a");
    str = str.replace(/[éèẻẽẹêếềểễệ]/g, "e");
    str = str.replace(/[íìỉĩị]/g, "i");
    str = str.replace(/[óòỏõọôốồổỗộơớờởỡợ]/g, "o");
    str = str.replace(/[úùủũụưứừửữự]/g, "u");
    str = str.replace(/[ýỳỷỹỵ]/g, "y");
    str = str.replace(/đ/g, "d");
    return str;
  },
  compareObjectEqual(object1, object2) {
    const keys1 = Object.keys(object1);
    const keys2 = Object.keys(object2);

    if (keys1.length !== keys2.length) {
      return false;
    }

    for (const key of keys1) {
      const val1 = object1[key];
      const val2 = object2[key];
      const areObjects = this.isObject(val1) && this.isObject(val2);
      if (
        (areObjects && !this.deepObjectEqual(val1, val2)) ||
        (!areObjects && val1 !== val2)
      ) {
        return false;
      }
    }
    return true;
  },
  isObject(object) {
    return object != null && typeof object === "object";
  },
};

export default helper;
