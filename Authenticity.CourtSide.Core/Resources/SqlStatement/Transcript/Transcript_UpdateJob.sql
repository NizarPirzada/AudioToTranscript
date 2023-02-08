-- Avoid to update a canceled job
IF NOT EXISTS ( SELECT 1 FROM [dbo].[TranscriptJob] WHERE [Id] = @Id AND [Status] = 5 )
BEGIN
	UPDATE [TranscriptJob] SET
		[Guid] = @JobGuid,
		[Status] = @Status,
		[Message] = @Message,
        [StartSendingToApiOn] = @StartSendingToApiOn,
        [SentToApiOn] = @SentToApiOn,
        [CompletedOn] = @CompletedOn
	WHERE [Id] = @Id
END