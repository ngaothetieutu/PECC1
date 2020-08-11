using GS.Core.Domain.Contracts;
using GS.Web.Areas.Admin.Models.Contract;

namespace GS.Web.Areas.Admin.Factories
{
    public partial interface IContractAttributeModelFactory
    {
        ContractAttributeSearchModel PrepareContractAttributeSearchModel(ContractAttributeSearchModel searchModel);

        ContractAttributeListModel PrepareContractAttributeListModel(ContractAttributeSearchModel searchModel);

        ContractAttributeModel PrepareContractAttributeModel(ContractAttributeModel model,
            ContractAttribute contractAttribute, bool excludeProperties = false);

        ContractAttributeValueListModel PrepareContractAttributeValueListModel(ContractAttributeValueSearchModel searchModel,
            ContractAttribute contractAttribute);

        ContractAttributeValueModel PrepareContractAttributeValueModel(ContractAttributeValueModel model,
            ContractAttribute contractAttribute, ContractAttributeValue contractAttributeValue, bool excludeProperties = false);
    }
}
