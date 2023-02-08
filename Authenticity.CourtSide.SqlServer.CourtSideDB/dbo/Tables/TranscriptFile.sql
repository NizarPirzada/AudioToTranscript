CREATE TABLE [dbo].[TranscriptFile]
(
	[Id]                    INT             IDENTITY (1, 1) NOT NULL,
	[TranscriptId]			INT 			NOT NULL,
    [Name]					NVARCHAR (128)  NOT NULL,
	[Path]			    	NVARCHAR (256)	NOT NULL,
	[CreatedOn]			    DATETIME		NOT NULL,
    CONSTRAINT [PK_TranscriptFile] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_TranscriptFile_Transcript] FOREIGN KEY ([TranscriptId]) REFERENCES [Transcript]([Id])
);
