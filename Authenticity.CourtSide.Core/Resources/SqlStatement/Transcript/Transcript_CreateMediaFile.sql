INSERT INTO [TranscriptFile] ([TranscriptId], [Name], [Path], [CreatedOn])
VALUES (@TranscriptId, @Name, @Path, GETUTCDATE());
SELECT CAST(SCOPE_IDENTITY() as int)