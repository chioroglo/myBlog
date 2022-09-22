namespace Domain.Validation
{
    public static class EntityConfigurationConstants
    {

        // todo rename constants according to convention
        // https://stackoverflow.com/questions/242534/c-sharp-naming-convention-for-constants
        #region User entity section
        public const int USER_USERNAME_MIN_LENGTH = 3;
        public const int USER_USERNAME_MAX_LENGTH = 20;
        public const int USER_FIRSTNAME_LASTNAME_MIN_LENGTH = 2;
        public const int USER_FIRSTNAME_LASTNAME_MAX_LENGTH = 20;
        public const int USER_PASSWORD_MIN_LENGTH = 3;
        public const int USER_PASSWORD_MAX_LENGTH = 20;
        #endregion

        #region Post entity section
        public const int POST_MAX_LENGTH = 10000;
        public const int POST_TITLE_MAX_LENGTH = 30;
        #endregion

        #region Comment entity section
        public const int COMMENT_MAX_LENGTH = 1000;
        #endregion

        #region Avatar entity section
        public const double MIN_AVATAR_WIDTH_PX = 128d;
        public const double MAX_AVATAR_WIDTH_PX = 320d;

        public const double MIN_AVATAR_HEIGHT_PX = 128d;
        public const double MAX_AVATAR_HEIGHT_PX = 320d;

        public const int MAX_AVATAR_SIZE_BYTES = 1024 * 5 * 1024; // 5MB
        #endregion

        public const string GETUTCDATE_SQL_EXPRESSION = "GETUTCDATE()";
    }
}
