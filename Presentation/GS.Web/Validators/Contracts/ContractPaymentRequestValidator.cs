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
    public class ContractPaymentRequestValidator : BaseGSValidator<ContractPaymentRequestModel>
    {
        public ContractPaymentRequestValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("AppWork.Contracts.ContractPaymentRequest.Fields.Name.Required"));
            RuleFor(x => x.Code).NotEmpty().WithMessage(localizationService.GetResource("AppWork.Contracts.ContractPaymentRequest.Fields.Code.Required"));
            RuleFor(x => x.Description).NotEmpty().WithMessage(localizationService.GetResource("AppWork.Contracts.ContractPaymentRequest.Fields.Description.Required"));
        }
    }
}
