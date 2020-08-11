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
    public class ContractPaymentValidator : BaseGSValidator<ContractPaymentModel>
    {
        public ContractPaymentValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage(localizationService.GetResource("AppWork.Contracts.ContractPayment.Fields.Code.Required"));
            RuleFor(x => x.Description).NotEmpty().WithMessage(localizationService.GetResource("AppWork.Contracts.ContractPayment.Fields.Description.Required"));
            RuleFor(x => x.Amount).NotEmpty().WithMessage(localizationService.GetResource("AppWork.Contracts.ContractPayment.Fields.Amount.Required"));
        }
    }
}
