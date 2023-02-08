SELECT [Id]
      ,[Name]
      ,[Code]
      ,[CreatedBy]
      ,[CreatedOn]
      ,[LastModifiedBy]
      ,[LastModifiedOn]
FROM [dbo].[TranscriptionEngine] WITH (NOLOCK)
WHERE [Id] = @TranscriptionEngineId