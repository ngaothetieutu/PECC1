namespace GS.Services.Customers
{
    /// <summary>
    /// Represents default values related to customer services
    /// </summary>
    public static partial class GSCustomerServiceDefaults
    {
        #region Customer attributes

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static string CustomerAttributesAllCacheKey => "GS.customerattribute.all";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : customer attribute ID
        /// </remarks>
        public static string CustomerAttributesByIdCacheKey => "GS.customerattribute.id-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : customer attribute ID
        /// </remarks>
        public static string CustomerAttributeValuesAllCacheKey => "GS.customerattributevalue.all-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : customer attribute value ID
        /// </remarks>
        public static string CustomerAttributeValuesByIdCacheKey => "GS.customerattributevalue.id-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string CustomerAttributesPatternCacheKey => "GS.customerattribute.";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string CustomerAttributeValuesPatternCacheKey => "GS.customerattributevalue.";

        #endregion

        #region Customer roles

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        public static string CustomerRolesAllCacheKey => "GS.customerrole.all-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : system name
        /// </remarks>
        public static string CustomerRolesBySystemNameCacheKey => "GS.customerrole.systemname-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string CustomerRolesPatternCacheKey => "GS.customerrole.";

        #endregion

        /// <summary>
        /// Gets a key for caching current customer password lifetime
        /// </summary>
        /// <remarks>
        /// {0} : customer identifier
        /// </remarks>
        public static string CustomerPasswordLifetimeCacheKey => "GS.customers.passwordlifetime-{0}";

        /// <summary>
        /// Gets a password salt key size
        /// </summary>
        public static int PasswordSaltKeySize => 5;
        
        /// <summary>
        /// Gets a max username length
        /// </summary>
        public static int CustomerUsernameLength => 100;
    }
}