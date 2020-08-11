using FluentValidation;
using GS.Web.Areas.Admin.Models.Forums;
using GS.Core.Domain.Forums;
using GS.Data;
using GS.Services.Localization;
using GS.Web.Framework.Validators;

namespace GS.Web.Areas.Admin.Validators.Forums
{
    public partial class ForumGroupValidator : BaseGSValidator<ForumGroupModel>
    {
        public ForumGroupValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContentManagement.Forums.ForumGroup.Fields.Name.Required"));

            SetDatabaseValidationRules<ForumGroup>(dbContext);
        }
    }
}