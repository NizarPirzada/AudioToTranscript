CREATE TABLE [dbo].[FileProvider]
(
	[Id]                    INT             IDENTITY(1,1) NOT NULL,
    [Name]                  NVARCHAR(50)    NOT NULL,
    [Parameters]            TEXT            NOT NULL,
    [IsCurrentProvider]     BIT             NOT NULL,
    [CreatedBy]             INT             NOT NULL,
	[CreatedOn]             DATETIME        NOT NULL,
	[LastModifiedBy]        INT             NULL,
	[LastModifiedOn]        DATETIME        NULL,
    CONSTRAINT [PK_FileProvider] PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC),
)
