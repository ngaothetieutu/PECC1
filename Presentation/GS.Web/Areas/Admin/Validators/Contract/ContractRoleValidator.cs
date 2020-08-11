using FluentValidation;
using GS.Web.Areas.Admin.Models.Contract;
using GS.Core.Domain.Contracts;
using GS.Data;
using GS.Services.Localization;
using GS.Web.Framework.Validators;

namespace GS.Web.Areas.Admin.Validators.Contract
{
    public partial class ContractRoleValidator : BaseGSValidator<ContractRoleModel>
    {
        public ContractRoleValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Contract.ContractRoles.Fields.Name.Required"));

            SetDatabaseValidationRules<ContractRole>(dbContext);
        }
    }
}
