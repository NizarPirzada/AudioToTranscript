import apiService from "./apiService";
import { axiosSettings } from "@/globalSettings";
import exportApiService from "./exportApiService";

const URL = {
  GetAllTranscripts: "/api/Transcript/GetAll",
  GetById: "/api/Transcript/GetById",
  SaveSimple: "/api/Transcript/SaveSimple",
  Edit: "/api/Transcript/Edit",
  GetTranscriptPersons: "/api/Transcript/GetPersons",
  SavePerson: "/api/Transcript/SavePerson",
  SaveMediaInfo: "/api/Transcript/SaveMediaInfo",
  GetTranscriptFiles: "/api/Transcript/GetMediaFileList",
  SaveRecordDate: "/api/Transcript/UpdateRecordingDate",
  GetTranscriptDialogs: "/api/Transcript/GetDialogs",
  GetAudioFile: `/api/File/GetAudioFile`,
  EditTranscriptDialog: "/api/Transcript/EditTranscriptDialog",
  EditAllSpeakers: "/api/Transcript/EditAllSpeakers",
  ExportTranscript: "/api/Export/ExportTranscript",
  TranslateExportTranscript: "/api/Export/TranslateExportTranscript",
  DeleteTranscriptPerson: "/api/Transcript/DeleteTranscriptPerson",
  GetJobStatus: "/api/Transcript/GetJobStatus",
  ResendTranscript: "/api/Transcript/ResendTranscript",
  DeleteTranscript: "/api/Transcript/Delete",
  SaveExaminations: "/api/Transcript/SaveExaminations",
  SaveExaminationTags: "/api/Transcript/SaveExaminationTags",
  UpdateSingleExaminationTag: "/api/Transcript/UpdateSingleExaminationTag",
  UpdateMassivelyExaminationTags: "/api/Transcript/UpdateMassivelyExaminationTags",
  LockTranscript: "/api/Transcript/LockTranscript",
  GetAllTranscriptionLanguages: "api/Transcript/GetAllTranscriptionLanguages",
  GetAllTranslationLanguages: "api/Export/GetAllTranslationLanguages",
  RequestHumanTranscription: "api/Transcript/RequestHumanTranscription",
};

export const TranscriptService = {
  GetAllTranscriptsAsync() {
    return apiService.get(URL.GetAllTranscripts);
  },
  GetTranscriptByIdAsync(id) {
    return apiService.get(`${URL.GetById}/${id}`);
  },
  SaveSimpleAsync(transcriptName) {
    let params = {
      Name: transcriptName
    };
    return apiService.post(URL.SaveSimple, params);
  },
  EditTranscriptAsync(transcript) {
    return apiService.post(URL.Edit, transcript);
  },
  SaveRecordDateAsync(transcript) {
    return apiService.post(URL.SaveRecordDate, transcript);
  },
  GetTranscriptPersonsByIdAsync(id) {
    return apiService.get(`${URL.GetTranscriptPersons}/${id}`);
  },
  SavePersonAsync(person) {
    return apiService.post(URL.SavePerson, person);
  },
  SaveMediaFileInfoAsync(mediaFile) {
    return apiService.post(URL.SaveMediaInfo, mediaFile);
  },
  GetTranscriptFilesByIdAsync(id) {
    return apiService.get(`${URL.GetTranscriptFiles}/${id}`);
  },
  GetTranscriptDialogsByIdAsync(id, page) {
    return apiService.get(`${URL.GetTranscriptDialogs}/${id}/${page}`);
  },
  GetAudioFileUrlAsync(transcriptId, token) {
    return `${axiosSettings.requestConfig.baseURL}${URL.GetAudioFile}?transcriptionId=${transcriptId}&tid=${token}`
  },
  EditDialogInformationAsync(dialog) {
    return apiService.post(URL.EditTranscriptDialog, dialog);
  },
  EditSpeakerAllDialogsAsync(dialog) {
    return apiService.post(URL.EditAllSpeakers, dialog);
  },
  ExportTranscriptAsync(exportTranscriptDto) {
    return exportApiService.post(URL.ExportTranscript, exportTranscriptDto);
  },
  TranslateExportTranscriptAsync(exportTranscriptDto) {
    return exportApiService.post(URL.TranslateExportTranscript, exportTranscriptDto);
  },
  DeleteTranscriptPersonAsync(transcriptPersonId) {
    return apiService.delete(
      `${URL.DeleteTranscriptPerson}/${transcriptPersonId}`
    );
  },
  GetJobStatusAsync(transcriptId) {
    return apiService.get(`${URL.GetJobStatus}/${transcriptId}`);
  },
  ResendTranscriptAsync(transcriptIdValue) {
    let params = {
      transcriptId: transcriptIdValue
    };
    return exportApiService.post(URL.ResendTranscript, params);
  },
  DeleteTranscriptAsync(transcriptId) {
    return apiService.delete(`${URL.DeleteTranscript}/${transcriptId}`);
  },
  SaveExaminationsAsync(transcriptId, examinatonDtoList) {
    return apiService.post(`${URL.SaveExaminations}/${transcriptId}`, examinatonDtoList);
  },
  UpdateSingleExaminationTagAsync(examinationDto) {
    return apiService.post(URL.UpdateSingleExaminationTag, examinationDto);
  },
  UpdateMassivelyExaminationTagsAsync(massiveDto) {
    return apiService.post(URL.UpdateMassivelyExaminationTags, massiveDto);
  },
  LockTranscriptAsync(lockTranscriptDto) {
    return apiService.post(URL.LockTranscript, lockTranscriptDto);
  },
  GetAllTranscriptionLanguagesAsync() {
    return apiService.get(URL.GetAllTranscriptionLanguages);
  },
  GetAllTranslationLanguagesAsync() {
    return apiService.get(URL.GetAllTranslationLanguages);
  },
  RequestHumanTranscriptionAsync(transcript) {
    return apiService.post(URL.RequestHumanTranscription, transcript);
  },
};