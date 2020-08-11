using GS.Web.Models.Contracts;
using FluentValidation;
using GS.Services.Localization;
using GS.Web.Framework.Validators;

namespace GS.Web.Validators.Contracts
{
    public class ContractTypeValidator: BaseGSValidator<ContractTypeModel>
    {
        public ContractTypeValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("AppWork.Contracts.ContractType.Fields.Name.Required"));
        }
    }
}
