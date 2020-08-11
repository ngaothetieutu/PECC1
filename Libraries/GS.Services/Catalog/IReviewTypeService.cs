using System.Collections.Generic;
using GS.Core.Domain.Catalog;

namespace GS.Services.Catalog
{
    /// <summary>
    /// Review type service interface
    /// </summary>
    public partial interface IReviewTypeService
    {
        #region ReviewType

        /// <summary>
        /// Delete the review type
        /// </summary>
        /// <param name="reviewType">Review type</param>
        void DeleteReiewType(ReviewType reviewType);

        /// <summary>
        /// Get all review types
        /// </summary>
        /// <returns>Review types</returns>
        IList<ReviewType> GetAllReviewTypes();

        /// <summary>
        /// Get the review type 
        /// </summary>
        /// <param name="reviewTypeId">Review type identifier</param>
        /// <returns>Review type</returns>
        ReviewType GetReviewTypeById(int reviewTypeId);

        /// <summary>
        /// Insert the review type
        /// </summary>
        /// <param name="reviewType">Review type</param>
        void InsertReviewType(ReviewType reviewType);

        /// <summary>
        /// Update the review type
        /// </summary>
        /// <param name="reviewType">Review type</param>
        void UpdateReviewType(ReviewType reviewType);

        #endregion

        
    }
}
