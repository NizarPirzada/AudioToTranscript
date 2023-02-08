SELECT CAST(1 AS BIT)
FROM [dbo].[Transcript] WITH(NOLOCK)
WHERE [Id] = @TranscriptId AND [CreatedBy] = @UserId