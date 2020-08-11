namespace GS.Services.Topics
{
    /// <summary>
    /// Represents default values related to topic services
    /// </summary>
    public static partial class GSTopicDefaults
    {
        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// {1} : ignore ACL?
        /// {2} : show hidden?
        /// </remarks>
        public static string TopicsAllCacheKey => "GS.topics.all-{0}-{1}-{2}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : topic ID
        /// </remarks>
        public static string TopicsByIdCacheKey => "GS.topics.id-{0}";

        /// <summary>
        /// Gets a pattern to clear cache
        /// </summary>
        public static string TopicsPatternCacheKey => "GS.topics.";
    }
}