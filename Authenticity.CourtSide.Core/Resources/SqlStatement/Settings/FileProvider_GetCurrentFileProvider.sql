SELECT [Id]
      ,[Name]
      ,[Parameters]
      ,[IsCurrentProvider]
      ,[CreatedBy]
      ,[CreatedOn]
      ,[LastModifiedBy]
      ,[LastModifiedOn]
FROM [dbo].[FileProvider] WITH (NOLOCK)
WHERE [IsCurrentProvider] = 1