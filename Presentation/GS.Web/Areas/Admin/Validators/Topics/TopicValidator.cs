using FluentValidation;
using GS.Web.Areas.Admin.Models.Topics;
using GS.Core.Domain.Topics;
using GS.Data;
using GS.Services.Localization;
using GS.Services.Seo;
using GS.Web.Framework.Validators;

namespace GS.Web.Areas.Admin.Validators.Topics
{
    public partial class TopicValidator : BaseGSValidator<TopicModel>
    {
        public TopicValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.SeName).Length(0, GSSeoDefaults.ForumTopicLength)
                .WithMessage(string.Format(localizationService.GetResource("Admin.SEO.SeName.MaxLengthValidation"), GSSeoDefaults.ForumTopicLength));

            SetDatabaseValidationRules<Topic>(dbContext);
        }
    }
}
