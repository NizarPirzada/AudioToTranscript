import { MutationsHelper } from "@/store/helpers";

export default {
  setUsers: MutationsHelper.set("users"),
  addUser: MutationsHelper.pushTo("users"),
  editUser: MutationsHelper.replaceRecordInList("users"),
  setRoles: MutationsHelper.set("roles"),
};
