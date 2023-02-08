import jwtDecode from "jwt-decode";

export const WebStorage = {
  tokenKey: "userToken",
  filterKey: "dashboardFilters",
  setToken: (token) => window.localStorage.setItem(WebStorage.tokenKey, token),
  getToken: () => window.localStorage.getItem(WebStorage.tokenKey),
  getDecodedToken: () => jwtDecode(window.localStorage.getItem(WebStorage.tokenKey)), 
  removeToken: () => {
    window.localStorage.removeItem(WebStorage.tokenKey);
  },
  setDashboardFilters: (filters) => {
    window.localStorage.setItem(WebStorage.filterKey, filters);
  },
  getDashboardFilters: () => {
    let filters = window.localStorage.getItem(WebStorage.filterKey);
    try {
      return JSON.parse(filters);
    } catch (e) {
      return {};
    }
  },
};
