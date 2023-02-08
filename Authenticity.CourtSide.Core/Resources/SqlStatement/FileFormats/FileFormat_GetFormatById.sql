SELECT 
    Id, 
    [Path],
	[FileName],
	[FormatName],
	[Type],
	[CreatedBy],
	[CreatedOn]
FROM [dbo].[FileExportFormat] WITH(NOLOCK)
WHERE Id =@FormatId;