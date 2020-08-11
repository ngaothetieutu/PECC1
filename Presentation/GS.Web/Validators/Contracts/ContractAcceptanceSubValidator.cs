﻿using FluentValidation;
using GS.Services.Localization;
using GS.Web.Framework.Validators;
using GS.Web.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Validators.Contracts
{
    public class ContractAcceptanceSubValidator : BaseGSValidator<ContractAcceptanceSubModel>
    {
        public ContractAcceptanceSubValidator(ILocalizationService localizationService)
        {
           
        }
    }
}