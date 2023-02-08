UPDATE [FileProvider] 
SET
	[Name] = @Name,
	[Parameters] = @Parameters,
	[IsCurrentProvider] = @IsCurrentProvider,
	[LastModifiedBy] = @LastModifiedBy,
	[LastModifiedOn] = GETUTCDATE()
WHERE Id = @Id

IF @IsCurrentProvider = 1
BEGIN
    UPDATE [dbo].[FileProvider]
    SET [IsCurrentProvider] = 0
    WHERE [Id] <> @Id
END