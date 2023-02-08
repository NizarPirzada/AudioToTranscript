<template>
  <div>
    <v-dialog
      content-class="cs-user-dialog"
      v-model="show"
      width="572"
      height="555"
      persistent
      @keydown.esc.exact="closeDialog"
      @keydown.enter.exact="saveForm"
    >
      <v-card>
        <v-card-title class="pa-0">
          <div class="mt-10 ml-8">
            <p class="cs-user-dialog-title-text">{{ cardTitle }}</p>
          </div>
        </v-card-title>
        <v-form ref="form" v-model="validForm" lazy-validation>
          <v-card-text class="pa-0">
            <v-row class="px-1 mx-1 pt-3">
              <v-col cols="10" class="pa-0 ma-0">
                <v-text-field
                  class="cs-user-dialog-input"
                  v-model="userDto.firstName"
                  :rules="firstNameRules"
                  label="First Name"
                  required
                  outlined
                  dense
                ></v-text-field>
              </v-col>
              <v-col cols="10" class="pa-0 ma-0">
                <v-text-field
                  class="cs-user-dialog-input"
                  v-model="userDto.lastName"
                  :rules="lastNameRules"
                  label="Last Name"
                  required
                  outlined
                  dense
                ></v-text-field>
              </v-col>
              <v-col cols="10" class="pa-0 ma-0">
                <v-text-field
                  class="cs-user-dialog-input"
                  v-model="userDto.email"
                  :rules="emailRules"
                  label="Email"
                  required
                  outlined
                  dense
                ></v-text-field>
              </v-col>
            </v-row>
            <v-row class="px-1 mx-1" v-if="isEditingStandardUser">
              <v-col cols="10" class="pa-0 ma-0">
                <v-text-field
                  class="cs-user-dialog-input"
                  v-model="userDto.apiUrl"
                  :rules="apiUrlRules"
                  label="API URL"
                  required
                  outlined
                  dense
                ></v-text-field>
              </v-col>
              <v-col cols="10" class="pa-0 ma-0">
                <v-text-field
                  class="cs-user-dialog-input"
                  v-model="userDto.apiGuid"
                  :rules="apiGuidRules"
                  label="API GUID"
                  required
                  outlined
                  dense
                  :append-icon="getGuidAppednIcon"
                  @click:append="refreshUserGuid"
                  :loading="isGettingNewUserGuid"
                >
                </v-text-field>
              </v-col>
              <v-col cols="10" class="pa-0 ma-0">
                <span class="v-label cs-label-select px-1">Engine</span>
                <select
                  class="cs-select cs-select-user-transcription-engine"
                  v-model="userDto.transcriptionEngineId"
                >
                  <option
                    v-for="option in transcriptionEngines"
                    v-bind:value="option.id"
                    :key="option.id"
                  >
                    {{ option.name }}
                  </option>
                </select>
              </v-col>
            </v-row>
            <v-row class="px-1 mx-1" v-if="isNewUser">
              <v-col class="pa-0 ma-0">
                <div class="mt-2 ml-7">
                  <p class="cs-user-dialog-title-text">Role</p>
                </div>
                <v-radio-group v-model="userDto.roleId" mandatory>
                  <v-radio
                    class="cs-user-dialog-input"
                    color="accent"
                    v-for="(item, key) in roles"
                    :key="key"
                    :label="item.name"
                    :value="item.roleId"
                    small
                  ></v-radio>
                </v-radio-group>
              </v-col>
            </v-row>
          </v-card-text>
          <v-card-actions class="pa-0">
            <v-spacer></v-spacer>
            <div class="pb-5 pr-2">
              <v-btn
                class="cs-btn"
                text
                @click="closeDialog"
                :disabled="loading"
              >
                Cancel
              </v-btn>
              <v-btn
                class="cs-btn ma-2"
                color="primary"
                :disabled="!validForm"
                :loading="loading"
                @click="saveForm()"
              >
                {{ isNewUser ? "Create" : "Submit" }}
              </v-btn>
            </div>
          </v-card-actions>
        </v-form>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import { mapActions, mapState } from "vuex";
import { VuexKeys } from "@/./environment.js";
import { RolesEnum, SnackbarType } from "@/utilities/Enumerations";

export default {
  name: "UserDialog",
  props: {
    show: {
      type: Boolean,
      required: true,
    },
    persistent: {
      type: Boolean,
      default: true,
    },
    loading: {
      type: Boolean,
      required: true,
    },
    userDto: {
      type: Object,
      default: function () {
        return {
          id: 0,
          firstName: "",
          lastName: "",
          email: "",
          status: 0,
          roleId: "2",
          transcriptionEngineId: 2,
        };
      },
    },
  },
  model: {
    prop: "show",
  },
  computed: {
    ...mapState("user", ["roles"]),
    cardTitle() {
      return this.isNewUser ? "New User" : "Edit User";
    },
    isNewUser() {
      return this.userDto.id == 0;
    },
    isEditingStandardUser() {
      return !this.isNewUser && this.userDto.roleId === RolesEnum.Standard;
    },
    isGettingNewUserGuid() {
      return this.apiGuidLoading && !!this.userDto.apiUrl;
    },
    getGuidAppednIcon() {
      return !this.isGettingNewUserGuid &&
        this.apiUrlRules[1](this.userDto.apiUrl) === true
        ? "mdi-refresh"
        : undefined;
    },
  },
  data: () => ({
    firstNameRules: [(v) => !!v || "First name is required"],
    lastNameRules: [(v) => !!v || "Last name is required"],
    emailRules: [
      (v) => !!v || "E-mail is required",
      (v) =>
        /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/.test(v) || "E-mail must be valid",
    ],
    validForm: true,
    apiUrlRules: [
      (v) => !!v || "API URL is required",
      (v) =>
        /^(http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$/.test(
          v
        ) || "Url format is not valid",
    ],
    apiGuidRules: [
      (v) => !!v || "API GUID is required",
      (v) => /^[a-zA-Z0-9]{8}$/.test(v) || "API GUID format is not valid",
    ],
    apiGuidLoading: false,
    transcriptionEngines: [],
  }),
  methods: {
    ...mapActions("user", [
      "getAllRolesAsync",
      "getNewUserGuidAsync",
      "getAllTranscriptionEnginesAsync",
    ]),
    saveForm() {
      if (this.$refs.form.validate()) {
        if (!this.isNewUser) {
          this.validateApiURl();
          this.validateAPIGUID();
          this.$emit("onUserDialogClosed", this.userDto, "edit");
        } else {
          this.$emit("onUserDialogClosed", this.userDto, "create");
        }
      }
    },
    closeDialog() {
      const dto = {
        id: 0,
        firstName: "",
        lastName: "",
        email: "",
        status: 0,
        roleId: 1,
        transcriptionEngineId: 2,
      };
      this.$emit("update:userDto", dto);
      this.$emit("input", false);
      if (this.$refs.form) {
        this.$refs.form.reset();
        this.$refs.form.resetValidation();
      }
    },
    async refreshUserGuid() {
      this.apiGuidLoading = true;
      this.validateApiURl();
      await this.getNewUserGuidAsync(this.userDto.apiUrl)
        .then((result) => {
          if (result.success) {
            this.userDto.apiGuid = result.data;
          } else {
            this.$eventBus.$emit(
              VuexKeys.Home.ShowSnackbarMessage,
              result.message,
              SnackbarType.Error
            );
          }
        })
        .finally(() => {
          this.apiGuidLoading = false;
        });
    },
    validateApiURl() {
      if (this.userDto.apiUrl) {
        this.userDto.apiUrl = this.userDto.apiUrl.replace(/\/?$/, "/");
      }
    },
    validateAPIGUID() {
      if (this.userDto.apiGuid) {
        this.userDto.apiGuid = this.userDto.apiGuid.toUpperCase();
      }
    },
  },
  async mounted() {
    await this.getAllRolesAsync();
    await this.getAllTranscriptionEnginesAsync().then((response) => {
      if (response.success) {
        this.transcriptionEngines = response.data;
      }
    });
    this.$eventBus.$on(VuexKeys.User.CloseDialog, this.closeDialog);
  },
};
</script>
<style lang="scss">
@import "@/scss/components/dialog/UserDialog.scss";
</style>