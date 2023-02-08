import Vue from "vue";
import VueRouter from "vue-router";
import { AuthenticationHelper } from "@/utilities/Helpers.js";
Vue.use(VueRouter);

const routes = [
  {
    path: "/",
    redirect: "/Login",
  },
  {
    path: "*",
    name: "NotFound",
    redirect: "/",
    component: () => import("@/components/pages/error/NotFoundPage"),
    props: true,
  },
  {
    path: "/Login",
    name: "HomeLogin",
    component: () => import("@/views/Login/Home.vue"),
    beforeEnter: AuthenticationHelper.CheckLoginAccessPage,
    children: [
      {
        path: "/",
        name: "Login",
        component: () => import("@/views/Login/Login.vue"),
      },
      {
        path: "resetPassword",
        name: "ResetPassword",
        component: () => import("@/views/Login/ResetPasswordPage.vue"),
      },
      {
        path: "passwordRecovery",
        name: "PasswordRecovery",
        component: () => import("@/views/Login/PasswordRecoveryPage.vue"),
      },
    ],
  },
  {
    path: "/ResetPassword/:activationId",
    name: "ResetPassword",
    component: () => import("@/components/pages/login/ResetPasswordPage"),
    beforeEnter: AuthenticationHelper.CheckLoginAccesPage,
  },
  {
    path: "/Home",
    name: "Home",
    component: () => import("@/views/Home.vue"),
    beforeEnter: AuthenticationHelper.CheckHomePage,
    children: [
      {
        path: "/Users",
        name: "Users",
        component: () => import("@/views/UsersPage"),
        beforeEnter: AuthenticationHelper.CheckAccessPermission,
        meta: {
          permission: "ReadUsers",
        },
      },
      {
        path: "/Dashboard",
        name: "Dashboard",
        component: () => import("@/views/Dashboard"),
        beforeEnter: AuthenticationHelper.CheckAccessPermission,
        meta: {
          permission: "ReadDashboard",
        },
      },
      {
        path: "/wizard/:transcriptId",
        name: "Wizard",
        component: () => import("@/views/TranscriptWizard"),
        beforeEnter: AuthenticationHelper.CheckTranscriptAccessPermission,
        meta: {
          permission: "CreateDashboard",
        },
      },
      {
        path: "/settings",
        name: "Settings",
        component: () => import("@/views/SettingsPage"),
        beforeEnter: AuthenticationHelper.CheckAccessPermission,
        meta: {
          permission: "ReadSettings",
        },
      },
    ],
  },
];

const mode = process.env.VUE_APP_ROUTER_MODE || "hash";

const router = new VueRouter({
  mode,
  routes,
});

export default router;
