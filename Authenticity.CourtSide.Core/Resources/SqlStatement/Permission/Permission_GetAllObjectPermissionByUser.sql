SELECT 
    O.Id, 
    O.Name, 
    P.Id AS PermissionId,
    P.Name
FROM [dbo].[Role] AS R WITH(NOLOCK)
JOIN [dbo].[User_Role] AS UR WITH(NOLOCK) ON R.Id = UR.RoleId
JOIN [dbo].[Role_ObjectPermission] AS ROP WITH(NOLOCK) ON R.Id = ROP.RoleId
JOIN [dbo].[Object_Permission] AS OP WITH(NOLOCK) ON ROP.ObjectPermissionId = OP.Id
JOIN [dbo].[Object] AS O WITH(NOLOCK) ON OP.ObjectId = O.Id
JOIN [dbo].[Permission] AS P WITH(NOLOCK) ON OP.PermissionId = P.Id
WHERE UR.UserId =@UserId;