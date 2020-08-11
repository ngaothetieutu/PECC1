using FluentValidation;
using GS.Services.Localization;
using GS.Web.Framework.Validators;
using GS.Web.Models.Contracts;
using GS.Web.Models.PaymentAdvance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Validators.PaymentAdvance
{
    public class ContractPaymentAdvanceValidator : BaseGSValidator<ContractPaymentAdvanceModel>
    {
        public ContractPaymentAdvanceValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("AppWork.Contracts.ContractPaymentAdvance.Fields.Name.Required"));
            RuleFor(x => x.Code).NotEmpty().WithMessage(localizationService.GetResource("AppWork.Contracts.ContractPaymentAdvance.Fields.Code.Required"));
            RuleFor(x => x.UnitId).GreaterThan(0).WithMessage(localizationService.GetResource("AppWork.Contracts.ContractPaymentAdvance.Fields.UnitId.Required"));
        }
    }
}
