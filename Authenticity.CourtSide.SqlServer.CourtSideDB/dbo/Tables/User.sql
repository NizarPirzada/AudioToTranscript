CREATE TABLE [dbo].[User]
(
	[Id]						INT				IDENTITY (1, 1) NOT NULL,
	[FirstName]					NVARCHAR(100)	NOT NULL,
	[LastName]					NVARCHAR(100)	NOT NULL,
	[Email]						NVARCHAR(100)	NOT NULL,
	[Password]					NVARCHAR(450)	NULL,
	[Status]					TINYINT			NOT NULL,
	EmailActivationId			VARCHAR(100)	NULL,
	[ApiUrl]					NVARCHAR(128)	NULL,
	[ApiGuid]					NVARCHAR(10)	NULL,
	[TemporalPassword]			NVARCHAR(16)	NULL,
	[TranscriptionEngineId]		TINYINT			NULL,
	[CreatedBy]					INT				NOT NULL,
	[CreatedOn]					DATETIME		NOT NULL,
	[LastModifiedBy]			INT				NULL,
	[LastModifiedOn]			DATETIME		NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC),
	UNIQUE NONCLUSTERED ([Email] ASC),
	CONSTRAINT [FK_User_To_TranscriptionEngine] FOREIGN KEY ([TranscriptionEngineId]) REFERENCES [TranscriptionEngine]([Id])
)
