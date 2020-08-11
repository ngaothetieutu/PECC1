using FluentValidation;
using GS.Services.Localization;
using GS.Web.Framework.Validators;
using GS.Web.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Validators.Contracts
{
    public class ProcuringAgencyValidator : BaseGSValidator<ProcuringAgencyModel>
    {
        public ProcuringAgencyValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("AppWork.Contracts.Unit.Fields.Name.Required"));
            RuleFor(x => x.Representer).NotEmpty().WithMessage(localizationService.GetResource("AppWork.Contracts.Unit.Fields.Representer.Required"));
            RuleFor(x => x.Description).NotEmpty().WithMessage(localizationService.GetResource("AppWork.Contracts.Unit.Fields.Description.Required"));
        }

    }
}
