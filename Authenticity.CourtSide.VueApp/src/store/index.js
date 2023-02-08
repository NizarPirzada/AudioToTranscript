import Vue from "vue";
import Vuex from "vuex";
import VuexPersist from "vuex-persist";
import { language, permission, authentication, transcript, user, settings } from "./modules";

const vuexLocalStorage = new VuexPersist({
  storage: window.localStorage,
  reducer: (state) => ({
    language: {
      locale: state.language.locale,
    },
  }),
});

Vue.use(Vuex);

export default new Vuex.Store({
  plugins: [vuexLocalStorage.plugin],
  state: {
    version: Object.freeze("0.0.1"),
  },
  modules: {
    language,
    permission,
    authentication,
    transcript,
    user,
    settings,
  },
});
