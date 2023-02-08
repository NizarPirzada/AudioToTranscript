UPDATE [User_Role]
SET [RoleId] = @RoleId, [LastModifiedBy] = @LastModifiedBy, [LastModifiedOn] = GETUTCDATE()
WHERE [UserId] = @UserId