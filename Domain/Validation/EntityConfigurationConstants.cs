namespace Domain.Validation
{
    public static class EntityConfigurationConstants
    {

        // todo rename constants according to convention
        // https://stackoverflow.com/questions/242534/c-sharp-naming-convention-for-constants
        #region User entity section
        public const int UsernameMinLength = 3;
        public const int UsernameMaxLength = 20;
        public const int UserFirstAndLastNameMinLength = 2;
        public const int UserFirstAndLastNameMaxLength = 20;
        public const int UserPasswordMinLength = 3;
        public const int UserPasswordMaxLength = 20;
        #endregion

        #region Post entity section
        public const int PostMaxLength = 10000;
        public const int PostTitleMaxLength = 30;
        #endregion

        #region Comment entity section
        public const int CommentMaxLength = 1000;
        #endregion

        #region Avatar entity section
        public const double MinAvatarWidthPx = 128d;
        public const double MaxAvatarWidthPx = 320d;

        public const double MinAvatarHeightPx = 128d;
        public const double MaxAvatarHeightPx = 320d;

        public const int MaxAvatarSizeBytes = 1024 * 5 * 1024; // 5MB
        #endregion

        public const string GetutcdateSqlExpression = "GETUTCDATE()";
    }
}
