DELETE FROM dbo.[UserLoginHistory] WHERE [UserId] = @UserId;
DELETE FROM dbo.[User_Role] where [UserId] = @UserId;
DELETE FROM dbo.[User] where [Id] = @UserId;

SELECT @@ROWCOUNT;