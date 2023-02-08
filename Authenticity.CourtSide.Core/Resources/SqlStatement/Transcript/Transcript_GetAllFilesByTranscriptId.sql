SELECT 
    TP.Id, 
    TP.[Name], 
    TP.TranscriptId,
    TP.[Path],
    TP.CreatedOn
FROM [dbo].[TranscriptFile] AS TP WITH(NOLOCK)
WHERE TP.TranscriptId =@TranscriptId;