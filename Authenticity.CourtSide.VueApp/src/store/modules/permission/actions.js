import PermissionService from "@/services/permissionService";
import { WebStorage } from "@/store/webStorage";

export default {
  async getAllObjectsAsync({ commit }) {
    if (WebStorage.getToken()) {
      let result = await PermissionService.GetAllObjectsAsync();
      let objects = result.data;
      commit("setAuthorizedObjects", objects);
      return result;
    }
  }
};
