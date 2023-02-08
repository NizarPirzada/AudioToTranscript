DECLARE @OriginalSpeakerId AS INT;
SELECT @OriginalSpeakerId = [OriginalSpeakerId]
FROM [Transcript_Dialog] WITH(NOLOCK)
WHERE [Id] = @Id;

UPDATE [Transcript_Dialog] SET
	[PersonId] = @PersonId
WHERE [OriginalSpeakerId] = @OriginalSpeakerId
    AND [TranscriptId] = @TranscriptId