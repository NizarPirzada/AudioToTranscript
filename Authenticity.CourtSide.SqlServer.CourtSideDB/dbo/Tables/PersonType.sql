CREATE TABLE [dbo].[PersonType]
(
	[Id]                    INT             IDENTITY (1, 1) NOT NULL,
    [Name]					NVARCHAR (64)   NOT NULL,
	[CreatedBy]			    INT				NOT NULL,
	[CreatedOn]			    DATETIME		NOT NULL,
    CONSTRAINT [PK_PersonType] PRIMARY KEY CLUSTERED ([Id] ASC)
)
