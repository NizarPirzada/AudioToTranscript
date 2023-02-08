<template>
  <v-card class="mb-4 cs-media-processing" color="white">
    <v-card-text>
      <v-container class="ma-0 pa-0">
        <div class="d-flex justify-center">
          <img
            class="cs-media-processing-img"
            src="../../assets/images/media_processing.png"
          />
        </div>
        <div class="d-flex justify-center">
          <h1 class="cs-media-processing-title">
            {{ $t("step3_media_processing_title") }}
          </h1>
        </div>
        <div
          class="d-flex align-center flex-wrap cs-media-processing-status-container-step-3"
        >
          <div v-if="jobStatusError">
            <v-list-item>
              <v-list-item-icon>
                <v-icon color="error">mdi-close-circle-outline</v-icon>
              </v-list-item-icon>
              <v-list-item-content>
                <v-list-item-title
                  class="cs-media-processing-text-step-3 text-wrap"
                  v-html="jobStatusErrorMessage"
                >
                </v-list-item-title>
              </v-list-item-content>
            </v-list-item>
          </div>
          <div v-else>
            <v-list-item>
              <v-list-item-icon>
                <v-progress-circular
                  v-if="jobStatusIsCreated"
                  indeterminate
                  :size="35"
                  color="primary"
                ></v-progress-circular>
                <v-icon v-else color="primary" large
                  >mdi-checkbox-marked-circle-outline</v-icon
                >
              </v-list-item-icon>
              <v-list-item-content>
                <v-list-item-title
                  class="cs-media-processing-text-step-3 text-wrap"
                  v-html="jobStatusCreatedMessage"
                >
                </v-list-item-title>
              </v-list-item-content>
            </v-list-item>

            <v-list-item v-show="messageIndex > 0">
              <v-list-item-icon>
                <v-progress-circular
                  v-if="jobStatusBeingSentToEngine"
                  indeterminate
                  :size="35"
                  color="primary"
                ></v-progress-circular>
                <v-icon v-else color="primary" large
                  >mdi-checkbox-marked-circle-outline</v-icon
                >
              </v-list-item-icon>
              <v-list-item-content>
                <v-list-item-title
                  class="cs-media-processing-text-step-3 text-wrap"
                  v-html="jobStatusBeingSentMessage"
                >
                </v-list-item-title>
              </v-list-item-content>
            </v-list-item>
            <v-list-item v-show="messageIndex > 1">
              <v-list-item-icon>
                <v-progress-circular
                  v-if="jobStatusSentToAuthenticity"
                  indeterminate
                  :size="35"
                  color="primary"
                ></v-progress-circular>
                <v-icon v-else color="primary" large
                  >mdi-checkbox-marked-circle-outline</v-icon
                >
              </v-list-item-icon>
              <v-list-item-content>
                <v-list-item-title
                  class="cs-media-processing-text-step-3 text-wrap"
                  v-html="jobStatusPendingMessage"
                >
                </v-list-item-title>
              </v-list-item-content>
            </v-list-item>
            <v-list-item v-show="messageIndex > 2">
              <v-list-item-icon>
                <v-progress-circular
                  v-if="jobStatusCompleted"
                  indeterminate
                  :size="35"
                  color="primary"
                ></v-progress-circular>
                <v-icon v-else color="primary" large
                  >mdi-checkbox-marked-circle-outline</v-icon
                >
              </v-list-item-icon>
              <v-list-item-content>
                <v-list-item-title
                  class="cs-media-processing-text-step-3 text-wrap"
                  v-html="jobStatusCompletedMessage"
                >
                </v-list-item-title>
              </v-list-item-content>
            </v-list-item>
          </div>
        </div>
        <div class="d-flex justify-center">
          <v-btn
            color="primary"
            class="cs-btn cs-btn-medium mb-3"
            @click="confirmResend"
            :disabled="isResendingJobs || jobStatusCompleted"
          >
            {{ $t("step3_resend_button") }}
          </v-btn>
        </div>
      </v-container>
    </v-card-text>
  </v-card>
</template>

<script>
import { TranscriptJobStatus } from "@/./environment.js";
export default {
  name: "MediaProcessing",
  props: {
    currentJob: {
      type: Object,
      required: false,
    },
    isResendingJobs: {
      type: Boolean,
      required: true,
    },
  },
  data: () => ({
    messageIndex: 0,
  }),
  computed: {
    jobStatus() {
      return this.currentJob.status;
    },
    jobStatusError() {
      return this.jobStatus === TranscriptJobStatus.Error;
    },
    jobStatusIsCreated() {
      return this.jobStatus === TranscriptJobStatus.Created;
    },
    jobStatusBeingSentToEngine() {
      if (
        this.jobStatus === TranscriptJobStatus.Processing &&
        this.currentJob.startSendingON !== null &&
        this.currentJob.sentOn === null
      ) {
        this.showMessageIndex(1);
        return true;
      }
      return false;
    },
    jobStatusSentToAuthenticity() {
      if (
        (this.jobStatus === TranscriptJobStatus.Processing ||
          this.jobStatus === TranscriptJobStatus.SentToAuthenticity) &&
        this.currentJob.sentOn !== null
      ) {
        this.showMessageIndex(2);
        return true;
      }
      return false;
    },
    jobStatusCompleted() {
      if (this.jobStatus === TranscriptJobStatus.Completed) {
        this.showMessageIndex(3);
        return true;
      }
      return false;
    },
    jobStatusCreatedMessage() {
      return this.$t("step3_media_processing_job_created");
    },
    jobStatusBeingSentMessage() {
      return this.$t("step3_media_processing_job_being_sent_to_engine").replace(
        "<DateTime>",
        this.$formatDateTimeDescriptive(this.currentJob.startSendingOn)
      );
    },
    jobStatusPendingMessage() {
      return this.$t("step3_media_processing_job_pending");
    },
    jobStatusErrorMessage() {
      return this.$t("step3_media_processing_job_failed");
    },
    jobStatusCompletedMessage() {
      return this.$t("step3_media_processing_job_completed");
    },
  },
  methods: {
    confirmResend() {
      this.$emit("onConfirmResend");
    },
    showMessageIndex(index) {
      this.messageIndex = index;
    },
  },
  watch: {
      isResendingJobs(val) {
          if(val === false) {
              this.showMessageIndex(0);
          }
      }
  }
};
</script>

<style>
</style>