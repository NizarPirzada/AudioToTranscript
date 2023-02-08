import apiService from "./apiService";

const URL = {
  GetAllFileProviders: "/api/Settings/GetAllFileProviders",
  CreateFileProvider: "/api/Settings/CreateFileProvider",
  UpdateFileProvider: "/api/Settings/UpdateFileProvider",
};

export default {
  GetAllFileProvidersAsync() {
    return apiService.get(URL.GetAllFileProviders);
  },
  CreateFileProviderAsync(createFileProviderDto) {
    return apiService.post(URL.CreateFileProvider, createFileProviderDto);
  },
  UpdateFileProviderAsync(updateFileProviderDto) {
    return apiService.put(URL.UpdateFileProvider, updateFileProviderDto);
  },
};