INSERT INTO [Transcript] ([Name], [MediaFileSize], [Status], [Step], [CreatedBy], [CreatedOn])
VALUES (@Name, 0, 0, 1, @CreatedBy, GETUTCDATE());
SELECT CAST(SCOPE_IDENTITY() as int)