<template>
  <v-card class="d-flex align-center justify-center flex-wrap" elevation="0">
    <v-progress-circular
      indeterminate
      :size="40"
      color="primary"
    ></v-progress-circular>
    <span class="cs-saving-transcript-label">{{ savingLabel }}</span>
  </v-card>
</template>

<script>
export default {
  name: "SavingTranscript",
  props: {},
  data: () => {
    return {};
  },
  computed: {
    savingLabel() {
      return this.$t("wizard_saving_transcript");
    },
  },
};
</script>
<style lang="scss">
@import "@/scss/components/wizard/SavingTranscript.scss";
</style>