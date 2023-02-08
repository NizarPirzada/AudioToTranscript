export default {
  initialize({ commit, state }, messages) {
    const translations = [];
    const locale = state.locale;
    let currentTranslation = null;

    if (messages) {
      for (const id in messages) {
        const message = messages[id];
        const translation = {
          id,
          flagIso: message.flagIso,
          displayName: message.displayName,
        };
        translations.push(translation);
        if (id === locale) {
          currentTranslation = translation;
        }
      }
    }

    if (!currentTranslation) {
      //eslint-disable-next-line
      console.error(`Language not found by key '${locale}'`);
    }

    commit("setCurrentTranslation", currentTranslation);
    commit("setTranslations", Object.freeze(translations));
  },

  changeLanguage({ commit, state }, locale) {
    const translation = state.translations.find((x) => x.id === locale);
    if (translation) {
      commit("setCurrentTranslation", translation);
    }
    commit("setLocale", locale);
  },
};
