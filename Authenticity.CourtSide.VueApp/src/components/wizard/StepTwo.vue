<template>
  <v-container class="py-0">
    <v-row no-gutters>
      <v-col cols="6">
        <FileUploader
          ref="fileUploader"
          :transcript="transcript"
          :fileList="audioFiles"
          :currentFiles="files"
          :isSavingFiles="isSavingFiles"
          :uploadProgress="uploadProgress"
        />
      </v-col>
    </v-row>

    <v-row no-gutters>
      <v-col cols="6">
        <v-card class="cs-step-card">
          <v-card-text>
            <span class="step-section-header">{{
              $t("step2_datetime_title")
            }}</span>
            <v-layout row wrap class="step-layout-horizontal">
              <v-menu
                v-model="datePicker"
                :close-on-content-click="false"
                transition="scale-transition"
                min-width="auto"
              >
                <template v-slot:activator="{ on, attrs }">
                  <v-text-field
                    v-model="transcriptDateFormatted"
                    class="step-text-field medium"
                    :label="$t('step2_datetime_label')"
                    append-icon="mdi-calendar"
                    @click:append="datePicker = !datePicker"
                    outlined
                    readonly
                    v-bind="attrs"
                    v-on="on"
                  ></v-text-field>
                </template>
                <v-date-picker
                  v-model="transcriptDate"
                  @input="datePicker = false"
                  no-title
                  color="primary"
                  :first-day-of-week="0"
                  :locale="currentUserLocale"
                ></v-date-picker>
              </v-menu>
            </v-layout>
            <v-layout row wrap class="step-layout-horizontal">
              <v-text-field
                class="step-text-field medium"
                :label="$t('step2_starttime_label')"
                type="time"
                hide-details
                outlined
                v-model="transcriptTime"
              ></v-text-field>
            </v-layout>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <v-row no-gutters>
      <v-col cols="6">
        <v-card class="cs-step-card" color="white">
          <v-card-text>
            <span class="step-section-header pb-3">{{
              $t("step2_language_title")
            }}</span>
            <v-layout row wrap class="step-layout-horizontal">
              <select
                class="cs-select cs-select-transcript-language"
                v-model="transcript.apiLanguageId"
                :disabled="!enableLanguageInput"
              >
                <option
                  v-for="(item, i) in transcriptionLanguages"
                  :key="i"
                  v-bind:value="item.apiLanguageId"
                  v-text="item.name"
                ></option>
              </select>
            </v-layout>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

     <StepperActionButtons
      :buttonList="buttonList"
      @next="next"
      @back="back"
    ></StepperActionButtons>
  </v-container>
</template>

<script>
import FileUploader from "@/components/files/FileUploader";
import { FileService } from "@/services/fileService";
import { mapActions } from "vuex";
import StepperActionButtons from "@/components/wizard/StepperActionButtons";

export default {
  name: "StepTwo",
  data: () => ({
    audioFiles: [],
    transcriptDate: "",
    transcriptTime: "",
    uploaded: false,
    isSavingFiles: false,
    filePath: "",
    uploadProgress: {
      percentage: 0,
    },
    datePicker: false,
    allTranscriptionLanguages: [],
  }),
  components: {
    FileUploader,
    StepperActionButtons,
  },
  props: {
    transcript: {
      type: Object,
      required: true,
    },
    files: {
      type: Array,
      required: true,
    },
    active: {
      type: Boolean,
      required: true,
    },
  },
  computed: {
    hasAudioFile() {
      return (
        (this.audioFiles && this.audioFiles.length > 0) ||
        (this.files && this.files.length > 0)
      );
    },
    transcriptDateFormatted() {
      return this.formatDate(this.transcriptDate);
    },
    currentUserLocale() {
      return this.$currentUserLocale();
    },
    transcriptionLanguages() {
      const languages = this.allTranscriptionLanguages.map((m) => ({
        ...m,
        name: this.$t(`step2_language_${m.name?.toLowerCase()}`),
      }));
      return languages;
    },
    enableLanguageInput() {
      return this.transcript.step === 2;
    },
    buttonList() {
      return [
        {
          text: this.$t("step_back_button"),
          disabled: this.isSavingFiles,
          icon: "mdi-chevron-left",
          color: "primary",
          action: "back",
        },
        {
          text: this.$t("step_next_button"),
          disabled: (!this.hasAudioFile && !this.active) || this.isSavingFiles,
          icon: "mdi-chevron-right",
          color: "primary",
          action: "next",
        },
      ];
    },
  },
  methods: {
    ...mapActions("transcript", ["getAllTranscriptionLanguagesAsync"]),
    setDateValues(dateValue) {
      this.transcriptDate = "";
      this.transcriptTime = "";
      if (dateValue) {
        const dateObject = new Date(this.$formatDateTime(dateValue));
        if (dateObject.getFullYear() !== new Date(null).getFullYear()) {
          this.transcriptDate = this.$formatHTMLInputDate(dateValue);
        }
        this.transcriptTime = this.$formatHourMinute(dateValue);
      }
    },
    processTranscriptToSave(fileMetadata) {
      this.transcript.file = null;
      if (fileMetadata) {
        this.transcript.file = fileMetadata;
        this.filePath = fileMetadata.filePath;
      }
      this.transcript.transcriptDate = this.transcriptDate;
      this.transcript.transcriptTime = this.transcriptTime;
      this.$emit("next");
    },
    validateFiles() {
      this.uploaded = false;
      if (this.audioFiles.length != 0) {
        this.submitFiles();
      } else if (this.files.length > 0) {
        this.processTranscriptToSave(null);
      }
    },
    submitFiles() {
      this.isSavingFiles = true;
      for (var i = 0; i < this.audioFiles.length; i++) {
        const file = this.audioFiles[i];
        this.submitOneFile(i, file);
      }
    },
    async submitOneFile(i, file) {
      let vm = this;
      this.isSavingFiles = true;
      const fileIndex = `${i}-${file.name}`;

      await FileService.FileUploadAsync(
        this.setUploadProgress,
        this.transcript.id,
        fileIndex,
        file,
        file.size
      )
        .then(function (data) {
          if (!data.success) {
            this.setUploadProgress({ fileIndex, value: 100, status: "error" });
          } else {
            vm.processTranscriptToSave({
              name: file.name,
              filePath: data.data,
              size: file.size,
            });
          }
        })
        .catch((error) => {
          this.isLoading = false;
          this.setUploadProgress({ fileIndex, value: 100, status: "error" });
          vm.processTranscriptToSave({
            name: file.name,
            filePath: "",
            size: file.size,
          });
        });
      this.isSavingFiles = false;
      this.audioFiles = [];
      this.uploaded = true;
    },
    setUploadProgress(uploadInformation) {
      this.uploadProgress.percentage = uploadInformation.value;
    },
    next() {
      this.validateFiles();
    },
    back() {
      this.$emit("back", { name: "name" });
    },
    formatDate(date) {
      if (!date) return null;

      const [year, month, day] = date.split("-");
      return `${month}/${day}/${year}`;
    },
  },
  watch: {
    transcript: function (val) {
      this.setDateValues(val.recordDate);
    },
  },
  async mounted() {
    this.setDateValues(this.transcript.recordDate);

    await this.getAllTranscriptionLanguagesAsync().then((response) => {
      if (response.success) {
        this.allTranscriptionLanguages = response.data;
      }
    });
  },
};
</script>
<style lang="scss">
@import "@/scss/components/wizard/StepTwo.scss";
</style>