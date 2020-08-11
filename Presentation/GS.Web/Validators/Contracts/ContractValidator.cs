using FluentValidation;
using GS.Services.Localization;
using GS.Web.Framework.Validators;
using GS.Web.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GS.Core.Infrastructure;
using GS.Web.Factories;

namespace GS.Web.Validators.Contracts
{
    public class ContractValidator : BaseGSValidator<ContractModel>
    {       
        public ContractValidator(ILocalizationService localizationService,
            IContractModelFactory contractModelFactory
            )
        {
           
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("AppWork.Contracts.Contract.Fields.Name.Required"));
            RuleFor(x => x.Code).NotEmpty().WithMessage(localizationService.GetResource("AppWork.Contracts.Contract.Fields.Code.Required"));
            RuleFor(x => x.Code1).NotEmpty().WithMessage(localizationService.GetResource("AppWork.Contracts.Contract.Fields.Code1.Required"));
            RuleFor(x=>x.EndDate).Must((model,enddate) =>
            {
                return contractModelFactory.CheckEndDate(model);
            }).WithMessage(localizationService.GetResource("AppWork.Contracts.Contract.Fields.EndDate.GreaterThanCreatedDate"));
            RuleFor(x => x.Code1).Must((model, code) =>
            {
                return contractModelFactory.CheckCodeP4Contract(model.Id,code);
            }).WithMessage(localizationService.GetResource("AppWork.Contracts.Contract.Fields.Code1.Check"));
            RuleFor(x => x.ConstructionId).NotNull().WithMessage(localizationService.GetResource("AppWork.Contracts.Contract.Fields.ConstructionId.Required"));
            
        }
    }
}
