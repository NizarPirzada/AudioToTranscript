<template>
  <v-container>
    <div class="preview-header">
      <span class="chunk-label-read-only chunk-timelapse">{{ this.$t("step4_transcript_preview_title") }}</span>
    </div>
    <div class="preview-container">
      <div
        v-for="(line, index) in linesToShow"
        :key="index"
        class="preview-container-line"
      >
        <div class="preview-numbered">
          <span>{{ index + 1 }}</span>
        </div>
        <v-divider vertical color="black"></v-divider>
        <div class="transcript-line">
          <span>&nbsp; {{ line }}</span>
        </div>
      </div>
      <div class="preview-footer">
        <v-btn icon @click="prevPage">
          <v-icon color="header-text-color">mdi-skip-previous</v-icon>
        </v-btn>
        <span class="chunk-label-read-only"
          >{{ currentPage + 1 }} / {{ totalPages }}</span
        >
        <v-btn icon @click="nextPage">
          <v-icon color="header-text-color">mdi-skip-next</v-icon>
        </v-btn>
      </div>
    </div>
  </v-container>
</template>

<script>

export default {
  name: "TranscriptPreview",
  props: {
    lines: {
      type: Array,
      required: true
    },
    linesPerPage: {
      type: Number,
      required: true
    }
  },
  data: () => {
    let currentPage = 0;
    return {
      currentPage,
    };
  },
  computed: {
    linesToShow() {
      const getPageLines = this.lines.slice(
        this.currentPage * this.linesPerPage,
        this.linesPerPage * (this.currentPage + 1)
      );
      if (getPageLines.length < this.linesPerPage) {
        let missingLines = this.linesPerPage - getPageLines.length;
        for (let i = 1; i <= missingLines; i++) {
          getPageLines.push("");
        }
      }
      return getPageLines;
    },
    totalPages() {
      let pages = this.lines.length / this.linesPerPage;
      return Math.ceil(pages);
    },
  },
  methods: {
    nextPage() {
      if (this.currentPage < this.totalPages -1){
        this.currentPage++;
      }
    },
    prevPage() {
      if (this.currentPage > 0){
        this.currentPage--;
      }
    },
  },
};
</script>