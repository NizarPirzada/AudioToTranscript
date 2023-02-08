import Vue from "vue";
import VueI18n from "vue-i18n";
import store from "@/store";

Vue.use(VueI18n);

const messages = {};
const locales = require.context(
  "../locales",
  true,
  /[A-Za-z0-9-_,\s]+\.json$/i
);

locales.keys().forEach((key) => {
  const matched = key.match(/([A-Za-z0-9-_]+)\./i);
  if (matched && matched.length > 1) {
    messages[matched[1]] = locales(key);
  }
});

store.dispatch("language/initialize", messages);
const { locale, fallbackLocale } = store.state.language;

const i18n = new VueI18n({
  locale,
  fallbackLocale,
  messages,
});

export default i18n;
