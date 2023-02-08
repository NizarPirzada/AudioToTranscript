import axios from "axios";
import { axiosSettings } from "@/globalSettings";
import { WebStorage } from "@/store/webStorage";

const successHandler = (response) =>
({
  status: response.status,
  success: response.data.success,
  data: response.data.data,
  message: response.data.message,
});

const errorHandler = (error) => ({
  status: error.response.status,
  success: error.response.data.success,
  data: error.response.data.data,
  message: error.response.data.message,
});

const apiService = axios.create(axiosSettings.requestConfig);

apiService.interceptors.request.use(
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

apiService.interceptors.response.use(
  (response) => successHandler(response),
  (error) => errorHandler(error)
);

if (axiosSettings.defaultAuthorizationHeader) {
  apiService.defaults.headers.common["Authorization"] =
    axiosSettings.defaultAuthorizationHeader;
}

export default apiService;
