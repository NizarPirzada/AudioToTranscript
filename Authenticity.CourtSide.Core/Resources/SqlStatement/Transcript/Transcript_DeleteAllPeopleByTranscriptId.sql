DELETE [dbo].[PersonalAdditionalInformation]
FROM [dbo].[PersonalAdditionalInformation] PAI
    RIGHT JOIN [dbo].[TranscriptPerson] TP ON PAI.Id = TP.PersonAdditionalInformationId
WHERE TP.[TranscriptId] = @TranscriptId

DELETE FROM [dbo].[TranscriptPerson]
WHERE [TranscriptId] = @TranscriptId