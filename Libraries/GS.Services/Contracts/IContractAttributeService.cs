using System.Collections.Generic;
using GS.Core.Domain.Contracts;

namespace GS.Services.Contracts
{
    public partial interface IContractAttributeService
    {
        #region ContractAttribute
        void DeleteContractAttribute(ContractAttribute contractAttribute);

        IList<ContractAttribute> GetAllContractAttributes();

        ContractAttribute GetContractAttributeById(int contractAttributeId);

        void InsertContractAttribute(ContractAttribute contractAttribute);

        void UpdateContractAttribute(ContractAttribute contractAttribute);
        #endregion

        #region ContractAttributeValue
        void DeleteContractAttributeValue(ContractAttributeValue contractAttributeValue);

        IList<ContractAttributeValue> GetContractAttributeValues(int contractAttributeId);

        ContractAttributeValue GetContractAttributeValueById(int contractAttributeValueId);

        void InsertContractAttributeValue(ContractAttributeValue contractAttributeValue);

        void UpdateContractAttributeValue(ContractAttributeValue contractAttributeValue);
        #endregion
    }
}
