namespace Domain.Validation
{
    public static class EntityConfigurationConstants
    {
        public const int USER_USERNAME_MIN_LENGTH = 3;
        public const int USER_USERNAME_MAX_LENGTH = 20;
        public const int USER_FIRSTNAME_LASTNAME_MIN_LENGTH = 2;
        public const int USER_FIRSTNAME_LASTNAME_MAX_LENGTH = 20;
        public const int USER_PASSWORD_MIN_LENGTH = 3;
        public const int USER_PASSWORD_MAX_LENGTH = 20;

        public const int POST_MAX_LENGTH = 10000;
        public const int POST_TITLE_MAX_LENGTH = 30;

        public const int COMMENT_MAX_LENGTH = 1000;


        public const string GETUTCDATE = "GETUTCDATE()";
    }
}
