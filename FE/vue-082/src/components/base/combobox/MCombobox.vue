<template>
  <div class="form-group">
    <label for="" class="m-lable" :class="{ 'label--required': required }">{{
      label
    }}</label>
    <div class="m-textfiled-icon m-combobox" :id="id">
      <input
        :placeholder="this.$Resource[this.$languageCode].Text.SelectValue"
        @keydown="onKeyDownInput"
        type="text"
        @input="changeValueInput"
        v-model="outputText"
        :class="{ 'm-textfield-error': messageError }"
        class="m-textfield"
        ref="input"
        @blur="onBulrInput"
      />
      <div
        @click="onClickIconDown"
        v-show="isShowIconDown"
        class="m-textfiled-icon--down"
      >
        <i class="fa-solid fa-angle-down"></i>
      </div>
      <div
        @click="onClickIconUp"
        v-show="isShowIconUp"
        class="m-textfiled-icon--up"
      >
        <i class="fa-solid fa-angle-up"></i>
      </div>
      <div
        class="m-combobox-list"
        style="
          font-size: 14px;
          display: flex;
          align-items: center;
          justify-content: center;
        "
        v-if="isShowNoValue"
      >
        {{ this.$Resource[this.$languageCode].ElementNotFound }}
      </div>
      <ul ref="combobox-list" class="m-combobox-list" v-show="isShowCombobox">
        <div
          v-for="(item, index) in dataFilter"
          :key="item[propValue]"
          class="m-combobox-list__item"
          @click="onClickSelectedItem(item)"
          :class="{
            'hover-item': lstItemHover[index],
            selected: item[this.propText] === this.outputText,
          }"
        >
          <li class="m-combobox-list__item-text">
            {{ item[propText] }}
          </li>
          <div class="m-combobox-list__item-icon">
            <i class="fa-solid fa-check"></i>
          </div>
        </div>
      </ul>
    </div>
    <span class="m-error-message">{{ messageError }}</span>
  </div>
</template>
<script>
const keyCode = {
  Enter: 13,
  ArrowUp: 38,
  ArrowDown: 40,
  ESC: 27,
};
export default {
  name: "MCombobox",
  props: {
    label: {
      type: String,
      required: false,
    },
    id: {
      type: String,
      required: true,
    },
    url: {
      type: String,
      required: true,
    },
    modelValue: {
      type: [String, Number],
      required: false,
    },
    propValue: {
      type: String,
      required: true,
    },
    propText: {
      type: String,
      required: true,
    },
    message: {
      type: String,
      required: false,
      default: "",
    },
    required: {
      type: Boolean,
      default: false,
    },
  },
  created() {
    this.loadData();
    this.messageError = this.message;
  },
  data() {
    return {
      outputText: "",
      dataCombobox: [],
      dataFilter: [],
      currentIndex: 0,
      isShowIconUp: false,
      isShowIconDown: true,
      isShowCombobox: false,
      lstItemHover: [],
      isShowNoValue: false,
      messageError: "",
      direction: null,
    };
  },
  watch: {
    async modelValue(newValue) {
      if (newValue && this.propText) {
        var res = await this.$httpRequest.get(this.url);
        this.dataCombobox = res.data;
        const itemSelected = this.dataCombobox.find((item) => {
          return item[this.propValue] === newValue;
        });
        if (itemSelected) {
          this.outputText = itemSelected[this.propText];
        }
      } else {
        this.outputText = "";
      }
    },
    outputText(newValue) {
      if (newValue === null || newValue === "" || newValue === undefined) {
        this.messageError = this.message;
        //nếu giá trị mới là rỗng hoặc null thì cập nhật giá trị = null
      } else {
        this.messageError = null;
      }
    },
    /**
     * Hàm theo dõi message lỗi truyển vào
     * @param {string} newValue
     * Author: LQHUY(08/12/2023)
     */
    message(newValue) {
      this.messageError = newValue;
    },
  },
  methods: {
    /**
     * Load dữ liệu combobox
     * Author: LQHUY(02/12/2023)
     */
    async loadData() {
      try {
        var res = await this.$httpRequest.get(this.url);
        switch (res.status) {
          case 200:
            this.dataCombobox = res.data;
            this.dataFilter = res.data;
            break;
          default:
            break;
        }
      } catch (error) {
        this.$emitter.emit("handleApiError", error);
      }
    },
    /**
     * Hàm xử lý khi click vào icon down
     * Author: LQHUY (02/12/2023)
     */
    onClickIconDown() {
      try {
        //hiện combobox
        this.onShowcombobox();
      } catch (error) {
        console.error(error);
      }
    },
    /**
     * Hàm click vào icon up
     * Author: LQHUY (02/12/2023)
     */
    onClickIconUp() {
      try {
        //ẩn combobox
        this.onHideCombobox();
      } catch (error) {
        console.error(error);
      }
    },
    /**
     * Hàm hiển thị combobox
     * Author: LQHUY (02/12/2023)
     */
    onShowcombobox() {
      try {
        //hiện combobox và foucs vào ô input
        this.isShowCombobox = true;
        this.$refs["input"].focus();
        this.lstItemHover[0] = true;
        this.onShowIcon(true);
        this.isShowNoValue = false;
        //set lại index
        this.currentIndex = 0;
      } catch (error) {
        console.error(error);
      }
    },
    /**
     * Hàm ẩn combobox
     * Author: LQHUY (02/12/2023)
     */
    onHideCombobox() {
      try {
        //hiện combobox và foucs vào ô input
        this.isShowCombobox = false;
        this.onShowIcon(false);
        this.removeHoverItem();
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm xử lý ẩn hiện icon up với down
     * @param {boolean} iconUp
     * Author: LQHUY (02/12/2023)
     */
    onShowIcon(iconUp) {
      try {
        if (iconUp) {
          this.isShowIconUp = true;
          this.isShowIconDown = false;
        } else {
          this.isShowIconUp = false;
          this.isShowIconDown = true;
        }
      } catch (error) {
        console.error(error);
      }
    },
    /**
     * Hàm thực hiện sự kiện oninput
     * @param {event} e
     * Author: LQHUY (02/12/2023)
     */
    changeValueInput() {
      try {
        let value = this.outputText;
        
        //lấy ra mảng giá trị match với giá trị input
        this.dataFilter= this.dataCombobox.filter((item) =>
          this.$helper
            .removeVietnameseTones(item[this.propText])
            .includes(this.$helper.removeVietnameseTones(value))
        );
        //nếu mảng rỗng hiển thị không có giá trị phù hợp
        if (this.dataFilter.length == 0) {
          //hiển thị không có giá trị nào phù hợp
          this.isShowNoValue = true;
          //ẩn combobox
          this.onHideCombobox();
          return;
        }
        this.onShowcombobox();

      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm xử lý khi click vào mỗi item
     * Author: LQHUY (02/12/2023)
     */
    onClickSelectedItem(item) {
      try {
        //set value cho input
        this.outputText = item[this.propText];
        this.$emit("update:modelValue", item[this.propValue]);
        //ẩn combobox và focus lại vào input
        this.onHideCombobox();
        this.$refs["input"].focus();
        this.isShowNoValue = false;
      } catch (error) {
        console.error(error);
      }
    },

    /**
     * Hàm thực hiện sự kiện keydown
     * Author: LQHUY (02/12/2023)
     */
    onKeyDownInput(e) {
      try {
        const keyCodePress = e.keyCode;
        switch (keyCodePress) {
          case keyCode.ArrowDown:
            this.currentIndex++;
            this.addHoverItem();
            this.direction = keyCode.ArrowDown;
            this.autoScroll();
            break;
          case keyCode.ArrowUp:
            this.currentIndex--;
            this.addHoverItem();
            this.direction = keyCode.ArrowUp;
            this.autoScroll();
            break;
          case keyCode.Enter:
            //set value cho input
            this.outputText =
              this.dataFilter[this.currentIndex][this.propText];
            this.$emit(
              "update:modelValue",
              this.dataFilter[this.currentIndex][this.propValue]
            );
            //ẩn combobox
            this.onHideCombobox();
            this.$refs["input"].focus();
            this.isShowNoValue = false;
            break;
          case keyCode.ESC:
            this.onHideCombobox();
            break;
          default:
            break;
        }
      } catch (error) {
        console.error(error);
      }
    },
    /**
     * Hàm thực hiện cuộn tự động
     * Author: LQHUY(26/01/2024)
     */
    autoScroll() {
      try {
        let scrollSpeed = 20; // Tốc độ cuộn
        let scrollTop = this.$refs["combobox-list"].scrollTop;
        console.log(scrollTop);
        // Xác định hướng cuộn
        let scrollValue =
          this.direction === keyCode.ArrowUp
            ? scrollTop - scrollSpeed
            : scrollTop + scrollSpeed;
        if (this.currentIndex === 0) {
          scrollValue = 0;
        }
        if (this.currentIndex === this.dataFilter.length - 1) {
          scrollValue = this.$refs["combobox-list"].scrollWidth;
        }
        // Cuộn đến vị trí mới
        this.$refs["combobox-list"].scrollTo({
          top: scrollValue,
          behavior: "smooth",
        });
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * Hàm thêm background khi di chuyển nút xuống trên bàn phím
     * Author: LQHUY (02/12/2023)
     */
    addHoverItem() {
      this.removeHoverItem();
      try {
        this.currentIndex =
          this.currentIndex < 0
            ? this.dataFilter.length - 1
            : this.currentIndex;
        this.currentIndex =
          this.currentIndex > this.dataFilter.length - 1
            ? 0
            : this.currentIndex;
        this.lstItemHover[this.currentIndex] = true;
      } catch (error) {
        console.error(error);
      }
    },
    /**
     * Hàm gỡ bỏ background khi di chuyển nút xuống trên bàn phím
     * Author: LQHUY (02/12/2023)
     */
    removeHoverItem() {
      this.lstItemHover = this.dataFilter.map(() => false);
    },
    /**
     * Set focus vào ô input
     * Author: LQHUY(07/12/2002)
     */
    setFocus() {
      this.$refs["input"].focus();
    },
  },
  mounted() {
    document.addEventListener("click", (e) => {
      if (!this.$el.contains(e.target)) {
        this.onHideCombobox();
        this.isShowNoValue = false;
      }
    });
  },
};
</script>
<style scoped>
@import url(../input/textfield.css);
@import url(./combobox.css);
</style>
