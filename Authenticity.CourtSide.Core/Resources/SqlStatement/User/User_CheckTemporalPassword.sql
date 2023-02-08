SELECT CAST(1 AS BIT)
FROM [dbo].[User] WITH(NOLOCK)
WHERE [Id] = @UserId AND [TemporalPassword] = @TemporalPassword