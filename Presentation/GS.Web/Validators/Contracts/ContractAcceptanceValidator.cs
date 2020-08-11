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
    public class ContractAcceptanceValidator : BaseGSValidator<ContractAcceptanceModel>
    {
        public ContractAcceptanceValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("AppWork.Contracts.ContractAcceptance.Fields.Name.Required"));
            //RuleFor(x => x.Ratio).LessThanOrEqualTo(100).WithMessage(localizationService.GetResource("AppWork.Contracts.ContractAcceptance.Fields.Ratio.LessThan100"));
        }
    }
}
