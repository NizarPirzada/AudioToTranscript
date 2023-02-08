CREATE TABLE [dbo].[TranscriptJob]
(
	[Id]                    INT             IDENTITY (1, 1) NOT NULL,
	[TranscriptId]			INT 			NOT NULL,
	[Guid] 		            NVARCHAR (128)  NOT NULL,
	[Status]    	        NVARCHAR (32)  	NOT NULL,
	[ApiUrl]				NVARCHAR(128)	NULL,
	[TranscriptionEngine]	NVARCHAR(10)	NULL,
	[Message]				NVARCHAR(256)	NULL,
	[StartSendingToApiOn]	DATETIME		NULL,
	[SentToApiOn]			DATETIME		NULL,
	[CompletedOn]			DATETIME		NULL,
    [CreatedBy]			    INT				NOT NULL,
	[CreatedOn]			    DATETIME		NOT NULL,
    CONSTRAINT [PK_TranscriptJob] PRIMARY KEY CLUSTERED ([Id] ASC), 
	CONSTRAINT [FK_TranscriptJob_Transcript] FOREIGN KEY ([TranscriptId]) REFERENCES [Transcript]([Id])
)
