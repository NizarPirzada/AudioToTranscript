<template>
  <v-container class="pa-0 ma-0">
    <div
      v-show="isOnEdition"
      v-closable="{
        exclude: [
          'select',
          'chgAll',
          'chgOne',
          'triggerEditor',
          'newSpeakerDialogCard',
        ],
        handler: 'onClose',
      }"
    >
      <select
        ref="select"
        class="cs-select light speaker-select"
        v-model="selected"
        @change.prevent="onChangeSpeakerOption($event)"
      >
        <option
          v-for="speaker in calculatedSpeakers"
          v-bind:value="speaker.id"
          :key="speaker.id"
        >
          {{ speaker.name }}
        </option>
        <option v-bind:value="newSpeakerOptionSelected">
          <span>+ {{ $t("step3_speaker_selector_add_new") }}</span>
        </option>
      </select>
      <a
        ref="chgAll"
        class="speaker-btn-primary"
        v-on:click="changeAllSpeakers()"
      >
        {{ this.$t("step3_speaker_change_all") }}
      </a>
      <a
        ref="chgOne"
        class="speaker-btn-secondary"
        v-on:click="changeOneSpeaker()"
      >
        {{ this.$t("step3_speaker_change_single") }}
      </a>
    </div>
    <div v-show="!isOnEdition" class="cs-speaker-name-container">
      <span class="cs-speaker-name-label">{{ speakerLabel }} </span>
      <a ref="triggerEditor" v-on:click="editSpeaker()">
        <v-icon color="black" size="17">mdi-pencil</v-icon>
      </a>
    </div>
    <v-dialog
      content-class="cs-new-speaker-dialog"
      v-model="showNewSpeakerDialog"
      width="495"
      @keydown.esc.exact="cancelNewSpeakerDialogForm"
      @keydown.enter.exact="saveNewSpeakerDialogForm"
      eager
    >
      <v-card ref="newSpeakerDialogCard">
        <v-card-title class="pa-0">
          <div class="mt-10 ml-8">
            <p class="cs-new-speaker-dialog-title">{{ newSpeakerDialogCardTitle }}</p>
          </div>
        </v-card-title>
        <v-form ref="newSpeakerDialogForm" v-model="validNewSpeakerDialogForm" lazy-validation>
          <v-card-text class="py-0">
            <v-row class="px-1 mx-1">
              <v-col cols="12" class="pa-0 ma-0">
                <v-text-field
                  class="cs-new-speaker-dialog-text-field"
                  v-model="newSpeaker.firstName"
                  :rules="firstNameRules"
                  :label="newSpeakerDialogFirstNameLabel"
                  required
                  outlined
                  dense
                ></v-text-field>
              </v-col>
              <v-col cols="12" class="pa-0 ma-0">
                <v-text-field
                  class="cs-new-speaker-dialog-text-field"
                  v-model="newSpeaker.lastName"
                  :rules="lastNameRules"
                  :label="newSpeakerDialogLastNameLabel"
                  required
                  outlined
                  dense
                ></v-text-field>
              </v-col>
            </v-row>
          </v-card-text>
          <v-card-actions class="pa-0">
            <v-spacer></v-spacer>
            <div class="pb-5 pr-2">
              <v-btn
                class="cs-btn"
                text
                @click="cancelNewSpeakerDialogForm()"
                :disabled="newSpeakerDialogLoading"
              >
                {{ newSpeakerDialogCardCancelButton }}
              </v-btn>
              <v-btn
                class="cs-btn ma-2"
                color="primary"
                :disabled="!validNewSpeakerDialogForm"
                :loading="newSpeakerDialogLoading"
                @click="saveNewSpeakerDialogForm()"
              >
                {{ newSpeakerDialogCardConfirmButton }}
              </v-btn>
            </div>
          </v-card-actions>
        </v-form>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script>
import { TranscriptType, PersonType } from "@/./environment.js";
import { FieldRulesHelper } from "@/./utilities/Helpers.js";
import { mapActions } from "vuex";
import { VuexKeys } from "@/./environment.js";
import { SnackbarType } from "@/utilities/Enumerations";

export default {
  name: "SpeakerSelector",
  props: {
    dialogPosition: {
      type: Number,
      required: true,
    },
    dialog: {
      type: Object,
      required: true,
    },
    speakers: {
      type: Array,
      required: true,
    },
    transcript: {
      type: Object,
      required: true,
    },
  },
  data: () => {
    let isOnEdition = false;
    return {
      isOnEdition,
      selected: 0,
      newSpeakerOptionSelected: "newUser",
      showNewSpeakerDialog: false,
      validNewSpeakerDialogForm: true,
      newSpeakerDialogLoading: false,
      newSpeaker: {},
    };
  },
  computed: {
    calculatedSpeakers() {
      let personsFiltered = [];

      if (this.isHearingTranscript) {
        const hearingSpeakers = this.speakers.filter((val) => {
          if (val.type !== PersonType.Deponent) {
            return val;
          }
        });

        personsFiltered = hearingSpeakers.map((x) => {
          return {
            id: x.id,
            name: `${x.firstName} ${x.lastName}`,
          };
        });
      } else {
        personsFiltered = this.speakers.map((x) => {
          return {
            id: x.id,
            name: `${x.firstName} ${x.lastName}`,
          };
        });
      }

      personsFiltered.push({
        id: 0,
        name: this.getSpeakerName(),
      });

      return personsFiltered;
    },
    speakerLabel() {
      let vm = this;
      let defaultSpeaker = this.getSpeakerName();
      if (this.dialog.personId === 0) {
        return defaultSpeaker;
      } else {
        let personSelected = _.find(this.speakers, function (item) {
          return item.id === vm.dialog.personId;
        });
        if (personSelected) {
          return `${personSelected.firstName} ${personSelected.lastName}`;
        } else {
          return defaultSpeaker;
        }
      }
    },
    isHearingTranscript() {
      return (
        this.transcript.transcriptType ===
        TranscriptType["Probable Cause Hearing"]
      );
    },
    newSpeakerDialogCardTitle() {
      return this.$t("step3_new_speaker_dialog_title");
    },
    newSpeakerDialogCardCancelButton() {
      return this.$t("step3_new_speaker_dialog_cancel_button");
    },
    newSpeakerDialogCardConfirmButton() {
      return this.$t("step3_new_speaker_dialog_confirm_button");
    },
    newSpeakerDialogForm() {
      return this.$refs.newSpeakerDialogForm;
    },
    newSpeakerDialogFirstNameLabel() {
      return this.$t("step3_new_speaker_dialog_first_name_label");
    },
    newSpeakerDialogLastNameLabel() {
      return this.$t("step3_new_speaker_dialog_last_name_label");
    },
    firstNameRules() {
      return [
        (v) => FieldRulesHelper.requiredField(v, this.newSpeakerDialogFirstNameLabel),
        (v) =>
          FieldRulesHelper.requiredMaxCharacters(v, this.newSpeakerDialogFirstNameLabel, 64),
      ];
    },
    lastNameRules() {
      return [
        (v) => FieldRulesHelper.requiredField(v, this.newSpeakerDialogLastNameLabel),
        (v) => FieldRulesHelper.requiredMaxCharacters(v, this.newSpeakerDialogLastNameLabel, 64),
      ];
    },
  },
  methods: {
    ...mapActions("transcript", ["savePersonAsync"]),
    getSpeakerName() {
      if (this.dialog.originalSpeakerName.length === 1) {
        return `Speaker ${this.dialog.originalSpeakerName}`;
      }
      return this.dialog.originalSpeakerName;
    },
    onClose() {
      this.selected = this.dialog.personId;
      this.isOnEdition = false;
      this.closeNewSpeakerDialog();
    },
    editSpeaker() {
      this.selected = this.dialog.personId;
      this.isOnEdition = true;
    },
    changeAllSpeakers() {
      this.dialog.personId = this.selected;
      this.isOnEdition = false;
      this.$emit("saveAll", this.dialog);
    },
    changeOneSpeaker() {
      this.dialog.personId = this.selected;
      this.isOnEdition = false;
      this.$emit("saveSingle", this.dialogPosition, this.dialog);
    },
    onChangeSpeakerOption(event) {
      if (event.target.value === this.newSpeakerOptionSelected) {
        this.showNewSpeakerDialog = true;
      }
    },
    closeNewSpeakerDialog() {
      this.showNewSpeakerDialog = false;
      this.newSpeakerDialogForm?.reset();
      this.newSpeakerDialogForm?.resetValidation();
    },
    cancelNewSpeakerDialogForm() {
      this.closeNewSpeakerDialog();
      this.selected = this.dialog.personId;
    },
    async saveNewSpeakerDialogForm() {
      try {
        this.newSpeakerDialogLoading = true;
        if (this.newSpeakerDialogForm.validate()) {
          this.newSpeaker.type = PersonType.AdditionalSpeaker;
          this.newSpeaker.transcriptId = this.transcript.id;
          
          await this.savePersonAsync(this.newSpeaker).then((response) => {
            if (response.success) {
              const newPersonDto = response.data;
              this.$emit("saveNewOnTheFlightSpeakerAsync", newPersonDto);
              setTimeout(() => (this.selected = newPersonDto.id), 500);
              this.closeNewSpeakerDialog();
            }
          });
        }
      } catch (error) {
        this.$eventBus.$emit(
          VuexKeys.Home.ShowSnackbarMessage,
          error,
          SnackbarType.Error
        );
      } finally {
        this.newSpeakerDialogLoading = false;
      }
    },
  },
};
</script>
<style lang="scss">
@import "@/scss/components/wizard/SpeakerSelector.scss";
</style>