<template>
  <v-container class="pa-0">
    <v-card class="cs-step-card">
      <v-card-text class="pb-0">
        <span class="step-section-header">{{ title }}</span>
        <div v-for="(person, index) in persons" :key="index">
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
              :label="lastNameLabel"
              outlined
              :rules="lastNameRules"
              v-model="person.lastName"
              :disabled="active"
            ></v-text-field>
            <v-btn
              v-show="showNewButton(index)"
              class="add-person-button add-additional-speaker-button"
              @click="newPerson"
              :disabled="active"
            >
              <span class="plus-icon">+</span> {{ $t("step1_person_add_new") }}
            </v-btn>
          </v-layout>
        </div>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script>
import { ObjectHelper } from "@/utilities/Helpers.js";
import { FieldRulesHelper } from "@/./utilities/Helpers.js";

export default {
  name: "AdditionalSpeakerList",
  props: {
    title: {
      type: String,
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
    type: {
      type: Number,
      required: true,
    },
    newEnabled: {
      type: Boolean,
    },
  },
  computed: {
    firstNameLabel() {
      return this.$t("step1_person_first_name_label");
    },
    lastNameLabel() {
      return this.$t("step1_person_last_name_label");
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
  },
  methods: {
    showNewButton(index) {
      return (
        this.newEnabled && this.persons && index === this.persons.length - 1
      );
    },
    newPerson() {
      this.persons.push(ObjectHelper.GetEmptyPersonObject(this.type, 0));
    },
  },
};
</script>