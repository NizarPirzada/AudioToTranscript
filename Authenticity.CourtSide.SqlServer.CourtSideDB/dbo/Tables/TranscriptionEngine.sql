CREATE TABLE [dbo].[TranscriptionEngine]
(
	[Id]	TINYINT IDENTITY (1, 1) NOT NULL,
	[Name]	NVARCHAR(20) NOT NULL,
	[Code]	NVARCHAR(7) NOT NULL,
	[CreatedBy]			INT				NOT NULL,
	[CreatedOn]			DATETIME		NOT NULL,
	[LastModifiedBy]	INT				NULL,
	[LastModifiedOn]	DATETIME		NULL,
    CONSTRAINT [PK_Transcription_Engine] PRIMARY KEY CLUSTERED ([Id] ASC),
)
