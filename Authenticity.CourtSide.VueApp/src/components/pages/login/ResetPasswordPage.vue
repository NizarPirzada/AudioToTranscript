<template>
  <v-app>
    <v-container fluid fill-height>
      <v-layout align-center justify-center>
        <v-flex>
          <v-row justify="center" align="center">
            <v-card tile elevation="0" width="400">
              <v-row class="d-flex flex-column">
                <v-card-title class="align-self-center"
                  >{{ $t("account_setup_title") }}</v-card-title
                >
                <v-card-subtitle class="align-self-center"
                  >{{ $t("account_setup_subtitle") }}</v-card-subtitle
                >
              </v-row>
              <v-form ref="validForm" v-model="validForm" @submit.prevent="ActivateAccount" lazy-validation>
                <v-card-text class="text--primary">
                  <v-text-field
                    v-model="setPasswordDto.password"
                    :rules="passwordRules"
                    :label="$t('account_setup_new_password_label')"
                    type="password"
                    solo
                    required
                  ></v-text-field>
                  <v-text-field
                    v-model="setPasswordDto.confirmPassword"
                    :rules="passwordMatchRules"
                    :label="$t('account_setup_new_password_repeat_label')"
                    type="password"
                    solo
                    required
                  ></v-text-field>
                </v-card-text>
                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn
                    :disabled="!validForm"
                    color="primary"
                    :loading="loading"
                    class="mb-3 mr-3"
                    type="submit"
                  >
                    {{ $t("account_setup_activate_button") }}
                  </v-btn>
                </v-card-actions>
              </v-form>
            </v-card>
          </v-row>
        </v-flex>
      </v-layout>
    </v-container>
  </v-app>
</template>

<script>
import router from "@/router";
import { mapActions, mapState } from "vuex";

export default {
  name: "ResetPasswordPage",
  computed: {
    passwordMatchRules() {
      return [
        () =>
          this.setPasswordDto.password ===
            this.setPasswordDto.confirmPassword || this.$t("account_setup_password_notmatch_message"),
        (v) => !!v || this.$t("account_setup_repeatpassword_required_message"),
      ];
    },
  },
  data: function () {
    return {
      validForm: true,
      loading: false,
      setPasswordDto: {
        password: "",
        confirmPassword: "",
        emailActivationId: "",
      },
      passwordRules: [
        (v) => !!v || this.$i18n.t("account_setup_password_required_message"),
        (v) =>
          /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$/.test(v) ||
          this.$i18n.t("account_setup_password_notvalid_message"),
      ],
    };
  },
  methods: {
    ...mapActions("user", ["setPasswordAsync"]),
    async ActivateAccount() {
      this.loading = true;
      if (this.$refs.validForm.validate()) {
        let result = await this.setPasswordAsync(this.setPasswordDto);

        if (result) {
          router.push({ name: "Login" });
        }
      }
      this.loading = false;
    },
  },
  async created() {
    this.setPasswordDto.emailActivationId = this.$route.params.activationId;
  },
};
</script>