namespace Common.Validation
{
    public static class EntityConfigurationConstants
    {
        #region User entity section

        public const int UsernameMinLength = 3;
        public const int UsernameMaxLength = 20;
        public const int UserFirstAndLastNameMinLength = 2;
        public const int UserFirstAndLastNameMaxLength = 20;
        public const int UserPasswordMinLength = 6;
        public const int UserPasswordMaxLength = 20;
        public const int HashedPasswordLengthSha256 = 64;

        public const string UserFirstnameAndLastnameRegEx = @"([A-Z][a-z]*)";

        // explanation of this regexp
        // https://stackoverflow.com/questions/12018245/regular-expression-to-validate-username
        public const string UsernameRegEx = @"^(?=.{3,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$";

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
        public const double MaxAvatarWidthPx = 512d;

        public const double MinAvatarHeightPx = 128d;
        public const double MaxAvatarHeightPx = 512d;

        public const int MaxAvatarSizeBytes = 1024 * 100; // 100KB

        #endregion

        #region Topic entity section

        public const int MaxTopicNameLength = 20;

        #endregion

        #region SQL Expressions

        public const string GetutcdateSqlExpression = "GETUTCDATE()";

        #endregion
    }
}