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
    public class ContractPaymentAcceptanceSubValidator : BaseGSValidator<ContractPaymentAcceptanceSubModel>
    {
        public ContractPaymentAcceptanceSubValidator(ILocalizationService localizationService)
        {
           
        }
    }
}
