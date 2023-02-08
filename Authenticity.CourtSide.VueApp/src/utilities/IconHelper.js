const IconsForMenu = {
  Dashboard: require("../assets/icons/dashboard.png"),
  Users: require("../assets/icons/users.png"),
  Deleted: require("../assets/icons/delete.png"),
  Logout: require("../assets/icons/logout.png"),
  NewUser: require("../assets/icons/new_user.png"),
  NewDashboard: require("../assets/icons/new_dashboard.png"),
  AdminUser: require("../assets/icons/admin_user.png"),
  StandardUser: require("../assets/icons/standard_user.png"),
  User: require("../assets/icons/user.png"),
  ChevronDown: require("../assets/icons/chevron_down.png"),
  Settings: require("../assets/icons/settings.png"),
};

const Icons = {
  // Examinations
  ExaminationChangeTag: require("../assets/icons/change_examination_tag.png"),
  ExaminationChangeTagDisabled: require("../assets/icons/change_examination_tag_disabled.png"),
  ExaminationNoTag: require("../assets/icons/no_examination_tag.png"),
};

const IconHelper = {
  GetMenuIcon(menuOption) {
    return IconsForMenu[menuOption];
  },
  GetIcon(option) {
    return Icons[option];
  }
};

export {
  IconHelper
}