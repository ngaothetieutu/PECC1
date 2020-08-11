using GS.Core;
using GS.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Services.Contracts
{
    public interface IContractPaymentService
    {
        #region ContractPayment

        /// <summary>
        /// Delete the review type
        /// </summary>
        /// <param name="item">Contract type</param>
        void DeleteContractPayment(ContractPayment item);

        /// <summary>
        /// Get all review types IsReceipts:0-all,1-thu,2-chi
        /// </summary> 
        /// <returns>Contract types</returns>
        IList<ContractPayment> GetAllContractsPayment(int ContractId =0, int AcceptanceId = 0, int TaskId = 0,int TypeId = 0,int IsReceipts =0);
        //IList<ContractPayment> GetListContractPaymentbylistId(List<int> listId);

        /// <summary>
        /// Get the review type 
        /// </summary>
        /// <param name="itemId">Contract type identifier</param>
        /// <returns>Contract type</returns>
        ContractPayment GetContractPaymentById(int itemId);
        /// <summary>
        /// Gets a customer by GUIDs
        /// </summary>
        /// <param name="contractGuid"></param>
        /// <returns></returns>
        ContractPayment GetContractPaymentByGuid(Guid contractGuid);
        /// <summary>
        /// Insert the review type
        /// </summary>
        /// <param name="item">Contract type</param>
        void InsertContractPayment(ContractPayment item);
        /// <summary>
        /// Update the review type
        /// </summary>
        /// <param name="item">Contract type</param>
        void UpdateContractPayment(ContractPayment item);
        decimal GetAllContractsPaymentAmountByPaymentRequestId(int PaymentRequestId);
        decimal GetAllContractsPaymentAmountByAcceptanceId(int AcceptanceId);
        decimal GetAllContractsPaymentAmountByTaskId(int TaskId);
        IList<ContractPayment> GetAllContractPaymentsByPaymentRequestId(int paymentRequestId);
        /// <summary>
        /// IsReceipts:0-all,1-thu,2-chi
        /// </summary>
        /// <param name="ContractId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="taskId"></param>
        /// <param name="SelectedListTaskIds"></param>
        /// <param name="KeySearch"></param>
        /// <param name="TypeId"></param>
        /// <param name="IsReceipts"></param>
        /// <returns></returns>
        IPagedList<ContractPayment> GetContractPayments(int ContractId =0, int pageIndex = 0, int pageSize = int.MaxValue,int taskId = 0,List<int> SelectedListTaskIds = null,string KeySearch = null, int TypeId = 0, int IsReceipts = 0);
        IList<ContractPayment> GetAllContractPaymentsByAcceptanceId(int acceptanceId);
        IList<ContractPayment> GetAllContractPaymentsByExpenditureId(int ExpenditureId);
        void DeleteContractPaymentsByExpenditureId(int ExpenditureId);
        #endregion
    }
}
