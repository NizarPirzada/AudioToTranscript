<template>
  <v-app>
    <NavigationDrawer
      :menuItems="menuItemsComputed"
      @new="onNewObject"
      :newIcon="newIcon"
    ></NavigationDrawer>
    <AppBar :user="user" ref="appBar"></AppBar>
    <v-main>
      <v-container fluid class="pb-0">
        <router-view></router-view>
      </v-container>
    </v-main>
    <ConfirmMessage ref="confirmMessage"></ConfirmMessage>
    <SnackbarMessage ref="snackbarMessage"></SnackbarMessage>
    <AlertDialog ref="alertMessage"></AlertDialog>
  </v-app>
</template>

<script>
import AppBar from "@/components/layout/AppBar";
import NavigationDrawer from "@/components/layout/NavigationDrawer";
import { IconHelper } from "@/utilities/IconHelper.js";
import { mapActions, mapState } from "vuex";
import { VuexKeys } from "@/./environment.js";
import ConfirmMessage from "@/components/dialog/ConfirmMessage";
import SnackbarMessage from "@/components/layout/SnackbarMessage";
import { WebStorage } from "@/store/webStorage";
import AlertDialog from "@/components/dialog/AlertDialog";

export default {
  name: "home",
  data: () => ({}),
  components: {
    AppBar,
    NavigationDrawer,
    ConfirmMessage,
    SnackbarMessage,
    AlertDialog,
  },
  computed: {
    ...mapState("permission", ["authorizedObjects"]),

    user() {
      const decodedToken = WebStorage.getDecodedToken();
      let userInformation = {
        fullName:
          decodedToken.firstName +
          " " +
          (decodedToken.lastName ? decodedToken.lastName[0] : ""),
      };

      return userInformation;
    },
    menuItemsComputed() {
      let items = [];
      this.authorizedObjects.forEach((element) => {
        items.push({
          icon: IconHelper.GetMenuIcon(element.name),
          text: this.$t(`menu_${element.name.toLowerCase()}`),
          router: element.name,
        });
      });
      items.push({
        icon: IconHelper.GetMenuIcon("Logout"),
        text: this.$t("menu_logout"),
        router: "SignOut",
      });
      return items;
    },
    confirm() {
      return this.$refs.confirmMessage;
    },
    snackbar() {
      return this.$refs.snackbarMessage;
    },
    newIcon() {
      if (this.$route.name === "Users") {
        return IconHelper.GetMenuIcon("NewUser");
      }
      return IconHelper.GetMenuIcon("NewDashboard");
    },
    alert() {
      return this.$refs.alertMessage;
    },
    appBar() {
      return this.$refs.appBar;
    },
  },
  methods: {
    ...mapActions("permission", ["getAllObjectsAsync"]),

    async loadMenuObjectsAsync() {
      await this.getAllObjectsAsync();
    },
    onNewObject() {
      if (this.$route.name === "Dashboard") {
        this.$eventBus.$emit(VuexKeys.Transcript.New);
      } else if (this.$route.name === "Users") {
        this.$eventBus.$emit(VuexKeys.User.New);
      }
    },
    ShowConfirmMesage(
      title,
      message,
      callBack,
      okCancelClick,
      okMessage,
      cancelMessage
    ) {
      this.confirm.show(
        title,
        message,
        callBack,
        okCancelClick,
        okMessage,
        cancelMessage
      );
    },
    ShowSnackbarMessage(message, type, timeout = 3000, bottom = true) {
      this.snackbar.show(message, type, timeout, bottom);
    },
    ShowAlertMessage(message, okCloseClick) {
      this.alert.show(message, okCloseClick);
    },
    setAppBarInfo(transcript) {
      this.appBar.transcript = transcript;
    },
  },
  async created() {
    this.$eventBus.$on(
      VuexKeys.Home.ShowConfirmMessage,
      this.ShowConfirmMesage
    );
    this.$eventBus.$on(
      VuexKeys.Home.ShowSnackbarMessage,
      this.ShowSnackbarMessage
    );
    this.$eventBus.$on(VuexKeys.Home.ShowAlertMessage, this.ShowAlertMessage);
    this.$eventBus.$on(VuexKeys.Home.SetAppBarInfo, this.setAppBarInfo);

    if (this.authorizedObjects == null || this.authorizedObjects.length === 0) {
      await this.loadMenuObjectsAsync();
    }
  },
};
</script>
