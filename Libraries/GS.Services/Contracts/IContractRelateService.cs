using GS.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Services.Contracts
{
    public interface IContractRelateService
    {
        #region Contract Relate

        /// <summary>
        /// Delete the review type
        /// </summary>
        /// <param name="item">Contract Relate</param>
        void DeleteContractRelate(ContractRelate item);

        /// <summary>
        /// Get all review types
        /// </summary>
        /// <returns>Contract Relates</returns>
        IList<ContractRelate> GetAllContractRelates();
        

        /// <summary>
        /// Get the review type 
        /// </summary>
        /// <param name="itemId">Contract Relate identifier</param>
        /// <returns>Contract Relate</returns>
        ContractRelate GetContractRelateById(int itemId);

        /// <summary>
        /// Insert the review type
        /// </summary>
        /// <param name="item">Contract Form</param>
        void InsertContractRelate(ContractRelate item);

        /// <summary>
        /// Update the review type
        /// </summary>
        /// <param name="item">Contract Relate</param>
        void UpdateContractRelate(ContractRelate item);

        IList<int> GetAllContractRelateIdByContractId(int ContractId);
        ContractRelate GetContractRelateByContract2Id(int contract2Id);
        #endregion
    }
}
