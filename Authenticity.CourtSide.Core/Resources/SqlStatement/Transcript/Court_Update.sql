UPDATE [Court] SET
    [TranscriptId] = @TranscriptId, 
    [Name] = @Name,
    [Address] = @Address,
	[City] = @City,
	[State] = @State,
	[Zipcode] = @Zipcode,
    LastModifiedBy = @LastModifiedBy,
    LastModifiedOn = GETUTCDATE()
WHERE [Id] = @CourtId