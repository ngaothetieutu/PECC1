namespace GS.Services.Security
{
    /// <summary>
    /// Represents default values related to security services
    /// </summary>
    public static partial class GSSecurityDefaults
    {
        #region Access control list

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity ID
        /// {1} : entity name
        /// </remarks>
        public static string AclRecordByEntityIdNameCacheKey => "GS.aclrecord.entityid-name-{0}-{1}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string AclRecordPatternCacheKey => "GS.aclrecord.";

        #endregion

        #region Permissions

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : customer role ID
        /// {1} : permission system name
        /// </remarks>
        public static string PermissionsAllowedCacheKey => "GS.permission.allowed-{0}-{1}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : customer role ID
        /// </remarks>
        public static string PermissionsAllByCustomerRoleIdCacheKey => "GS.permission.allbycustomerroleid-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string PermissionsPatternCacheKey => "GS.permission.";

        #endregion
    }
}