import AuthenticationService from "@/services/authenticationService";
import { WebStorage } from "@/store/webStorage";

export default {
    async loginAsync({ commit }, loginDto) {
        let response = await AuthenticationService.LoginAsync(loginDto);
        if (response.success) {
            WebStorage.removeToken();
            let token = response.data.token;
            WebStorage.setToken(token);
            let user = response.data.user;
            commit("setUserInfo", user);
        }
        return response;
    }
};
