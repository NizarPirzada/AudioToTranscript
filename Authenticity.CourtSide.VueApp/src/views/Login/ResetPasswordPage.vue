<template>
  <v-container class="pa-0 ma-0">
    <v-row justify="center" align="center">
      <v-col cols="12" class="pa-0 ma-0">
        <p class="cs-title-card" align="center">{{ titleText }}</p>
      </v-col>
      <v-col cols="12" class="pa-0 ma-0">
        <p
          class="cs-subtitle-card"
          align="center"
          id="csLoginSubtitle"
          :class="subtitleClass"
        >
          {{ subtitleText }}
        </p>
      </v-col>
    </v-row>
    <v-form
      ref="form"
      @submit.prevent="validateForm"
      lazy-validation
      v-if="!passwordHasBenReset"
    >
      <v-card-text class="text--primary pa-0">
        <v-row justify="center" align="center">
          <v-col cols="12" class="pa-0 ma-0" align="center">
            <input
              type="text"
              class="cs-login-input"
              :disabled="loading"
              placeholder="Email"
              v-model="email"
            />
          </v-col>
        </v-row>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn
          text
          color="primary"
          :loading="loading"
          :disabled="loading"
          class="cs-btn mt-0"
          @click="goBack()"
        >
          {{ $t("app_cancel_button") }}
        </v-btn>
        <v-btn
          color="primary"
          :loading="loading"
          :disabled="loading"
          type="submit"
          class="cs-btn-primary mt-0"
        >
          {{ $t("recover_password_reset_button") }}
        </v-btn>
      </v-card-actions>
    </v-form>
  </v-container>
</template>

<script>
import router from "@/router";
import { mapActions } from "vuex";
import { FieldRulesHelper } from "@/utilities/Helpers";

const SUCCESS_CLASS = "cs-subtitle-card-success";
const ERROR_CLASS = "cs-subtitle-card-warning";

export default {
  name: "ResetPasswordPage",
  computed: {
    form() {
      return this.$refs.form;
    },
    emailFieldIsValid() {
      return FieldRulesHelper.requiredValidEmail(this.email,"Email");
    },
    titleText() {
      return this.successTittle
        ? this.$t("recover_password_success_message")
        : this.$t("recover_password_title");
    },
    subtitleText() {
      if (this.errorRaised) {
        return this.errorMessage;
      } else if (this.successTittle) {
        return this.$t("recover_password_success_subtitle");
      } else if (this.loading) {
        return this.$t("recover_password_subtitle");
      } else {
        return this.$t("recover_password_subtitle");
      }
    },
  },
  data: function () {
    return {
      email: "",
      loading: false,
      subtitleClass: null,
      passwordHasBenReset: false,
      successTittle: false,
      errorRaised: false,
      errorMessage: "",
    };
  },
  methods: {
    ...mapActions("user", ["resetPasswordAsync"]),
    goBack() {
      this.form.reset();
      router.push({ name: "Login" });
    },
    async validateForm() {
      this.errorRaised = false;
      this.errorMessage = "";
      try {
        this.loading = true;
        if (this.emailFieldIsValid === true) {
          this.subtitleClass = null;

          await this.resetPasswordAsync(this.email).then((response) => {
            if (response.success) {
              this.showSuccessMessage();
            } else if (response.message === "Incorrect Email") {
              throw this.$t("recover_password_invalid_email_message");
            }
          });
        } else {
          throw this.$t("recover_password_invalid_email_message");
        }
      } catch (error) {
        this.showErrorMessage(error);
      } finally {
        this.loading = false;
      }
    },
    showErrorMessage(error) {
      this.errorRaised = true;
      this.errorMessage = error;
      this.subtitleClass = ERROR_CLASS;
    },
    showSuccessMessage() {
      this.successTittle = true;
      this.subtitleClass = SUCCESS_CLASS;
      this.passwordHasBenReset = true;
    },
  },
};
</script>