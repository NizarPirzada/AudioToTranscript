CREATE TABLE [dbo].[Language]
(
	[Id]				TINYINT             IDENTITY (1, 1) NOT NULL,
	[LanguageType]		TINYINT			NOT NULL,
	[Name]				NVARCHAR(20)	NOT NULL,
	[ApiCode]			NVARCHAR(5)		NOT NULL,
	[GenericCode]		NVARCHAR(5)		NOT NULL,
	[CreatedBy]			INT				NOT NULL,
	[CreatedOn]			DATETIME		NOT NULL,
	[LastModifiedBy]	INT				NULL,
	[LastModifiedOn]	DATETIME		NULL,
	CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED ([Id] ASC)
)