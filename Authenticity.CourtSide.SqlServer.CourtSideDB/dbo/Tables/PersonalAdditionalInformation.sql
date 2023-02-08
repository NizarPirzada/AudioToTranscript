CREATE TABLE [dbo].[PersonalAdditionalInformation]
(
	[Id]        INT IDENTITY (1, 1) NOT NULL,
	[BarNumber] NVARCHAR (20)	NULL,
	[Title]		NVARCHAR (128)	NOT NULL,
	[Address]	NVARCHAR (128)	NOT NULL,
	[Telephone] NVARCHAR (20)	NOT NULL,
	[LegalFirm] NVARCHAR (128)	NULL,
	CONSTRAINT [PK_PersonalAdditionalInformation] PRIMARY KEY CLUSTERED ([Id] ASC),
)
