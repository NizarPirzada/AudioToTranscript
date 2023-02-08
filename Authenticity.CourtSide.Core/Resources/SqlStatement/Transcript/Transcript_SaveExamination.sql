UPDATE [dbo].[Transcript_Dialog] SET
	[ExaminationType] = @ExaminationType,
	[ExaminationTag] = @ExaminationTag
WHERE [TranscriptId] = @TranscriptId
	AND [Id] >= @StartId
	AND [Id] <= @EndId;

select @@ROWCOUNT;