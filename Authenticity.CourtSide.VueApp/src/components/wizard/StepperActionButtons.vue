<template class="pa-0 ma-0">
  <div class="cs-stepper-action-btn" :class="customClass">
    <v-btn
      fab
      small
      v-for="item in buttonList"
      :key="item.action"
      :disabled="item.disabled"
      :color="item.color"
      @click="emitAction(item.action)"
      :loading="item.loading"
    >
      <v-icon color="secondary">{{ item.icon }}</v-icon>
    </v-btn>
  </div>
</template>

<script>
export default {
  name: "StepperActionButtons",
  props: {
    buttonList: {
      type: Array,
      required: true,
    },
    customClass: {
      type: String,
    },
  },
  methods: {
    emitAction(action) {
      this.$emit(action);
    },
  },
};
</script>