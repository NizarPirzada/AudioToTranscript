import axios from "axios";
import { axiosExportSettings } from "@/globalSettings";
import { WebStorage } from "@/store/webStorage";

const successHandler = (response) =>
({
  data: response.data,
  success: !!response.data
});

const errorHandler = (error) => {
  const response = JSON.parse(String.fromCharCode.apply(null, new Uint8Array(error.response.data)))
  return {
    status: error.response.status,
    success: response.success,
    message: response.message,
  }
}

const exportApiService = axios.create(axiosExportSettings.requestConfig);

exportApiService.interceptors.request.use(
  (config) => {
    const token = WebStorage.getToken();
    if (token) {
      config.headers["Authorization"] = "Bearer " + token;
    }
    return config;
  },
  (error) => {
    Promise.reject(error);
  }
);

exportApiService.interceptors.response.use(
  (response) => successHandler(response),
  (error) => errorHandler(error)
);

if (axiosExportSettings.defaultAuthorizationHeader) {
  exportApiService.defaults.headers.common["Authorization"] =
    axiosExportSettings.defaultAuthorizationHeader;
}

export default exportApiService;
