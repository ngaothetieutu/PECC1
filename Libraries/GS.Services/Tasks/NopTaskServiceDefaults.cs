namespace GS.Services.Tasks
{
    public static partial class GSTaskServiceDefaults
    {
        #region Task Attributes
        public static string TaskAttributesAllCacheKey => "GS.taskattribute.all";

        public static string TaskAttributesByIdCacheKey => "GS.taskattribute.id-{0}";

        public static string TaskAttributeValuesAllCacheKey => "GS.taskattributevalue.all-{0}";

        public static string TaskAttributeValuesByIdCacheKey => "GS.taskattributevalue.id-{0}";

        public static string TaskAttributesPatternCacheKey => "GS.taskattribute.";

        public static string TaskAttributeValuesPatternCacheKey => "GS.taskattributevalue.";
        #endregion

        #region Task roles
        public static string TaskRolesAllCacheKey => "GS.taskrole.all-{0}";

        public static string TaskRolesBySystemNameCacheKey => "GS.taskrole.systemname-{0}";

        public static string TaskRolesPatternCacheKey => "GS.taskrole.";
        #endregion

        public static string TaskPasswordLifetimeCacheKey => "GS.Task.passwordlifetime-{0}";

        public static int PasswordSaltKeySize => 5;

        public static int TaskUsernameLength => 100;
    }
}
