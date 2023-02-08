import apiService from './apiService';

const URL = {
  GetAllObjects: "/api/Permission/GetAllObjects",
  CheckUserPermission: "/api/Permission/CheckUserPermission",
  CheckTranscriptPermission: "api/Permission/CheckTranscriptPermission",
};

export default {
  GetAllObjectsAsync() {
    return apiService.get(URL.GetAllObjects);
  },
  CheckUserPermissionAsync(permissionName) {
    return apiService.get(`${URL.CheckUserPermission}/${permissionName}`);
  },
  CheckTranscriptPermissionAsync(transcriptId) {
    return apiService.get(`${URL.CheckTranscriptPermission}/${transcriptId}`);
  },
};