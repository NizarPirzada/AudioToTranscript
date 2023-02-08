INSERT INTO [TranscriptJob] ([TranscriptId], [Guid], [Status], [ApiUrl], [TranscriptionEngine], [Message], [CreatedBy], [CreatedOn])
VALUES (@TranscriptId, @JobGuid, @Status, @ApiUrl, @TranscriptionEngine, @Message, @CreatedBy, GETUTCDATE());
SELECT CAST(SCOPE_IDENTITY() as int)