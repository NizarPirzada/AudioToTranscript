//
// plugins/accessories/components/extensions
//

import "babel-polyfill";
import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import vuetify from "./plugins/vuetify";
import i18n from "./plugins/i18n";
import FlagIcon from "vue-flag-icon";
import VuetifyDialog from "vuetify-dialog";
import cssVars from "css-vars-ponyfill";
import VueMoment from "vue-moment";
import moment from "moment-timezone";
import Locale from "./plugins/locale";
import eventBus from "./eventBus";
import VuePlyr from 'vue-plyr'
import 'vue-plyr/dist/vue-plyr.css'
//
// styles(scss/sass/css) / fonts
//

import "vuetify-dialog/dist/vuetify-dialog.css";
import "@mdi/font/css/materialdesignicons.css";
import "roboto-fontface/css/roboto/sass/roboto-fontface.scss";
import "@/scss/app/index.scss";

cssVars({
  include: 'style,link[rel="stylesheet"]:not([href*="//"])',
  preserveVars: true,
  silent: true,
  onlyLegacy: true,
  watch: true,
});

// Directives
let handleOutsideClick;
Vue.directive("closable", {
  bind(el, binding, vnode) {
    handleOutsideClick = (e) => {
      e.stopPropagation()
      const { handler, exclude } = binding.value
      let clickedOnExcludedEl = false
      exclude.forEach((refName) => {
        if (!clickedOnExcludedEl) {
          const excludedEl = vnode.context.$refs[refName];
          if (excludedEl){
            if(excludedEl?.$el) {
              clickedOnExcludedEl = excludedEl.$el.contains(e.target)
            }
            else{
              clickedOnExcludedEl = excludedEl.contains(e.target);
            }
          }
        }
      })
      if (!el.contains(e.target) && !clickedOnExcludedEl) {
        vnode.context[handler]()
      }
    }
    document.addEventListener("click", handleOutsideClick);
    document.addEventListener("touchstart", handleOutsideClick);
  },

  unbind () {
    document.removeEventListener("click", handleOutsideClick);
    document.removeEventListener("touchstart", handleOutsideClick);
  }
});

//
// Vue setups
//

Vue.config.productionTip = false;

Vue.prototype.$eventBus = eventBus;
Vue.use(VuePlyr);
Vue.use(FlagIcon);
Vue.use(Locale);
Vue.use(VueMoment, {
  moment,
});
Vue.use(VuetifyDialog, {
  context: {
    vuetify,
  },
});

new Vue({
  router,
  store,
  vuetify,
  i18n,
  render: (h) => h(App),
}).$mount("#app");
