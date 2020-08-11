using FluentValidation;
using GS.Web.Areas.Admin.Models.Tasks;
using GS.Core.Domain.Tasks;
using GS.Data;
using GS.Services.Localization;
using GS.Web.Framework.Validators;

namespace GS.Web.Areas.Admin.Validators.Tasks
{
    public partial class TaskRoleValidator : BaseGSValidator<TaskRoleModel>
    {
        public TaskRoleValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Tasks.TaskRoles.Fields.Name.Required"));

            SetDatabaseValidationRules<TaskRole>(dbContext);
        }
    }
}
