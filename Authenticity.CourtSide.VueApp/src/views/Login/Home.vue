<template>
  <v-app>
    <v-navigation-drawer
      app
      v-model="drawer"
      absolute
      permanent
      color="accent"
      width="36.30%"
    >
      <v-container fluid fill-height>
        <v-row style="height: 5%">
          <div class="logo">
            <Logo></Logo>
          </div>
        </v-row>
        <v-row style="height: 90%">
          <v-container fluid fill-height class="pa-0 ma-0">
            <v-layout align-center justify-center>
              <v-flex>
                <p class="cs-welcome-title-text">{{ $t("login_welcome") }}</p>
                <p class="cs-welcome-subtitle-text">
                  {{ $t("login_slogan") }}
                </p>
              </v-flex>
            </v-layout>
          </v-container>
        </v-row>
        <v-row style="height: 5%" class="d-flex align-end">
          <p class="cs-footer-text ml-0">
            &copy;{{ currentDate.getFullYear() }} Authenticity.ai
          </p>
          <v-spacer></v-spacer>
          <p class="cs-footer-text">{{ $t("login_contact") }}</p>
        </v-row>
      </v-container>
    </v-navigation-drawer>
    <v-main>
      <v-layout align-center justify-end class="cs-external-language">
        <SwitchLanguage style="right:10px !important"/>
      </v-layout>
      <v-container fluid fill-height class="pa-0 ma-0">
        <v-layout align-center justify-center>
          <v-flex>
            <v-row justify="center" align="center">
              <v-card tile elevation="0" class="cs-login-card" max-width="500">
                <router-view></router-view>
              </v-card>
            </v-row>
          </v-flex>
        </v-layout>
      </v-container>
    </v-main>
  </v-app>
</template>

<script>
import Logo from "@/components/layout/Logo";
import SwitchLanguage from "@/components/language/SwitchLanguage";

export default {
  name: "Home",
  components: {
    Logo,
    SwitchLanguage,
  },
  data: () => ({
    drawer: true,
    currentDate: new Date(),
  }),
  created() {
    const locale = navigator.language || navigator.userLanguage;
    this.$locale = locale.split("-")[0];
  }
};
</script>

<style lang="scss">
@import "@/scss/views/login/Home.scss";
</style>
