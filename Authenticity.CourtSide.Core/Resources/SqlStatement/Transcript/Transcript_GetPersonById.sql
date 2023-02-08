SELECT 
    TP.Id, 
    TP.FirstName,
    TP.MiddleName,
    TP.LastName, 
    TP.TypeId AS [Type],
    TP.TranscriptId,
    TP.CreatedBy,
    TP.CreatedOn,
    TP.PersonAdditionalInformationId,
    AI.[BarNumber], 
    AI.[Title], 
    AI.[Address], 
    AI.[Telephone],
    AI.[LegalFirm]
FROM [dbo].[TranscriptPerson] AS TP WITH(NOLOCK) 
LEFT JOIN [dbo].[PersonalAdditionalInformation] AS AI WITH(NOLOCK) ON TP.PersonAdditionalInformationId = AI.Id
WHERE TP.Id = @PersonId