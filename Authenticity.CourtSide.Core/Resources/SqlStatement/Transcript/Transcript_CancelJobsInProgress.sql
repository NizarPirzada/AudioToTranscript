UPDATE [TranscriptJob] SET
	[Status] = 5
WHERE [TranscriptId] = @TranscriptId
	AND [Status] NOT IN (2, 5)