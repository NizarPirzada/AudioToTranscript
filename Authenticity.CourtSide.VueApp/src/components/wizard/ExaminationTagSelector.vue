<template>
  <v-container class="ma-0 pa-0">
    <div @click="toggleShow" class="cs-examination-tag" :class="customExaminationClass">
      <span class="chunk-label-read-only">{{ tagLabel.label }} </span>
      <img
        class="cs-examination-tag-img"
        :src="tagLabel.icon"
        :alt="tagLabel.text"
      />
    </div>
    <div v-if="showMenu && !disabled" class="menu">
      <div
        class="menu-item"
        v-for="(item, index) in examinationTagOptions"
        :key="index"
        @click="itemClicked(item)"
      >
        {{ item.text }}
      </div>
    </div>
  </v-container>
</template>

<script>
import { ExaminationTags } from "@/./environment.js";
import { IconHelper } from "@/utilities/IconHelper.js";

export default {
  name: "ExaminationTagSelector",
  props: {
    dialog: {
      type: Object,
      required: true,
    },
  },
  data: () => {
    return {
      showMenu: false,
    };
  },
  computed: {
    disabled(){
      return this.dialog.examinationTagDisabled;
    },
    tagLabel() {
      let tagLabel = {};
      switch (this.dialog.examinationTag) {
        case ExaminationTags.Question:
          tagLabel = {
            label: this.$t("step3_tag_menu_question_label"),
            icon: this.disabled ? IconHelper.GetIcon("ExaminationChangeTagDisabled") : IconHelper.GetIcon("ExaminationChangeTag"),
          };
          break;
        case ExaminationTags.Answer:
          tagLabel = {
            label: this.$t("step3_tag_menu_answer_label"),
            icon: IconHelper.GetIcon("ExaminationChangeTag"),
          };
          break;
        case ExaminationTags.NoTag:
          tagLabel = {
            label: "S",
            icon: IconHelper.GetIcon("ExaminationChangeTag"),
          };
          break;
      }

      return tagLabel;
    },
    tagOption() {
      let option = {};
      switch (this.dialog.examinationTag) {
        case ExaminationTags.Question:
          option = {
            tagLabelToChange: this.$t("step3_tag_menu_answer_text"),
            tagValueToChange: ExaminationTags.Answer,
          };
          break;
        case ExaminationTags.Answer:
          option = {
            tagLabelToChange: this.$t("step3_tag_menu_question_text"),
            tagValueToChange: ExaminationTags.Question,
          };
          break;
      }
      return option;
    },
    examinationTagOptions() {
      let options = [];

      switch (this.dialog.examinationTag) {
        case ExaminationTags.Question:
        case ExaminationTags.Answer:
          options = [
            {
              text: this.$t("step3_tag_menu_option_tag_all").replace("<label>", this.tagOption.tagLabelToChange),
              handleItem: this.onUpdateExaminationTagToAllSpeakers,
              value: this.tagOption.tagValueToChange,
            },
            {
              text: this.$t("step3_tag_menu_option_tag_single").replace("<label>", this.tagOption.tagLabelToChange),
              handleItem: this.onUpdateExaminationTagToSingleSpeaker,
              value: this.tagOption.tagValueToChange,
            },
            {
              text: this.$t("step3_tag_menu_option_remove_all"),
              handleItem: this.onUpdateExaminationTagToAllSpeakers,
              value: ExaminationTags.NoTag,
            },
            {
              text: this.$t("step3_tag_menu_option_remove_single"),
              handleItem: this.onUpdateExaminationTagToSingleSpeaker,
              value: ExaminationTags.NoTag,
            },
          ];
          break;
        case ExaminationTags.NoTag:
          options = [
            {
              text: this.$t("step3_tag_menu_option_tag_all").replace("<label>", this.$t("step3_tag_menu_question_text")),
              handleItem: this.onUpdateExaminationTagToAllSpeakers,
              value: ExaminationTags.Question,
            },
            {
              text: this.$t("step3_tag_menu_option_tag_single").replace("<label>", this.$t("step3_tag_menu_question_text")),
              handleItem: this.onUpdateExaminationTagToSingleSpeaker,
              value: ExaminationTags.Question,
            },
            {
              text: this.$t("step3_tag_menu_option_tag_all").replace("<label>", this.$t("step3_tag_menu_answer_text")),
              handleItem: this.onUpdateExaminationTagToAllSpeakers,
              value: ExaminationTags.Answer,
            },
            {
              text: this.$t("step3_tag_menu_option_tag_single").replace("<label>", this.$t("step3_tag_menu_answer_text")),
              handleItem: this.onUpdateExaminationTagToSingleSpeaker,
              value: ExaminationTags.Answer,
            },
          ];
          break;
      }
      return options;
    },
    customExaminationClass() {
      return this.disabled ? 'cs-examination-tag-disabled' : '';
    },
  },
  methods: {
    toggleShow: function() {
      this.showMenu = !this.showMenu;
    },
    clickaway(e) {
      if (!this.$el.contains(e.target)) this.showMenu = false;
    },
    itemClicked: function (item) {
      this.toggleShow();
      item.handleItem(item.value, this.dialog.originalSpeakerId)
    },
    onUpdateExaminationTagToSingleSpeaker(examinationTag) {
      this.$emit("onUpdateExaminationTagToSingleSpeaker", examinationTag);
    },
    onUpdateExaminationTagToAllSpeakers(examinationTag, originalSpeakerId) {
      this.$emit(
        "onUpdateExaminationTagToAllSpeakers",
        examinationTag,
        originalSpeakerId,
      );
    },
  },
  mounted() {
    document.addEventListener("click", this.clickaway);
  },
};
</script>
<style lang="scss">
@import "@/scss/components/wizard/ExaminationTagSelector.scss";
</style>