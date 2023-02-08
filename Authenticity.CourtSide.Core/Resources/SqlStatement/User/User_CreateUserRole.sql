INSERT INTO [User_Role] ([UserId], [RoleId], [CreatedBy], [CreatedOn]) 
VALUES (@UserId, @RoleId, @CreatedBy, GETUTCDATE());