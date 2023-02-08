<template>
  <v-container class="pa-0 m-a0">
    <v-row justify="center" align="center">
      <v-col cols="12" class="pa-0 ma-0">
        <p class="cs-title-card" align="center">{{ $t("login_header") }}</p>
      </v-col>
      <v-col cols="12" class="pa-0 ma-0">
        <p class="cs-subtitle-card" align="center" id="csLoginSubtitle">
          {{ subtitle }}
        </p>
      </v-col>
    </v-row>
    <v-form ref="form" @submit.prevent="validateForm" lazy-validation>
      <v-card-text class="text--primary pa-0">
        <v-row justify="center" align="center">
          <v-col cols="12" class="pa-0 ma-0" align="center">
            <input
              type="text"
              class="cs-login-input"
              :placeholder="$t('login_email_placeholder')"
              v-model="loginDto.email"
            />
          </v-col>
          <v-col cols="12" class="pa-0 ma-0" align="center">
            <input
              type="password"
              class="cs-login-input"
              :placeholder="$t('login_passw_placeholder')"
              v-model="loginDto.password"
            />
          </v-col>
          <v-col cols="12" class="pl-7 py-0" align="start">
            <p class="cs-forgot-password-text ma-0" @click="forgotPassword">
              {{ $t("login_forgot_passw") }}
            </p>
          </v-col>
        </v-row>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn
          color="primary"
          :loading="loading"
          type="submit"
          class="cs-btn-primary mt-0"
        >
          {{ $t("login_button") }}
        </v-btn>
      </v-card-actions>
    </v-form>
  </v-container>
</template>

<script>
import router from "@/router";
import { mapActions } from "vuex";
import { WebStorage } from "@/store/webStorage";

export default {
  name: "Login",
  data: () => ({
    drawer: true,
    loginDto: { email: null, password: null },
    currentDate: new Date(),
    loading: false,
    formError: null,
    subtitleErrorClass: "cs-subtitle-card-warning",
  }),
  computed: {
    subtitle() {
      if (this.formError) {
        return this.formError;
      } else {
        return this.subtitleText;
      }
    },
    subtitleText() {
      return this.$t("login_header_sub");
    },
    subitleError() {
      return this.$t("login_error_failed");
    },
    locale() {
      return this.$locale;
    }
  },
  methods: {
    ...mapActions("permission", ["getAllObjectsAsync"]),
    ...mapActions("authentication", ["loginAsync"]),
    validateEmailField() {
      const email = this.loginDto.email?.trim();
      return /.+@.+\..+/.test(email);
    },
    validatePasswordField() {
      const password = this.loginDto.password?.trim();
      return /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$/.test(
        password
      );
    },
    async validateForm() {
      try {
        this.loading = true;

        if (this.validateEmailField() && this.validatePasswordField()) {
          document
            .getElementById("csLoginSubtitle")
            .classList.remove(this.subtitleErrorClass);

          this.formError = null;
          let response = await this.loginAsync(this.loginDto);

          if (response.success) {
            WebStorage.setDashboardFilters({});
            this.getAllObjectsAsync();
            router.push({ name: "Home" });
            this.$router.go();
          } else {
            throw this.subitleError;
          }
        } else {
          throw this.subitleError;
        }
      } catch (error) {
        this.showErrorMessage(error);
      } finally {
        this.loading = false;
      }
    },
    showErrorMessage(error) {
      this.formError = error;
      document
        .getElementById("csLoginSubtitle")
        .classList.add(this.subtitleErrorClass);
    },
    forgotPassword() {
      router.push({ name: "ResetPassword" });
    },
  },
  created() {
    this.formError = null;
  },
};
</script>
