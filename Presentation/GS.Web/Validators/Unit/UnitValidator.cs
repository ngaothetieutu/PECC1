using FluentValidation;
using GS.Services.Localization;
using GS.Web.Framework.Validators;
using GS.Web.Models.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Validators.Unit
{
    public class UnitValidator : BaseGSValidator<UnitModel>
    {
        public UnitValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage(localizationService.GetResource("AppWork.Customer.Unit.Fields.Code.Required"));
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("AppWork.Customer.Unit.Fields.Name.Required"));
        }
    }
}
