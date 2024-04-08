<template>
  <MDialog @compareValueObject="compareValueObject">
    <template #body>
      <slot name="form"></slot>
    </template>
    <template #footer>
      <MButton
        v-tippy="{
          content: this.$Resource[this.$languageCode].Text.Cancel,
          placement: 'top',
        }"
        @click="this.$emitter.emit('toggleShowForm')"
        class="m-button--sub"
        :text="this.$Resource[this.$languageCode].Text.Cancel"
      ></MButton>
      <div
        v-if="
          formMode === this.$Enum.FormMode.Add ||
          formMode === this.$Enum.FormMode.Clone
        "
        class="m-dialog__group-button"
      >
        <MButton
          v-tippy="{
            content: this.$Resource[this.$languageCode].Text.Save,
            placement: 'top',
          }"
          @click="btnSaveOn"
          :text="this.$Resource[this.$languageCode].Text.Save"
          class="m-button--sub"
        ></MButton>
        <MButton
          v-tippy="{
            content: this.$Resource[this.$languageCode].Text.SaveAndAdd,
            placement: 'top',
          }"
          @click="btnSaveOnAndAdd"
          :text="this.$Resource[this.$languageCode].Text.SaveAndAdd"
        ></MButton>
      </div>
      <div
        v-else-if="formMode === this.$Enum.FormMode.Edit"
        class="m-dialog__group-button"
      >
        <MButton
          v-tippy="{
            content: this.$Resource[this.$languageCode].Text.EditInfo,
            placement: 'top',
          }"
          @click="btnSaveOn"
          :text="this.$Resource[this.$languageCode].Text.EditInfo"
        ></MButton>
      </div>
    </template>
  </MDialog>
</template>
<script>
export default {
  name: "MForm",
  props: ["formMode", "compareValueObject"],
  emits: ["isCheckData","compareValueObject"],
  methods: {
    /**
     * Hàm xử lí thêm mới một đối tượng
     * Author: LQHUY(07/12/2002)
     */
    btnSaveOn() {
      this.$emit("isCheckData", false);
    },

    /**
     * Hàm xử lí nhân bản một đối tượng
     * Author: LQHUY(07/12/2002)
     */
    btnSaveOnAndAdd() {
      this.$emit("isCheckData", true);
    },
  },
};
</script>
<style scoped>
@import url(../dialog/dialog.css);
</style>
