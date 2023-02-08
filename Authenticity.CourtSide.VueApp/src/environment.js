const VuexKeys = {
  Transcript: {
    New: "Dashboard:NewTranscript"
  },
  User: {
    New: "User:NewUser",
    CloseDialog: "User:CloseDialog",
  },
  Home: {
    ShowConfirmMessage: "Home:ShowConfirmMessage",
    ShowSnackbarMessage: "Home:ShowSnackbarMessage",
    ShowAlertMessage: "Home:ShowAlertMessage",
    SetAppBarInfo: "Home:SetAppBarInfo",
    UnlockTranscript: "Home:UnlockTranscript",
  },
};

const PersonType = {
  Plaintiff: 1,
  Defendant: 2,
  Deponent: 3,
  PlaintiffAttorney: 5,
  DefendantAttorney: 6,
  AdditionalSpeaker: 7,
  Other: 8,
};

const TranscripStatus = {
  New: 0,
  Processing: 1,
  Editing: 2,
  Completed: 3,
};

const ObjectPermissionDefinition = {
  ViewUser: "ViewUser",
}

const TranscriptType = {
  "Probable Cause Hearing": 1,
  Deposition: 2,
}

const ExaminationTypes = {
  NoExamination: 0,
  StartDirect: 1,
  StartCross: 2,
  InsideDirectExamination: 3,
  CloseDirect: 4,
  InsideCrossExamination: 5,
  CloseCross: 6,
};

const ExaminationTags = {
  NoTag: 0,
  Question: 1,
  Answer: 2,
};

const TranscriptJobStatus = {
  Created: 0,
  SentToAuthenticity: 1,
  Completed: 2,
  Error: 3,
  Processing: 4,
  Canceled: 5,
};

export {
  VuexKeys,
  PersonType,
  TranscripStatus,
  ObjectPermissionDefinition,
  TranscriptType,
  ExaminationTypes,
  ExaminationTags,
  TranscriptJobStatus,
};