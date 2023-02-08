<template>
  <v-menu
    v-model="menu"
    :close-on-content-click="false"
    transition="scale-transition"
    offset-y
    min-width="290px"
  >
    <template v-slot:activator="{ on }">
      <v-text-field
        single-line
        outlined
        dense
        readonly
        label=""
        class="cs-filter-input-text"
        v-model="dateLabel"
        v-on="on"
      ></v-text-field>
    </template>
    <v-container pa-0>
      <v-row justify-end>
        <v-btn color="primary" class="mx-5 my-1" @click="toggleAdvanced">
          {{ advancedLabel }}</v-btn
        >
        <v-btn text class="mx-3 my-1" @click="clearFilter">
          {{ $t("dashboard_filter_date_clear") }}
        </v-btn>
      </v-row>
      <v-row v-if="!advanced">
        <v-date-picker
          v-model="startDate"
          @input="menu = false"
          no-title
          color="primary"
          :first-day-of-week="0"
          :locale="currentUserLocale"
        ></v-date-picker>
      </v-row>
      <v-row v-if="advanced" class="ml-3 my-1 pt-2">
        <DateMenuSelector 
          :id="'start-date'"
          :value.sync="startDate"
          :label="$t('dashboard_filter_date_start')"
        ></DateMenuSelector>
      </v-row>
      <v-row v-if="advanced" class="ml-3 my-1">
        <DateMenuSelector
          :id="'end-date'"
          :value.sync="endDate"
          :label="$t('dashboard_filter_date_end')"
        ></DateMenuSelector>
      </v-row>
    </v-container>
  </v-menu>
</template>

<script>
import DateMenuSelector from "@/components/filters/DateMenuSelector";

export default {
  name: "TableDateFilter",
  components: {
    DateMenuSelector,
  },
  props: {
    id: {
      type: String,
      required: false,
    },
    value: {
      type: Object,
      required: false,
    },
  },
  data: () => ({
    menu: false,
    startDate: "",
    endDate: "",
    advanced: false,
  }),
  computed: {
    currentUserLocale() {
      return this.$currentUserLocale();
    },
    advancedLabel() {
      if (this.advanced) {
        return this.$t("dashboard_filter_date_hide_advanced");
      } else {
        return this.$t("dashboard_filter_date_advanced");
      }
    },
    dateLabel() {
      let dateText = "";
      if (this.startDate) {
        dateText = this.startDate;
      }
      if (this.advanced) {
        if (this.endDate) {
          dateText = dateText + " - " + this.endDate;
        }
      }
      return dateText;
    },
  },
  methods: {
    clearFilter() {
      this.startDate = "";
      this.endDate = "";
    },
    toggleAdvanced() {
      this.advanced = !this.advanced;
    },
  },
  watch: {
    startDate: function (val) {
      if (!this.advanced) {
        this.$emit("applyFilter", { start: val });
      } else {
        this.$emit("applyFilter", { start: val, end: this.endDate });
      }
    },
    endDate: function (val) {
      if (this.advanced) {
        this.$emit("applyFilter", { start: this.startDate, end: val });
      }
    },
    advanced: function (val) {
      if (!val) {
        this.$emit("applyFilter", { start: this.startDate });
      } else {
        this.$emit("applyFilter", { start: this.startDate, end: this.endDate });
      }
    },
    value: function (val) {
      if (val.start) {
        this.startDate = val.start;
      }
      if (val.end) {
        this.advanced = true;
        this.endDate = val.end;
      }
    },
  },
  async created() {
    if (this.value) {
      if (this.value.start) {
        this.startDate = this.value.start;
      }
      if (this.value.end) {
        this.advanced = true;
        this.endDate = this.value.end;
      }
    }
  },
};
</script>