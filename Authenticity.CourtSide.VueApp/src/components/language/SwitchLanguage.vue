<template>
  <v-menu bottom>
    <template v-slot:activator="{ on }">
      <v-btn icon v-on="on">
        <flag
          :iso="currentTranslation.flagIso"
          :title="currentTranslation.displayName"
          :squared="false"
        ></flag>
      </v-btn>
    </template>

    <v-list>
      <v-list-item
        v-for="translation in translations"
        :key="translation.id"
        @click="change(translation.id)"
      >
        <v-list-item-icon class="mr-2">
          <flag
            :iso="translation.flagIso"
            :title="translation.displayName"
            :squared="false"
          ></flag>
        </v-list-item-icon>
        <v-list-item-content>
          <v-list-item-title>{{ translation.displayName }}</v-list-item-title>
        </v-list-item-content>
      </v-list-item>
    </v-list>
  </v-menu>
</template>

<script>
import { mapState, mapActions } from "vuex";

export default {
  name: "SwitchLanguage",
  computed: {
    ...mapState("language", ["translations", "currentTranslation"]),
  },
  methods: {
    ...mapActions("language", ["changeLanguage"]),
    change(locale) {
      this.$i18n.locale = locale;
      this.changeLanguage(locale);
    },
  },
};
</script>
