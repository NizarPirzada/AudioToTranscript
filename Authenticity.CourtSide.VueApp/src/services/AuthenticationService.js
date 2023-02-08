import apiService from "./apiService";

const URL = {
  Login: "/Authentication/Login",
  CheckUserPermission: "/Authentication/CheckUserPermission",
};

export default {
  LoginAsync(userDto) {
    return apiService.post(URL.Login, userDto);
  },
  CheckUserPermissionAsync(permissionName) {
    return apiService.get(`${URL.CheckUserPermission}/${permissionName}`);
  }
};