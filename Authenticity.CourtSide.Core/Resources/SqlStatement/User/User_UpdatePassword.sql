UPDATE [User]
SET [Password] = @Password, [LastModifiedOn] = GETUTCDATE(), [LastModifiedBy] = @LastModifiedBy
WHERE Id = @Id