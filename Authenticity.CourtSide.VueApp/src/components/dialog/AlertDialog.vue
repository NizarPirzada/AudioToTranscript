<template>
  <v-dialog
    content-class="cs-alert-dialog"
    v-model="visible"
    width="409"
    persistent
    @keydown.enter.exact="closeClick"
  >
    <v-card>
      <v-container grid-list-md class="pa-0">
        <v-card-text class="pa-0">
          <div class="cs-alert-dialog-text">
            <p v-html="message" class="text-center"></p>
          </div>
        </v-card-text>
      </v-container>
      <v-card-actions class="pt-0 pb-4 justify-center">
        <v-btn color="primary" @click="closeClick" class="cs-btn">{{
          closeText
        }}</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
<script>
export default {
  name: "AlertDialog",
  data() {
    return {
      visible: false,
      message: "",
      closeText: "Close",
    };
  },
  computed: {
    widthDialog() {
      return Number(this.width || 400);
    },
    maxWidthDialog() {
      return Number(this.maxWidth || 800);
    },
  },
  methods: {
    clear() {
      Object.assign(this.$data, this.$options.data());
    },
    closeClick() {
      this.clear();
      if (this._OnCloseClick && typeof this._OnCloseClick === "function") {
        this._OnCloseClick();
      }
    },
    show(message, okCloseClick) {
      this.visible = true;
      this.message = message;
      this._OnCloseClick = okCloseClick;
    },
  },
};
</script>

<style lang="scss">
@import "@/scss/components/dialog/AlertDialog.scss";
</style>
