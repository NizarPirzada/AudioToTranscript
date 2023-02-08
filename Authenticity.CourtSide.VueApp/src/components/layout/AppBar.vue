<template>
  <v-app-bar
    app
    elevation="0"
    color="secondary"
    style="padding-top: 7px"
    height="70"
  >
    <div v-if="showTranscriptHeader" class="mt-2">
      <h1 v-text="transcript.name" class="transcript-header-name"></h1>
      <h3 v-text="subHeaderText" class="transcript-header-date"></h3>
    </div>
    <v-spacer></v-spacer>
    <SwitchLanguage />
    <v-icon v-if="showNotificationIcon" color="primary">mdi-bell</v-icon>
    <div class="cs-app-bar-menu">
      <v-menu offset-y>
        <template v-slot:activator="{ on }">
          <v-btn text style="text-transform: none" v-on="on">
            <v-img :src="userIcon" class="cs-user-icon"></v-img>
            <span
              v-text="user.fullName"
              class="text-sm-body-2 cs-app-bar-user-name"
            ></span>
            <v-img :src="chevronDownIcon" class="cs-chevron-down-icon"></v-img>
          </v-btn>
        </template>
        <v-list>
          <v-list-item-group active-class="item-active">
            <v-list-item @click="changePassword()">
              <v-list-item-title>{{
                $t("menu_change_password")
              }}</v-list-item-title>
            </v-list-item>
            <v-list-item @click="signOut()">
              <v-list-item-title>{{ $t("menu_logout") }}</v-list-item-title>
            </v-list-item>
          </v-list-item-group>
        </v-list>
      </v-menu>
    </div>
    <ChangePasswordDialog
      v-if="showChangePasswordDialog"
      v-model="showChangePasswordDialog"
      @onDialogClosed="onChangePasswordDialogClosed"
    ></ChangePasswordDialog>
  </v-app-bar>
</template>

<script>
import router from "@/router";
import { WebStorage } from "@/store/webStorage";
import { IconHelper } from "@/utilities/IconHelper.js";
import SwitchLanguage from "@/components/language/SwitchLanguage";
import ChangePasswordDialog from "@/components/dialog/ChangePasswordDialog";
import { VuexKeys } from "@/./environment.js";
import { SnackbarType } from "@/utilities/Enumerations";

export default {
  name: "AppBar",
  components: {
    SwitchLanguage,
    ChangePasswordDialog,
  },
  props: {
    user: {
      type: Object,
      required: true,
    },
  },
  data: () => ({
    userOptions: [{ title: "Logout", route: "/logout" }, { title: "Two" }],
    showChangePasswordDialog: false,
    transcript: {},
  }),
  computed: {
    showTranscriptHeader() {
      return this.$route.name === "Wizard";
    },
    subHeaderText() {
      return `${this.$t(
        "step_created_label"
      )} - ${this.$formatDateDescriptive(this.transcript.createdOn)}`;
    },
    chevronDownIcon() {
      return IconHelper.GetMenuIcon("ChevronDown");
    },
    userIcon() {
      return IconHelper.GetMenuIcon("User");
    },
    showNotificationIcon() {
      if (this.$route.name === "Users") {
        return false;
      }

      return true;
    },
    passwordChangedSuccessMessage() {
      return this.$t("change_password_change_success_message");
    },
  },
  methods: {
    signOut() {
      WebStorage.removeToken();
      router.push({ name: "Login" });
    },
    changePassword() {
      this.showChangePasswordDialog = true;
    },
    onChangePasswordDialogClosed(isCanceled) {
      this.showChangePasswordDialog = false;
      if (!isCanceled) {
        this.showSnackBar(
          this.passwordChangedSuccessMessage,
          SnackbarType.Success
        );
      }
    },
    showSnackBar(message, type) {
      this.$eventBus.$emit(VuexKeys.Home.ShowSnackbarMessage, message, type);
    },
  },
};
</script>
<style lang="scss">
@import "@/scss/components/layout/AppBar.scss";
</style>