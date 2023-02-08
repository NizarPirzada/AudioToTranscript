<template>
  <v-card
    :class="[
      { 'ml-10': transcriptDialog.examinationType != 0 },
      { 'cs-examination': transcriptDialog.examinationType != 0 },
      'mb-4',
      'transcription-chunk',
    ]"
    :id="`transcription-chunk-${index}`"
  >
    <v-card-title class="transcription-tittle">
      <div class="selector-container my-3 mr-5">
        <div class="d-flex justify-space-between">
          <div class="d-flex col-4 py-0 justify-start pl-0">
            <div class="pa-0" v-if="isAnExamination">
              <ExaminationTagSelector
                style="text-align: left"
                :dialog.sync="transcriptDialog"
                @onUpdateExaminationTagToSingleSpeaker="
                  onUpdateExaminationTagToSingleSpeaker
                "
                @onUpdateExaminationTagToAllSpeakers="
                  onUpdateExaminationTagToAllSpeakers
                "
              >
              </ExaminationTagSelector>
            </div>
            <div class="pa-0" :class="isAnExamination ? 'col-9' : 'col-12'">
              <SpeakerSelector
                style="text-align: left"
                :dialogPosition="index"
                :dialog="transcriptDialog"
                :speakers="persons"
                :transcript.sync="transcript"
                @saveAll="changeAllSpeakers"
                @saveSingle="changeOneSpeaker"
                @saveNewOnTheFlightSpeakerAsync="saveNewOnTheFlightSpeakerAsync"
              />
            </div>
          </div>
          <div class="d-flex col-3 py-0 justify-start">
            <select
              ref="select"
              class="cs-select light cs-examination-select"
              v-model="examination"
              @change="onChangeExamination"
              :disabled="transcriptInEdition"
            >
              <option
                v-for="option in examinationOptions"
                v-bind:value="option.value"
                :key="option.value"
                :disabled="option.disabled"
              >
                {{ option.text }}
              </option>
            </select>
          </div>
          <div class="d-flex col-1 py-0 justify-center">
            <span class="chunk-label-read-only">
              {{ formatDuration(transcriptDialog.duration) }}
            </span>
          </div>
          <div class="d-flex col-4 py-0 justify-end">
            <span class="chunk-label-read-only">{{
              formatTimelapse(
                transcriptDialog.startTime,
                transcriptDialog.duration
              )
            }}</span>
          </div>
        </div>
      </div>
    </v-card-title>
    <v-divider class="mx-0 transcription-divide"></v-divider>
    <v-card-text style="padding: 0px">
      <v-layout row align-center>
        <v-flex col-1 class="justify-center">
          <v-btn
            class="transcrition-play-button"
            icon
            color="primary"
            @click="playPause(transcriptDialog)"
          >
            <v-icon color="secondary">{{
              transcriptDialog.chunkButton.icon
            }}</v-icon>
          </v-btn>
        </v-flex>
        <v-flex col-11>
          <TranscriptionEditor
            :dialogPosition="index"
            :dialog="transcriptDialog"
            @saveTranscription="changeTranscriptionText"
            @onEnableScroll="onEnableScroll"
          />
        </v-flex>
      </v-layout>
    </v-card-text>
  </v-card>
</template>

<script>
import { mapState } from "vuex";
import ExaminationTagSelector from "@/components/wizard/ExaminationTagSelector";
import SpeakerSelector from "@/components/wizard/SpeakerSelector";
import TranscriptionEditor from "@/components/wizard/TranscriptionEditor";
import { ExaminationTypes } from "@/./environment.js";

export default {
  name: "TranscriptCard",
  data: () => {
    let examination = ExaminationTypes.NoExamination;
    return {
      examination,
    };
  },
  props: {
    index: {
      type: Number,
      required: true,
    },
    transcriptDialog: {
      type: Object,
      required: true,
    },
    persons: {
      type: Array,
      required: true,
    },
    examinationOpened: {
      type: Boolean,
    },
     transcript: {
      type: Object,
      required: true,
    },
  },
  components: {
    ExaminationTagSelector,
    SpeakerSelector,
    TranscriptionEditor,
  },
  computed: {
    ...mapState("transcript", ["transcriptInEdition"]),
    examinationOptions() {
      if (
        this.transcriptDialog.examinationType ===
          ExaminationTypes.InsideDirectExamination ||
        this.transcriptDialog.examinationType === ExaminationTypes.CloseDirect
      ) {
        return [
          { value: ExaminationTypes.InsideDirectExamination, text: "--" },
          { value: ExaminationTypes.CloseDirect, text: this.$t("step3_exam_direct_close") }
        ];
      } else if (
        this.transcriptDialog.examinationType ===
          ExaminationTypes.InsideCrossExamination ||
        this.transcriptDialog.examinationType === ExaminationTypes.CloseCross
      ) {
        return [
          { value: ExaminationTypes.InsideCrossExamination, text: "--" },
          { value: ExaminationTypes.CloseCross, text: this.$t("step3_exam_cross_close") }
        ];
      } else {
        return [
          { value: ExaminationTypes.NoExamination, text: "--" },
          {
            value: ExaminationTypes.StartDirect,
            text: this.$t("step3_exam_direct_start"),
            disabled: this.anExaminationIsAlreadyOpened,
          },
          {
            value: ExaminationTypes.StartCross,
            text: this.$t("step3_exam_cross_start"),
            disabled: this.anExaminationIsAlreadyOpened,
          },
        ];
      }
    },
    examinationType() {
      return this.transcriptDialog.examinationType;
    },
    anExaminationIsAlreadyOpened() {
      return this.examinationOpened && this.examination;
    },
    isAnExamination() {
      return this.transcriptDialog.examinationType > 0;
    },
  },
  methods: {
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
    async changeAllSpeakers(index, dialogObject) {
      this.$emit("onChangeAllSpeakers", index, dialogObject);
    },
    async changeOneSpeaker(index, dialogObject) {
      this.$emit("onChangeOneSpeaker", index, dialogObject);
    },
    async changeTranscriptionText(index, dialogObject) {
      this.$emit("onChangeText", index, dialogObject);
    },
    playPause(item) {
      this.$emit("onPlayPause", item);
    },
    onChangeExamination() {
      this.$emit("onSetExamination", this.index, this.examination);
    },
    onUpdateExaminationTagToSingleSpeaker(examinationTag) {
      this.$emit(
        "onUpdateExaminationTagToSingleSpeaker",
        this.index,
        examinationTag
      );
    },
    onUpdateExaminationTagToAllSpeakers(examinationTag, originalSpeakerId) {
      this.$emit(
        "onUpdateExaminationTagToAllSpeakers",
        this.index,
        examinationTag,
        originalSpeakerId
      );
    },
    onEnableScroll(val) {
      this.$emit("onEnableScroll", val);
    },
    saveNewOnTheFlightSpeakerAsync(newPersonDto){
      this.$emit("saveNewOnTheFlightSpeakerAsync", newPersonDto);
    },
  },
  watch: {
    examinationType: function (val) {
      this.examination = val;
      this.transcriptDialog.examinationTagDisabled =
        this.transcriptDialog.examinationType ===
          ExaminationTypes.StartDirect ||
        this.transcriptDialog.examinationType === ExaminationTypes.StartCross;
    },
  },
  mounted() {
    this.examination = this.transcriptDialog.examinationType;
  },
};
</script>
<style lang="scss">
@import "@/scss/components/wizard/StepThree.scss";
</style>