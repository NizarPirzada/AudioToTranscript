CREATE TABLE [dbo].[Transcript_Dialog]
(
	[Id]					INT IDENTITY (1, 1)	NOT NULL,
	[TranscriptId]			INT					NOT NULL,
	[OriginalSpeakerId]		INT					NOT NULL,
	[OriginalSpeakerName]	NVARCHAR (32)		NULL,
	[PersonId]				INT					NULL,
	[StartTime]				NUMERIC(10, 2)		NOT NULL,
	[Duration]				NUMERIC(10, 2)		NOT NULL,
	[Transcription]			NVARCHAR(MAX)		NOT NULL,
	[ExaminationType]		TINYINT				NOT NULL DEFAULT 0,
	[ExaminationTag]	TINYINT				NOT NULL DEFAULT 0,
	CONSTRAINT [PK_Transcript_Dialog] PRIMARY KEY CLUSTERED ([Id] ASC), 
	CONSTRAINT [FK_Transcript_Dialog_ToTranscript] FOREIGN KEY ([TranscriptId]) REFERENCES [Transcript]([Id])
)