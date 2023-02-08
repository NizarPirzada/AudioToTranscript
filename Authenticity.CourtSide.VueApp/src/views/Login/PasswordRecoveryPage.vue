<template>
  <v-container class="pa-0 ma-0">
    <v-row justify="center" align="center">
      <v-col cols="12" class="pa-0 ma-0">
        <p class="cs-title-card" align="center">{{ titleText }}</p>
      </v-col>
      <v-col cols="9" class="pa-0 ma-0">
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
    <v-form ref="form" @submit.prevent="validateForm" lazy-validation>
      <v-card-text class="text--primary pa-0" v-if="!passwordHasBenChanged">
        <v-row justify="center" align="center">
          <v-col cols="12" class="pa-0 ma-0" align="center">
            <input
              type="text"
              class="cs-login-input"
              :disabled="loading"
              :class="emailInputClass"
              :placeholder="$t('recover_password_change_form_email_label')"
              v-model="changePasswordDto.email"
            />
          </v-col>
          <v-col cols="12" class="pa-0 ma-0" align="center">
            <input
              type="password"
              class="cs-login-input"
              :disabled="loading"
              :class="temporaryPasswordInputClass"
              :placeholder="$t('recover_password_change_form_temporary_password_label')"
              v-model="changePasswordDto.temporalPassword"
            />
          </v-col>
          <v-col cols="12" class="pa-0 ma-0" align="center">
            <input
              type="password"
              class="cs-login-input"
              :disabled="loading"
              :class="passwordInputClass"
              :placeholder="$t('recover_password_change_form_new_password_label')"
              v-model="changePasswordDto.password"
            />
          </v-col>
          <v-col cols="12" class="pa-0 ma-0" align="center">
            <input
              type="password"
              class="cs-login-input"
              :disabled="loading"
              :class="passwordInputClass"
              :placeholder="$t('recover_password_change_form_enter_new_password_label')"
              v-model="changePasswordDto.confirmPassword"
            />
          </v-col>
        </v-row>
      </v-card-text>
      <v-card-actions v-if="!passwordHasBenChanged">
        <v-spacer></v-spacer>
        <v-btn
          color="primary"
          :loading="loading"
          type="submit"
          class="cs-btn-primary mt-0"
        >
          {{ $t("app_submit_button")}}
        </v-btn>
      </v-card-actions>
      <v-card-actions v-else>
        <v-spacer></v-spacer>
        <v-btn
          color="primary"
          :loading="loading"
          :disabled="loading"
          @click="goToLoginPage"
          class="cs-btn-primary mt-0"
        >
          {{ $t("recover_password_login_button")}}
        </v-btn>
        <v-spacer></v-spacer>
      </v-card-actions>
    </v-form>
  </v-container>
</template>

<script>
import router from "@/router";
import { mapActions } from "vuex";
import { FieldRulesHelper } from "@/utilities/Helpers";

const ERROR_CLASS = "cs-subtitle-card-warning";
const INPUT_ERROR_CLASS = "cs-login-input-error";

export default {
  name: "PasswordRecoveryPage",
  computed: {
    form() {
      return this.$refs.form;
    },
    emailTemporaryRuleMessage() {
      return this.$t("recover_password_email_temaporary_password_message");
    },
    passwordMatchRuleMessage() {
      return this.$t("recover_password_password_match_rule_message");
    },
    strongPasswordRuleMessage() {
      return this.$t("recover_password_strong_password_rule_message");
    },
    titleText() {
      return this.isSuccessTittle
        ? this.$t("recover_password_change_form_success_title")
        : this.$t("recover_password_change_form_title");
    },
    subtitleText() {
      if (this.errorRaised) {
        return this.errorMessage;
      } else if (this.isSuccessTittle) {
        return this.$t("recover_password_change_form_success_subtitle");
      } else if (this.loading) {
        return this.$t("recover_password_change_form_subtitle");
      } else {
        return this.$t("recover_password_change_form_subtitle");
      }
    },
  },
  data: function () {
    return {
      changePasswordDto: {},
      loading: false,
      subtitleClass: null,
      passwordHasBenChanged: false,
      emailInputClass: null,
      temporaryPasswordInputClass: null,
      passwordInputClass: null,
      isSuccessTittle: false,
      errorRaised: false,
      errorMessage: "",
    };
  },
  methods: {
    ...mapActions("user", ["passwordRecoveryAsync"]),
    goToLoginPage() {
      this.form.reset();
      router.push({ name: "Login" });
    },
    validateEmailAndTemporalPasswordRules() {
      const emailValue = this.changePasswordDto.email;
      const temporalPassword = this.changePasswordDto.temporalPassword;
      const tempPassRule = FieldRulesHelper.requiredField(
        temporalPassword,
        "Temporary Password"
      );

      const emailRule = FieldRulesHelper.requiredValidEmail(
        emailValue,
        "Email"
      );

      if (tempPassRule !== true || emailRule !== true) {
        this.emailInputClass = this.temporaryPasswordInputClass = INPUT_ERROR_CLASS;
        throw this.emailTemporaryRuleMessage;
      }
    },
    validatePasswordRules() {
      const newPasswordValue = this.changePasswordDto.password;
      const newPasswordRegex = FieldRulesHelper.requiredValidPassword(
        newPasswordValue,
        "New Password"
      );
      if (newPasswordRegex !== true) {
        this.passwordInputClass = INPUT_ERROR_CLASS;
        throw this.strongPasswordRuleMessage;
      }

      const confirmPasswordValue = this.changePasswordDto.confirmPassword;

      if (newPasswordValue !== confirmPasswordValue) {
        this.passwordInputClass = INPUT_ERROR_CLASS;
        throw this.passwordMatchRuleMessage;
      }
    },
    async validateForm() {
      this.errorRaised = false;
      try {
        this.loading = true;
        this.resetRules();
        this.validateEmailAndTemporalPasswordRules();
        this.validatePasswordRules();

        await this.passwordRecoveryAsync(this.changePasswordDto).then(
          (response) => {
            if (response.success) {
              this.showSuccessMessage();
            } else if (
              response.message === "Incorrect Email or Temporary Password"
            ) {
              this.emailInputClass = this.temporaryPasswordInputClass = INPUT_ERROR_CLASS;
              throw this.emailTemporaryRuleMessage;
            }
          }
        );
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
      this.isSuccessTittle = true;
      this.subtitleClass = null;
      this.passwordHasBenChanged = true;
    },
    resetRules() {
      this.subtitleClass = this.emailInputClass = this.temporaryPasswordInputClass = this.passwordInputClass = null;
    },
  },
};
</script>