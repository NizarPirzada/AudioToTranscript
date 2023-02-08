<template>
  <div class="cs-settings-page">
    <h1 class="cs-page-title">System Settings</h1>

    <v-card class="cs-settings-card">
      <p class="cs-settings-header">File Server</p>

      <v-radio-group
        class="ml-5 mb-5"
        v-model="fileServerTypeSelected"
        row
        @change="getDataFileProviderSelected"
      >
        <v-radio
          v-for="(item, key) in fileServerTypes"
          :key="key"
          :label="item.label"
          :value="item.value"
          color="black"
        ></v-radio>
      </v-radio-group>

      <v-form
        ref="fileProviderForm"
        v-model="validFileProviderForm"
        lazy-validation
      >
        <v-text-field
          class="cs-settigns-text-field medium"
          label="Server Url"
          outlined
          required
          :rules="fileServerRules"
          v-model="fileProviderParameters.Uri"
        ></v-text-field>

        <v-text-field
          class="cs-settigns-text-field small"
          label="User"
          outlined
          required
          :rules="formEmptyRules"
          v-model="fileProviderParameters.Username"
        ></v-text-field>

        <v-text-field
          class="cs-settigns-text-field small"
          label="Password"
          outlined
          type="password"
          required
          :rules="formEmptyRules"
          v-model="fileProviderParameters.Password"
        ></v-text-field>

        <v-checkbox
          v-if="sftpSelected"
          class="mx-5 my-0 pa-0"
          label="SSL Enabled"
          v-model="fileProviderParameters.IsSSL"
        ></v-checkbox>

        <v-text-field
          v-if="!sftpSelected"
          class="cs-settigns-text-field small"
          label="Root folder"
          outlined
          required
          :rules="formEmptyRules"
          v-model="fileProviderParameters.RootFolder"
        ></v-text-field>

        <v-text-field
          v-if="!sftpSelected"
          class="cs-settigns-text-field small"
          label="Port"
          outlined
          required
          :rules="formNumericRules"
          v-model="fileProviderParameters.Port"
        ></v-text-field>
      </v-form>
    </v-card>

    <div class="cs-btn-settings-container">
      <v-btn
        color="primary"
        class="cs-btn cs-btn-save-settings"
        @click="saveSettings"
      >
        Save
      </v-btn>
    </div>
  </div>
</template>
<script>
import { mapActions } from "vuex";
import { SnackbarType } from "@/utilities/Enumerations";
import { VuexKeys } from "@/./environment.js";

export default {
  name: "SettingsPage",
  components: {},
  data: () => ({
    fileServerTypes: [
      { label: "FTP/FTPS", value: "FTP/FTPS" },
      { label: "SFTP", value: "SFTP" },
    ],
    fileServerTypeSelected: "",
    currentFileProvider: {},
    fileProviderParameters: {
      Uri: "",
      Username: "",
      Password: "",
      IsSSL: false,
      Port: 0,
      RootFolder: "",
    },
    fileProviders: [],
    fileProviderDto: {},
    validFileProviderForm: true,
    formEmptyRules: [(v) => !!v],
    formNumericRules: [(v) => !!v, (v) => /^\d+$/.test(v)],
    formFtpRules: [(v) => !!v, (v) => /^(ftp:\/\/|ftps:\/\/)/.test(v)],
    successMessage: "System Settings updated successfully",
    failedMessage: "File Server settings must be valid",
  }),
  computed: {
    sftpSelected() {
      return this.currentFileProvider?.name === this.fileServerTypes[0].value;
    },
    fileServerRules() {
      return this.sftpSelected ? this.formFtpRules : this.formEmptyRules;
    },
  },
  methods: {
    ...mapActions("settings", [
      "GetAllFileProvidersAsync",
      "CreateFileProviderAsync",
      "UpdateFileProviderAsync",
    ]),
    getDataFileProviderSelected() {
      this.currentFileProvider = this.fileProviders.find(
        (p) => p.name === this.fileServerTypeSelected
      );
      if (this.currentFileProvider) {
        this.currentFileProvider.isCurrentProvider = true;
        this.fileProviderParameters = JSON.parse(
          this.currentFileProvider.parameters
        );
      } else {
        this.fileProviderParameters = {};
      }
    },
    getCurrentFileProvider() {
      this.currentFileProvider = this.fileProviders.find(
        (p) => p.isCurrentProvider === true
      );
      if (this.currentFileProvider) {
        this.fileServerTypeSelected = this.currentFileProvider.name;
        this.fileProviderParameters = JSON.parse(
          this.currentFileProvider.parameters
        );
      }
    },
    async saveSettings() {
      try {
        await this.saveFileProvider();
        this.showSnackBar(this.successMessage, SnackbarType.Success);
      } catch (error) {
        this.showSnackBar(error, SnackbarType.Error);
      }
    },
    showSnackBar(message, type) {
      this.$eventBus.$emit(VuexKeys.Home.ShowSnackbarMessage, message, type);
    },
    async saveFileProvider() {
      if (this.$refs.fileProviderForm.validate()) {
        this.fileProviderDto = {
          name: this.fileServerTypeSelected,
          parameters: JSON.stringify(this.fileProviderParameters),
          isCurrentProvider: true,
        };

        if (this.currentFileProvider && this.currentFileProvider.id > 0) {
          this.fileProviderDto.id = this.currentFileProvider.id;

          const updateResult = await this.UpdateFileProviderAsync(
            this.fileProviderDto
          );
          if (updateResult.success) {
            this.currentFileProvider = updateResult.data;
          } else {
            throw updateResult.message;
          }
        } else {
          const createResult = await this.CreateFileProviderAsync(
            this.fileProviderDto
          );
          if (createResult.success) {
            this.currentFileProvider = createResult.data;
          } else {
            throw updateResult.message;
          }
        }
      } else {
        throw this.failedMessage;
      }
    },
  },
  async mounted() {
    const requestResult = await this.GetAllFileProvidersAsync();
    if (requestResult.success) {
      this.fileProviders = requestResult.data;
      this.getCurrentFileProvider();
    }
  },
};
</script>
<style lang="scss">
@import "@/scss/views/settingsPage.scss";
</style>
