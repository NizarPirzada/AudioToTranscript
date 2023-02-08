INSERT INTO [Court] ([TranscriptId], [Name], [Address], [City], [State], [Zipcode], [CreatedBy], [CreatedOn])
VALUES (@TranscriptId, @Name, @Address, @City, @State, @Zipcode, @CreatedBy, GETUTCDATE());
SELECT CAST(SCOPE_IDENTITY() as int)