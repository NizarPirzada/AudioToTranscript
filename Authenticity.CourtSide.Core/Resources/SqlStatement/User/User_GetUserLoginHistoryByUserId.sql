
SELECT TOP(1) [Id], [UserId], [LoginTime]
FROM [UserLoginHistory] WITH (NOLOCK)
WHERE [UserId] = @UserId
ORDER BY [LoginTime] DESC