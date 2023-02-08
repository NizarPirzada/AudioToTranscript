<template>
  <v-snackbar
    :timeout="timeOut"
    v-model="visible"
    :bottom="bottom"
    :top="!bottom"
    :color="color"
    outlined
    right
    class="cs-snackbar"
  >
    <v-layout>
      <span v-html="message"></span>
    </v-layout>
  </v-snackbar>
</template>
<script>
export default {
  name: "SnackbarMessage",
  data() {
    return {
      visible: false,
      message: "",
      timeOut: -1,
      color: "primary",
      bottom: true,
    };
  },
  methods: {
    show(message, color, timeOut, bottom) {
      this.message = message;
      this.color = color;
      this.visible = true;
      this.timeOut = timeOut;
      this.bottom = bottom;
    },
  },
};
</script>

<style lang="scss" scoped>
@import "@/scss/components/layout/SnackbarMessage.scss";
</style>
