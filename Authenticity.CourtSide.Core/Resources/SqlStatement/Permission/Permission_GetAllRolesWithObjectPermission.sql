SELECT
    R.Id as RoleId,
    R.Name,
    OP.Id AS ObjectPermissionId, 
    P.Id AS PermissionId,
	P.Name AS PermissionName,
    O.Id AS ObjectId,
    O.Name AS ObjectName
FROM [dbo].[Role] AS R WITH(NOLOCK)
JOIN [dbo].[Role_ObjectPermission] AS ROP WITH(NOLOCK) ON R.Id = ROP.RoleId
JOIN [dbo].[Object_Permission] AS OP WITH(NOLOCK) ON ROP.ObjectPermissionId = OP.Id
JOIN [dbo].[Object] AS O WITH(NOLOCK) ON OP.ObjectId = O.Id
JOIN [dbo].[Permission] AS P WITH(NOLOCK) ON OP.PermissionId = P.Id