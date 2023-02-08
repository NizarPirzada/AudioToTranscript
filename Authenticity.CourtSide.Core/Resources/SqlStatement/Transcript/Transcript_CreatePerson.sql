INSERT INTO [TranscriptPerson] ([FirstName], [MiddleName], [LastName], [TypeId], [TranscriptId], [CreatedBy], [CreatedOn])
VALUES (@FirstName, @MiddleName, @LastName, @Type, @TranscriptId, @CreatedBy, GETUTCDATE());
SELECT CAST(SCOPE_IDENTITY() as int)