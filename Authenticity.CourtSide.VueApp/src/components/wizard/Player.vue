<template class="pa-0 ma-0">
  <v-container class="cs-player-container" fill-height fluid>
    <v-row no-gutters align="center" justify="center">
      <v-col cols="3" md="4" lg="3" xl="3" align="center" justify="center">
        <v-row no-gutters align="center" justify="center">
          <v-col cols="3">
            <v-chip color="primary" class="justify-center">{{
              fileInfo.fileExtension
            }}</v-chip>
          </v-col>
          <v-col cols="9" align="start" class="cs-player-separator">
            <div class="cs-audioName text-truncate" :title="fileInfo.fileName">
              {{ fileInfo.fileName }}
            </div>
            <div class="cs-audioDate">
              {{ this.$t("step3_player_added_label") }} -
              {{ fileInfo.fileAdded }}
            </div>
          </v-col>
        </v-row>
      </v-col>
      <v-col cols="9" md="8" lg="9" xl="9" align="center" justify="center">
        <v-row no-gutters align="center" justify="center">
          <v-col cols="3" md="6" lg="4" xl="3">
            <v-btn
              class="mx-3 cs-btn-rewind"
              large
              icon
              @click="playPrevChunk()"
            >
              <v-icon medium color="blue-grey lighten-2">
                mdi-skip-previous-outline
              </v-icon>
            </v-btn>
            <v-btn
              class="mx-3 cs-btn-play"
              fab
              small
              color="secondary"
              @click="togglePlay()"
            >
              <v-icon medium color="blue-grey lighten-2">{{ playIcon }}</v-icon>
            </v-btn>
            <v-btn class="mx-3" icon large @click="playNextChunk()">
              <v-icon medium color="blue-grey lighten-2">
                mdi-skip-next-outline
              </v-icon>
            </v-btn>
          </v-col>
          <v-col cols="8" md="6" lg="7" xl="8">
            <vue-plyr ref="plyr">
              <audio :data-plyr-config="vuePlyrConfig">
                <source :src="transcriptionUrl" type="audio/mp3" />
              </audio>
            </vue-plyr>
          </v-col>
          <v-col cols="1" md="1" lg="1" xl="1">
            <v-btn class="mx-3" icon large @click="toggleVolume()">
              <v-icon medium color="blue-grey lighten-2">
                {{ volumeIcon }}
              </v-icon>
            </v-btn>
          </v-col>
        </v-row>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import { mapState, mapActions } from "vuex";

export default {
  name: "Player",
  props: {
    dialogs: Array,
    transcript: {
      type: Object,
      required: true,
    },
    enableScroll: {
      type: Boolean,
      required: true,
    },
  },
  computed: {
    ...mapState("transcript", ["currentFile"]),
    player() {
      return this.$refs.plyr.player;
    },
    fileInfo() {
      const fileExtension = this.currentFile?.name
        .split(".")
        .pop()
        .toUpperCase();

      const fileName = this.currentFile?.name.split(".").slice(0, -1).join(".");

      const fileAdded = this.$formatDateDescriptive(
        this.currentFile?.createdOn
      );

      return {
        fileName,
        fileExtension,
        fileAdded,
      };
    },

    vuePlyrConfig() {
      const vuePlyrConfig = {
        controls: ["current-time", "progress", "duration"],
      };

      return JSON.stringify(vuePlyrConfig);
    },
  },
  data: () => ({
    currentChunkIndex: -1,
    isLoadingAudio: false,
    transcriptionUrl: "",
    playerTime: 0,

    playIcon: "mdi-play",
    volumeIcon: "mdi-volume-high",
    chunkToPlay: null,
    playOnlyOneChunk: false,
  }),
  methods: {
    ...mapActions("transcript", ["getAudioFileUrlAsync"]),
    async getTranscriptionFileAsync() {
      this.transcriptionUrl = await this.getAudioFileUrlAsync(
        this.transcript.id
      );
    },
    async togglePlay() {
      if (this.player.playing) {
        this.pause();
      } else {
        this.play();
      }
    },
    toggleVolume() {
      if (this.player.muted) {
        this.setVolumeHigh();
      } else {
        this.setVolumeOff();
      }
    },
    setVolumeHigh() {
      this.volumeIcon = "mdi-volume-high";
      this.player.muted = false;
    },
    setVolumeOff() {
      this.volumeIcon = "mdi-volume-off";
      this.player.muted = true;
    },
    play() {
      this.player.play();
      this.playIcon = "mdi-pause";
    },
    pause() {
      this.player.pause();
      this.playIcon = "mdi-play";
    },
    playPrevChunk() {
      this.playOnlyOneChunk = false;
      let prevChunk = this.getPrevChunk();
      if (prevChunk) {
        this.chunkToPlay = null;
        this.player.currentTime = prevChunk.startTime;
      }
    },
    playNextChunk() {
      this.playOnlyOneChunk = false;
      let nextChunk = this.getNextChunk();
      if (nextChunk) {
        this.chunkToPlay = null;
        this.player.currentTime = nextChunk.startTime;
      }
    },
    getPrevChunk() {
      let prevChunkIndex = this.currentChunkIndex - 1;
      return this.dialogs[prevChunkIndex];
    },
    getNextChunk() {
      let nextChunkIndex = this.currentChunkIndex + 1;
      return this.dialogs[nextChunkIndex];
    },
    getCurrentChunkIndex() {
      let currentTime = parseFloat(this.player.currentTime.toFixed(2));
      this.currentChunkIndex = this.dialogs.findIndex(
        (el) => currentTime >= el.startTime && currentTime <= el.endTime - 0.02
      );
    },
    setScroll() {
      if (this.enableScroll) {
        const elem = this.getDomElementByCurrentChunkIndex();
        const stepElem = document.getElementsByClassName("cs-stepper-items")[0];
        if (elem && stepElem) {
          const yOffset =
            elem.getBoundingClientRect().top + stepElem.scrollTop - 200;
          stepElem.scrollTo({ top: yOffset, behavior: "smooth" });
        }
      }
    },
    // method to play the specific chunk
    playChunk(chunk) {
      if (this.chunkToPlay) {
        this.play();
      } else {
        this.chunkToPlay = chunk;
        this.player.currentTime = chunk.startTime;
        this.playOnlyOneChunk = true;
        this.currentChunkIndex = this.dialogs.findIndex(
          (el) => el.id === chunk.id
        );
        this.play();
      }
    },
    addCurrentChunkStyle() {
      this.$emit(
        "addCurrentChunkStyle",
        this.getDomElementByCurrentChunkIndex(),
        this.currentChunkIndex,
        this.player.playing
      );
    },
    getDomElementByCurrentChunkIndex() {
      return document.getElementById(
        `transcription-chunk-${this.currentChunkIndex}`
      );
    },
  },
  async mounted() {
    await this.getTranscriptionFileAsync();
    this.setVolumeHigh();

    this.$refs.plyr.player.on("seeking", () => {
      if (this.chunkToPlay === undefined || this.chunkToPlay === null) {
        this.getCurrentChunkIndex();
        this.setScroll();
        if (this.playOnlyOneChunk) {
          setTimeout(() => this.pause(), 100);
          this.playOnlyOneChunk = false;
        }
      }
      this.addCurrentChunkStyle();
    });

    this.$refs.plyr.player.on("timeupdate", () => {
      if (!this.playOnlyOneChunk) {
        this.getCurrentChunkIndex();
        this.setScroll();
      }
      if (
        this.player.currentTime >= this.chunkToPlay?.endTime ||
        this.player.currentTime < this.chunkToPlay?.startTime
      ) {
        this.$emit("changeChunkPlayingState", this.chunkToPlay);
        this.chunkToPlay = null;
        this.pause();
      }
      this.addCurrentChunkStyle();
    });
    this.player.on("ended", () => {
      this.pause();
    });
  },
};
</script>
<style lang="scss">
@import "@/scss/components/wizard/Player.scss";
</style>