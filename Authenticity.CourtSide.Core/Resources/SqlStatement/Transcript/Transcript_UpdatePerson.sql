
UPDATE [TranscriptPerson] SET
	[FirstName] = @FirstName,
	[MiddleName] = @MiddleName,
	[LastName] = @LastName,
	[PersonAdditionalInformationId] = @PersonAdditionalInformationId
WHERE [Id] = @Id