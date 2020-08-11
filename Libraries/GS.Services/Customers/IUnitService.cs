using GS.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Services.Customers
{
    public interface IUnitService
    {
        #region Unit

        /// <summary>
        /// Delete the review type
        /// </summary>
        /// <param name="item">Unit</param>
        bool DeleteUnit(Unit item);

        /// <summary>
        /// Get all review types
        /// </summary>
        /// <returns>Units</returns>
        IList<Unit> GetAllUnits(string Name=null, int? ParentId=null);

        /// <summary>
        /// Get the review type 
        /// </summary>
        /// <param name="itemId">Unit identifier</param>
        /// <returns>Unit</returns>
        Unit GetUnitById(int itemId);

        /// <summary>
        /// Insert the review type
        /// </summary>
        /// <param name="item">Unit</param>
        bool InsertUnit(Unit item);

        /// <summary>
        /// Update the review type
        /// </summary>
        /// <param name="item">Unit</param>
        bool UpdateUnit(Unit item);

        Decimal GetTotalUnit();

        #endregion
        #region Add Customer_Unit_Mapping

        IList<Customer> GetAllCustomerByUnitId(int UnitId);

        bool insertCustomerUnit(int UnitId, int CustomerId);

        bool deleteCustomerUnit(int UnitId, int CustomerId);

        #endregion
    }
}
