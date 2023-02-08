<template>
  <v-card class="cs-step-card">
    <v-card-text class="uploader-card-text">
      <span class="step-section-header">{{ $t("step2_name") }}</span>
      <div v-show="currentFiles.length < maxFiles" class="upload-section">
        <form ref="fileform" v-show="!isLoading">
          <v-row v-if="files.length < maxFiles" justify="center" class="mx-2">
            <img :src="uploadImage" height="40" />
          </v-row>
          <v-row v-show="files.length < maxFiles" justify="center" class="mx-2">
            <div class="drop-message">
              <input
                style="display: none;"
                multiple
                type="file"
                id="file"
                ref="file"
                accept=".mp3, .wav"
                v-on:change="handleFileUpload($event)"
              />
              <span class="step-section-subheader">{{ $t("step2_drag_audio_label") }}</span>
            </div>
          </v-row>
          <v-row v-show="files.length < maxFiles" justify="center" class="mx-2">
            <span class="step-section-subheader ligth" color="secondary">{{ $t("step2_or_label") }}</span>
          </v-row>
          <v-row v-if="files.length < maxFiles" justify="center" class="mx-2">
            <v-btn
              color="primary"
              class="cs-upload-button"
              @click="$refs.file.click()"
            >
              {{ $t("step2_choose_file_label") }}
            </v-btn>
          </v-row>
          <v-row justify="center" class="my-2" v-if="showFileError">
            <v-col cols="12" justify="center" class="text-center">
              <span class="cs-error-message">{{ $t("step2_error_file_format_invalid") }}</span>
            </v-col>
          </v-row>
        </form>
        <progress
          v-if="isSavingFiles"
          max="100"
          :value="uploadProgress.percentage"
          class="file-upload-progress"
        ></progress>
        <div v-for="(file, key) in files" :key="key" class="file-listing">
          <v-layout row align-center justify-space-around>
            <v-flex col-1> </v-flex>
            <v-flex col-8>
              <v-input readonly class="file-name-list">
                <v-tooltip bottom>
                  <template v-slot:activator="{ on }">
                    <span v-on="on">{{
                      key + 1 + ". " + truncate(file.name, maxCharsName)
                    }}</span>
                  </template>
                  <span>{{ file.name }}</span>
                </v-tooltip>
              </v-input>
            </v-flex>
            <v-flex col-2>
              <a class="remove" v-on:click="removeFile(key)" v-if="!isLoading && !isSavingFiles">
                <v-icon size="32" color="primary" class="fileIcon">mdi-delete</v-icon>
              </a>
              <a class="remove" v-if="isLoading || isSavingFiles">
                <v-icon size="32" color="primary" class="fileIcon">mdi-loading</v-icon>
              </a>
            </v-flex>
            <v-flex col-1> </v-flex>
          </v-layout>
        </div>
      </div>
      <div v-if="currentFiles.length > 0" class="upload-section">
        <div
          v-for="(file, key) in currentFiles"
          :key="key"
          class="file-listing"
        >
          <v-layout row align-center justify-space-around>
            <v-flex col-1> </v-flex>
            <v-flex col-8>
              <v-input readonly class="file-name-list">
                <v-tooltip bottom>
                  <template v-slot:activator="{ on }">
                    <span v-on="on">{{
                      key + 1 + ". " + truncate(file.name, maxCharsName)
                    }}</span>
                  </template>
                  <span>{{ file.name }}</span>
                </v-tooltip>
              </v-input>
            </v-flex>
            <v-flex col-2>
              <a
                class="remove"
                v-on:click="removeCurrentFile(key)"
                v-if="!isLoading"
              >
                <v-icon v-if="file.id === 0" size="32" color="primary" class="fileIcon">mdi-delete</v-icon>
              </a>
              <a class="remove" v-if="isLoading">
                <v-icon size="32" color="primary" class="fileIcon">mdi-loading</v-icon>
              </a>
            </v-flex>
            <v-flex col-1> </v-flex>
          </v-layout>
        </div>
      </div>
    </v-card-text>
  </v-card>
</template>

<script>
import _ from "lodash";
import FileService from "@/services/fileService";

const MAX_FILE_SIZE_BYTES = 4294967295;

export default {
  name: "FileUploader",
  data() {
    return {
      dragAndDropCapable: false,
      uploaded: false,
      maxFiles: 1,
      maxCharsName: 45,
      isLoading: false,
      loadEnds: false,
      loadStatus: "",
      bigFile: false,
      limitFiles: false,
      files: [],
      filesExistent: [],
      uploadImage: require("../../assets/icons/upload.png"),
      showFileError: false,
    };
  },
  props: {
    transcript: {
      type: Object,
      required: true
    },
    fileList: {
      type: Array,
      required: true
    },
    currentFiles: {
      type: Array,
      required: true
    },
    isSavingFiles: {
      type: Boolean,
      required: true
    },
    uploadProgress: {
      type: Object,
      required: true
    },
  },
  computed: {
    allowDeleteCurrentFile() {
      if (this.transcript.status === 1) {
        return false;
      }
      return true;
    },
  },
  methods: {
    truncate(text, size) {
      let textToShow = text;
      if (text.length > size) {
        textToShow = `${text.substring(0, size)}...`;
      }
      return textToShow;
    },
    removeFile(key) {
      const fileName = this.files[key].name;
      this.files.splice(key, 1);
      this.fileList.splice(key, 1);
      if (this.files.length < this.maxFiles) {
        this.limitFiles = false;
      }
    },
    removeCurrentFile(index) {
      // TO DO This feature will be implemented in other story
    },
    determineDragAndDropCapable() {
      var div = document.createElement("div");
      return (
        ("draggable" in div || ("ondragstart" in div && "ondrop" in div)) &&
        "FormData" in window &&
        "FileReader" in window
      );
    },
    clearFiles() {
      this.files = [];
      this.isLoading = false;
      this.loadEnds = false;
      this.loadStatus = "";
      this.limitFiles = false;
      this.bigFile = false;
    },
    handleFileUpload(e) {
      this.setFiles(e.target.files);
    },
    setFiles(filesToTransfer) {
      let vm = this;
      this.bigFile = false;
      this.loadStatus = "";

      if (vm.loadEnds) {
        vm.clearFiles();
      }
      if (vm.files.length + filesToTransfer.length <= vm.maxFiles) {
        for (let i = 0; i < filesToTransfer.length; i++) {
          const fileName = filesToTransfer[i].name.toLowerCase();
          if (this.isMediaFile(fileName)) {
            this.showFileError = false;
            let transferFile = filesToTransfer[i];
            if (transferFile.size < MAX_FILE_SIZE_BYTES) {
              let isAlreadyAdded = _.some(vm.files, { name: transferFile.name });
              if (!isAlreadyAdded) {
                vm.files.push(transferFile);
                vm.fileList.push(transferFile);
              }
            } else {
              this.loadStatus = "status-error";
              this.bigFile = true;
            }
          }
          else{
            this.showFileError = true;
            return;
          }
        }
        vm.limitFiles = this.files.length >= this.maxFiles;
      } else {
        vm.limitFiles = true;
      }
    },
    isMediaFile(file) {
      const ext = [".mp3", ".wav"];
      return ext.some((el) => file.endsWith(el));
    },
  },
  mounted() {
    this.dragAndDropCapable = this.determineDragAndDropCapable();
    if (this.dragAndDropCapable) {
      [
        "drag",
        "dragstart",
        "dragend",
        "dragover",
        "dragenter",
        "dragleave",
        "drop",
      ].forEach(
        function (evt) {
          this.$refs.fileform.addEventListener(
            evt,
            function (e) {
              e.preventDefault();
              e.stopPropagation();
            }.bind(this),
            false
          );
        }.bind(this)
      );

      this.$refs.fileform.addEventListener(
        "drop",
        function (e) {
          if(this.loadStatus !== "") {
            this.files = [];
            this.loadStatus = "";
            this.isLoading = false;
          }
          this.setFiles(e.dataTransfer.files);
        }.bind(this)
      );
    }
  },
};
</script>