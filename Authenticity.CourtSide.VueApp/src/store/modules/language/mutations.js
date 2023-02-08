import { MutationsHelper } from "@/store/helpers";

export default {
  setLocale: MutationsHelper.set("locale"),
  setTranslations: MutationsHelper.set("translations"),
  setCurrentTranslation: MutationsHelper.set("currentTranslation"),
};
