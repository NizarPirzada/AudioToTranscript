UPDATE [TranscriptRecordingInfo] SET
    [TranscriptId] = @TranscriptId, 
    [VenueCompany] = @VenueCompany,
    [StreetAddress] = @StreetAddress,
	[City] = @City,
	[State] = @State,
	[Zipcode] = @Zipcode,
    LastModifiedBy = @LastModifiedBy,
    LastModifiedOn = GETUTCDATE()
WHERE [Id] = @TranscriptRecordingInfoId