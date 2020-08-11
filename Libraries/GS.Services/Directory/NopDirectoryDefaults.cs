namespace GS.Services.Directory
{
    /// <summary>
    /// Represents default values related to directory services
    /// </summary>
    public static partial class GSDirectoryDefaults
    {
        #region Countries

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// {1} : show hidden records?
        /// </remarks>
        public static string CountriesAllCacheKey => "GS.country.all-{0}-{1}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string CountriesPatternCacheKey => "GS.country.";

        #endregion

        #region Currencies

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : currency ID
        /// </remarks>
        public static string CurrenciesByIdCacheKey => "GS.currency.id-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        public static string CurrenciesAllCacheKey => "GS.currency.all-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string CurrenciesPatternCacheKey => "GS.currency.";

        #endregion

        #region Measures

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static string MeasureDimensionsAllCacheKey => "GS.measuredimension.all";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : dimension ID
        /// </remarks>
        public static string MeasureDimensionsByIdCacheKey => "GS.measuredimension.id-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static string MeasureWeightsAllCacheKey => "GS.measureweight.all";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : weight ID
        /// </remarks>
        public static string MeasureWeightsByIdCacheKey => "GS.measureweight.id-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string MeasureDimensionsPatternCacheKey => "GS.measuredimension.";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string MeasureWeightsPatternCacheKey => "GS.measureweight.";

        #endregion

        #region States and provinces

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : country ID
        /// {1} : language ID
        /// {2} : show hidden records?
        /// </remarks>
        public static string StateProvincesAllCacheKey => "GS.stateprovince.all-{0}-{1}-{2}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string StateProvincesPatternCacheKey => "GS.stateprovince.";

        #endregion
    }
}