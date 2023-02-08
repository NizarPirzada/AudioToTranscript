INSERT INTO [User] ([FirstName], [LastName], [Email], [Status], [EmailActivationId], [ApiUrl], [ApiGuid], [TranscriptionEngineId], [CreatedBy],[CreatedOn])
VALUES (@FirstName, @LastName, @Email, @Status, @EmailActivationId, @ApiUrl, @ApiGuid, @TranscriptionEngineId, @CreatedBy, GETUTCDATE());
SELECT CAST(SCOPE_IDENTITY() as int)