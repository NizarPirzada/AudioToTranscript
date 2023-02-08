<template>
  <v-menu
    v-model="menu"
    :close-on-content-click="false"
    transition="scale-transition"
    offset-y
    min-width="290px">
    <template v-slot:activator="{ on }">
      <v-text-field
        outlined
        dense
        readonly
        :label="label"
        class="cs-date-menu-input-text"
        v-model="myDate"
        v-on="on"
      ></v-text-field>
    </template>
    <v-date-picker
      v-model="myDate"
      @input="menu = false"
      no-title
      color="primary"
      :first-day-of-week="0"
      :locale="currentUserLocale"
    ></v-date-picker>
  </v-menu>
</template>

<script>
export default {
  name: "DateMenuSelector",
  props: {
    label: {
      type: String,
      required: true,
    },
    value: {
      type: String,
      required: true,
    },
    id: {
      type: String,
      required: false,
    },
  },
  data: () => ({
    menu: false,
    myDate: "",
  }),
  computed: {
    currentUserLocale() {
      return this.$currentUserLocale();
    },
  },
  watch: {
    myDate: function (val) {
      this.$emit("update:value", val);
    },
    value: function (val) {
      this.myDate = val;
    },
  },
  created() {
    this.myDate = this.value;
  },
};
</script>
<style lang="scss">
@import "@/scss/components/filters/DateMenuSelector.scss";
</style>
