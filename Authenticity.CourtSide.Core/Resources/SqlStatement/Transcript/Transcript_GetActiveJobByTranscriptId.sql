SELECT 
    TJ.Id, 
    TJ.TranscriptId,
    TJ.[Guid] AS JobGuid,
    TJ.[Status],
    TJ.[Message],
    TJ.[StartSendingToApiOn],
    TJ.[SentToApiOn],
    TJ.[CompletedOn],
    TJ.CreatedBy,
    TJ.CreatedOn
FROM [dbo].[TranscriptJob] AS TJ WITH(NOLOCK)
WHERE TJ.[TranscriptId] = @TranscriptId AND [Status] != 5
ORDER BY TJ.CreatedOn;