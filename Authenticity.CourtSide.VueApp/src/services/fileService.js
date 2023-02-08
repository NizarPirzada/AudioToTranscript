import apiService from "./apiService";

const URL = {
  FileUpload: "/api/File/UploadAudioFile",
};

export const FileService = {
  FileUploadAsync(
    setUploadProgress,
    transcriptId,
    fileIndex,
    fileData,
    fileSize,
  ) {
    const onProgress = setUploadProgress;
  
    let formData = new FormData();
    formData.append("file", fileData);
    formData.append("id", transcriptId);
    formData.append("size", fileSize);

    return apiService.post(URL.FileUpload, formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
      onUploadProgress: function (progressEvent) {
        const uploadPercentage = parseInt(
          Math.round((progressEvent.loaded * 100) / progressEvent.total)
        );
        const status = uploadPercentage < 100 ? "loading" : "success";
        onProgress({ fileIndex, value: uploadPercentage, status });
      }
    });
  },
};