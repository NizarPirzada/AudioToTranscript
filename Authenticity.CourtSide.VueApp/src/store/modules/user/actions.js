import UserService from "@/services/userService";
import { WebStorage } from "@/store/webStorage";

export default {
  async getAllUsersWithRolesAsync({ commit }) {
    if (WebStorage.getToken()) {
      const result = await UserService.GetAllUsersWithRolesAsync();
      if (result.success) {
        const users = result.data;
        commit("setUsers", users);
      }
      return result;
    }
  },
  async getAllRolesAsync({ commit }) {
    if (WebStorage.getToken()) {
      const result = await UserService.GetAllRolesAsync();
      if (result.success) {
        const roles = result.data;
        commit("setRoles", roles);
      }
      return result;
    }
  },
  async createUserAsync({ commit }, userDto) {
    if (WebStorage.getToken()) {
      const result = await UserService.CreateUserAsync(userDto);
      if (result.success) {
        const user = result.data;
        commit("addUser", user);
      }
      return result;
    }
  },
  async editUserAsync({ commit }, userDto) {
    if (WebStorage.getToken()) {
      const result = await UserService.EditUserAsync(userDto);
      if (result.success) {
        const user = result.data;
        commit("editUser", user);
      }
      return result;
    }
  },
  async setPasswordAsync({ }, setPasswordDto) {
    return await UserService.SetPasswordAsync(setPasswordDto);
  },
  async resetPasswordAsync({ }, email) {
    return await UserService.ResetPasswordAsync(email);
  },
  async getNewUserGuidAsync({ }, apiUrl) {
    return await UserService.GetNewUserGuidAsync(apiUrl);
  },
  async changePasswordAsyn({ }, changePasswordDto) {
    if (WebStorage.getToken()) {
      return await UserService.ChangePasswordAsync(changePasswordDto);
    }
  },
  async passwordRecoveryAsync({ }, changePasswordDto) {
    return await UserService.PasswordRecoveryAsync(changePasswordDto); s
  },
  async deleteUserAsync({ commit }, userId) {
    if (WebStorage.getToken()) {
      return await UserService.DeleteUserAsync(userId);
    }
  },
  async deleteStandardUserAsync({ }, userId) {
    if (WebStorage.getToken()) {
      return await UserService.DeleteStandardUserAsync(userId);
    }
  },
  async getAllTranscriptionEnginesAsync({}) {
    if (WebStorage.getToken()) {
      return await UserService.GetAllTranscriptionEnginesAsync();
    }
  }
};
