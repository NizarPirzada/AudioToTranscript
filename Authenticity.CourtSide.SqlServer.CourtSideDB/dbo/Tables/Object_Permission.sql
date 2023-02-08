CREATE TABLE [dbo].[Object_Permission]
(
	[Id]			INT	IDENTITY(1,1) NOT NULL,
	[ObjectId]		INT NOT NULL,
	[PermissionId]	INT NOT NULL,
	[CreatedBy]		INT NOT NULL,
	[CreatedOn]		DATETIME NOT NULL,
	CONSTRAINT [PK_Object_Permission] PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_Object_Permission_ToObject] FOREIGN KEY ([ObjectId]) REFERENCES [Object]([Id]),
	CONSTRAINT [FK_Object_Permission_ToPermission] FOREIGN KEY ([PermissionId]) REFERENCES [Permission]([Id]) 
)
