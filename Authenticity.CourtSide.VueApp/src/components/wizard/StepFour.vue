<template>
  <div>
    <ExportTranscriptDialog
      v-model="showExportDialog"
      :transcript="transcript"
      :isExportingFile="downloading"
      :allTranslationLanguages="allTranslationLanguages"
      @export="selectExportAsync"
    ></ExportTranscriptDialog>
    <v-container class="cs-step4-container">
      <v-row>
        <v-col>
          <TranscriptPreview :lines="lines" :linesPerPage="25" />
        </v-col>
        <v-col>
          <div class="cs-human-transcript-container">
            <div class="preview-header">
              <span class="chunk-label-read-only chunk-timelapse">{{ this.$t("step4_transcript_humantranscription_title") }}</span>
            </div>
            <div class="cs-human-transcript"
              v-if="transcript.humanTranscriptionStatus === 0">
              <v-btn
                color="primary"
                class="cs-btn cs-btn-large"
                @click="createHumanTranscription"
              >
                {{ $t("step4_transcript_humantranscription_send_button") }}
              </v-btn>
            </div>
            <div
              class="cs-human-transcript text-center"
              justify="center"
              v-if="transcript.humanTranscriptionStatus !== 0">
              <img
                class="cs-media-processing-img"
                src="../../assets/images/media_processing.png"
              />
              <h1 class="cs-media-processing-title">
                {{ $t("step4_transcript_humantranscription_subtitle") }}
              </h1>
              <p class="cs-media-processing-text">
                {{ $t("step4_transcript_humantranscription_wait_text") }}
              </p>
              <p class="cs-media-processing-text">
                {{ textHumanTranscriptionSent(transcript.lastModifiedOn) }}
              </p>
            </div>
          </div>
        </v-col>
      </v-row>
    </v-container>
    <StepperActionButtons
      :buttonList="buttonList"
      @openExportDialog="openExportDialog"
      @back="back"
    ></StepperActionButtons>
  </div>
</template>

<script>
import { StringHelper, TableHelper } from "@/utilities/Helpers.js";
import TranscriptPreview from "@/components/wizard/TranscriptPreview";
import ExportTranscriptDialog from "@/components/dialog/ExportTranscriptDialog";
import { mapActions } from "vuex";
import { VuexKeys } from "@/./environment.js";
import { SnackbarType } from "@/utilities/Enumerations";
import moment from "moment";
import StepperActionButtons from "@/components/wizard/StepperActionButtons";

const maxCharacterByLine = 66;
const tabFormat = "     ";

export default {
  name: "StepFour",
  data: () => ({
    showExportDialog: false,
    lines: [],
    downloading: false,
    allTranslationLanguages: [],
  }),
  props: {
    transcript: {
      type: Object,
      required: true,
    },
    persons: {
      type: Array,
      required: true,
    },
    dialogs: {
      type: Array,
      required: true,
    },
  },
  components: {
    TranscriptPreview,
    ExportTranscriptDialog,
    StepperActionButtons,
  },
  computed: {
    speakerIndentation() {
      let indentation = "";
      for (let i = 1; i <= 2; i++) {
        indentation = `${indentation}${tabFormat}`;
      }
      return indentation;
    },
    textIndentation() {
      let indentation = "";
      for (let i = 1; i <= 1; i++) {
        indentation = `${indentation}${tabFormat}`;
      }
      return indentation;
    },
    buttonList() {
      return [
        {
          text: this.$t("step_back_button"),
          disabled: false,
          icon: "mdi-chevron-left",
          color: "primary",
          action: "back",
        },
        {
          text: this.$t("step4_export_button"),
          disabled: this.btnNextDisabled,
          icon: "mdi-download",
          color: "primary",
          action: "openExportDialog",
          loading: this.downloading,
        },
      ];
    },
  },
  methods: {
    ...mapActions("transcript", [
      "exportTranscriptAsync",
      "translateExportTranscriptAsync",
      "getAllTranslationLanguagesAsync",
      "createHumanTranscriptionAsync",
      "getTranscriptById",
    ]),
    getDialogs() {
      this.$emit("loadDialogs");
    },
    getPreviousSpace(text, fromPosition) {
      let spacePosition = fromPosition;
      if (text[spacePosition] != " " && fromPosition < text.length) {
        for (let i = fromPosition; i > 0; i--) {
          if (text[i] === " ") {
            spacePosition = i;
            break;
          }
        }
      }
      return spacePosition;
    },
    getFormattedSpeakerName(dialog) {
      if (dialog.originalSpeakerName.length === 1) {
        return `Speaker ${dialog.originalSpeakerName}`;
      }
      return dialog.originalSpeakerName;
    },
    getSpeakerName(dialog) {
      let speakerName = this.getFormattedSpeakerName(dialog);
      if (dialog.personId != 0) {
        let personSelected = _.find(this.persons, function (item) {
          return item.id === dialog.personId;
        });
        if (personSelected) {
          speakerName = `${personSelected.firstName} ${personSelected.lastName}`;
        }
      }
      return `${this.speakerIndentation}${speakerName}`;
    },
    formatDialogToAllowedLines(dialogText) {
      let startIndex = 0;
      let newText = "";
      while (startIndex < dialogText.length - 1) {
        dialogText =
          dialogText.substring(0, startIndex) +
          this.textIndentation +
          dialogText.substring(startIndex);
        const endIndex = startIndex + maxCharacterByLine;
        const lastSpace = this.getPreviousSpace(dialogText, endIndex);
        if (endIndex - lastSpace >= maxCharacterByLine) {
          lastSpace = endIndex;
        }
        if (endIndex != lastSpace && lastSpace != 0) {
          endIndex = lastSpace;
        }
        const line = dialogText.substring(startIndex, endIndex);
        if (line) {
          newText += `${line}\n`;
          startIndex = endIndex;
        } else {
          startIndex = dialogText.length;
        }
      }
      return newText;
    },
    getTranscriptLines() {
      let formattedDialog = this.dialogs.map((d) => {
        let textFormatted = `${this.getSpeakerName(d)}: ${d.transcription}`;
        textFormatted = this.formatDialogToAllowedLines(textFormatted);
        return textFormatted.split("\n");
      });
      let allTranscritWithNoiseLines = formattedDialog.flat();
      this.lines = allTranscritWithNoiseLines.filter(
        (line) => !StringHelper.IsNullOrEmpty(line)
      );
    },
    openExportDialog() {
      this.showExportDialog = true;
    },
    async selectExportAsync(exportSettings) {
      this.downloading = true;
      let response = {};
      try {
        switch (exportSettings.exportTye) {
          case 1:
            const exportTranscriptDto = {
              transcriptId: this.transcript.id,
              extension: 2,
              offset: moment().utcOffset(),
            };
            response = await this.exportTranscriptAsync(exportTranscriptDto);
            break;
          case 2:
            const exportTranslateTranscriptDto = {
              transcriptId: this.transcript.id,
              apiLanguageId: exportSettings.apiLanguageId,
              extension: 2,
              offset: moment().utcOffset(),
            };
            response = await this.translateExportTranscriptAsync(
              exportTranslateTranscriptDto
            );
            break;
          case 3:
            const exportHumanTranscriptDto = {
              transcriptId: this.transcript.id,
              extension: 2,
              offset: moment().utcOffset(),
            };
            response = await this.exportTranscriptAsync(exportHumanTranscriptDto);
            break;
        }

        if (response.success) {
          const fileName = `${this.transcript.name}.docx`;
          this.downloadBlobFile(response.data, fileName);
        } else {
          if (response.message === "Transcription is on the same language") {
            throw this.$t("step4_export_same_language_error").replace(
              "<language>",
              exportSettings.languageLabel
            );
          }
        }
      } catch (error) {
        this.$eventBus.$emit(
          VuexKeys.Home.ShowSnackbarMessage,
          error,
          SnackbarType.Error
        );
      } finally {
        this.downloading = false;
      }
    },
    downloadBlobFile(body, fileName) {
      const blob = new Blob([body]);

      if (navigator.msSaveBlob) {
        // IE 10+
        navigator.msSaveBlob(blob, fileName);
      } else {
        const link = document.createElement("a");
        // Browsers that support HTML5 download attribute
        if (link.download !== undefined) {
          const url = URL.createObjectURL(blob);
          link.setAttribute("href", url);
          link.setAttribute("download", fileName);
          link.style.visibility = "hidden";
          document.body.appendChild(link);
          link.click();
          document.body.removeChild(link);
        }
      }
    },
    back() {
      this.$emit("back", { name: "name" });
    },
    async createHumanTranscription() {
      let temporalTranscript = {};
      let response = await this.createHumanTranscriptionAsync(this.transcript);
      await this.getTranscriptById(this.transcript.id).then(
        async (response) => {
          if (response.success) {
            temporalTranscript = response.data;
          }
        }
      );
      if (response) {
        this.transcript.humanTranscriptionStatus = temporalTranscript.humanTranscriptionStatus;
        this.transcript.lastModifiedOn = temporalTranscript.lastModifiedOn;
      }
    },
    textHumanTranscriptionSent(date) {
      let text = this.$t("step4_transcript_humantranscription_date_sent");
      text = text.replace("<date>", TableHelper.FormatListDateWithHour(this, date));
      return text;
    },
  },
  watch: {
    dialogs: function (val) {
      if (val) {
        this.getTranscriptLines();
      }
    },
  },
  async mounted() {
    await this.getDialogs();
    this.getTranscriptLines();

    await this.getAllTranslationLanguagesAsync().then((response) => {
      if (response.success) {
        this.allTranslationLanguages = response.data;
      }
    });
  },
};
</script>
<style lang="scss">
@import "@/scss/components/wizard/StepFour.scss";
</style>