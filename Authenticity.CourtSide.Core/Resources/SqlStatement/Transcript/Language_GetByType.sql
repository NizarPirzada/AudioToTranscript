SELECT 
 [Id] AS [ApiLanguageId]
,[LanguageType]
,[Name]
,[ApiCode]
,[GenericCode]
,[CreatedBy]
,[CreatedOn]
,[LastModifiedBy]
,[LastModifiedOn]
 FROM [dbo].[Language] WITH (NOLOCK)
 WHERE [LanguageType] = @LanguageType