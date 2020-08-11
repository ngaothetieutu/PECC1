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
    public class PaymentExpenditureValidator : BaseGSValidator<PaymentExpenditureModel>
    {
        public PaymentExpenditureValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("AppWork.Contracts.PaymentExpenditure.Fields.Name.Required"));
            RuleFor(x => x.Code).NotEmpty().WithMessage(localizationService.GetResource("AppWork.Contracts.PaymentExpenditure.Fields.Code.Required"));
        }
    }
}
