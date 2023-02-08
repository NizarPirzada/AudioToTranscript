<template>
  <v-container class="pa-0 ma-0">
    <v-card
      class="transcription-chunk my-4"
      v-for="(item, index) in loaderBlocks"
      :key="index"
    >
      <v-card-title class="pa-0 ma-0">
        <v-row class="pa-0 ma-0" no-gutters>
          <v-col cols="6" class="pa-0 ma-0">
            <v-skeleton-loader
              v-bind="attrs"
              type="table-heading"
            ></v-skeleton-loader>
          </v-col>
          <v-col cols="6">
            <v-skeleton-loader
              v-bind="attrs"
              type="table-heading"
            ></v-skeleton-loader>
          </v-col>
        </v-row>
      </v-card-title>
      <v-divider></v-divider>
      <v-card-text>
        <v-skeleton-loader
          v-bind="attrs"
          type="list-item-avatar-three-line"
        ></v-skeleton-loader>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script>
export default {
  name: "TranscriptCardSkeleton",
  data: () => ({
    loaderBlocks: 8,
    attrs: {
      boilerplate: false,
      elevation: 0,
    },
  }),
};
</script>