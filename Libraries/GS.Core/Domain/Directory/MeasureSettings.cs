﻿using GS.Core.Configuration;

namespace GS.Core.Domain.Directory
{
    /// <summary>
    /// Measure settings
    /// </summary>
    public class MeasureSettings : ISettings
    {
        /// <summary>
        /// Base dimension identifier
        /// </summary>
        public int BaseDimensionId { get; set; }

        /// <summary>
        /// Base weight identifier
        /// </summary>
        public int BaseWeightId { get; set; }
    }
}