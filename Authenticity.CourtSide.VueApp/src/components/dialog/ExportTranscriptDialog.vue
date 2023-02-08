<template>
  <v-dialog v-model="show" width="547" height="306" persistent>
    <v-card>
      <br />
      <v-radio-group v-model="selectedExportOption" mandatory>
        <v-radio
          v-for="(item, key) in exportOptions"
          :key="key"
          :label="item.label"
          :value="item.id"
          color="black"
        ></v-radio>
      </v-radio-group>
      <div class="cs-select-container">
        <select
          class="cs-select"
          v-model="selectedLanguage"
          :disabled="selectedExportOption != 2"
        >
          <option
            v-for="option in translationLanguages"
            v-bind:value="option.apiLanguageId"
            :key="option.apiLanguageId"
          >
            {{ option.name }}
          </option>
        </select>
      </div>
      <br />
      <br />

      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn
          class="cs-btn"
          text
          @click="cancelAction"
          :disabled="isExportingFile"
        >
          {{ $t("app_cancel_button") }}
        </v-btn>
        <v-btn
          class="cs-btn"
          color="primary"
          @click="exportAction()"
          :loading="isExportingFile"
          :disabled="isExportingFile || !isValidExportOption"
        >
          {{ $t("step4_export_button") }}
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  name: "ExportTranscriptDialog",
  data: () => ({
    selectedExportOption: 1,
    selectedLanguage: 0,
  }),
  props: {
    transcript: {
      type: Object,
      required: true,
    },
    isExportingFile: {
      type: Boolean,
      required: true,
    },
    show: {
      type: Boolean,
      required: true,
    },
    persistent: {
      type: Boolean,
      default: true,
    },
    allTranslationLanguages: {
      type: Array,
      required: true,
    },
  },
  model: {
    prop: "show",
    event: "input",
  },
  computed: {
    exportOptions() {
      let options = [
        { id: 1, label: this.$t("step4_export_option_just_export") },
        { id: 2, label: this.$t("step4_export_option_translate_export") },
      ];
      if (
        this.transcript.humanTranscriptionStatus !== 0 &&
        this.transcript.humanTranscriptionStatus !== 3) {
        options.push({
          id: 3,
          label: this.$t("step4_export_option_human_export"),
        });
      }
      return options;
    },
    translationLanguages() {
      const languages = this.allTranslationLanguages.map((m) => ({
        ...m,
        name: this.$t(`step4_export_language_${m.name?.toLowerCase()}`),
      }));

      languages.unshift({
        apiLanguageId: 0,
        name: this.$t("step4_export_default_language"),
      });

      return languages;
    },
    isValidExportOption() {
      return (
        this.selectedExportOption === 1 ||
        this.selectedExportOption === 3 ||
        (this.selectedExportOption === 2 && this.selectedLanguage != "0")
      );
    },
  },
  methods: {
    cancelAction() {
      this.$emit("input", false);
      this.selectedLanguage = 3;
      this.selectedExportOption = 1;
    },
    exportAction() {
      const language = this.translationLanguages.find(
        (obj) => obj.apiLanguageId === this.selectedLanguage
      );
      let exportSettings = {
        exportTye: this.selectedExportOption,
        apiLanguageId: language.apiLanguageId,
        languageLabel: language.name,
      };
      this.$emit("export", exportSettings);
    },
  },
};
</script>
<style lang="scss">
@import "@/scss/components/dialog/ExportTranscriptDialog.scss";
</style>