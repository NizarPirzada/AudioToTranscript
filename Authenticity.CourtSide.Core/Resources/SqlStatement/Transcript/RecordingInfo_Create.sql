INSERT INTO [TranscriptRecordingInfo] ([TranscriptId], [VenueCompany], [StreetAddress], [City], [State], [Zipcode], [CreatedBy], [CreatedOn])
VALUES (@TranscriptId, @VenueCompany, @StreetAddress, @City, @State, @Zipcode, @CreatedBy, GETUTCDATE());
SELECT CAST(SCOPE_IDENTITY() as int)