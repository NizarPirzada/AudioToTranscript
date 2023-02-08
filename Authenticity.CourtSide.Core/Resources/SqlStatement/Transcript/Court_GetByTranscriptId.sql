SELECT 
    C.[Id], 
    C.[TranscriptId], 
    C.[Name],
    C.[Address],
	C.[City],
	C.[State],
	C.[Zipcode],
    C.CreatedBy,
    C.CreatedOn,
    C.LastModifiedBy,
    C.LastModifiedOn
FROM [dbo].[Court] AS C WITH(NOLOCK)
WHERE C.[TranscriptId] =@TranscriptId;