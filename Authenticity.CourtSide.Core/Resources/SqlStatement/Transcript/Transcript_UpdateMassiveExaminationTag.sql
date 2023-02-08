UPDATE [dbo].[Transcript_Dialog]
SET [ExaminationTag] = @ExaminationTag
WHERE [TranscriptId] = @TranscriptId
	AND [OriginalSpeakerId] = @OriginalSpeakerId
	AND [Id] >= @StartId
	AND [Id] <= @EndId;

select @@ROWCOUNT;