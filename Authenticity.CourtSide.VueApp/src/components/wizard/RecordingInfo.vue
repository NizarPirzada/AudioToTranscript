<template>
  <v-card class="cs-step-card">
    <v-card-text class="pb-0">
      <span class="step-section-header">{{ $t("step1_recording_title") }}</span>
      <v-layout row wrap class="step-layout-horizontal">
        <v-text-field
          class="step-text-field medium"
          :label="venueLabel"
          :rules="venueCompanyRules"
          :disabled="active"
          outlined
          v-model="data.venueCompany"
        ></v-text-field>
      </v-layout>
      <v-layout row wrap class="step-layout-horizontal">
        <v-text-field
          class="step-text-field medium"
          :label="addressLabel"
          :rules="streetAddressRules"
          :disabled="active"
          outlined
          v-model="data.streetAddress"
        ></v-text-field>
        <v-text-field
          class="step-text-field small"
          :label="cityLabel"
          :rules="cityRules"
          :disabled="active"
          outlined
          v-model="data.city"
        ></v-text-field>
        <v-text-field
          class="step-text-field small"
          :label="stateLabel"
          :rules="stateRules"
          :disabled="active"
          outlined
          v-model="data.state"
        ></v-text-field>
        <v-text-field
          class="step-text-field small"
          :label="zipcodeLabel"
          :rules="zipCodeRules"
          :disabled="active"
          outlined
          v-model="data.zipcode"
        ></v-text-field>
      </v-layout>
    </v-card-text>
  </v-card>
</template>

<script>
import { FieldRulesHelper } from "@/./utilities/Helpers.js";

export default {
  name: "RecordingInfo",
  props: {
    data: {
      type: Object,
      required: true,
    },
    active: {
      type: Boolean,
      required: true,
    },
  },
  components: {},
  data: () => {
    return {};
  },
  computed: {
    venueLabel() {
      return this.$t("step1_recording_venue");
    },
    addressLabel() {
      return this.$t("step1_recording_address");
    },
    cityLabel() {
      return this.$t("step1_recording_city");
    },
    stateLabel() {
      return this.$t("step1_recording_state");
    },
    zipcodeLabel() {
      return this.$t("step1_recording_zipcode");
    },
    venueCompanyRules() {
      return [
        (v) => FieldRulesHelper.optionalMaxCharacters(v, this.venueLabel, 64),
      ];
    },
    streetAddressRules() {
      return [
        (v) => FieldRulesHelper.optionalMaxCharacters(v, this.addressLabel, 64),
      ];
    },
    cityRules() {
      return [
        (v) => FieldRulesHelper.optionalAlphanumeric(v, this.cityLabel),
        (v) => FieldRulesHelper.optionalMaxCharacters(v, this.cityLabel, 64),
      ];
    },
    stateRules() {
      return [
        (v) => FieldRulesHelper.optionalAlphanumeric(v, this.stateLabel),
        (v) => FieldRulesHelper.optionalMaxCharacters(v, this.stateLabel, 64),
      ];
    },
    zipCodeRules() {
      return [
        (v) => FieldRulesHelper.optionalAlphanumeric(v, this.zipcodeLabel),
        (v) => FieldRulesHelper.optionalMaxCharacters(v, this.zipcodeLabel, 64),
      ];
    },
  },
};
</script>