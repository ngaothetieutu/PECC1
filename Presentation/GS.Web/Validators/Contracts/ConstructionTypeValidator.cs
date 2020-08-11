using GS.Web.Models.Contracts;
using FluentValidation;
using GS.Services.Localization;
using GS.Web.Framework.Validators;

namespace GS.Web.Validators.Contracts
{
    public class ConstructionTypeValidator : BaseGSValidator<ConstructionTypeModel>
    {
        public ConstructionTypeValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("AppWork.Contracts.ConstructionType.Fields.Name.Required"));
        }
    }
}
