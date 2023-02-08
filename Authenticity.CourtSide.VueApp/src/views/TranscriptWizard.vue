<template>
  <v-layout column class="cs-wizard">
    <div v-show="isTranscriptLocked">
      <SavingTranscript class="cs-saving-transcript" />
    </div>

    <v-stepper alt-labels v-model="currentStep" class="cs-stepper pa-0">
      <v-stepper-header class="cs-stepper-header">
        <v-stepper-step
          step="1"
          @click="goToStep(1)"
          class="cs-stepper-step"
          :class="getStepCompletedClass(1)"
          color="info"
        >
          {{ $t("step1_name") }}
        </v-stepper-step>

        <v-divider :class="getStepCompletedClass(1)"></v-divider>

        <v-stepper-step
          step="2"
          @click="goToStep(2)"
          class="cs-stepper-step"
          :class="getStepCompletedClass(2)"
          color="info"
        >
          {{ $t("step2_name") }}
        </v-stepper-step>

        <v-divider :class="getStepCompletedClass(2)"></v-divider>

        <v-stepper-step
          step="3"
          @click="goToStep(3)"
          class="cs-stepper-step"
          :class="getStepCompletedClass(3)"
          color="info"
        >
          {{ $t("step3_name") }}
        </v-stepper-step>

        <v-divider :class="getStepCompletedClass(3)"></v-divider>

        <v-stepper-step
          step="4"
          @click="goToStep(4)"
          class="cs-stepper-step"
          :class="getStepCompletedClass(4)"
          color="info"
        >
          {{ $t("step4_name") }}
        </v-stepper-step>

        <v-progress-linear
          v-show="showProgressBar"
          class="cs-stepper-probres-bar"
          color="primary"
          indeterminate
        ></v-progress-linear>
      </v-stepper-header>
      <v-stepper-items class="cs-stepper-items" v-show="currentStep > 0">
        <v-stepper-content step="1">
          <StepOne
            v-if="transcriptModel"
            :transcript.sync="transcriptModel"
            :persons="persons"
            :active="savingStepInformation"
            @next="onStepOneNextAction"
          ></StepOne>
        </v-stepper-content>

        <v-stepper-content step="2">
          <StepTwo
            v-if="transcriptModel"
            :transcript.sync="transcriptModel"
            :files="files"
            :active="savingStepInformation"
            @next="onStepTwoNextAction"
            @back="onStepBackAction"
          ></StepTwo>
        </v-stepper-content>

        <v-stepper-content step="3">
          <StepThree
            ref="stepThree"
            v-if="currentStep === 3 && transcriptModel"
            :transcript.sync="transcriptModel"
            :persons="persons"
            :dialogs="dialogs"
            :currentJob="currentJob"
            :examinationOpened.sync="examinationOpened"
            @next="onStepThreeNextAction"
            @back="onStepBackAction"
            @lockTranscript="lockTranscript"
            @loadDialogs="getDialogsByTranscriptId"
            @saveNewOnTheFlightSpeakerAsync="saveNewOnTheFlightSpeakerAsync"
          ></StepThree>
        </v-stepper-content>

        <v-stepper-content step="4">
          <StepFour
            v-if="currentStep === 4 && transcriptModel"
            :transcript.sync="transcriptModel"
            :persons="persons"
            :dialogs="dialogs"
            @next="onStepFourNextAction"
            @back="onStepBackAction"
            @loadDialogs="getDialogsByTranscriptId"
          ></StepFour>
        </v-stepper-content>
      </v-stepper-items>
    </v-stepper>
  </v-layout>
</template>
<script>
import StepOne from "@/components/wizard/StepOne";
import StepTwo from "@/components/wizard/StepTwo";
import StepThree from "@/components/wizard/StepThree";
import StepFour from "@/components/wizard/StepFour";
import { mapActions, mapMutations, mapState } from "vuex";
import {
  TranscripStatus,
  ExaminationTypes,
  TranscriptJobStatus,
} from "@/./environment.js";
import { StringHelper } from "@/utilities/Helpers.js";
import { VuexKeys } from "@/./environment.js";
import { SnackbarType } from "@/utilities/Enumerations";
import SavingTranscript from "@/components/wizard/SavingTranscript";
import { interval } from "rxjs";
import { takeWhile } from "rxjs/operators";

const interval$ = interval(5000);

export default {
  name: "TranscriptWizard",
  data: () => ({
    currentStep: 0,
    transcriptModel: null,
    persons: [],
    files: [],
    savingStepInformation: false,
    currentJob: {},
    examinationOpened: {},
    dialogs: [],
    counter: 0,
  }),
  components: {
    StepOne,
    StepTwo,
    StepThree,
    StepFour,
    SavingTranscript,
  },
  computed: {
    ...mapState("transcript", ["currentTranscriptLocked"]),
    transcriptId() {
      return this.$route.params.transcriptId;
    },
    showProgressBar() {
      return this.savingStepInformation || this.currentStep === 0;
    },
    isTranscriptLocked() {
      return (
        this.currentTranscriptLocked.transcriptId == this.transcriptId &&
        this.currentTranscriptLocked.locked == true
      );
    },
    transcriptStatus() {
      return this.transcriptModel?.status;
    },
  },
  methods: {
    ...mapMutations("transcript", ["setCurrentTranscriptLocked"]),
    ...mapActions("transcript", [
      "getTranscriptById",
      "updateTranscriptAsync",
      "updateRecordDateAsync",
      "getTranscriptPersonsByIdAsync",
      "savePersonAsync",
      "saveFileInfoAsync",
      "getTranscriptFilesByIdAsync",
      "deleteTranscriptPersonAsync",
      "getJobStatusAsync",
      "getTranscriptDialogsByTranscriptIdAsync",
    ]),

    validateTranscriptStep(localInfo, nextStep) {
      if (localInfo.step > nextStep) {
        return localInfo.step;
      }
      return nextStep;
    },
    getArrayWithAllPersons(transcriptObject) {
      let allPersons = [];
      return allPersons.concat(
        transcriptObject.plaintiffs,
        transcriptObject.defendants,
        transcriptObject.deponents,
        transcriptObject.plaintiffAttorneys,
        transcriptObject.defendantAttorneys,
        transcriptObject.additionalSpeakers
      );
    },
    async asyncForEach(array, callback) {
      for (let index = 0; index < array.length; index++) {
        await callback(array[index], index, array);
      }
    },
    async onStepOneNextAction(transcriptInfo) {
      this.savingStepInformation = true;
      try {
        let allPersons = this.getArrayWithAllPersons(transcriptInfo);
        await this.asyncForEach(allPersons, async (element, index) => {
          if (
            element.id > 0 &&
            StringHelper.IsNullOrEmpty(element.firstName) &&
            StringHelper.IsNullOrEmpty(element.lastName)
          ) {
            const deleteResult = await this.deleteTranscriptPersonAsync(
              element.id
            );
            if (!deleteResult.success) {
              throw deleteResult.message;
            }
            allPersons.splice(index, 1);
          } else {
            let saveResult = await this.savePersonAsync(element);

            if (saveResult.success) {
              element.id = saveResult.data.id;
              element.personAdditionalInformationId =
                saveResult.data.personAdditionalInformationId;
              element.additionalInfo = saveResult.data.additionalInfo;
            }
          }
        });

        this.persons = allPersons;
        const nextStep = 2;
        transcriptInfo.step = this.validateTranscriptStep(
          transcriptInfo,
          nextStep
        );
        await this.updateTranscriptAsync(transcriptInfo);
        this.currentStep = nextStep;
      } catch (error) {
        this.$eventBus.$emit(
          VuexKeys.Home.ShowSnackbarMessage,
          error,
          SnackbarType.Error
        );
      } finally {
        this.savingStepInformation = false;
      }
    },
    async onStepTwoNextAction() {
      this.savingStepInformation = true;

      this.files = await this.getTranscriptFilesByIdAsync(
        this.transcriptModel.id
      );
      if (this.transcriptModel.file) {
        await this.saveFileInfoAsync({
          transcriptId: this.transcriptModel.id,
          name: this.transcriptModel.file.name,
          path: this.transcriptModel.file.filePath,
          size: this.transcriptModel.file.size,
        });
        this.files = await this.getTranscriptFilesByIdAsync(
          this.transcriptModel.id
        );
        this.transcriptModel.mediaFileSize =
          this.transcriptModel.file.size / 1024;
      }
      let fullDateTimeObject = null;

      if (this.transcriptModel.transcriptDate) {
        fullDateTimeObject = new Date(
          this.transcriptModel.transcriptDate.replace("-", "/")
        );
      }

      if (this.transcriptModel.transcriptTime) {
        if (!fullDateTimeObject) {
          fullDateTimeObject = new Date(null);
        }
        const hoursAndMinutes = this.transcriptModel.transcriptTime.split(":");
        fullDateTimeObject.setHours(hoursAndMinutes[0] || "00");
        fullDateTimeObject.setMinutes(hoursAndMinutes[1] || "00");
      }

      if (fullDateTimeObject) {
        this.transcriptModel.recordDate = fullDateTimeObject;
      }

      if (this.transcriptStatus < 1) {
        this.transcriptModel.status = 1;
      }
      if (this.transcriptModel.step < 3) {
        this.transcriptModel.step = 3;
      }

      await this.updateTranscriptAsync(this.transcriptModel);

      await this.getTranscriptById(this.transcriptId).then((res) => {
        this.transcriptModel = res.data;
      });
      this.currentStep = this.transcriptModel.step;
      this.savingStepInformation = false;
    },
    async onStepThreeNextAction() {
      this.savingStepInformation = true;
      this.transcriptModel.step = 4;
      this.transcriptModel.status = 3;
      await this.updateTranscriptAsync(this.transcriptModel);
      this.currentStep = this.transcriptModel.step;
      this.savingStepInformation = false;
      interval$.unsubscribe;
    },
    onStepFourNextAction(transcriptInfo) {
      // TO DO
    },
    onStepBackAction() {
      if (this.examinationOpened && this.examinationOpened.value) {
        this.openExaminationConfirmMessage(async () => {
          this.examinationOpened = {};
          this.currentStep--;
        });
      } else {
        this.currentStep--;
      }
    },
    async getCurrentJobStatusAsync() {
      if (this.transcriptId) {
        await this.getJobStatusAsync(this.transcriptId).then(
          (jobStatusResponse) => {
            if (jobStatusResponse.success) {
              this.currentJob = jobStatusResponse.data;
            }
          }
        );
      } else {
        interval$.unsubscribe;
      }
    },
    async checkTranscriptionComplete() {
      await this.getCurrentJobStatusAsync();
      if (this.currentJob?.status === TranscriptJobStatus.Completed) {
        this.getTranscriptById(this.transcriptId).then(
          async (transcriptResponse) => {
            if (transcriptResponse.success) {
              this.transcriptModel = transcriptResponse.data;
              this.currentStep = this.transcriptModel.step;
              await this.getDialogsByTranscriptId();
              if (this.$refs.stepThree) {
                this.$refs.stepThree.isLoadingDialogs = false;
                this.$refs.stepThree.isProcessingTranscript = false;
              }
            }
          }
        );
      }
    },
    goToStep(step) {
      if (this.transcriptModel.step < step) {
        return;
      }

      if (this.examinationOpened && this.examinationOpened.value) {
        this.openExaminationConfirmMessage(async () => {
          this.examinationOpened = {};
          this.currentStep = step;
        });
      } else {
        this.currentStep = step;
      }
    },
    getStepCompletedClass(step) {
      if (this.currentStep > step) {
        return "cs-stepper-step-completed";
      }
      return "cs-stepper-step-uncompleted";
    },
    applyStepperContainerStyle() {
      var stepperItemContainer = document.getElementsByClassName(
        "cs-stepper-items"
      )[0];

      switch (this.currentStep) {
        case 1:
        case 2:
        case 4:
          stepperItemContainer.style.height = "calc(100vh - 14rem)";
          break;
        case 3:
          if (this.dialogs && this.dialogs.length > 0) {
            stepperItemContainer.style.height = "calc(100vh - 21rem)";
          }
        default:
          break;
      }
    },
    async loadDialogsByPageAsync(pageNumber) {
      let transcriptId = this.transcriptModel.id;
      let dialogs = await this.getTranscriptDialogsByTranscriptIdAsync({
        transcriptId: transcriptId,
        page: pageNumber,
      });
      dialogs = dialogs.map((obj) => ({
        ...obj,
        chunkButton: { icon: "mdi-play", playing: false },
        examinationTagDisabled:
          obj.examinationType === ExaminationTypes.StartDirect ||
          obj.examinationType === ExaminationTypes.StartCross
            ? true
            : false,
      }));
      return dialogs;
    },
    async completeDialogsLoadAsync(dialogs) {
      let pageNumber = 2;
      let lastDialogCount = dialogs?.length || 0;
      while (lastDialogCount === 100 && pageNumber < 25) {
        let newDialogs = await this.loadDialogsByPageAsync(pageNumber);
        lastDialogCount = newDialogs?.length || 0;
        this.dialogs = this.dialogs.concat(newDialogs);
        pageNumber++;
      }
    },
    async getDialogsByTranscriptId() {
      let dialogs = await this.loadDialogsByPageAsync(1);
      this.completeDialogsLoadAsync(dialogs);
      this.dialogs = dialogs;
      if (this.$refs.stepThree) {
        this.$refs.stepThree.isLoadingDialogs = false;
      }
      this.applyStepperContainerStyle();
    },
    openExaminationConfirmMessage(callBack) {
      this.$eventBus.$emit(
        VuexKeys.Home.ShowConfirmMessage,
        this.$t("step3_leave_confirm_dialog_title"),
        this.$t("step3_leave_confirm_dialog_text").replace(
          "<examination>",
          this.examinationOpened.examinationTypeText
        ),
        callBack,
        null,
        this.$t("step3_leave_confirm_dialog_yes_button"),
        this.$t("app_cancel_button")
      );
    },
    lockTranscript(value) {
      this.transcriptModel.locked = value;
    },
    async saveNewOnTheFlightSpeakerAsync(newPersonDto) {
      this.persons.push(newPersonDto);
    },
  },
  async mounted() {
    await this.getTranscriptById(this.transcriptId).then(async (response) => {
      if (response.success) {
        this.transcriptModel = response.data;
        this.setCurrentTranscriptLocked({
          transcriptId: this.transcriptModel.id,
          locked: this.transcriptModel.locked,
        });

        this.$eventBus.$emit(VuexKeys.Home.SetAppBarInfo, this.transcriptModel);

        // get transcript persons
        this.persons = await this.getTranscriptPersonsByIdAsync(
          this.transcriptId
        );

        // Get transcript files
        this.files = await this.getTranscriptFilesByIdAsync(this.transcriptId);
        this.currentStep = this.transcriptModel.step;
      }
    });
  },
  watch: {
    async currentStep(val) {
      this.applyStepperContainerStyle();
      if (val === 3 && this.transcriptModel.step < 4) {
        await this.getCurrentJobStatusAsync();
        interval$
          .pipe(
            takeWhile(
              () => this.transcriptModel.status !== TranscripStatus.Editing
            )
          )
          .subscribe({
            next: async () => await this.checkTranscriptionComplete(),
          });
      }
    },
  },
  beforeRouteLeave(to, from, next) {
    if (to.name !== "Login") {
      if (this.examinationOpened && this.examinationOpened.value) {
        this.openExaminationConfirmMessage(async () => {
          next();
        });
      } else {
        next();
      }
    } else {
      next();
    }
  },
};
</script>
<style lang="scss">
@import "@/scss/views/wizard.scss";
</style>