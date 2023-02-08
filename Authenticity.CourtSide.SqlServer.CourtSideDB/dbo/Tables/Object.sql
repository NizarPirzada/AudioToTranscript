CREATE TABLE [dbo].[Object]
(
	[Id]                    INT             IDENTITY (1, 1) NOT NULL,
    [Name]                  NVARCHAR (100)  NOT NULL,
    [CreatedBy]			    INT				NOT NULL,
	[CreatedOn]			    DATETIME		NOT NULL,
	[LastModifiedBy]	    INT				NULL,
	[LastModifiedOn]	    DATETIME		NULL,
    CONSTRAINT [PK_Object] PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC)
)
