using GS.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Services.Contracts
{
    public interface IContractTypeService
    {
        #region ContractType

        /// <summary>
        /// Delete the review type
        /// </summary>
        /// <param name="item">Contract Type</param>
        void DeleteContractType(ContractType item);

        /// <summary>
        /// Get all review types
        /// </summary>
        /// <returns>Contract Types</returns>
        IList<ContractType> GetAllContractTypes(IList<int> Ids = null);

        /// <summary>
        /// Get the review type 
        /// </summary>
        /// <param name="itemId">Contract Type identifier</param>
        /// <returns>Contract Type</returns>
        ContractType GetContractTypeById(int itemId);

        /// <summary>
        /// Insert the review type
        /// </summary>
        /// <param name="item">Contract Type</param>
        void InsertContractType(ContractType item);

        /// <summary>
        /// Update the review type
        /// </summary>
        /// <param name="item">Contract Type</param>
        void UpdateContractType(ContractType item);
        /// <summary>
        /// Get ContractType By Name
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        IList<ContractType> GetAllContractTypeByName(string Name = "");
        #endregion
        #region contracttype mapping
        IList<ContractContractTypeMapping> GetContractTypeMapping(int ContractId =0, int ContractTypeId=0);
        #endregion
    }
}
