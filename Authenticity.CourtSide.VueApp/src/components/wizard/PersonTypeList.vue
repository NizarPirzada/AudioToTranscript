<template>
  <v-container class="pa-0">
    <div v-for="(item, index) in persons" :key="index">
      <PersonSimpleForm
        v-if="persons"
        :title="getCountLabel(index)"
        :person="item"
        :active="active"
        :transcriptType="transcriptType"
      ></PersonSimpleForm>

      <v-btn
        v-if="newEnabled && persons && index === persons.length - 1"
        class="add-person-button"
        @click="newPerson"
        :disabled="active"
      >
        <span class="plus-icon">+</span> {{ $t("step1_person_add_new") }}
      </v-btn>
    </div>
  </v-container>
</template>

<script>
import { ObjectHelper } from "@/utilities/Helpers.js";
import PersonSimpleForm from "@/components/wizard/PersonSimpleForm";

export default {
  name: "PersonTypeList",
  props: {
    title: {
      type: String,
      required: true,
    },
    type: {
      type: Number,
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
    newEnabled: {
      type: Boolean,
    },
    transcriptType: {
      type: Number,
      required: false,
    },
  },
  components: {
    PersonSimpleForm,
  },
  methods: {
    getCountLabel(index) {
      let label = this.title;
      if (index > 0) {
        label = this.title.concat(` #${index + 1}`);
      }
      return label;
    },
    newPerson() {
      this.persons.push(ObjectHelper.GetEmptyPersonObject(this.type, 0));
    },
  },
};
</script>