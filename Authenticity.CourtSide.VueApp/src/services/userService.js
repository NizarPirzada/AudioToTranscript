import apiService from "./apiService";

const URL = {
  GetAllUsers: "/api/User/GetAllUsers",
  CreateUser: "/api/User/CreateUser",
  EditUser: "/api/User/EditUser",
  GetAllRoles: "/api/User/GetAllRoles",
  SetPassword: "/api/User/SetPassword",
  ResetPassword: "/api/User/ResetPassword",
  GetNewUserGuid: "/api/User/GetNewUserGuid",
  ChangePassword: "/api/User/ChangePassword",
  PasswordRecovery: "/api/User/PasswordRecovery",
  DeleteUser: "/api/User/DeleteUser",
  DeleteStandardUser: "/api/User/DeleteStandardUser",
  GetAllTranscriptionEngines: "/api/User/GetAllTranscriptionEngines"
};

export default {
  GetAllUsersWithRolesAsync() {
    return apiService.get(URL.GetAllUsers);
  },
  CreateUserAsync(createUserDto) {
    return apiService.post(URL.CreateUser, createUserDto);
  },
  EditUserAsync(editUserDto) {
    return apiService.put(URL.EditUser, editUserDto);
  },
  GetAllRolesAsync() {
    return apiService.get(URL.GetAllRoles);
  },
  SetPasswordAsync(setPasswordDto) {
    return apiService.post(URL.SetPassword, setPasswordDto);
  },
  ResetPasswordAsync(email) {
    return apiService.get(`${URL.ResetPassword}/${email}`);
  },
  GetNewUserGuidAsync(apiUrl) {
    return apiService.post(URL.GetNewUserGuid, null, {params: { apiUrl }});
  },
  ChangePasswordAsync(changePasswordDto) {
    return apiService.post(URL.ChangePassword, changePasswordDto);
  },
  PasswordRecoveryAsync(changePasswordDto) {
    return apiService.post(URL.PasswordRecovery, changePasswordDto);
  },
  DeleteUserAsync(userId) {
    return apiService.delete(`${URL.DeleteUser}/${userId}`);
  },
  DeleteStandardUserAsync(userId) {
    return apiService.delete(`${URL.DeleteStandardUser}/${userId}`);
  },
  GetAllTranscriptionEnginesAsync() {
    return apiService.get(URL.GetAllTranscriptionEngines);
  }
};