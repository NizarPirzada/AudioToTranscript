<template>
  <div>
    <v-dialog
      content-class="cs-change-password-dialog"
      v-model="show"
      width="572"
      height="555"
      persistent
      @keydown.esc.exact="closeDialog(true)"
      @keydown.enter.exact="saveForm()"
    >
      <v-card>
        <v-card-title class="pa-0">
          <div class="mt-10 ml-8">
            <p class="cs-change-password-dialog-title-text">
              {{ $t("change_password_title") }}
            </p>
            <p
              v-show="isCurrentPasswordInvalid"
              class="cs-change-password-dialog-subtitle-text-error"
            >
              {{ invalidCurrentPasswordMessage }}
            </p>
          </div>
        </v-card-title>
        <v-form ref="form" v-model="validForm" lazy-validation>
          <v-card-text class="pa-0">
            <v-row class="px-1 mx-1 pt-3">
              <v-col cols="10" class="pa-0 ma-0">
                <v-text-field
                  class="cs-change-password-dialog-input"
                  v-model="changePasswordDto.currentPassword"
                  :rules="currentPasswordRules"
                  :label="currentPasswordLabel"
                  type="password"
                  required
                  single-line
                  outlined
                  dense
                ></v-text-field>
              </v-col>
              <v-col cols="10" class="pa-0 ma-0">
                <v-text-field
                  class="cs-change-password-dialog-input"
                  v-model="changePasswordDto.newPassword"
                  :rules="newPasswordRules"
                  :label="newPasswordLabel"
                  type="password"
                  @keyup="validatePasswordMatch"
                  required
                  single-line
                  outlined
                  dense
                ></v-text-field>
              </v-col>
              <v-col cols="10" class="pa-0 ma-0">
                <v-text-field
                  class="cs-change-password-dialog-input"
                  v-model="changePasswordDto.confirmNewPassword"
                  :error-messages="invalidPasswordMessages"
                  :rules="confirmPasswordRules"
                  :label="newConfirmationPasswordLabel"
                  type="password"
                  @keyup="validatePasswordMatch"
                  required
                  single-line
                  outlined
                  dense
                ></v-text-field>
              </v-col>
            </v-row>
          </v-card-text>
          <v-card-actions class="pa-0">
            <v-spacer></v-spacer>
            <div class="pb-5 pr-2">
              <v-btn
                class="cs-btn"
                text
                @click="closeDialog(true)"
                :disabled="loading"
              >
                {{ $t("app_cancel_button") }}
              </v-btn>
              <v-btn
                class="cs-btn ma-2"
                color="primary"
                :disabled="!validForm"
                :loading="loading"
                @click="saveForm()"
              >
                {{ $t("app_submit_button") }}
              </v-btn>
            </div>
          </v-card-actions>
        </v-form>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import { mapActions } from "vuex";
import { FieldRulesHelper } from "@/./utilities/Helpers.js";

export default {
  name: "ChangePasswordDialog",
  props: {
    show: {
      type: Boolean,
      required: true,
    },
  },
  model: {
    prop: "show",
  },
  computed: {
    form() {
      return this.$refs.form;
    },
    currentPasswordLabel() {
      return this.$t("change_password_current_label");
    },
    newPasswordLabel() {
      return this.$t("change_password_new_label");
    },
    newConfirmationPasswordLabel() {
      return this.$t("change_password_newconfirm_label");
    },
    passwordDoNotMatchMessage() {
      return this.$t("change_password_not_match_message");
    },
    invalidCurrentPasswordMessage() {
      return this.$t("change_password_bad_current_message");
    },
    currentPasswordRules() {
      return [
        (v) => FieldRulesHelper.requiredField(v, this.currentPasswordLabel),
        (v) =>
          FieldRulesHelper.requiredLengthPassword(v, this.currentPasswordLabel, {
              from: 8,
              to: 16,
            }),
        (v) => FieldRulesHelper.requiredStrongPassword(v, this.currentPasswordLabel),
      ];
    },
    newPasswordRules() {
      return [
        (v) => FieldRulesHelper.requiredField(v, this.newPasswordLabel),
        (v) =>
          FieldRulesHelper.requiredLengthPassword(v, this.newPasswordLabel, {
            from: 8,
            to: 16,
          }),
        (v) => FieldRulesHelper.requiredStrongPassword(v, this.newPasswordLabel),
      ];
    },
    confirmPasswordRules() {
      return [
        (v) => FieldRulesHelper.requiredField(v, this.newConfirmationPasswordLabel),
      ];
    },
  },
  data: () => ({
    loading: false,
    validForm: true,
    changePasswordDto: {},
    invalidPasswordMessages: [],
    isCurrentPasswordInvalid: false,
  }),
  methods: {
    ...mapActions("user", ["changePasswordAsyn"]),
    async saveForm() {
      if (this.form.validate()) {
        this.isCurrentPasswordInvalid = false;
        await this.changePasswordAsyn(this.changePasswordDto).then(
          (response) => {
            if (response.success) {
              this.closeDialog(false);
            } else if (response.message === "Invalid current password") {
              this.isCurrentPasswordInvalid = true;
            }
          }
        );
      }
    },
    closeDialog(isCanceled) {
      this.$emit("onDialogClosed", isCanceled);
      this.form.reset();
      this.invalidPasswordMessages = [];
    },
    validatePasswordMatch() {
      this.invalidPasswordMessages = [];
      if (
        this.changePasswordDto.newPassword !==
        this.changePasswordDto.confirmNewPassword
      ) {
        this.invalidPasswordMessages.push(this.passwordDoNotMatchMessage);
      }
    },
  },
};
</script>
<style lang="scss">
@import "@/scss/components/dialog/ChangePasswordDialog.scss";
</style>