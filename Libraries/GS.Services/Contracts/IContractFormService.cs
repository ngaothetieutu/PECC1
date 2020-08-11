using GS.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Services.Contracts
{
    public interface IContractFormService
    {
        #region ContractForm

        /// <summary>
        /// Delete the review type
        /// </summary>
        /// <param name="item">Contract Form</param>
        void DeleteContractForm(ContractForm item);

        /// <summary>
        /// Get all review types
        /// </summary>
        /// <returns>Contract Forms</returns>
        IList<ContractForm> GetAllContractForms(IList<int> Ids=null);
        IList<ContractForm> GetAllContractFormsByName(string Name = "");
        /// <summary>
        /// Get the review type 
        /// </summary>
        /// <param name="itemId">Contract Form identifier</param>
        /// <returns>Contract Form</returns>
        ContractForm GetContractFormById(int itemId);

        /// <summary>
        /// Insert the review type
        /// </summary>
        /// <param name="item">Contract Form</param>
        void InsertContractForm(ContractForm item);

        /// <summary>
        /// Update the review type
        /// </summary>
        /// <param name="item">Contract Form</param>
        void UpdateContractForm(ContractForm item);

        #endregion
    }
}
