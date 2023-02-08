UPDATE [Transcript_Dialog] 
SET [ExaminationTag] = @ExaminationTag, [ExaminationType] = @ExaminationType
WHERE [Id] = @Id AND [TranscriptId] = @TranscriptId