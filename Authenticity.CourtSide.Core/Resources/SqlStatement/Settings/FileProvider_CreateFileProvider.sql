INSERT INTO [dbo].[FileProvider] ([Name], [Parameters], [IsCurrentProvider], [CreatedBy], [CreatedOn])
VALUES (@Name, @Parameters, @IsCurrentProvider, @CreatedBy, GETUTCDATE());
DECLARE @FileProviderCreatedId INT = (SELECT CAST(SCOPE_IDENTITY() as int))

IF @IsCurrentProvider = 1
BEGIN
    UPDATE [dbo].[FileProvider]
    SET [IsCurrentProvider] = 0
    WHERE [Id] <> @FileProviderCreatedId
END

SELECT @FileProviderCreatedId