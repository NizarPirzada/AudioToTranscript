INSERT INTO [PersonalAdditionalInformation] ([BarNumber], [Title], [Address], [Telephone], [LegalFirm])
VALUES (@BarNumber, @Title, @Address, @Telephone, @LegalFirm);
SELECT CAST(SCOPE_IDENTITY() as int)