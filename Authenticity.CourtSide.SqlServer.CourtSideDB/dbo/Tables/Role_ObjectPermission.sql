CREATE TABLE [dbo].[Role_ObjectPermission]
(
	[RoleId]				INT NOT NULL,
	[ObjectPermissionId]	INT NOT NULL,
	[CreatedBy]				INT NOT NULL,
	[CreatedOn]				DATETIME NOT NULL,
	CONSTRAINT [FK_Role_ObjectPermission_ToRole] FOREIGN KEY ([RoleId]) REFERENCES [Role]([Id]),
	CONSTRAINT [FK_Role_ObjectPermission_ToObjectPermission] FOREIGN KEY ([ObjectPermissionId]) REFERENCES [Object_Permission]([Id])
)
