using FluentValidation;
using GS.Web.Areas.Admin.Models.Contract;
using GS.Core.Domain.Contracts;
using GS.Data;
using GS.Services.Localization;
using GS.Web.Framework.Validators;

namespace GS.Web.Areas.Admin.Validators.Contract
{
    public class ContractAttributeValidator : BaseGSValidator<ContractAttributeModel>
    {
        public ContractAttributeValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Contract.ContractAttributes.Fields.Name.Required"));

            SetDatabaseValidationRules<ContractAttribute>(dbContext);
        }
    }
}
