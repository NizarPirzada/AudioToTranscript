UPDATE [Transcript] SET 
	[OriginalTranscriptResponse] = @Transcription, 
	[LastModifiedOn] = GETUTCDATE(),
	[Status] = 2
WHERE Id = @Id