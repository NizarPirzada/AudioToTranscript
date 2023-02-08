CREATE TABLE [dbo].[Court]
(
	[Id]                INT IDENTITY (1, 1) NOT NULL,
	[TranscriptId]		INT				NOT NULL,
	[Name]				NVARCHAR (128)	NULL,
	[Address]			NVARCHAR (128)	NULL,
	[City]				NVARCHAR (128)	NULL,
	[State]				NVARCHAR (128)	NULL,
	[Zipcode]			NVARCHAR (20)	NULL,
	[CreatedBy]			INT				NOT NULL,
	[CreatedOn]			DATETIME		NOT NULL,
	[LastModifiedBy]	INT				NULL,
	[LastModifiedOn]	DATETIME		NULL,
    CONSTRAINT [PK_Court] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Court_ToTranscript] FOREIGN KEY ([TranscriptId]) REFERENCES [Transcript]([Id])
)