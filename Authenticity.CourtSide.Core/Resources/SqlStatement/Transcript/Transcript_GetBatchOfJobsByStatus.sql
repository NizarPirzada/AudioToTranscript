MERGE INTO [dbo].[TranscriptJob] AS TARGET
USING
(
    SELECT TOP 5
        Id, 
        TranscriptId,
        [Guid],
        [Status],
        [ApiUrl],
        [TranscriptionEngine],
        [Message],
        [StartSendingToApiOn],
        [SentToApiOn],
        [CompletedOn],
        CreatedBy,
        CreatedOn
    FROM [dbo].[TranscriptJob]
    WHERE [Status] =  @Status
) AS SOURCE
ON TARGET.[Id] = SOURCE.[Id]
WHEN MATCHED THEN UPDATE SET [Status] = 4
OUTPUT INSERTED.[Id]
        ,INSERTED.TranscriptId
        ,INSERTED.[Guid] AS JobGuid
        ,INSERTED.[Status]
        ,INSERTED.[ApiUrl]
        ,INSERTED.[TranscriptionEngine]
        ,INSERTED.[Message]
        ,INSERTED.[StartSendingToApiOn]
        ,INSERTED.[SentToApiOn]
        ,INSERTED.[CompletedOn]
        ,INSERTED.CreatedBy
        ,INSERTED.CreatedOn;