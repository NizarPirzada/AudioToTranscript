UPDATE [User] 
SET 
	[FirstName] = @FirstName,
	[LastName] = @LastName,
	[Email] = @Email, 
	[Status] = @Status,
	[EmailActivationId] = CASE WHEN @EmailActivationId IS NOT NULL THEN @EmailActivationId ELSE [EmailActivationId] END,
	[ApiUrl] = @ApiUrl,
	[ApiGuid] = @ApiGuid,
	[TranscriptionEngineId] = @TranscriptionEngineId,
    [LastModifiedBy] = @LastModifiedBy,
    [LastModifiedOn] = GETUTCDATE() 
WHERE [Id] = @Id