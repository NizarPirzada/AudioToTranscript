<template>
  <v-container class="py-0">
    <v-form ref="stepOneForm" v-model="validStepOneForm" lazy-validation>
      <v-row no-gutters>
        <v-col cols="4">
          <div class="step-text-select-field">
            <p for="transcriptType" class="ma-0 pa-0">
              {{ $t("step1_transcript_type_label") }}:
            </p>
            <select
              class="cs-select cs-select-transcipttype"
              name="transcriptType"
              v-model="transcript.transcriptType"
            >
              <option
                v-for="(item, i) in getTranscriptTypes"
                :key="i"
                v-bind:value="item.value"
                v-text="item.key"
              ></option>
            </select>
          </div>
        </v-col>
      </v-row>

      <v-row no-gutters>
        <v-col cols="12">
          <v-card class="cs-step-card">
            <v-card-text class="pb-0">
              <span class="step-section-header">{{
                $t("step1_court_info_label")
              }}</span>
              <v-layout row wrap class="step-layout-horizontal">
                <v-text-field
                  class="step-text-field medium"
                  :label="$t('step1_court_name_label')"
                  outlined
                  v-model="transcript.court.name"
                  :disabled="active"
                ></v-text-field>
              </v-layout>
              <v-layout row wrap class="step-layout-horizontal">
                <v-text-field
                  v-if="isHearingTranscript"
                  class="step-text-field medium"
                  :label="$t('step1_address_label')"
                  outlined
                  v-model="transcript.court.address"
                  :disabled="active"
                ></v-text-field>
                <v-text-field
                  v-if="isHearingTranscript || isDepositionTranscript"
                  class="step-text-field small"
                  :label="courtCityLabel"
                  outlined
                  v-model="transcript.court.city"
                  :disabled="active"
                ></v-text-field>
                <v-text-field
                  v-if="isHearingTranscript || isDepositionTranscript"
                  class="step-text-field small"
                  :label="$t('step1_state_label')"
                  outlined
                  v-model="transcript.court.state"
                  :disabled="active"
                ></v-text-field>
                <v-text-field
                  v-if="isHearingTranscript"
                  class="step-text-field small"
                  :label="$t('step1_zipcode_label')"
                  outlined
                  v-model="transcript.court.zipcode"
                  :disabled="active"
                ></v-text-field>
              </v-layout>
              <v-layout row wrap class="step-layout-horizontal">
                <v-text-field
                  v-if="isHearingTranscript || isDepositionTranscript"
                  class="step-text-field small"
                  :label="$t('step1_casenumber_label')"
                  outlined
                  v-model="transcript.caseNumber"
                  :disabled="active"
                ></v-text-field>
                <v-text-field
                  v-if="isHearingTranscript"
                  class="step-text-field small"
                  :label="$t('step1_judge_name_label')"
                  outlined
                  v-model="transcript.judgeName"
                  :disabled="active"
                ></v-text-field>
                <v-text-field
                  v-if="isHearingTranscript"
                  class="step-text-field small"
                  :label="$t('step1_judge_title_label')"
                  outlined
                  v-model="transcript.judgeTitle"
                  :disabled="active"
                ></v-text-field>
                <v-text-field
                  v-if="isDepositionTranscript"
                  class="step-text-field small"
                  :label="$t('step1_dept_number_label')"
                  :rules="deptNumberRules"
                  outlined
                  v-model="transcript.deptNumber"
                  :disabled="active"
                ></v-text-field>
              </v-layout>
            </v-card-text>
          </v-card>
        </v-col>
      </v-row>

      <v-row no-gutters v-if="isDepositionTranscript">
        <v-col cols="12">
          <RecordingInfo
            :data="transcript.transcriptRecordingInfo"
            :active="active"
          ></RecordingInfo>
        </v-col>
      </v-row>

      <v-row no-gutters v-if="isDepositionTranscript">
        <v-col cols="12">
          <PersonTypeList
            class="cs-person-type"
            :title="$t('step1_deponent_label')"
            :type="getPersonDeponentType"
            :persons="deponents"
            :transcriptType="transcript.transcriptType"
            :active="active"
          ></PersonTypeList>
        </v-col>
      </v-row>

      <v-row v-if="isHearingTranscript || isDepositionTranscript">
        <v-col cols="6">
          <PersonTypeList
            class="cs-person-type"
            :title="$t('step1_plaintiff_label')"
            :type="getPersonPlaintiffType"
            :persons="plaintiffs"
            :active="active"
          ></PersonTypeList>

          <PersonTypeList
            class="cs-person-type"
            :title="$t('step1_plaintiff_attorney_label')"
            :type="getPersonPlaintiffAttorneyType"
            :persons="plaintiffAttorneys"
            :transcriptType="transcript.transcriptType"
            :active="active"
            newEnabled
          ></PersonTypeList>
        </v-col>
        <v-col cols="6">
          <PersonTypeList
            class="cs-person-type"
            :title="$t('step1_defendant_label')"
            :type="getPersonDefendantType"
            :persons="defendants"
            :active="active"
          ></PersonTypeList>

          <PersonTypeList
            class="cs-person-type"
            :title="$t('step1_defendant_attorney_label')"
            :type="getPersonDefendantAttorneyType"
            :persons="defendantAttorneys"
            :transcriptType="transcript.transcriptType"
            :active="active"
            newEnabled
          ></PersonTypeList>
        </v-col>
      </v-row>

      <v-row
        no-gutters
        v-if="isHearingTranscript || isDepositionTranscript"
        class="mt-2"
      >
        <v-col cols="12">
          <AdditionalSpeakerList
            :title="$t('step1_additional_speaker_label')"
            :type="getPersonAdditionalSpeakerType"
            :persons="additionalSpeakers"
            :active="active"
            newEnabled
          >
          </AdditionalSpeakerList>
        </v-col>
      </v-row>
    </v-form>

    <StepperActionButtons
      :buttonList="buttonList"
      @next="next"
    ></StepperActionButtons>
  </v-container>
</template>

<script>
import { StringHelper, ObjectHelper } from "@/utilities/Helpers.js";
import PersonTypeList from "@/components/wizard/PersonTypeList";
import { PersonType, TranscriptType, VuexKeys } from "@/./environment.js";
import { SnackbarType } from "@/utilities/Enumerations";
import RecordingInfo from "@/components/wizard/RecordingInfo";
import { FieldRulesHelper } from "@/./utilities/Helpers.js";
import AdditionalSpeakerList from "@/components/wizard/AdditionalSpeakerList";
import StepperActionButtons from "@/components/wizard/StepperActionButtons";

export default {
  name: "StepOne",
  props: {
    transcript: {
      type: Object,
      required: true,
    },
    persons: {
      type: Array,
      required: true,
    },
    active: {
      type: Boolean,
      required: true,
    },
  },
  components: {
    PersonTypeList,
    RecordingInfo,
    AdditionalSpeakerList,
    StepperActionButtons,
  },
  data: () => {
    let plaintiffs = [];
    let defendants = [];
    let plaintiffAttorneys = [];
    let defendantAttorneys = [];
    let additionalSpeakers = [];
    let validStepOneForm = true;
    let deponents = [];
    const deptNumberRules = [
      (v) => FieldRulesHelper.optionalAlphanumeric(v, "Dept Number"),
      (v) => FieldRulesHelper.optionalMaxCharacters(v, "Dept Number", 8),
    ];
    return {
      plaintiffs,
      defendants,
      plaintiffAttorneys,
      defendantAttorneys,
      additionalSpeakers,
      validStepOneForm,
      deponents,
      deptNumberRules,
    };
  },
  computed: {
    stepOneFormErrorMessage() {
      return this.$t("step1_error_form_not_valid");
    },
    getPersonPlaintiffType() {
      return PersonType.Plaintiff;
    },
    getPersonPlaintiffAttorneyType() {
      return PersonType.PlaintiffAttorney;
    },
    getPersonDefendantType() {
      return PersonType.Defendant;
    },
    getPersonDefendantAttorneyType() {
      return PersonType.DefendantAttorney;
    },
    getPersonAdditionalSpeakerType() {
      return PersonType.AdditionalSpeaker;
    },
    getPersonDeponentType() {
      return PersonType.Deponent;
    },
    getTranscriptTypes() {
      let transcriptTypes = [];
      for (const [key, value] of Object.entries(TranscriptType)) {
        let labelName = `step1_type_${key.toLowerCase().replace(/\s+/g, "")}`;
        let keyName = this.$t(labelName);
        let type = { key: keyName, value };
        transcriptTypes.push(type);
      }
      return transcriptTypes;
    },
    isDepositionTranscript() {
      return this.transcript.transcriptType === TranscriptType["Deposition"];
    },
    isHearingTranscript() {
      return (
        this.transcript.transcriptType ===
        TranscriptType["Probable Cause Hearing"]
      );
    },
    courtCityLabel() {
      return this.isDepositionTranscript
        ? this.$t("step1_county_label")
        : this.$t("step1_city_label");
    },
    buttonList() {
      return [
        {
          text: this.$t("step_next_button"),
          disabled: this.active,
          icon: "mdi-chevron-right",
          color: "primary",
          action: "next",
        },
      ];
    },
  },
  methods: {
    formatPersonList(list) {
      let temporalList = list.filter(
        (x) =>
          !!x.id ||
          !StringHelper.IsNullOrEmpty(x.firstName) ||
          !StringHelper.IsNullOrEmpty(x.lastName)
      );
      temporalList.forEach((element) => {
        element.transcriptId = this.transcript.id;
      });
      return temporalList;
    },
    next() {
      if (this.$refs.stepOneForm.validate()) {
        this.transcript.plaintiffs = this.formatPersonList(this.plaintiffs);
        this.transcript.defendants = this.formatPersonList(this.defendants);
        this.transcript.deponents = this.formatPersonList(this.deponents);
        this.transcript.plaintiffAttorneys = this.formatPersonList(
          this.plaintiffAttorneys
        );
        this.transcript.defendantAttorneys = this.formatPersonList(
          this.defendantAttorneys
        );
        this.transcript.additionalSpeakers = this.formatPersonList(
          this.additionalSpeakers
        );

        this.$emit("next", this.transcript);
      } else {
        this.$eventBus.$emit(
          VuexKeys.Home.ShowSnackbarMessage,
          this.stepOneFormErrorMessage,
          SnackbarType.Error
        );
      }
    },
  },
  watch: {
    persons: function (val) {
      if (val && val.length != 0) {
        this.plaintiffs = val.filter((x) => x.type === PersonType.Plaintiff);
        this.defendants = val.filter((x) => x.type === PersonType.Defendant);
        this.additionalSpeakers = val.filter(
          (x) => x.type === PersonType.AdditionalSpeaker
        );
        this.plaintiffAttorneys = val.filter(
          (x) => x.type === PersonType.PlaintiffAttorney
        );
        this.defendantAttorneys = val.filter(
          (x) => x.type === PersonType.DefendantAttorney
        );
        this.deponents = val.filter((x) => x.type === PersonType.Deponent);
      }
      if (!this.plaintiffs || this.plaintiffs.length === 0) {
        this.plaintiffs.push(
          ObjectHelper.GetEmptyPersonObject(
            PersonType.Plaintiff,
            this.transcript.id
          )
        );
      }
      if (!this.plaintiffAttorneys || this.plaintiffAttorneys.length === 0) {
        this.plaintiffAttorneys.push(
          ObjectHelper.GetEmptyPersonObject(
            PersonType.PlaintiffAttorney,
            this.transcript.id
          )
        );
      }
      if (!this.defendants || this.defendants.length === 0) {
        this.defendants.push(
          ObjectHelper.GetEmptyPersonObject(
            PersonType.Defendant,
            this.transcript.id
          )
        );
      }
      if (!this.defendantAttorneys || this.defendantAttorneys.length === 0) {
        this.defendantAttorneys.push(
          ObjectHelper.GetEmptyPersonObject(
            PersonType.DefendantAttorney,
            this.transcript.id
          )
        );
      }
      if (!this.additionalSpeakers || this.additionalSpeakers.length === 0) {
        this.additionalSpeakers.push(
          ObjectHelper.GetEmptyPersonObject(
            PersonType.AdditionalSpeaker,
            this.transcript.id
          )
        );
      }
      if (!this.deponents || this.deponents.length === 0) {
        this.deponents.push(
          ObjectHelper.GetEmptyPersonObject(
            PersonType.Deponent,
            this.transcript.id
          )
        );
      }
    },
  },
  mounted() {
    if (this.transcript && !this.transcript.transcriptType) {
      this.transcript.transcriptType = TranscriptType["Probable Cause Hearing"];
    }
  },
};
</script>