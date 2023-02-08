SELECT 
    TP.Id, 
    [TranscriptId],
	[OriginalSpeakerId],
	COALESCE([OriginalSpeakerName], 'Speaker') AS [OriginalSpeakerName],
	[PersonId],
	[StartTime],
	[Duration],
	[Transcription],
	[ExaminationType],
	[ExaminationTag]
FROM [dbo].[Transcript_Dialog] AS TP WITH(NOLOCK)
WHERE TP.TranscriptId =@TranscriptId
ORDER BY [StartTime] ASC
OFFSET 100*(@Page-1) ROWS FETCH NEXT 100 ROWS ONLY;