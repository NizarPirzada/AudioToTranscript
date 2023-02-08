CREATE TABLE [dbo].[FileExportFormat]
(
	[Id]                INT				IDENTITY (1, 1) NOT NULL,
	[Path]				NVARCHAR(265)	NOT NULL,
	[FileName]			NVARCHAR(265)	NOT NULL,
	[FormatName]		NVARCHAR(128)	NOT NULL,
	[Type]				NVARCHAR(8)		NOT NULL,
	[CreatedBy]			INT				NOT NULL,
	[CreatedOn]			DATETIME		NOT NULL,
	[LastModifiedBy]	INT				NULL,
	[LastModifiedOn]	DATETIME		NULL,
    CONSTRAINT [PK_FileExportFormat] PRIMARY KEY CLUSTERED ([Id] ASC)
)
