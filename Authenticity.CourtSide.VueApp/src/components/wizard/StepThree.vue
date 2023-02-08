<template>
  <div>
    <v-alert
      dense
      type="warning"
      class="cs-examination-alert"
      v-if="showExaminationAlert"
      elevation="2"
      v-html="examinationAlerMessage"
      outlined
    >
    </v-alert>
    <div class="transcription-container">
      <TranscriptCardSkeleton v-if="isLoadingDialogs" />
      <div v-else-if="!isProcessingTranscript">
        <TranscriptCard
          v-for="(item, index) in dialogs"
          :key="index"
          :index="index"
          :transcriptDialog="item"
          :persons="persons"
          :examinationOpened="examinationOpened.value"
          :transcript.sync="transcript"
          @onPlayPause="playPauseChunk"
          @onChangeText="changeTranscriptionText"
          @onChangeOneSpeaker="changeOneSpeaker"
          @onChangeAllSpeakers="changeAllSpeakers"
          @onSetExamination="onChangeExamination"
          @onUpdateExaminationTagToAllSpeakers="
            onUpdateExaminationTagToAllSpeakers
          "
          @onUpdateExaminationTagToSingleSpeaker="
            onUpdateExaminationTagToSingleSpeaker
          "
          @onEnableScroll="onEnableScroll"
          @saveNewOnTheFlightSpeakerAsync="saveNewOnTheFlightSpeakerAsync"
        >
        </TranscriptCard>
      </div>
      <div v-else>
        <MediaProcessing
          :currentJob="currentJob"
          :isResendingJobs="isResendingJobs"
          @onConfirmResend="onConfirmResend"
        ></MediaProcessing>
      </div>
    </div>
    <StepperActionButtons
      :buttonList="buttonList"
      :customClass="stepperActionButtonsClass"
      @next="next"
      @back="back"
    ></StepperActionButtons>
    <Player
      class="cs-player"
      :dialogs.sync="dialogs"
      :transcript.sync="transcript"
      :enableScroll="enableScroll"
      v-if="existDialog"
      @addCurrentChunkStyle="addCurrentChunkStyle"
      @changeChunkPlayingState="changeChunkPlayingState"
      ref="player"
    />
  </div>
</template>

<script>
import { mapActions, mapMutations, mapState } from "vuex";
import Player from "@/components/wizard/Player";
import TranscriptCard from "@/components/wizard/TranscriptCard";
import {
  VuexKeys,
  ExaminationTypes,
  ExaminationTags,
  TranscripStatus,
  TranscriptJobStatus,
} from "@/./environment.js";
import { SnackbarType } from "@/utilities/Enumerations";
import StepperActionButtons from "@/components/wizard/StepperActionButtons";
import TranscriptCardSkeleton from "@/components/wizard/TranscriptCardSkeleton";
import MediaProcessing from "@/components/wizard/MediaProcessing";

export default {
  name: "StepThree",
  data: () => {
    let btnNextDisabled = false;
    let showExaminationAlert = false;
    let examinationDto = {};
    let currentExaminationType = 0;
    let enableScroll = true;
    return {
      isResendingJobs: false,
      btnNextDisabled,
      showExaminationAlert,
      examinationDto,
      currentExaminationType,
      enableScroll,
      isLoadingDialogs: true,
      isProcessingTranscript: false,
    };
  },
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
    currentJob: {
      type: Object,
      required: false,
    },
    examinationOpened: {
      type: Object,
    },
  },
  components: {
    Player,
    TranscriptCard,
    StepperActionButtons,
    TranscriptCardSkeleton,
    MediaProcessing,
  },
  computed: {
    ...mapState("transcript", ["currentTranscriptLocked"]),
    confirmMessageWhenSendingMedia() {
      return this.$t("step3_resend_dialog_warning_upload_inprogress");
    },
    confirmMessageWhenIsTranscribing() {
      return this.$t("step3_resend_dialog_warning_transcription_inprogress");
    },
    existDialog() {
      return this.dialogs && this.dialogs[0] && this.dialogs[0].id != 0;
    },
    playerComponent() {
      return this.$refs.player;
    },
    examinationTypeText() {
      let examinationType = this.currentExaminationType;
      let examinationTypeText;
      switch (examinationType) {
        case ExaminationTypes.StartDirect:
        case ExaminationTypes.InsideDirectExamination:
        case ExaminationTypes.CloseDirect:
          examinationTypeText = this.$t("step3_exam_text_direct");
          break;
        case ExaminationTypes.StartCross:
        case ExaminationTypes.InsideCrossExamination:
        case ExaminationTypes.CloseCross:
          examinationTypeText = this.$t("step3_exam_text_cross");
        default:
          break;
      }
      return examinationTypeText;
    },
    examinationAlerMessage() {
      if (this.showExaminationAlert) {
        return this.$t("step3_examination_opened_warning").replace(
          "<strong></strong>",
          `<strong>${this.examinationTypeText}</strong>`
        );
      }
      return "";
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
          text: this.$t("step_next_button"),
          disabled: this.btnNextDisabled || !this.existDialog,
          icon: "mdi-chevron-right",
          color: "primary",
          action: "next",
        },
      ];
    },
    stepperActionButtonsClass() {
      return this.existDialog ? "cs-stepper-action-btn-step-three" : "";
    },
    transcriptStatus() {
      return this.transcript.status;
    },
  },
  methods: {
    ...mapMutations("transcript", ["setCurrentTranscriptLocked"]),
    ...mapActions("transcript", [
      "editDialogAsync",
      "editSpeakerAllDialogsAsync",
      "resendJobsAsync",
      "saveExaminationsAsync",
      "updateSingleExaminationTagAsync",
      "updateMassivelyExaminationTagsAsync",
    ]),
    emptyDialog() {
      return {
        duration: 0,
        id: 0,
        originalSpeakerId: 0,
        originalSpeakerName: "",
        personId: 0,
        startTime: 0,
        transcriptId: 0,
        transcription: "",
      };
    },
    formatDuration(timeInSeconds) {
      return new Date(timeInSeconds * 1000).toISOString().substr(11, 8);
    },
    formatTimelapse(startTimeInSeconds, durationInSeconds) {
      let startFormat = this.formatDuration(startTimeInSeconds);
      let endFormat = this.formatDuration(
        startTimeInSeconds + durationInSeconds
      );
      return `${startFormat} - ${endFormat}`;
    },
    editSpeaker(index, item) {
      item.isOnEdition = true;
    },
    async changeAllSpeakers({ id, transcriptId, personId, originalSpeakerId }) {
      const massiveChangeRequest = { id, transcriptId, personId };

      await this.editSpeakerAllDialogsAsync(massiveChangeRequest).then(
        (response) => {
          if (response.success) {
            this.dialogs.forEach((item) => {
              if (item.originalSpeakerId === originalSpeakerId) {
                item.personId = personId;
              }
            });
          } else {
            this.showSnackbarMessage(response.error, SnackbarType.Error);
          }
        }
      );
    },
    async changeOneSpeaker(index, dialogObject) {
      // Updating the with information returned by the component
      this.dialogs[index].personId = dialogObject.personId;
      await this.editDialogAsync(dialogObject);
    },
    async changeTranscriptionText(index, dialogObject) {
      // Updating the with information returned by the component
      this.dialogs[index].transcription = dialogObject.transcription;
      await this.editDialogAsync(dialogObject);
    },
    next() {
      this.$emit("next", { name: "name" });
    },
    back() {
      this.$emit("back", { name: "name" });
    },
    getDialogs() {
      this.$emit("loadDialogs");
    },

    // player methods
    addCurrentChunkStyle(elem, index, playing) {
      this.removeCurrentChunkStyle();
      if (elem && index > -1) {
        elem.classList.remove("white");
        if (playing) {
          this.playChunk(this.dialogs[index]);
        } else {
          this.pauseChunk(this.dialogs[index]);
        }
        elem.classList.add("currentChunk");
      }
    },
    removeCurrentChunkStyle() {
      var activeChunks = document.getElementsByClassName(
        "transcription-chunk currentChunk"
      );
      // remove class active for all chunks
      if (activeChunks) {
        activeChunks.forEach(function (el) {
          el.classList.remove("currentChunk");
        });
      }
      // reset the player icon in all chunks
      this.dialogs.forEach((el) => {
        if (el.chunkButton.icon == "mdi-pause") {
          this.pauseChunk(el);
        }
      });
    },
    playChunk({ id }) {
      if (id) {
        const index = this.dialogs.findIndex((x) => x.id === id);
        if (index > -1) {
          this.dialogs[index].chunkButton = {
            icon: "mdi-pause",
            playing: true,
          };
        }
      }
    },
    pauseChunk({ id }) {
      if (id) {
        const index = this.dialogs.findIndex((x) => x.id === id);
        if (index > -1) {
          this.dialogs[index].chunkButton = {
            icon: "mdi-play",
            playing: false,
          };
        }
      }
    },
    playPauseChunk(item) {
      if (
        this.playerComponent.chunkToPlay &&
        this.playerComponent.chunkToPlay.id !== item.id
      ) {
        this.playerComponent.pause();
        this.playChunk(item);
        this.changeChunkPlayingState(this.$refs.player.chunkToPlay);
        this.playerComponent.chunkToPlay = null;
        this.playerComponent.playChunk(item);
        return;
      }

      if (item.chunkButton.playing) {
        this.pauseChunk(item);
        this.playerComponent.pause();
      } else {
        this.playChunk(item);
        this.playerComponent.playChunk(item);
      }
    },
    changeChunkPlayingState(item) {
      this.pauseChunk(item);
    },
    onConfirmResend() {
      let message = this.confirmMessageWhenIsTranscribing;
      if (this.currentJob.status === TranscriptJobStatus.Processing) {
        message = this.confirmMessageWhenSendingMedia;
      }
      this.$eventBus.$emit(
        VuexKeys.Home.ShowConfirmMessage,
        this.$t("step3_resend_dialog_title"),
        message,
        async () => await this.resendJobAsync(this.transcript.id)
      );
    },
    async resendJobAsync(transcriptId) {
      this.isResendingJobs = true;
      await this.resendJobsAsync(transcriptId);
      this.showSnackbarMessage(
        this.$t("step3_resend_dialog_success"),
        SnackbarType.Success
      );
      this.isResendingJobs = false;
    },
    async onChangeExamination(index, examinationType) {
      let startId = this.dialogs[index].id;
      let endId = this.dialogs[index].id;
      let endExaminationId;
      let breakExamination = [
        ExaminationTypes.StartDirect,
        ExaminationTypes.StartCross,
      ];
      let newExamination = ExaminationTypes.InsideDirectExamination;
      this.btnNextDisabled = true;
      this.showStartExaminationMessage(true, examinationType);
      if (examinationType === ExaminationTypes.NoExamination) {
        breakExamination = [
          ExaminationTypes.StartDirect,
          ExaminationTypes.StartCross,
        ];
        newExamination = ExaminationTypes.NoExamination;
        this.showStartExaminationMessage(false);
      } else if (examinationType === ExaminationTypes.StartCross) {
        breakExamination = [
          ExaminationTypes.StartDirect,
          ExaminationTypes.StartCross,
          6,
        ];
        newExamination = ExaminationTypes.InsideCrossExamination;
      } else if (
        examinationType === ExaminationTypes.CloseDirect ||
        examinationType === ExaminationTypes.CloseCross
      ) {
        breakExamination = [
          ExaminationTypes.StartDirect,
          ExaminationTypes.StartCross,
        ];
        endExaminationId = this.dialogs[index].id;
        newExamination = ExaminationTypes.NoExamination;
        this.showStartExaminationMessage(false);
      }

      for (let i = index; i < this.dialogs.length; i++) {
        if (
          breakExamination.indexOf(this.dialogs[i].examinationType) >= 0 &&
          index != i
        ) {
          break;
        } else if (index != i) {
          this.dialogs[i].examinationType = newExamination;
        } else {
          this.dialogs[i].examinationType = examinationType;
        }

        endId = this.dialogs[i].id;
      }

      await this.saveExaminations(
        index,
        examinationType,
        newExamination,
        startId,
        endId,
        endExaminationId
      );
    },
    showSnackbarMessage(message, type, timeout, bottom) {
      this.$eventBus.$emit(
        VuexKeys.Home.ShowSnackbarMessage,
        message,
        type,
        timeout,
        bottom
      );
    },
    showStartExaminationMessage(value, examinationType) {
      this.showExaminationAlert = value;
      if (examinationType) {
        this.currentExaminationType = examinationType;
        this.$emit("update:examinationOpened", {
          value: true,
          examinationTypeText: this.examinationTypeText,
          examinationType: examinationType,
        });
      }
    },
    async saveExaminations(
      index,
      examinationType,
      newExamination,
      startId,
      endId,
      endExaminationId
    ) {
      if (
        newExamination === ExaminationTypes.InsideDirectExamination ||
        newExamination === ExaminationTypes.InsideCrossExamination
      ) {
        this.examinationDto = {
          examinationType: examinationType,
          newExamination: newExamination,
          startExaminationId: startId,
          closeExaminationId: endId,
          index: index,
        };

        // Set examination tags
        this.setExaminationTags(index);
      } else {
        this.btnNextDisabled = false;
        this.$emit("update:examinationOpened", {});
        this.setCurrentTranscriptLocked({
          transcriptId: this.transcript.id,
          locked: true,
        });

        const startIndex =
          this.examinationDto.index === undefined
            ? index
            : Math.min(this.examinationDto.index, index);
        startId = this.examinationDto.startExaminationId ?? startId;
        endId = endExaminationId ?? endId;

        let endIndex = 0;
        for (let i = startIndex; i < this.dialogs.length; i++) {
          endIndex = i;
          if (this.dialogs[i].id === startId || this.dialogs[i].id === endId) {
            continue;
          }
          if (
            this.dialogs[i].examinationType === ExaminationTypes.StartDirect ||
            this.dialogs[i].examinationType === ExaminationTypes.StartCross
          ) {
            break;
          }
        }

        let examinatonDtoList = [];

        if (index >= 0) {
          for (let i = startIndex; i <= endIndex; i++) {
            const dto = {
              examinationType: this.dialogs[i].examinationType,
              dialogId: this.dialogs[i].id,
            };
            dto.examinationTag =
              this.dialogs[i].examinationType === ExaminationTypes.NoExamination
                ? ExaminationTags.NoTag
                : this.dialogs[i].examinationTag;
            examinatonDtoList.push(dto);
          }
          const saveExaminationRequest = {
            transcriptId: this.transcript.id,
            examinatonDtoList,
          };
          await this.saveExaminationsAsync(saveExaminationRequest);
        }
      }
    },
    setExaminationTags(index) {
      this.dialogs[index].examinationTag = ExaminationTags.Question;

      for (let i = index + 1; i < this.dialogs.length; i++) {
        if (
          this.dialogs[i].examinationType === ExaminationTypes.StartDirect ||
          this.dialogs[i].examinationType === ExaminationTypes.StartCross
        ) {
          break;
        }

        const prevDialog = this.dialogs[i - 1];
        const prevDialogExaminationtag = prevDialog
          ? prevDialog.examinationTag
          : ExaminationTags.NoTag;
        switch (prevDialogExaminationtag) {
          case ExaminationTags.NoTag:
            this.dialogs[i].examinationTag = ExaminationTags.Question;
            break;
          case ExaminationTags.Question:
            if (
              prevDialog.examinationType === ExaminationTypes.CloseDirect ||
              prevDialog.examinationType === ExaminationTypes.CloseCross
            ) {
              this.dialogs[i].examinationTag = ExaminationTags.Question;
            } else {
              this.dialogs[i].examinationTag = ExaminationTags.Answer;
            }
            break;
          case ExaminationTags.Answer:
            this.dialogs[i].examinationTag = ExaminationTags.Question;
            break;
        }
      }
    },
    async onUpdateExaminationTagToAllSpeakers(
      index,
      examinationTag,
      originalSpeakerId
    ) {
      let examinationTagDto = {
        transcriptId: this.transcript.id,
        examinationtag: examinationTag,
        originalSpeakerId: originalSpeakerId,
      };

      // Get start Examination Id
      for (let i = index; i >= 0; --i) {
        if (
          this.dialogs[i].examinationType === ExaminationTypes.StartDirect ||
          this.dialogs[i].examinationType === ExaminationTypes.StartCross
        ) {
          break;
        }
        examinationTagDto.startExaminationId = this.dialogs[i].id;
        if (this.dialogs[i].originalSpeakerId === originalSpeakerId) {
          this.dialogs[i].examinationTag = examinationTag;
        }
      }

      // Get close Examination Id
      for (let i = index; i <= this.dialogs.length; i++) {
        if (this.dialogs[i].originalSpeakerId === originalSpeakerId) {
          this.dialogs[i].examinationTag = examinationTag;
        }

        if (
          this.dialogs[i].examinationType === ExaminationTypes.CloseDirect ||
          this.dialogs[i].examinationType === ExaminationTypes.CloseCross
        ) {
          examinationTagDto.closeExaminationId = this.dialogs[i].id;
          break;
        }
      }

      await this.updateMassivelyExaminationTagsAsync(examinationTagDto);
    },
    async onUpdateExaminationTagToSingleSpeaker(index, examinationTag) {
      const dialog = this.dialogs[index];
      const examinationTagDto = {
        transcriptId: dialog.transcriptId,
        dialogId: dialog.id,
        examinationTag: examinationTag,
      };

      this.dialogs[index].examinationTag = examinationTag;
      await this.updateSingleExaminationTagAsync(examinationTagDto);
    },
    onEnableScroll(val) {
      this.enableScroll = val;
    },
    showDialogs(transcriptStatus) {
      switch (transcriptStatus) {
        case TranscripStatus.Processing:
          this.isProcessingTranscript = true;
          this.isLoadingDialogs = false;
          break;
        case TranscripStatus.Editing:
        case TranscripStatus.Completed:
          this.isProcessingTranscript = false;
          this.isLoadingDialogs = true;
          break;
      }
    },
    saveNewOnTheFlightSpeakerAsync(newPersonDto) {
      this.$emit("saveNewOnTheFlightSpeakerAsync", newPersonDto);
    },
  },
  watch: {
    async currentTranscriptLocked(val) {
      if (!val.locked && val.transcriptId == this.transcript.id) {
        await this.getDialogs();
      }
    },
    transcriptStatus(val) {
      this.showDialogs(val);
    },
  },
  async mounted() {
    this.showDialogs(this.transcriptStatus);
    await this.getDialogs();
    var stepperItemContainer = document.getElementsByClassName(
      "cs-stepper-items"
    )[0];
    if (this.existDialog) {
      stepperItemContainer.style.height = "calc(100vh - 21rem)";
    } else {
      stepperItemContainer.style.height = "calc(100vh - 15rem)";
    }
  },
};
</script>
<style lang="scss">
@import "@/scss/components/wizard/StepThree.scss";
</style>