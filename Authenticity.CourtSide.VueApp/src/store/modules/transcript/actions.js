import { TranscriptService } from "@/services/transcriptService";
import { WebStorage } from "@/store/webStorage";

export default {
  async getAllTranscripts() {
    if (WebStorage.getToken()) {
      let result = await TranscriptService.GetAllTranscriptsAsync();
      let objects = result.data;
      return objects;
    }
  },
  async getTranscriptById({ }, id) {
    if (WebStorage.getToken()) {
      let result = await TranscriptService.GetTranscriptByIdAsync(id);
      return result;
    }
  },
  async saveTranscriptAsync({ }, transcriptName) {
    if (WebStorage.getToken()) {
      let response = await TranscriptService.SaveSimpleAsync(transcriptName);
      return response;
    }
  },
  async updateTranscriptAsync({ }, transcript) {
    if (WebStorage.getToken()) {
      const response = await TranscriptService.EditTranscriptAsync(transcript);
      return response;
    }
  },
  async updateRecordDateAsync({ }, transcript) {
    if (WebStorage.getToken()) {
      const response = await TranscriptService.SaveRecordDateAsync(transcript);
      return response;
    }
  },
  async getTranscriptPersonsByIdAsync({ }, transcriptId) {
    if (WebStorage.getToken()) {
      const response = await TranscriptService.GetTranscriptPersonsByIdAsync(
        transcriptId
      );
      return response.data;
    }
  },
  async savePersonAsync({ commit }, person) {
    if (WebStorage.getToken()) {
      const response = await TranscriptService.SavePersonAsync(person);
      return response;
    }
  },
  async saveFileInfoAsync({ }, mediaFile) {
    if (WebStorage.getToken()) {
      return await TranscriptService.SaveMediaFileInfoAsync(mediaFile);
    }
  },
  async getTranscriptFilesByIdAsync({ commit }, transcriptId) {
    if (WebStorage.getToken()) {
      const response = await TranscriptService.GetTranscriptFilesByIdAsync(
        transcriptId
      );
      if (response.success && response.data.length) {
        commit("setCurrentFile", response.data[0]);
      }
      return response.data;
    }
  },
  async getTranscriptDialogsByTranscriptIdAsync({}, { transcriptId, page }) {
    if (WebStorage.getToken()) {
      const response = await TranscriptService.GetTranscriptDialogsByIdAsync(
        transcriptId,
        page,
      );
      return response.data;
    }
  },
  async getAudioFileUrlAsync({ }, transcriptId) {
    let token = WebStorage.getToken();
    if (token) {
      const fileUrl = TranscriptService.GetAudioFileUrlAsync(transcriptId, token);
      return fileUrl;
    }
  },
  async editDialogAsync({ }, dialog) {
    if (WebStorage.getToken()) {
      const response = await TranscriptService.EditDialogInformationAsync(dialog);
      return response;
    }
  },
  async editSpeakerAllDialogsAsync({ }, dialog) {
    if (WebStorage.getToken()) {
      const response = await TranscriptService.EditSpeakerAllDialogsAsync(dialog);
      return response;
    }
  },
  async exportTranscriptAsync({}, exportTranscriptDto) {
    if (WebStorage.getToken()) {
      const response = await TranscriptService.ExportTranscriptAsync(exportTranscriptDto);
      return response;
    }
  },
  async translateExportTranscriptAsync({}, exportTranscriptDto) {
    if (WebStorage.getToken()) {
      const response = await TranscriptService.TranslateExportTranscriptAsync(exportTranscriptDto);
      return response;
    }
  },
  async deleteTranscriptPersonAsync({}, transcriptPersonId) {
    if (WebStorage.getToken()) {
      return await TranscriptService.DeleteTranscriptPersonAsync(transcriptPersonId);
    }
  },
  async getJobStatusAsync({ }, transcriptId) {
    if (WebStorage.getToken()) {
      return await TranscriptService.GetJobStatusAsync(transcriptId);
    }
  },
  async resendJobsAsync({ }, transcriptId) {
    if (WebStorage.getToken()) {
      const response = await TranscriptService.ResendTranscriptAsync(transcriptId);
      return response;
    }
  },
  async deleteTranscriptAsync({ }, transcriptId) {
    if (WebStorage.getToken()) {
      return await TranscriptService.DeleteTranscriptAsync(transcriptId);
    }
  },
  async saveExaminationsAsync({}, { transcriptId, examinatonDtoList }) {
    if (WebStorage.getToken()) {
      return await TranscriptService.SaveExaminationsAsync(transcriptId, examinatonDtoList);
    }
  },
  async updateSingleExaminationTagAsync({}, examinationDto) {
    if (WebStorage.getToken()) {
      return await TranscriptService.UpdateSingleExaminationTagAsync(examinationDto);
    }
  },
  async updateMassivelyExaminationTagsAsync({}, massiveDto) {
    if (WebStorage.getToken()) {
      return await TranscriptService.UpdateMassivelyExaminationTagsAsync(massiveDto);
    }
  },
  async lockTranscriptAsync({}, lockTranscriptDto) {
    return await TranscriptService.LockTranscriptAsync(lockTranscriptDto);
  },
  async getAllTranscriptionLanguagesAsync({}) {
    if (WebStorage.getToken()) {
      return await TranscriptService.GetAllTranscriptionLanguagesAsync();
    }
  },
  async getAllTranslationLanguagesAsync({}) {
    if (WebStorage.getToken()) {
      return await TranscriptService.GetAllTranslationLanguagesAsync();
    }
  },
  async createHumanTranscriptionAsync({ }, transcript) {
    if (WebStorage.getToken()) {
      const response = await TranscriptService.RequestHumanTranscriptionAsync(transcript);
      return response;
    }
  },
};
