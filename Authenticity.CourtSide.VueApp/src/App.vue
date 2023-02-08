<template>
  <v-container fluid style="padding: 0">
    <router-view></router-view>
  </v-container>
</template>

<script>
import * as signalR from "@aspnet/signalr";
import { mapMutations } from "vuex";
import { VuexKeys } from "@/./environment.js";
import { SnackbarType } from "@/utilities/Enumerations";

export default {
  name: "App",
  methods: {
    ...mapMutations("transcript", ["setCurrentTranscriptLocked"]),
    AddSignalRHub(hubPath, methods = []) {
      const baseURL =
        process.env.VUE_APP_API_SERVICE_HOST ||
        `${location.protocol}//${window.location.hostname}`;
      const userToken = localStorage.getItem("userToken");

      let connection = new signalR.HubConnectionBuilder()
        .withUrl(`${baseURL}/${hubPath}?userToken=${userToken}`)
        .configureLogging(signalR.LogLevel.None)
        .build();

      connection.start().catch(function (err) {
        // eslint-disable-next-line no-console
        return console.error("HUB ERROR", err.toString());
      });

      methods.forEach((method) => {
        connection.on(method.name, method.implementation);
      });
    },
  },
  mounted() {
    const transcriptionMethods = [
      {
        name: "NotifyExaminationSavingStatus",
        implementation: async (user, message, success) => {
          const transcriptId = this.$route.params?.transcriptId;
          if (success && message === "Changes are saving" && !!transcriptId) {
            this.setCurrentTranscriptLocked({
              transcriptId: transcriptId,
              locked: false,
            });
            this.$eventBus.$emit(
              VuexKeys.Home.ShowSnackbarMessage,
              this.$t("step3_examination_saved_message"),
              SnackbarType.Success
            );
          }
        },
      },
    ];

    this.AddSignalRHub("transcription-notification-hub", transcriptionMethods);
  },
};
</script>

<style lang="scss">
@import "@/scss/app/global.scss";
</style>
