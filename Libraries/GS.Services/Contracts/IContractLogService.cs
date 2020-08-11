using GS.Core;
using GS.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Services.Contracts
{
    public interface IContractLogService
    {
        #region Contract Log

        /// <summary>
        /// Get all review types
        /// </summary>
        /// <returns>Contract Logs</returns>
        IList<ContractLog> GetAllContractLogs();

        /// <summary>
        /// Get the review type 
        /// </summary>
        /// <param name="itemId">Contract Log identifier</param>
        /// <returns>Contract Log</returns>
        ContractLog GetContractLogById(int itemId);

        /// <summary>
        /// Insert the review type
        /// </summary>
        /// <param name="item">Contract Form</param>
        void InsertContractLog(ContractLog item, string note);
        /// <summary>
        /// Gets all contractlog by ContractId
        /// </summary>
        /// <returns>Review types</returns>
        IList<ContractLog> GetAllContractLogsByContractId(int contractId);
        IPagedList<ContractLog> GetContractLogByContractIdorName(int ContractID = 0, string Note = "", int pageIndex = 0, int pageSize = int.MaxValue);

        #endregion
    }
}
