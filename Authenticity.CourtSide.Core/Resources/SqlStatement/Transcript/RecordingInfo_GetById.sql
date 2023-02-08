SELECT
    [Id],
    [TranscriptId], 
    [VenueCompany],
    [StreetAddress],
	[City],
	[State],
	[Zipcode],
    CreatedBy,
    CreatedOn,
    LastModifiedBy,
    LastModifiedOn
FROM [dbo].[TranscriptRecordingInfo] WITH(NOLOCK)
WHERE [Id] = @Id;