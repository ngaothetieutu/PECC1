using GS.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Services.Contracts
{
    public interface IConstructionTypeService
    {
        #region ConstructionType

        /// <summary>
        /// Delete the review type
        /// </summary>
        /// <param name="item">Construction type</param>
        void DeleteConstructionType(ConstructionType item);

        /// <summary>
        /// Get all review types
        /// </summary>
        /// <returns>Construction types</returns>
        IList<ConstructionType> GetAllConstructionTypes();

        IList<ConstructionType> GetConstructionTypeByName(string Name = "");

        /// <summary>
        /// Get the review type 
        /// </summary>
        /// <param name="itemId">Construction type identifier</param>
        /// <returns>Construction type</returns>
        ConstructionType GetConstructionTypeById(int itemId);

        /// <summary>
        /// Insert the review type
        /// </summary>
        /// <param name="item">Construction type</param>
        void InsertConstructionType(ConstructionType item);

        /// <summary>
        /// Update the review type
        /// </summary>
        /// <param name="item">Construction type</param>
        void UpdateConstructionType(ConstructionType item);

        #endregion
    }
}
