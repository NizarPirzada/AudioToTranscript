
SELECT 
    T.Id, 
    T.Name, 
    T.MediaFileSize,
    T.[Status],
    T.[Step],
    T.TranscriptDateTime AS RecordDate,
    T.DeptNumber,
    T.TranscriptType,
    T.Locked,
    T.CreatedBy,
    T.CreatedOn,
    T.LastModifiedOn,
    T.LastModifiedBy
FROM [dbo].[Transcript] AS T WITH(NOLOCK)
WHERE T.CreatedBy =@UserId;