<template>
  <v-dialog
    content-class="cs-confirm-dialog"
    v-model="visible"
    :width="widthDialog"
    :max-width="maxWidthDialog"
    persistent
    @keydown.esc.exact="cancelClick"
    @keydown.enter.exact="okClick"
  >
    <v-card>
      <v-card-title class="cs-confirm-dialog-text title" v-html="title"></v-card-title>
      <v-container class="pt-0" grid-list-md>
        <v-card-text class="cs-confirm-dialog-text content" v-html="message"></v-card-text>
      </v-container>
      <v-divider></v-divider>
      <v-card-actions class="pa-3">
        <v-spacer></v-spacer>
        <v-btn text @click="cancelClick" class="cs-btn">{{ cancelText }}</v-btn>
        <v-btn small @click="okClick" class="cs-btn primary">{{
          okText
        }}</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
<script>
export default {
  name: "ConfirmMessage",
  props: ["width", "maxWidth"],
  data() {
    return {
      visible: false,
      title: "",
      message: "",
      okText: "Confirm",
      cancelText: "Cancel",
    };
  },
  computed: {
    widthDialog() {
      return Number(this.width || 500);
    },
    maxWidthDialog() {
      return Number(this.maxWidth || 800);
    },
  },
  methods: {
    clear() {
      Object.assign(this.$data, this.$options.data());
    },
    cancelClick() {
      this.clear();
      if (this._OnCancelClick && typeof this._OnCancelClick === "function") {
        this._OnCancelClick();
      }
    },
    okClick() {
      this.clear();
      if (this._OnOKClick && typeof this._OnOKClick === "function") {
        this._OnOKClick();
      }
    },
    show(title, message, onOKClick, okCancelClick, okMessage, cancelMessage) {
      this.visible = true;
      this.title = title;
      this.message = message;

      if (okMessage) {
        this.okText = okMessage;
      }

      if (cancelMessage) {
        this.cancelText = cancelMessage;
      }

      this._OnOKClick = onOKClick;
      this._OnCancelClick = okCancelClick;
    },
  },
};
</script>

<style lang="scss">
@import "@/scss/components/dialog/ConfirmMessage.scss";
</style>
