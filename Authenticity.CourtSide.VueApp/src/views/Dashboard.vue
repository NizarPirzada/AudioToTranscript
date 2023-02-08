<template>
  <v-layout column class="cs-dashboard">
    <NewTranscriptDialog
      v-model="showNewTranscriptDialog"
      :transcriptList="transcriptList"
      @create="onNewTranscriptDialogClosed"
    ></NewTranscriptDialog>

    <h1 class="cs-table-title pb-3">{{ $t("dashboard_title") }}</h1>

    <v-flex md12 style="overflow: auto">
      <v-data-table
        class="cs-table cs-table-dashboard"
        v-if="this.transcriptList && this.transcriptList.length > 0"
        :headers="headers"
        :items="transcriptList"
        :items-per-page="20"
        :footer-props="footerProps"
        :sort-by="'lastModifiedOn'"
        :sort-desc="true"
        @click:row="goTranscriptDetail"
        height="78vh"
        fixed-header
      >
        <template v-slot:header>
          <tr class="cs-filter-row">
            <th></th>
            <th>
              <v-text-field
                single-line
                outlined
                dense
                clearable
                :label="$t('dashboard_search_placeholder')"
                @click:clear="onFilterByName"
                @keyup.enter="onFilterByName"
                @blur="onFilterByName"
                class="cs-filter-input-text"
                v-model="filterName"
              ></v-text-field>
            </th>
            <th>
              <div class="d-flex align-start">
                <select
                  class="cs-select cs-select-filter-dashboard cs-select-file-size"
                  v-model="FileSizeOperation"
                >
                  <option
                    v-for="(item, i) in fileSizeOperationOptions"
                    :key="i"
                    v-bind:value="item.value"
                    v-text="item.label"
                  ></option>
                </select>
                <v-text-field
                  single-line
                  outlined
                  dense
                  clearable
                  type="number"
                  hide-details
                  :label="$t('dashboard_search_placeholder')"
                  @click:clear="onFilterByFileSize"
                  @keyup.enter="onFilterByFileSize"
                  @blur="onFilterByFileSize"
                  style="width: 7rem"
                  class="cs-filter-input-text"
                  v-model="filterFileSize"
                ></v-text-field>
              </div>
            </th>
            <th>
              <div style="max-width: auto">
                <select
                  class="cs-select cs-select-filter-dashboard cs-select-status"
                  @change="onFilterByStatus"
                  v-model="filterStatus"
                >
                  <option selected v-bind:value="-1">
                    {{ $t("dashboard_filter_status_default_option") }}
                  </option>
                  <option
                    v-for="(item, i) in statusFilterOptions"
                    :key="i"
                    v-bind:value="item.value"
                    v-text="item.label"
                  ></option>
                </select>
              </div>
            </th>
            <th id="last-modified-date">
              <TableDateFilter
                :id="'last-modified-date'"
                :value="filterLastModifiedDate"
                @applyFilter="onFilterByLastModifiedDate"
              ></TableDateFilter>
            </th>
            <th id="created-on-date">
              <TableDateFilter
                :id="'created-on-date'"
                :value="filterCreatedOnDate"
                @applyFilter="onFilterByCreatedOnDate"
              ></TableDateFilter>
            </th>
          </tr>
        </template>
        <template v-slot:item.actions="{ item }">
          <v-menu content-class="cs-transcript-menu" bottom left>
            <template v-slot:activator="{ on, attrs }">
              <v-btn dark icon v-bind="attrs" v-on="on">
                <v-icon color="accent">mdi-dots-vertical</v-icon>
              </v-btn>
            </template>
            <v-list>
              <v-list-item
                :disabled="item.locked"
                @click="confirmDeleteTranscript(item)"
              >
                <v-list-item-content>
                  <v-list-item-title
                    :class="{ 'cs-transcript-disabled-menu': item.locked }"
                  >
                    {{ $t("transcript_menu_delete") }}</v-list-item-title
                  >
                </v-list-item-content>
              </v-list-item>
            </v-list>
          </v-menu>
        </template>
        <template v-slot:item.icon="{ item }">
          <div class="cs-table-chip cs-table-file-extension">
            {{ getFileExtension(item) }}
          </div>
        </template>
        <template v-slot:item.mediaFileSize="{ item }">
          {{ formatFileSize(item.mediaFileSize) }}
        </template>
        <template v-slot:item.status="{ item }">
          {{ formatTranscriptStatus(item.status, item.locked) }}
        </template>
        <template v-slot:item.lastModifiedOn="{ item }">
          {{ formatListDate(item.lastModifiedOn) }}
        </template>
        <template v-slot:item.createdOn="{ item }">
          {{ formatListDate(item.createdOn) }}
        </template>
        <template v-slot:no-results>
          {{ $t("dashboard_filter_no_result") }}
        </template>
      </v-data-table>
    </v-flex>
    <ProgressDialog
      :show.sync="showProgressDialog"
      :items.sync="transcriptDeletionStatus"
      :disableCloseButton.sync="disableDeleteTranscriptCloseButton"
    ></ProgressDialog>
  </v-layout>
</template>
<script>
import NewTranscriptDialog from "@/components/dialog/NewTranscriptDialog";
import { mapActions, mapState } from "vuex";
import { TableHelper } from "@/utilities/Helpers.js";
import { AuthenticationHelper } from "@/utilities/Helpers.js";
import router from "@/router";
import { VuexKeys } from "@/./environment.js";
import ProgressDialog from "@/components/dialog/ProgressDialog";
import TableDateFilter from "@/components/filters/TableDateFilter";
import { WebStorage } from "@/store/webStorage";

export default {
  name: "UsersPage",
  components: {
    NewTranscriptDialog,
    ProgressDialog,
    TableDateFilter,
  },
  data: () => ({
    showNewTranscriptDialog: false,
    transcriptList: [],
    showProgressDialog: false,
    transcriptDeletionStatus: [],
    disableDeleteTranscriptCloseButton: true,
    filterName: "",
    filterFileSize: "",
    FileSizeOperation: "===",
    filterStatus: -1,
    filterLastModifiedDate: {},
    filterCreatedOnDate: {},
  }),
  computed: {
    ...mapState("permission", ["authorizedObjects"]),
    headers() {
      return [
        {
          align: "start",
          sortable: false,
          value: "icon",
          width: "5%",
        },
        {
          text: this.$t("dashboard_column_name"),
          align: "start",
          value: "name",
          sortable: true,
          filter: (value) => this.filterColumnName(value),
        },
        {
          text: this.$t("dashboard_column_size"),
          align: "start",
          value: "mediaFileSize",
          sortable: false,
          filter: (value) => this.filterColumnFileSize(value),
        },
        {
          text: this.$t("dashboard_column_status"),
          align: "start",
          value: "status",
          sortable: false,
          filter: (value) => this.filterColumnStatus(value),
        },
        {
          text: this.$t("dashboard_column_modified"),
          align: "start",
          value: "lastModifiedOn",
          filter: (value) => this.filterColumnLastModifiedOn(value),
        },
        {
          text: this.$t("dashboard_column_created"),
          align: "start",
          value: "createdOn",
          filter: (value) => this.filterColumnCreatedOn(value),
        },
        {
          align: "end",
          sortable: false,
          value: "actions",
          width: "2%",
        },
      ];
    },
    statusFilterOptions() {
      return [
        { value: 0, label: this.$t("transcript_status_new") },
        { value: 1, label: this.$t("transcript_status_transcribing") },
        { value: 2, label: this.$t("transcript_status_editing") },
        { value: 3, label: this.$t("transcript_status_completed") },
        { value: "locked", label: this.$t("transcript_status_saving") },
      ];
    },
    fileSizeOperationOptions() {
      return [
        { value: "===", label: "=" },
        { value: ">=", label: ">=" },
        { value: "<=", label: "<=" },
      ];
    },
    footerProps() {
      return {
        "items-per-page-options": [10, 15, 20],
        "items-per-page-text": this.$t("dashboard_footer_items_per_page_text"),
        "page-text": `{0}-{1} ${this.$t("dashboard_footer_page_text")}  {2}`,
      };
    },
  },
  methods: {
    ...mapActions("transcript", [
      "getAllTranscripts",
      "saveTranscriptAsync",
      "deleteTranscriptAsync",
    ]),

    formatFileSize(sizeInKB) {
      return TableHelper.FormatFileSizeFromKBToMB(sizeInKB, "MB");
    },
    formatTranscriptStatus(status, locked) {
      let statusLabel = "";
      switch (status) {
        case 0:
          statusLabel = this.$t("transcript_status_new");
          break;
        case 1:
          statusLabel = this.$t("transcript_status_transcribing");
          break;
        case 2:
          statusLabel = this.$t("transcript_status_editing");
          break;
        case 3:
          statusLabel = this.$t("transcript_status_completed");
          break;
        case "locked":
          statusLabel = this.$t("transcript_status_saving");
          break;
      }
      return statusLabel;
    },
    formatListDate(date) {
      return TableHelper.FormatListDate(this, date);
    },
    async showUpNewTranscript() {
      if (
        await AuthenticationHelper.CheckUserCanCreateObject(
          "Create",
          "Dashboard"
        )
      ) {
        this.showNewTranscriptDialog = true;
      }
    },
    onNewTranscriptDialogClosed(data) {
      let nameFormated = data.trim().replace(/\s+/g, " ");
      this.saveTranscriptAsync(nameFormated).then((response) => {
        if (response.success) {
          this.showNewTranscriptDialog = false;
          this.redirectToWizardPage(response.data.id);
        }
      });
    },
    goTranscriptDetail(event, row) {
      this.redirectToWizardPage(row.item.id);
    },
    getFileExtension(transcript) {
      return transcript.transcriptFile?.name.split(".").pop().toUpperCase();
    },
    async confirmDeleteTranscript(transcript) {
      this.$eventBus.$emit(
        VuexKeys.Home.ShowConfirmMessage,
        this.$t("dashboard_delete_transcript_title"),
        this.$t("dashboard_delete_transcript_message").replace(
          "<transcriptName>",
          transcript.name
        ),
        async () => await this.deleteTranscript(transcript.id),
        null,
        this.$t("app_confirm_button"),
        this.$t("app_cancel_button")
      );
    },
    async deleteTranscript(transcriptId) {
      this.disableDeleteTranscriptCloseButton = true;
      this.transcriptDeletionStatus = [
        {
          status: "inProgress",
          message: this.$t("dashboard_delete_transcript_deleting_message"),
        },
      ];
      this.showProgressDialog = true;
      this.deleteTranscriptAsync(transcriptId)
        .then((result) => {
          if (result.success) {
            this.$set(this.transcriptDeletionStatus, 0, {
              status: "finished",
              message: this.$t("dashboard_delete_transcript_success"),
            });
            this.removeTranscriptFromList(transcriptId);
          } else {
            this.$set(this.transcriptDeletionStatus, 0, {
              status: "error",
              message: result.message,
            });
          }
        })
        .finally(() => {
          this.disableDeleteTranscriptCloseButton = false;
        });
    },
    removeTranscriptFromList(transcriptId) {
      const index = this.transcriptList.findIndex((t) => t.id === transcriptId);
      if (index > -1) {
        this.transcriptList.splice(index, 1);
      }
    },
    redirectToWizardPage(transcriptId) {
      router.push({ name: "Wizard", params: { transcriptId } });
    },
    getFiltersObject() {
      let filters = {
        filterName: this.filterName,
        filterFileSize: this.filterFileSize,
        fileSizeOperation: this.FileSizeOperation,
        filterStatus: this.filterStatus,
        filterLastModifiedDate: this.filterLastModifiedDate,
        filterCreatedOnDate: this.filterCreatedOnDate,
      };
      return JSON.stringify(filters);
    },
    setFiltersObject(filters) {
      this.filterName = filters.filterName || "";
      this.filterFileSize = filters.filterFileSize || "";
      this.FileSizeOperation = filters.fileSizeOperation || "===";
      this.filterStatus = filters.filterStatus || -1;
      this.filterLastModifiedDate = filters.filterLastModifiedDate || {};
      this.filterCreatedOnDate = filters.filterCreatedOnDate || {};
    },
    onFilterByName(e){
      this.filterName = e.target._value;
      WebStorage.setDashboardFilters(this.getFiltersObject());
    },
    onFilterByFileSize(e){
      this.filterFileSize = e.target._value;
      WebStorage.setDashboardFilters(this.getFiltersObject());
    },
    onFilterByStatus(e) {
      WebStorage.setDashboardFilters(this.getFiltersObject());
    },
    onFilterByLastModifiedDate(dates){
      this.filterLastModifiedDate = dates;
      WebStorage.setDashboardFilters(this.getFiltersObject());
    },
    onFilterByCreatedOnDate(dates){
      this.filterCreatedOnDate = dates;
      WebStorage.setDashboardFilters(this.getFiltersObject());
    },
    filterColumnName(value) {
      if (!this.filterName) {
        return true;
      }
      return value.toLowerCase().includes(this.filterName.toLowerCase());
    },
    filterColumnFileSize(value) {
      if (!this.filterFileSize || isNaN(this.filterFileSize)) {
        return true;
      }
      const newValue = Math.round(value / 1024);
      return eval(
        `${newValue} ${this.FileSizeOperation} ${this.filterFileSize}`
      );
    },
    filterColumnStatus(value) {
      if (this.filterStatus === -1) {
        return true;
      }
      return value === this.filterStatus;
    },
    filterColumnLastModifiedOn(value) {
      if (!this.filterLastModifiedDate) {
        return true;
      } else if (!!this.filterLastModifiedDate.hasOwnProperty("end")) {
        let filterStart = this.$moment(this.filterLastModifiedDate.start);
        let filterEnd = this.$moment(this.filterLastModifiedDate.end);
        let dateValue = this.$moment(value);

        let matchEnd = true;
        let matchStart = true;
        if (this.filterLastModifiedDate.start !== "") {
          matchStart = dateValue >= filterStart;
        }
        if (this.filterLastModifiedDate.end !== "") {
          matchEnd = dateValue <= filterEnd;
        }
        return matchStart && matchEnd;
      } else if (this.filterLastModifiedDate.start) {
        return value.startsWith(this.filterLastModifiedDate.start);
      } else {
        return true;
      }
    },
    filterColumnCreatedOn(value) {
      if (!this.filterCreatedOnDate) {
        return true;
      } else if (!!this.filterCreatedOnDate.hasOwnProperty("end")) {
        let filterStart = this.$moment(this.filterCreatedOnDate.start);
        let filterEnd = this.$moment(this.filterCreatedOnDate.end);
        let dateValue = this.$moment(value);

        let matchEnd = true;
        let matchStart = true;
        if (this.filterCreatedOnDate.start !== "") {
          matchStart = dateValue >= filterStart;
        }
        if (this.filterCreatedOnDate.end !== "") {
          matchEnd = dateValue <= filterEnd;
        }
        return matchStart && matchEnd;
      } else if (this.filterCreatedOnDate.start) {
        return value.startsWith(this.filterCreatedOnDate.start);
      } else {
        return true;
      }
    },
  },
  async created() {
    const transcriptList = await this.getAllTranscripts();
    this.transcriptList = transcriptList.map((obj) => ({
      ...obj,
      status: obj.locked ? "locked" : obj.status,
    }));
    this.$eventBus.$on(VuexKeys.Transcript.New, this.showUpNewTranscript);
    let objectFilters = WebStorage.getDashboardFilters();
    this.setFiltersObject(objectFilters);
  },
};
</script>
<style lang="scss">
@import "@/scss/views/dashboard.scss";
</style>