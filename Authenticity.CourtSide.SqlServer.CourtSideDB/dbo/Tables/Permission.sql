CREATE TABLE [dbo].[Permission]
(
	[Id]                    INT             IDENTITY (1, 1) NOT NULL,
    [Name]                  NVARCHAR (100)  NOT NULL,
	[IsGenericPermission]   BIT				NOT NULL DEFAULT 1,
    [CreatedBy]			    INT				NOT NULL,
	[CreatedOn]			    DATETIME		NOT NULL,
	[LastModifiedBy]	    INT				NULL,
	[LastModifiedOn]	    DATETIME		NULL,
    CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC)
)
