using GS.Core;
using GS.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Services.Contracts
{
    public interface IConstructionService
    {
        #region Construction

        /// <summary>
        /// Delete the review type
        /// </summary>
        /// <param name="item">Construction type</param>
        void DeleteConstruction( Construction item);

        /// <summary>
        /// Get all review types
        /// </summary>
        /// <returns>Construction types</returns>
        IList< Construction> GetAllConstructions();

        Decimal GetTotalConstruction(int year = 0);

        IPagedList<Construction> GetContructionByName(int TypeId = 0, string Name = "", int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Get the review type 
        /// </summary>
        /// <param name="itemId">Construction type identifier</param>
        /// <returns>Construction type</returns>
        Construction GetConstructionById(int itemId);

        /// <summary>
        /// Insert the review type
        /// </summary>
        /// <param name="item">Construction type</param>
        void InsertConstruction( Construction item);

        /// <summary>
        /// Update the review type
        /// </summary>
        /// <param name="item">Construction type</param>
        void UpdateConstruction( Construction item);

        #endregion
    }
}
