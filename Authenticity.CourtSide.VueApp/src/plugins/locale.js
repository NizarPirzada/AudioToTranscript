import i18n from "@/plugins/i18n";
export default {
  install(Vue) {
    const getCurrentUserLocale = () => {
      return i18n.locale;
    };

    const getTimeZoneAbbreviation = (momentObj) => {
      momentObj = new Vue.moment();
      var isDST = momentObj.isDST(),
        offset = momentObj.utcOffset(),
        offsetHrMins = offset / 60,
        offsetHours = offsetHrMins > 0 ? Math.floor(offsetHrMins) : Math.ceil(offsetHrMins),
        offsetMins = Math.abs(offsetHrMins % 1),
        abbreviation = "";

      switch (offsetHours) {
        case -12:
        case -11:
        case -10:
          abbreviation = isDST ? "" : "HAST";
          break;
        case -9:
          abbreviation = isDST ? "HADT" : "AKST";
          break;
        case -8:
          abbreviation = isDST ? "AKDT" : "PST";
          break;
        case -7:
          abbreviation = isDST ? "PDT" : "MST";
          break;
        case -6:
          abbreviation = isDST ? "MDT" : "CST";
          break;
        case -5:
          abbreviation = isDST ? "CDT" : "EST";
          break;
        case -4:
          abbreviation = isDST ? "EDT" : "AST";
          break;
        case -3:
          abbreviation = isDST ? "ADT" : "";
          break;
        case -2:
        case -1:
        case 1:
          abbreviation = isDST ? "WEST" : "CET";
          break;
        case 2:
          abbreviation = isDST ? "CEST" : "EET";
          break;
        case 3:
          abbreviation = isDST ? "EEST" : "";
          break;
        case 4:
        case 5:
        case 6:
        case 7:
          abbreviation = "CXT";
          break;
        case 8:
        case 9:
          abbreviation = offsetMins === 0.5 ? "CST" : "";
          break;
        case 10:
          abbreviation = offsetMins === 0.5 ? "CST" : "EST";
          break;
        case 11:
          abbreviation = offsetMins === 0.5 ? "NFT" : "EST";
          break;
        default:
          abbreviation = "GMT";
          break;
      }

      return abbreviation;
    };

    Vue.prototype.$formatNumber = (number) => {
      if (number === 0) {
        return 0;
      }
      if (!!number) {
        const locale = getCurrentUserLocale();
        return new Intl.NumberFormat(locale).format(number);
      }
    };

    Vue.prototype.$formatTime = (timestamp) => {
      if (!!timestamp) {
        return timestamp.substring(1, 8);
      }
    };

    Vue.prototype.$formatDateTime = (date) => {
      if (!!date) {
        const locale = getCurrentUserLocale();
        return `${Vue.moment(date + "Z")
          .locale(locale)
          .format("L LTS")} ${getTimeZoneAbbreviation()}`;
      }
      return "";
    };

    Vue.prototype.$convertToLocalDateTime = (date) => {
      if (!!date) {
        const locale = getCurrentUserLocale();
        return Vue.moment(date + "Z")
          .locale(locale)
          .format();
      }
      return Vue.moment(date).format();
    };

    Vue.prototype.$formatDateDescriptive = (date) => {
      if (!!date) {
        const locale = getCurrentUserLocale();
        return `${Vue.moment(date + "Z")
          .locale(locale)
          .format("MMM DD, YYYY")}`;
      }
      return "";
    };

    Vue.prototype.$formatDateTimeDescriptive = (date) => {
      if (!!date) {
        const locale = getCurrentUserLocale();
        return `${Vue.moment(date + "Z")
          .locale(locale)
          .format("MMM DD, YYYY HH:mm")}`;
      }
      return "";
    };


    Vue.prototype.$formatHourMinute = (date) => {
      if (!!date) {
        const locale = getCurrentUserLocale();
        return `${Vue.moment(date + "Z")
          .locale(locale)
          .format("HH:mm")}`;
      }
      return "";
    };
    Vue.prototype.$formatHTMLInputDate = (date) => {
      if (!!date) {
        const locale = getCurrentUserLocale();
        return `${Vue.moment(date + "Z")
          .locale(locale)
          .format("YYYY-MM-DD")}`;
      }
      return "";
    };
    Vue.prototype.$formatExportDate = (date) => {
      if (!!date) {
        const locale = getCurrentUserLocale();
        return `${Vue.moment(date + "Z")
          .locale(locale)
          .format("MMDDYYYYHHmm")}`;
      }
      return "";
    };
    Vue.prototype.$currentUserLocale = () => {
      return getCurrentUserLocale();
    };
  },
};