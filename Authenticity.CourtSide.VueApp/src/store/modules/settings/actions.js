import SettingsService from "@/services/settingsService";
import { WebStorage } from "@/store/webStorage";
export default {
  async GetAllFileProvidersAsync({ commit }) {
    if (WebStorage.getToken()) {
      return await SettingsService.GetAllFileProvidersAsync();
    }
  },
  async CreateFileProviderAsync({ commit }, createFileProviderDto) {
    if (WebStorage.getToken()) {
      return await SettingsService.CreateFileProviderAsync(createFileProviderDto);
    }
  },
  async UpdateFileProviderAsync({ commit }, updateFileProviderDto) {
    if (WebStorage.getToken()) {
      return await SettingsService.UpdateFileProviderAsync(updateFileProviderDto);
    }
  },
};
