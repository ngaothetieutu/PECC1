using FluentValidation;
using GS.Services.Localization;
using GS.Web.Factories;
using GS.Web.Framework.Validators;
using GS.Web.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Validators.Contracts
{
    public class ContractSettlementValidator : BaseGSValidator<ContractSettlementModel>
    {
        public ContractSettlementValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("AppWork.Contracts.ContractSettlement.Fields.Name.Required"));
            RuleFor(x => x.Code).NotEmpty().WithMessage(localizationService.GetResource("AppWork.Contracts.ContractSettlement.Fields.Code.Required"));
        }
    }
}
