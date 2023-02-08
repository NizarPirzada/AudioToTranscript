CREATE TABLE [dbo].[User_Role]
(
	[UserId]			INT NOT NULL,
	[RoleId]			INT NOT NULL,
	[CreatedBy]			INT NOT NULL,
	[CreatedOn]			DATETIME NOT NULL,
	[LastModifiedBy]	INT NULL,
	[LastModifiedOn]	DATETIME NULL,
	CONSTRAINT [FK_User_Role_ToUser] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]),
	CONSTRAINT [FK_User_Role_ToRole] FOREIGN KEY ([RoleId]) REFERENCES [Role]([Id]) 
)
