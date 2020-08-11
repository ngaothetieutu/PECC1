using GS.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Services.Contracts
{
    public interface IConstructionCapitalService
    {
        #region ConstructionCapital

        /// <summary>
        /// Delete the review type
        /// </summary>
        /// <param name="item">Construction type</param>
        void DeleteConstructionCapital(ConstructionCapital item);

        /// <summary>
        /// Get all review types
        /// </summary>
        /// <returns>Construction types</returns>
        IList<ConstructionCapital> GetAllConstructionCapital();

        IList<ConstructionCapital> GetConstructionCapitalsByName(string Name);

        /// <summary>
        /// Get the review type 
        /// </summary>
        /// <param name="itemId">Construction type identifier</param>
        /// <returns>Construction type</returns>
        ConstructionCapital GetConstructionCapitalById(int itemId);

        /// <summary>
        /// Insert the review type
        /// </summary>
        /// <param name="item">Construction type</param>
        void InsertConstructionCapital(ConstructionCapital item);

        /// <summary>
        /// Update the review type
        /// </summary>
        /// <param name="item">Construction type</param>
        void UpdateConstructionCapital(ConstructionCapital item);

        #endregion
    }
}
