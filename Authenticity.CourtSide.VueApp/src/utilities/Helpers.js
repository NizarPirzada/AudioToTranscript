import { WebStorage } from "@/store/webStorage";
import PermissionService from "@/services/permissionService";
import { UserStatusEnum } from "@/utilities/Enumerations";
import { PersonType } from "@/./environment.js";
import i18n from "@/plugins/i18n";

const AuthenticationHelper = {
  async CheckLoginAccessPage(to, from, next) {
    try {
      if (await CheckUserAuthentication()) {
        next("/Home");
      } else {
        next();
      }
    } catch (error) {
      next("/");
    }
  },
  async CheckUserCanCreateObject(permission, object) {
    let hasCreatePermission = false;
    if (WebStorage.getToken()) {
      const response = await PermissionService.CheckUserPermissionAsync(`${permission}${object}`);

      if (response.success) {
        hasCreatePermission = response.data;
      }
    }
    return hasCreatePermission;
  },
  async CheckHomePage(to, from, next) {
    try {
      if (!CheckUserAuthentication()) {
        next("/");
      } else {
        if (to.name === "Home") {
          let decodedToken = WebStorage.getDecodedToken();
          switch (decodedToken.role) {
            case "Administrator":
              next({ name: "Users" });
              break;
            case "Standard":
              next({ name: 'Dashboard' });
              break;
            default:
              next("/");
              break;
          }
        }
        next();
      }
    } catch (error) {
      next("/");
    }
  },
  async CheckAccessPermission(to, from, next) {
    try {

      let hasUserPermission = false;

      if (!CheckUserAuthentication()) {
        next("/");
      }

      if (WebStorage.getToken()) {
        const response = await PermissionService.CheckUserPermissionAsync(to.meta.permission);

        if (response.success) {
          hasUserPermission = response.data;
        }

        if (!hasUserPermission) {
          next("/Home");
        } else {
          next();
        }

      } else {
        next('/');
      }

    } catch (error) {
      next('/');
    }
  },
  async CheckTranscriptAccessPermission(to, from, next) {
    try {

      if (!CheckUserAuthentication()) {
        next({ name: "Home" });
      }

      if (WebStorage.getToken()) {
        const response = await PermissionService.CheckTranscriptPermissionAsync(to.params.transcriptId);

        if (response.success) {
          next();
        } else {
          next({ name: "Home" });
        }

      } else {
        next({ name: "Home" });
      }

    } catch (error) {
      next({ name: "Home" });
    }
  },
};

const TableHelper = {
  FormatFileSizeFromKBToMB(fileSizeInKB, formatSuffix) {
    let sizeFormatted = Math.round(fileSizeInKB / 1024);
    if (formatSuffix) {
      formatSuffix = ` ${formatSuffix}`;
    } else {
      formatSuffix = "";
    }
    return `${sizeFormatted}${formatSuffix}`;
  },
  FormatListDate(contextVue, dateString) {
    contextVue.$moment.locale(i18n.locale);
    let dateFormatted = "";
    let dateToFormat = contextVue.$moment(
      contextVue.$convertToLocalDateTime(dateString)
    );
    let currentDate = contextVue.$moment();
    var diff = currentDate.diff(dateToFormat, "hours");
    if (diff > 23) {
      dateFormatted = dateToFormat.format(i18n.t("dashboard_date_format"));
    } else if (diff > 1) {
      dateFormatted = i18n
        .t("dashboard_date_hours_ago")
        .replace("<hours>", `${diff}`);
    } else {
      dateFormatted = i18n.t("dashboard_date_moment_ago");;
    }
    return dateFormatted;
  },
  FormatListDateWithHour(contextVue, dateString) {
    contextVue.$moment.locale(i18n.locale);
    let dateFormatted = "";
    let dateToFormat = contextVue.$moment(
      contextVue.$convertToLocalDateTime(dateString)
    );
    let currentDate = contextVue.$moment();
    dateFormatted = dateToFormat.format(i18n.t("dashboard_datetime_format"));
    return dateFormatted;
  },
};

const StringHelper = {
  IsNullOrEmpty(strValue) {
    if (!strValue || strValue === "") {
      return true;
    }
    return false;
  },
  RemoveAllSpaces(strValue) {
    if (strValue) {
      return strValue.replace(/\s/g, "");
    }
    return strValue;
  },
};

const ObjectHelper = {
  GetEmptyPersonObject(personType, transcriptId) {
    let person = {
      firstName: "",
      id: 0,
      lastName: "",
      transcriptId: transcriptId,
      type: personType,
      personAdditionalInformationId: 0,
    };

    if (personType === PersonType.PlaintiffAttorney || personType === PersonType.DefendantAttorney) {
      person.additionalInfo = {
        barNumber: "",
        title: "",
        address: "",
        telephone: "",
      }
    }
    return person;
  }
};

const CheckUserAuthentication = () => {
  try {
    const decodedToken = WebStorage.getDecodedToken();
    if (!decodedToken || decodedToken.exp < Date.now() / 1000) {
      return false;
    }
    return true;
  } catch (error) {
    return false;
  }
};

const DatesHelper = {

  GetFormattedDate(date) {
    let castedDate;
    if (date) {
      date = convertUTCDateToLocalDate(date);
      var year = date.getFullYear();

      var month = (1 + date.getMonth()).toString();
      month = month.length > 1 ? month : '0' + month;

      var day = date.getDate().toString();
      day = day.length > 1 ? day : '0' + day;

      castedDate = month + '/' + day + '/' + year;
    }

    return castedDate;
  }
}

const convertUTCDateToLocalDate = (date) => {
  var newDate = new Date(date);
  let timezone = new Date().getTimezoneOffset();
  newDate.setMinutes(-timezone);
  return newDate;
}

const UsersHelper = {
  GetUserStatus(status) {
    switch (status) {
      case 0:
        return UserStatusEnum.Pending;
      case 1:
        return UserStatusEnum.Active;
      case 2:
        return UserStatusEnum.Deactivated;
    }
  }
}

const ALPHANUMERIC_REGEX = /^([^_\W]|\s)+$/;
const PASSWORD_FULL_REGEX = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$/;
const PASSWORD_STRONG_REGEX = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z])/;
const EMAIL_REGEX = /.+@.+\..+/;
const FieldRulesHelper = {
  optionalAlphanumeric: (value, fieldName) =>
    !!value && value.length > 0
      ? ALPHANUMERIC_REGEX.test(value) ||
        `${fieldName} ${i18n.t("validator_error_invalid_format")}`
      : true,
  optionalMaxCharacters: (value, fieldName, maxNumber) =>
    !!value && value.length > 0
      ? value.length <= maxNumber ||
        `${fieldName} ${i18n
          .t("validator_error_max_length")
          .replace("<number>", maxNumber)}`
      : true,
  requiredField: (value, fieldName) =>
    !!value ||
    i18n.t("validator_error_field_required").replace("<field>", fieldName),
  requiredStrongPassword: (value, fieldName) =>
    PASSWORD_STRONG_REGEX.test(value) ||
    i18n.t("validator_error_password_characters").replace("<field>", fieldName),
  requiredLengthPassword: (value, fieldName, { from, to }) =>
    (!!value && value.length >= from && value.length <= to) ||
    i18n
      .t("validator_error_field_length_range")
      .replace("<field>", fieldName)
      .replace("<min>", from)
      .replace("<max>", to),
  requiredValidPassword: (value, fieldName) =>
    PASSWORD_FULL_REGEX.test(value) || `${fieldName} must be valid`,
  requiredValidEmail: (value, fieldName) =>
    EMAIL_REGEX.test(value) || `${fieldName} must be valid`,
  requiredMaxCharacters:  (value, fieldName, maxNumber) => 
  (!!value && value.length <= maxNumber) || `${fieldName} ${i18n
    .t("validator_error_max_length")
    .replace("<number>", maxNumber)}`
}

export {
  AuthenticationHelper,
  DatesHelper,
  UsersHelper,
  TableHelper,
  StringHelper,
  ObjectHelper,
  FieldRulesHelper,
}