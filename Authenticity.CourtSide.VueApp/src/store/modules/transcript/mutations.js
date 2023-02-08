import { MutationsHelper } from "@/store/helpers";

export default {
  setCurrentFile: MutationsHelper.set('currentFile'),
  setTranscriptInEdition: MutationsHelper.set('transcriptInEdition'),
  setCurrentTranscriptLocked: MutationsHelper.set('currentTranscriptLocked'),
};
