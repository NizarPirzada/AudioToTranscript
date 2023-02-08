SELECT
    U.Id
   ,U.FirstName
   ,U.LastName
   ,U.Email
   ,U.Password
   ,U.Status
   ,U.EmailActivationId
   ,U.ApiUrl
   ,U.ApiGuid
   ,U.TranscriptionEngineId
   ,U.CreatedBy
   ,U.CreatedOn
   ,U.LastModifiedBy
   ,U.LastModifiedOn
   ,R.Id RoleId
   ,R.Name
FROM [User] U WITH (NOLOCK)
JOIN User_Role UR
    ON U.Id = UR.UserId
JOIN [Role] R
    ON UR.RoleId = R.Id
WHERE U.Id = @UserId;