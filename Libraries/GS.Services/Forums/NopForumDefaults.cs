namespace GS.Services.Forums
{
    /// <summary>
    /// Represents default values related to forums services
    /// </summary>
    public static partial class GSForumDefaults
    {
        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static string ForumGroupAllCacheKey => "GS.forumgroup.all";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : forum group ID
        /// </remarks>
        public static string ForumAllByForumGroupIdCacheKey => "GS.forum.allbyforumgroupid-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ForumGroupPatternCacheKey => "GS.forumgroup.";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ForumPatternCacheKey => "GS.forum.";
    }
}