using GS.Web.Models.Contracts;
using FluentValidation;
using GS.Services.Localization;
using GS.Web.Framework.Validators;

namespace GS.Web.Validators.Contracts
{
    public class ContractFormValidator : BaseGSValidator<ContractFormModel>
    {
        public ContractFormValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("AppWork.Contracts.ContractForm.Fields.Name.Required"));
        }
    }
}
