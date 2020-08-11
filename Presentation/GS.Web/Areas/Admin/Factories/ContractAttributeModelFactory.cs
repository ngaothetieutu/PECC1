using System;
using System.Linq;
using GS.Core.Domain.Contracts;
using GS.Services.Contracts;
using GS.Services.Localization;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Areas.Admin.Models.Contract;
using GS.Web.Framework.Extensions;
using GS.Web.Framework.Factories;

namespace GS.Web.Areas.Admin.Factories
{
    public partial class ContractAttributeModelFactory : IContractAttributeModelFactory
    {
        #region Fields

        private readonly IContractAttributeService _contractAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedModelFactory _localizedModelFactory;

        #endregion

        #region Ctor

        public ContractAttributeModelFactory(IContractAttributeService contractAttributeService,
            ILocalizationService localizationService,
            ILocalizedModelFactory localizedModelFactory)
        {
            this._contractAttributeService = contractAttributeService;
            this._localizationService = localizationService;
            this._localizedModelFactory = localizedModelFactory;
        }
        #endregion

        #region Utilities
        protected virtual ContractAttributeValueSearchModel PrepareContractAttributeValueSearchModel(ContractAttributeValueSearchModel searchModel,
            ContractAttribute contractAttribute)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (contractAttribute == null)
                throw new ArgumentNullException(nameof(contractAttribute));

            searchModel.ContractAttributeId = contractAttribute.Id;

            searchModel.SetGridPageSize();

            return searchModel;
        }
        #endregion

        #region Methods
        public virtual ContractAttributeSearchModel PrepareContractAttributeSearchModel(ContractAttributeSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            searchModel.SetGridPageSize();

            return searchModel;
        }

        public virtual ContractAttributeListModel PrepareContractAttributeListModel(ContractAttributeSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            var contractAttributes = _contractAttributeService.GetAllContractAttributes();

            var model = new ContractAttributeListModel
            {
                Data = contractAttributes.PaginationByRequestModel(searchModel).Select(attribute =>
                {
                    var attributeModel = attribute.ToModel<ContractAttributeModel>();

                    attributeModel.AttributeControlTypeName = _localizationService.GetLocalizedEnum(attribute.AttributeControlType);

                    return attributeModel;
                }),
                Total = contractAttributes.Count
            };
            return model;
        }

        public virtual ContractAttributeModel PrepareContractAttributeModel(ContractAttributeModel model, 
            ContractAttribute contractAttribute, bool excludeProperties = false)
        {
            Action<ContractAttributeLocalizedModel, int> localizedModelConfiguration = null;

            if (contractAttribute != null)
            {
                model = model ?? contractAttribute.ToModel<ContractAttributeModel>();

                PrepareContractAttributeValueSearchModel(model.ContractAttributeValueSearchModel, contractAttribute);

                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Name = _localizationService.GetLocalized(contractAttribute, entity => entity.Name, languageId, false, false);
                };
            }

            if (!excludeProperties)
                model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return model;
        }

        public virtual ContractAttributeValueListModel PrepareContractAttributeValueListModel(ContractAttributeValueSearchModel searchModel, ContractAttribute contractAttribute)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (contractAttribute == null)
                throw new ArgumentNullException(nameof(contractAttribute));

            var contractAttributeValues = _contractAttributeService.GetContractAttributeValues(contractAttribute.Id);

            var model = new ContractAttributeValueListModel
            {
                Data = contractAttributeValues.PaginationByRequestModel(searchModel)
                    .Select(value => value.ToModel<ContractAttributeValueModel>()),
                Total = contractAttributeValues.Count
            };

            return model;
        }

        public virtual ContractAttributeValueModel PrepareContractAttributeValueModel(ContractAttributeValueModel model, 
            ContractAttribute contractAttribute, ContractAttributeValue contractAttributeValue, bool excludeProperties = false)
        {
            if (contractAttribute == null)
                throw new ArgumentNullException(nameof(contractAttribute));

            Action<ContractAttributeValueLocalizedModel, int> localizedModelConfiguration = null;

            if (contractAttributeValue != null)
            {
                model = model ?? contractAttributeValue.ToModel<ContractAttributeValueModel>();

                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Name = _localizationService.GetLocalized(contractAttributeValue, entity => entity.Name,languageId,false,false);
                };
            }

            model.ContractAttributeId = contractAttribute.Id;

            if (!excludeProperties)
                model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return model;
        }
        
        #endregion
    }
}
