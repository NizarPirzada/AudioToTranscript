UPDATE [User]
SET [Status] = @Status, [LastModifiedOn] = GETUTCDATE(), [LastModifiedBy] = @LastModifiedBy
WHERE Id = @Id