namespace Authenticity.CourtSide.Core
{
    public class Constants
    {
        public const string AUTHENTICATION_SCHEMA = "Token";
        public const string INVALID_TOKEN_MESSAGE = "Invalid token";
        public const string PASSWORD_POLICY_MESSAGE = "Password must have minimum 8 characters and contain at least one lowercase letter, one uppercase letter and one special character or one number";
        public const string PASSWORD_POLICY_REGEX = @"(?=^.{8,}$)((?!.*\s)(?=.*[A-Z])(?=.*[a-z]))((?=(.*\d){1,})|(?=(.*\W){1,}))^.*$";
        public const string PASSWORD_REQUIRED_MESSAGE = "Password is required";
        public const string PASSWORD_DO_NOT_MATCH_MESSAGE = "Passwords do not match";
    }
}