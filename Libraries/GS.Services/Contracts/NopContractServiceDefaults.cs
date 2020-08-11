namespace GS.Services.Contracts
{
    public static partial class GSContractServiceDefaults
    {
        #region Customer attributes
        public static string ContractAttributesAllCacheKey => "GS.contractattribute.all";

        public static string ContractAttributesByIdCacheKey => "GS.contractattribute.id-{0}";

        public static string ContractAttributeValuesAllCacheKey => "GS.contractattributevalue.all-{0}";

        public static string ContractAttributeValuesByIdCacheKey => "GS.contractattributevalue.id-{0}";

        public static string ContractAttributesPatternCacheKey => "GS.contractattribute.";

        public static string ContractAttributeValuesPatternCacheKey => "GS.contractattributevalue.";
        #endregion

        #region Customer roles

        public static string ContractRolesAllCacheKey => "GS.contractrole.all-{0}";

        public static string ContractRolesBySystemNameCacheKey => "GS.contractrole.systemname-{0}";

        public static string ContractRolesPatternCacheKey => "GS.contractrole.";

        #endregion

        public static string ContractPasswordLifetimeCacheKey => "GS.Contract.passwordlifetime-{0}";

        public static int PasswordSaltKeySize => 5;

        public static int ContractUsernameLength => 100;
    }
}
