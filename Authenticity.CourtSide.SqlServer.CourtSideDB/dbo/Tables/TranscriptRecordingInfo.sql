CREATE TABLE [dbo].[TranscriptRecordingInfo]
(
	[Id]                INT				IDENTITY (1, 1) NOT NULL,
	[TranscriptId]		INT				NOT NULL,
	[VenueCompany]		NVARCHAR (128)	NULL,
	[StreetAddress]		NVARCHAR (128)	NULL,
	[City]				NVARCHAR (128)	NULL,
	[State]				NVARCHAR (128)	NULL,
	[Zipcode]			NVARCHAR (20)	NULL,
	[CreatedBy]			INT				NOT NULL,
	[CreatedOn]			DATETIME		NOT NULL,
	[LastModifiedBy]	INT				NULL,
	[LastModifiedOn]	DATETIME		NULL,
    CONSTRAINT [PK_TranscriptRecordingInfo] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_TranscriptRecordingInfo_ToTranscript] FOREIGN KEY ([TranscriptId]) REFERENCES [Transcript]([Id]),
	UNIQUE NONCLUSTERED ([TranscriptId] ASC)
)
