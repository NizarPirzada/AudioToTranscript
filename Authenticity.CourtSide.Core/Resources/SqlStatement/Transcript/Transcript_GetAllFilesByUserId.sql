SELECT 
    TP.Id, 
    TP.[Name], 
    TP.TranscriptId,
    TP.[Path],
    TP.CreatedOn
FROM [dbo].[TranscriptFile] AS TP WITH(NOLOCK) 
INNER JOIN [dbo].[Transcript] AS T WITH(NOLOCK) ON TP.TranscriptId = T.Id
WHERE T.CreatedBy = @UserId
ORDER BY TP.Id;