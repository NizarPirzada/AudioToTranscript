<template>
  <v-card class="cs-step-card">
    <v-card-text class="pb-0">
      <span class="step-section-header">{{ title }}</span>
      <v-layout row wrap class="step-layout-horizontal">
        <v-text-field
          class="step-text-field medium"
          :label="firstNameLabel"
          outlined
          :rules="firstNameRules"
          v-model="person.firstName"
          :disabled="active"
        ></v-text-field>
        <v-text-field
          class="step-text-field medium"
          :label="middleNameLabel"
          outlined
          v-if="isDeponentDeposition"
          :rules="middleNameRules"
          v-model="person.middleName"
          :disabled="active"
        ></v-text-field>
        <v-text-field
          class="step-text-field medium"
          :label="lastNameLabel"
          outlined
          :rules="lastNameRules"
          v-model="person.lastName"
          :disabled="active"
        ></v-text-field>
      </v-layout>
      <v-layout row wrap class="step-layout-horizontal" v-if="isAttorney">
        <v-text-field
          class="step-text-field small"
          :label="$t('step1_person_bar_number_label')"
          v-if="!isDepositionTranscript"
          outlined
          v-model="person.additionalInfo.barNumber"
          :disabled="active"
        ></v-text-field>
        <v-text-field
          class="step-text-field small"
          :label="legalFirmLabel"
          v-if="isDepositionTranscript"
          :rules="legalFirmRules"
          outlined
          v-model="person.additionalInfo.legalFirm"
          :disabled="active"
        ></v-text-field>
        <v-text-field
          class="step-text-field small"
          :label="$t('step1_person_title_label')"
          outlined
          v-model="person.additionalInfo.title"
          :disabled="active"
        ></v-text-field>
        <v-text-field
          class="step-text-field"
          :class="isDepositionTranscript ? 'medium' : 'small'"
          :label="$t('step1_person_address_label')"
          outlined
          v-model="person.additionalInfo.address"
          :disabled="active"
        ></v-text-field>
        <v-text-field
          class="step-text-field small"
          :label="$t('step1_person_telephone_label')"
          outlined
          v-if="!isDepositionTranscript"
          v-model="person.additionalInfo.telephone"
          :disabled="active"
        ></v-text-field>
      </v-layout>
    </v-card-text>
  </v-card>
</template>

<script>
import { PersonType, TranscriptType } from "@/./environment.js";
import { FieldRulesHelper } from "@/./utilities/Helpers.js";

export default {
  name: "PersonSimpleForm",
  props: {
    title: {
      type: String,
      required: true,
    },
    person: {
      type: Object,
      required: true,
    },
    active: {
      type: Boolean,
      required: true,
    },
    transcriptType: {
      type: Number,
      required: false,
    },
  },
  data: () => {
    return {};
  },
  computed: {
    firstNameLabel() {
      return this.$t("step1_person_first_name_label");
    },
    lastNameLabel() {
      return this.$t("step1_person_last_name_label");
    },
    middleNameLabel() {
      return this.$t("step1_person_middle_name_label");
    },
    legalFirmLabel() {
      return this.$t("step1_person_legal_firm_label");
    },
    firstNameRules() {
      return [
        (v) =>
          FieldRulesHelper.optionalMaxCharacters(v, this.firstNameLabel, 64),
      ];
    },
    lastNameRules() {
      return [
        (v) =>
          FieldRulesHelper.optionalMaxCharacters(v, this.lastNameLabel, 64),
      ];
    },
    middleNameRules() {
      return [
        (v) =>
          FieldRulesHelper.optionalMaxCharacters(v, this.middleNameLabel, 64),
      ];
    },
    legalFirmRules() {
      return [
        (v) =>
          FieldRulesHelper.optionalMaxCharacters(v, this.legalFirmLabel, 64),
      ];
    },
    isAttorney() {
      return (
        this.person.type === PersonType.PlaintiffAttorney ||
        this.person.type === PersonType.DefendantAttorney
      );
    },
    isDepositionTranscript() {
      return this.transcriptType === TranscriptType["Deposition"];
    },
    isDeponentDeposition() {
      return (
        this.person.type === PersonType.Deponent && this.isDepositionTranscript
      );
    },
  },
};
</script>