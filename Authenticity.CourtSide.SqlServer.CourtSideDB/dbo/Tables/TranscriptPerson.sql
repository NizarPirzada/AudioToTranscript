CREATE TABLE [dbo].[TranscriptPerson]
(
	[Id]                    INT             IDENTITY (1, 1) NOT NULL,
    [FirstName]             NVARCHAR (128)  NOT NULL,
	[LastName]				NVARCHAR (128)	NOT NULL,
	[MiddleName]			NVARCHAR (128)	NOT NULL,
	[TypeId]  				INT 			NOT NULL,
	[TranscriptId]			INT 			NOT NULL,
    [CreatedBy]			    INT				NOT NULL,
	[CreatedOn]			    DATETIME		NOT NULL,
    CONSTRAINT [PK_TranscriptPerson] PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_TranscriptPerson_PersonType] FOREIGN KEY ([TypeId]) REFERENCES [PersonType]([Id]),
	CONSTRAINT [FK_TranscriptPerson_Transcript] FOREIGN KEY ([TranscriptId]) REFERENCES [Transcript]([Id])
)
