UPDATE [Transcript] SET 
	[Locked] = @Locked,
	[LastModifiedBy] = @LastModifiedBy,
	[LastModifiedOn] = GETUTCDATE()
WHERE Id = @TranscriptId