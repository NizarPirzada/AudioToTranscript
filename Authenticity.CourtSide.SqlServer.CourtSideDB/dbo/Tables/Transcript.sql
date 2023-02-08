CREATE TABLE [dbo].[Transcript]
(
	[Id]							INT             IDENTITY (1, 1) NOT NULL,
    [Name]							NVARCHAR (100)  NOT NULL,
	[MediaFileSize]					NUMERIC 		NOT NULL,
	[Status]  						TINYINT 		NOT NULL,
	[Step]  						TINYINT 		NOT NULL,
	[TranscriptDateTime]			DATETIME		NULL,
	[OriginalTranscriptResponse]	NVARCHAR(MAX)	NULL,
	[CaseNumber]					NVARCHAR(20)	NULL,
	[JudgeName]						NVARCHAR(128)	NULL,
	[JudgeTitle]					NVARCHAR(128)	NULL,
	[TranscriptType]				TINYINT			NOT NULL,
    [ApiUrl]						NVARCHAR(128)	NULL,
	[DeptNumber]					NVARCHAR(8)		NULL,
	[ApiLanguageId]					TINYINT			NOT NULL,
	[HumanTranscriptionStatus]		TINYINT			NOT NULL,
    [CreatedBy]						INT				NOT NULL,
	[CreatedOn]						DATETIME		NOT NULL,
	[LastModifiedBy]				INT				NULL,
	[LastModifiedOn]				DATETIME		NULL,
    CONSTRAINT [PK_Transcript] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Transcript_To_Language] FOREIGN KEY ([ApiLanguageId]) REFERENCES [Language]([Id])
)
