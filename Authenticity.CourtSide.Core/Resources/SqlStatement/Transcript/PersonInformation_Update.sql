
UPDATE [PersonalAdditionalInformation] SET
    [BarNumber] = @BarNumber, 
    [Title] = @Title,
    [Address] = @Address,	
	[Telephone] = @Telephone,
    [LegalFirm] = @LegalFirm
WHERE [Id] = @PersonAdditionalInformationId