<template>
  <v-navigation-drawer
    app
    v-model="drawer"
    absolute
    permanent
    color="accent"
    width="327"
    class="cs-navdrawer"
  >
    <div class="d-flex justify-center mt-5">
      <Logo @click.native="goToHome"></Logo>
    </div>
    <div class="ml-5 mt-12">
      <div class="cs-new-btn" @click="launchNewDialog">
        <v-img :src="newIcon" :class="newIconClass"></v-img>
        <span class="cs-new-btn-text">{{ $t("dashboard_new_button") }}</span>
      </div>
    </div>

    <v-list nav dense class="cs-navdrawer-menu">
      <v-list-item-group
        v-model="itemSelected"
        mandatory
        active-class="cs-navdrawer-menu-item-active"
      >
        <v-list-item
          v-for="(item, i) in menuItems"
          :key="i"
          class="cs-navdrawer-menu-item"
          @click="GoToRoute(item, i)"
        >
          <v-list-item-icon class="cs-">
            <img
              :src="item.icon"
              :alt="item.text"
              class="cs-navdrawer-menu-item-icon"
            />
          </v-list-item-icon>
          <v-list-item-content>
            <v-list-item-title
              v-text="item.text"
              class="cs-navdrawer-menu-item-text"
            ></v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list-item-group>
    </v-list>
  </v-navigation-drawer>
</template>

<script>
import Logo from "@/components/layout/Logo";
import router from "@/router";
import { WebStorage } from "@/store/webStorage";

export default {
  name: "NavigationDrawer",
  props: {
    menuItems: {
      type: Array,
      required: true,
    },
    newIcon: {
      type: String,
    },
  },
  data: () => ({
    drawer: true,
    itemSelected: 0,
  }),
  computed: {
    newIconClass() {
      if (this.$route.name === "Users") {
        return "cs-new-user-icon";
      }
      return "cs-new-transcript-icon";
    },
  },
  methods: {
    GoToRoute(item, index) {
      if (item.router === "SignOut") {
        WebStorage.removeToken();
        router.push({ name: "Login" });
      } else {
        this.itemSelected = index;
        this.pushRoute(item.router);
      }
    },
    launchNewDialog() {
      this.$emit("new");
    },
    goToHome() {
      const decodedToken = WebStorage.getDecodedToken();
      let route;
      switch (decodedToken.role) {
        case "Administrator":
          route = "Users";
          break;
        case "Standard":
          route = "Dashboard";
          break;
        default:
          break;
      }
      this.pushRoute(route);
    },
    pushRoute(route) {
      router.push({ name: route }).catch((error) => {
        if (error.name != "NavigationDuplicated") {
          throw error;
        }
      });
    }
  },
  components: {
    Logo,
  },
};
</script>

<style lang="scss">
@import "@/scss/components/layout/NavigationDrawer.scss";
</style>
