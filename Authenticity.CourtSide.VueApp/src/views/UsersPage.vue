<template>
  <div>
    <h1 class="cs-table-title">Administrators</h1>

    <v-data-table
      class="cs-table"
      elevation="0"
      :loading="loadingAdminUsers"
      loading-text="Loading users... Please wait"
      :headers="adminUserHeaders"
      :items="adminUsers"
    >
      <template v-slot:item.actions="{ item }">
        <v-menu content-class="cs-user-menu" bottom left>
          <template v-slot:activator="{ on, attrs }">
            <v-btn dark icon v-bind="attrs" v-on="on">
              <v-icon color="accent">mdi-dots-vertical</v-icon>
            </v-btn>
          </template>
          <v-list>
            <div
              v-for="(menuItem, menuIndex) in item.userMenuItems"
              :key="menuIndex"
            >
              <v-list-item
                v-if="menuItem.show"
                @click="menuItem.handleItem(item)"
              >
                <v-list-item-content>
                  <v-list-item-title>{{ menuItem.title }}</v-list-item-title>
                  <v-list-item-subtitle
                    v-if="isTheLastItemMenu(menuItem, menuIndex)"
                  >
                    <v-divider></v-divider>
                  </v-list-item-subtitle>
                </v-list-item-content>
              </v-list-item>
            </div>
          </v-list>
        </v-menu>
      </template>
      <template v-slot:item.icon>
        <div class="cs-table-chip">
          <v-img :src="adminIcon" class="cs-table-admin-icon"></v-img>
        </div>
      </template>
    </v-data-table>
    <h1 class="cs-table-title">Users</h1>
    <v-data-table
      class="cs-table"
      elevation="0"
      :loading="loadingNotAdminUsers"
      loading-text="Loading users... Please wait"
      :headers="notAdminUserHeaders"
      :items="notAdminUsers"
    >
      <template v-slot:item.actions="{ item }">
        <v-menu content-class="cs-user-menu" bottom left>
          <template v-slot:activator="{ on, attrs }">
            <v-btn dark icon v-bind="attrs" v-on="on">
              <v-icon color="accent">mdi-dots-vertical</v-icon>
            </v-btn>
          </template>
          <v-list>
            <div
              v-for="(menuItem, menuIndex) in item.userMenuItems"
              :key="menuIndex"
            >
              <v-list-item
                v-if="menuItem.show"
                @click="menuItem.handleItem(item)"
              >
                <v-list-item-content>
                  <v-list-item-title>{{ menuItem.title }}</v-list-item-title>
                  <v-list-item-subtitle
                    v-if="isTheLastItemMenu(menuItem, menuIndex)"
                  >
                    <v-divider></v-divider>
                  </v-list-item-subtitle>
                </v-list-item-content>
              </v-list-item>
            </div>
          </v-list>
        </v-menu>
      </template>
      <template v-slot:item.icon>
        <div class="cs-table-chip">
          <v-img :src="standardIcon" class="cs-table-standard-icon"></v-img>
        </div>
      </template>
    </v-data-table>
    <UserDialog
      v-model="showNewUserDialog"
      @onUserDialogClosed="onUserDialogClosed"
      :userDto.sync="userDto"
      :loading="userDialogLoading"
    ></UserDialog>
    <ProgressDialog
      :show.sync="showProgressDialog"
      :items.sync="userDeletionStatus"
      :disableCloseButton.sync="disableDeleteUserCloseButton"
    ></ProgressDialog>
  </div>
</template>
<script>
import { mapActions, mapState } from "vuex";
import { RolesEnum, SnackbarType } from "@/utilities/Enumerations";
import { DatesHelper, UsersHelper, StringHelper } from "@/utilities/Helpers";
import UserDialog from "@/components/dialog/UserDialog";
import { VuexKeys } from "@/./environment.js";
import { IconHelper } from "@/utilities/IconHelper.js";
import ProgressDialog from "@/components/dialog/ProgressDialog";
import { WebStorage } from "@/store/webStorage";
import * as signalR from "@aspnet/signalr";

export default {
  name: "UsersPage",
  components: {
    UserDialog,
    ProgressDialog,
  },
  computed: {
    ...mapState("user", ["users"]),
    user() {
      const decodedToken = WebStorage.getDecodedToken();
      let userInformation = {
        id: decodedToken.id,
        fullName:
          decodedToken.firstName +
          " " +
          (decodedToken.lastName ? decodedToken.lastName[0] : ""),
      };

      return userInformation;
    },
    adminUsers() {
      let users = this.users.filter((u) =>
        u.roles.find((r) => r.roleId === RolesEnum.Administrator)
      );
      return users.map((u) => {
        return {
          id: u.id,
          firstName: u.firstName,
          lastName: u.lastName,
          fullName: `${u.firstName} ${u.lastName}`,
          email: u.email,
          roleId: u.roles[0].roleId,
          status: u.status,
          createdOn: DatesHelper.GetFormattedDate(u.createdOn),
          lastLogin: DatesHelper.GetFormattedDate(u.lastLogin) || "--",
          emailActivationId: u.emailActivationId,
          userMenuItems: this.getMenuItems(u.emailActivationId, u.id),
        };
      });
    },
    notAdminUsers() {
      let users = this.users.filter((u) =>
        u.roles.find((r) => r.roleId !== RolesEnum.Administrator)
      );
      return users.map((u) => {
        return {
          id: u.id,
          firstName: u.firstName,
          lastName: u.lastName,
          fullName: `${u.firstName} ${u.lastName}`,
          email: u.email,
          roleId: u.roles[0].roleId,
          status: u.status,
          createdOn: DatesHelper.GetFormattedDate(u.createdOn),
          statusName: UsersHelper.GetUserStatus(u.status),
          emailActivationId: u.emailActivationId,
          userMenuItems: this.getMenuItems(u.emailActivationId, u.id),
          apiUrl: u.apiUrl,
          apiGuid: u.apiGuid,
          transcriptionEngineId: u.transcriptionEngineId,
        };
      });
    },
    adminIcon() {
      return IconHelper.GetMenuIcon("AdminUser");
    },
    standardIcon() {
      return IconHelper.GetMenuIcon("StandardUser");
    },
  },
  data: () => ({
    userDto: {
      id: 0,
      firstName: "",
      lastName: "",
      email: "",
      status: 0,
      roleId: "2",
      transcriptionEngineId: 2,
    },
    showNewUserDialog: false,
    loadingAdminUsers: true,
    loadingNotAdminUsers: true,
    adminUserHeaders: [
      {
        align: "start",
        sortable: false,
        value: "icon",
        width: "5%",
      },
      {
        text: "Name",
        align: "start",
        sortable: true,
        value: "fullName",
      },
      {
        text: "Created",
        align: "center",
        sortable: false,
        value: "createdOn",
      },
      {
        text: "Last Login",
        align: "center",
        sortable: false,
        value: "lastLogin",
      },
      {
        text: "Email",
        align: "start",
        sortable: true,
        value: "email",
      },
      {
        align: "end",
        sortable: false,
        value: "actions",
        width: "2%",
      },
    ],
    notAdminUserHeaders: [
      {
        align: "start",
        sortable: false,
        value: "icon",
        width: "5%",
      },
      {
        text: "Name",
        align: "start",
        sortable: true,
        value: "fullName",
      },
      {
        text: "Created",
        align: "center",
        sortable: false,
        value: "createdOn",
      },
      {
        text: "Status",
        align: "center",
        sortable: false,
        value: "statusName",
      },
      {
        text: "Email",
        align: "start",
        sortable: true,
        value: "email",
      },
      {
        align: "start",
        sortable: false,
        value: "actions",
        width: "2%",
      },
    ],
    emailActivationId: null,
    userDialogLoading: false,
    showProgressDialog: false,
    userDeletionStatus: [],
    disableDeleteUserCloseButton: true,
  }),
  methods: {
    ...mapActions("user", [
      "getAllUsersWithRolesAsync",
      "createUserAsync",
      "editUserAsync",
      "resetPasswordAsync",
      "deleteUserAsync",
    ]),
    getMenuItems(emailActivationId, userId) {
      let userMenuItems = [
        {
          title: "Edit",
          handleItem: this.editUser,
          show: true,
        },
        {
          title: "Reset Password",
          handleItem: this.resetPassword,
          show: !!emailActivationId,
        },
      ];
      if (userId != this.user.id) {
        userMenuItems.push({
          title: "Delete",
          handleItem: this.confirmDeleteUserAsync,
          show: true,
        });
      }
      return userMenuItems;
    },
    editUser(item) {
      this.userDto = Object.assign({}, item);
      this.showNewUserDialog = true;
    },
    async resetPassword(item) {
      await this.resetPasswordAsync(item.email).then((response) => {
        if (response.success) {
          this.$eventBus.$emit(
            VuexKeys.Home.ShowAlertMessage,
            "Password has been Reset"
          );
        }
      });
    },
    showUpNewUser() {
      this.userDto.roleId = 2;
      this.showNewUserDialog = true;
    },
    async onUserDialogClosed(userDto, action) {
      this.userDialogLoading = true;
      userDto.email = StringHelper.RemoveAllSpaces(userDto.email);
      if (action === "edit") {
        let editUserResponse = await this.editUserAsync(userDto);

        if (editUserResponse.success) {
          this.$eventBus.$emit(VuexKeys.User.CloseDialog);
          this.showSnackBar("User updated successfully", SnackbarType.Success);
        } else {
          this.showSnackBar(editUserResponse.message, SnackbarType.Error);
        }
      } else if (action === "create") {
        let newUserResponse = await this.createUserAsync(userDto);
        if (newUserResponse.success) {
          if (!!newUserResponse.message) {
            this.showSnackBar(newUserResponse.message, SnackbarType.Warning);
          } else {
            this.showSnackBar(
              "User created successfully",
              SnackbarType.Success
            );
          }
          this.$eventBus.$emit(VuexKeys.User.CloseDialog);
        } else {
          this.showSnackBar(newUserResponse.message, SnackbarType.Error);
        }
      }
      this.userDialogLoading = false;
    },
    showSnackBar(message, type) {
      this.$eventBus.$emit(VuexKeys.Home.ShowSnackbarMessage, message, type);
    },
    isTheLastItemMenu(item, index) {
      let objectKeys = Object.keys(item).length;

      if (item.show) {
        return objectKeys - 1 > index;
      }

      return objectKeys > index;
    },
    async confirmDeleteUserAsync(item) {
      this.$eventBus.$emit(
        VuexKeys.Home.ShowConfirmMessage,
        "Delete User",
        `Are you sure you want to delete <strong>${item.fullName}</strong> user?`,
        async () => await this.deleteUserActionAsync(item)
      );
    },
    async deleteUserActionAsync(item) {
      // When user is admin
      if (item.roleId === 1) {
        this.$set(this.userDeletionStatus, 0, {
          status: "inProgress",
          message: "Deleting user information...",
        });
        this.showProgressDialog = true;
        this.disableDeleteUserCloseButton = true;
        this.deleteUserAsync(item.id)
          .then((result) => {
            if (result.success) {
              this.$set(this.userDeletionStatus, 0, {
                status: "finished",
                message: "User information deleted",
              });
              this.removeUserFromList(item.id);
            } else {
              this.$set(this.userDeletionStatus, 0, {
                status: "error",
                message: result.message,
              });
            }
          })
          .finally(() => {
            this.disableDeleteUserCloseButton = false;
          });
      } else if (item.roleId === 2) {
        this.showProgressDialog = true;
        this.disableDeleteUserCloseButton = true;
        let vm = this;
        vm.$set(this.userDeletionStatus, 0, {
          status: "inProgress",
          message: "Starting delete process...",
        });

        this.deleteUserAsync(item.id)
          .then((result) => {
            if (result.success) {
              vm.$set(vm.userDeletionStatus, 2, {
                status: "finished",
                message: "User information deleted.",
              });
              this.removeUserFromList(item.id);
            } else {
              this.$set(
                this.userDeletionStatus,
                this.userDeletionStatus.length - 1,
                {
                  status: "error",
                  message: result.message,
                }
              );
            }
          })
          .finally(() => {
            this.disableDeleteUserCloseButton = false;
          });
      }
    },
    removeUserFromList(userId) {
      const index = this.users.findIndex((u) => u.id === userId);
      if (index > -1) {
        this.users.splice(index, 1);
      }
    },
  },
  created: function () {
    let vm = this;
    const baseURL =
      process.env.VUE_APP_API_SERVICE_HOST ||
      `${location.protocol + "//" + window.location.hostname}`;

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(`${baseURL}/admin-notification-hub`)
      .configureLogging(signalR.LogLevel.Information)
      .build();
    this.connection.start().catch(function (err) {
      //eslint-disable-next-line no-console
      return console.error("HUB ERROR", err.toString());
    });
    this.connection.on("NotifyDeletionStep", function (user, message) {
      if (message === "Deleting media files...") {
        vm.$set(vm.userDeletionStatus, 0, {
          status: "inProgress",
          message: message,
        });
      } else if (message === "Media files deleted") {
        vm.$set(vm.userDeletionStatus, 0, {
          status: "finished",
          message: message,
        });
        vm.$set(vm.userDeletionStatus, 1, {
          status: "inProgress",
          message: "Delete transcriptions...",
        });
      } else if (message === "Transcriptions deleted") {
        vm.$set(vm.userDeletionStatus, 1, {
          status: "finished",
          message: message,
        });
        vm.$set(vm.userDeletionStatus, 2, {
          status: "inProgress",
          message: "Deleting user information...",
        });
      }
    });
  },
  async mounted() {
    await this.getAllUsersWithRolesAsync();
    this.loadingAdminUsers = false;
    this.loadingNotAdminUsers = false;
    this.$eventBus.$on(VuexKeys.User.New, this.showUpNewUser);
  },
};
</script>
<style lang="scss">
@import "@/scss/views/usersPage.scss";
</style>