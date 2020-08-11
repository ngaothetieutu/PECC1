using FluentValidation;
using GS.Web.Areas.Admin.Models.News;
using GS.Core.Domain.News;
using GS.Data;
using GS.Services.Localization;
using GS.Services.Seo;
using GS.Web.Framework.Validators;

namespace GS.Web.Areas.Admin.Validators.News
{
    public partial class NewsItemValidator : BaseGSValidator<NewsItemModel>
    {
        public NewsItemValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContentManagement.News.NewsItems.Fields.Title.Required"));

            RuleFor(x => x.Short).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContentManagement.News.NewsItems.Fields.Short.Required"));

            RuleFor(x => x.Full).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContentManagement.News.NewsItems.Fields.Full.Required"));

            RuleFor(x => x.SeName).Length(0, GSSeoDefaults.SearchEngineNameLength)
                .WithMessage(string.Format(localizationService.GetResource("Admin.SEO.SeName.MaxLengthValidation"), GSSeoDefaults.SearchEngineNameLength));

            SetDatabaseValidationRules<NewsItem>(dbContext);
        }
    }
}