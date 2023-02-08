<template>
  <v-dialog v-model="show" persistent content-class="cs-transcript-dialog">
    <v-card>
      <v-card-title class="pa-0">
        <div class="mt-8 ml-8">
          <p class="cs-new-transcript-title">{{ $t("transcript_new_tittle") }}</p>
        </div>
      </v-card-title>
      <v-card-text class="pa-0">
        <v-row class="pa-0 ma-0">
              <v-col cols="12" class="pa-0 ma-0">
                <v-text-field
                  v-model="transcriptName"
                  class="cs-new-transcript-input"
                  outlined
                  :rules="nameRules"
                  required
                  single-line
                  :error-messages="modelErrors"
                ></v-text-field>
              </v-col>
        </v-row>
      </v-card-text>
      <v-card-actions class="pa-0">
        <v-spacer></v-spacer>
        <div class="pb-8 pr-10">
          <v-btn class="cs-btn" text @click="cancelAction()"> {{ $t("transcript_new_cancel_button") }} </v-btn>
          <v-btn
            class="cs-btn"
            color="primary"
            @click="saveAction()"
            :disabled="!isValidName"
          >
            {{ $t("transcript_new_create_button") }}
          </v-btn>
        </div>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  name: "NewTranscriptDialog",
  data: () => ({
    transcriptName: "",
  }),
  props: {
    show: {
      type: Boolean,
      required: true,
    },
    persistent: {
      type: Boolean,
      default: true,
    },
    transcriptList: {
      type: Array,
    },
  },
  model: {
    prop: "show",
    event: "input",
  },
  computed: {
    nameRules() {
      return [
        (v) => !!v || this.$t("transcript_new_error_name_required"),
        (v) => v.length <= 64 || this.$t("transcript_new_error_name_length"),
        (v) =>
          /^(\w|\s)+$/.test(v) || this.$t("transcript_new_error_name_type"),
      ];
    },
    isValidName() {
      let nameFormat = /^(\w|\s)+$/.test(this.transcriptName);
      let isDuplicated = this.isNameDuplicated();
      return nameFormat && !isDuplicated;
    },
    modelErrors() {
      const errors = [];
      let duplicated = this.isNameDuplicated();
      if (duplicated) {
        errors.push(this.$t("transcript_new_error_same_name"));
      }
      return errors;
    },
  },
  methods: {
    isNameDuplicated() {
      return this.transcriptList.find(
        (t) => t.name === this.transcriptName.trim().replace(/\s+/g, " ")
      );
    },
    cancelAction() {
      this.transcriptName = "";
      this.$emit("input", false);
    },
    saveAction() {
      let duplicated = this.isNameDuplicated();
      if (!duplicated) {
        let newTranscript = this.transcriptName;
        this.transcriptName = "";
        this.$emit("create", newTranscript);
      }
    },
  },
};
</script>
<style lang="scss">
@import "@/scss/components/dialog/NewTranscriptDialog.scss";
</style>