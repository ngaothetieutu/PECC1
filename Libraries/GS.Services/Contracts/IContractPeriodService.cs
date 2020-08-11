using GS.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Services.Contracts
{
    public interface IContractPeriodService
    {
        #region Contract Period

        /// <summary>
        /// Delete the review type
        /// </summary>
        /// <param name="item">Contract Period</param>
        void DeleteContractPeriod(ContractPeriod item);

        /// <summary>
        /// Get all review types
        /// </summary>
        /// <returns>Contract Periods</returns>
        IList<ContractPeriod> GetAllContractPeriods();
        IList<ContractPeriod> GetAllContractPeriodsByName(string Name="");

        /// <summary>
        /// Get the review type 
        /// </summary>
        /// <param name="itemId">Contract Period identifier</param>
        /// <returns>Contract Period</returns>
        ContractPeriod GetContractPeriodById(int itemId);

        /// <summary>
        /// Insert the review type
        /// </summary>
        /// <param name="item">Contract Form</param>
        void InsertContractPeriod(ContractPeriod item);

        /// <summary>
        /// Update the review type
        /// </summary>
        /// <param name="item">Contract Period</param>
        void UpdateContractPeriod(ContractPeriod item);
        IList<ContractPeriod> GetAllContractPeriodsByListId(IList<int> Ids = null);

        #endregion
    }
}
