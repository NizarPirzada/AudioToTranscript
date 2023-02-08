<template>
  <v-dialog
    content-class="cs-progress-dialog"
    v-model="show"
    max-width="550"
    persistent
    @keydown.enter.exact="closeClick"
  >
    <v-card>
      <v-container grid-list-md class="pa-0">
        <v-card-text>
          <v-list-item v-for="(item, i) in items" :key="i">
            <v-list-item-icon>
              <v-progress-circular
                v-if="item.status == 'inProgress'"
                indeterminate
                :size="25"
                color="primary"
              ></v-progress-circular>
              <v-icon v-else-if="item.status == 'finished'" color="primary" large
                >mdi-checkbox-marked-circle-outline</v-icon
              >
              <v-icon v-else-if="item.status == 'error'" color="error"
                >mdi-close-circle-outline</v-icon
              >
            </v-list-item-icon>
            <v-list-item-content>
              <v-list-item-title class="cs-progress-dialog-text title text-wrap" v-html="item.message">
              </v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </v-card-text>
      </v-container>
      <v-card-actions class="pt-0 pb-4 justify-center">
        <v-btn
          color="primary"
          @click="closeClick"
          class="cs-btn"
          :disabled="disableCloseButton"
          >{{ closeText }}</v-btn
        >
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
<script>
export default {
  name: "ProgressDialog",
  props: {
    show: {
      type: Boolean,
      required: true,
    },
    items: {
      type: Array,
    },
    disableCloseButton: {
      type: Boolean,
    },
  },
  data() {
    return {
      visible: false,
    };
  },
  computed: {
    closeText() {
      return this.$t("app_close_button");
    },
  },
  methods: {
    clear() {
      Object.assign(this.$data, this.$options.data());
    },
    closeClick() {
      this.$emit("update:show", false);
      this.$emit("update:items", []);
    },
  },
};
</script>

<style lang="scss">
@import "@/scss/components/dialog/ProgressDialog.scss";
</style>
