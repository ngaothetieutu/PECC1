using FluentValidation;
using GS.Web.Areas.Admin.Models.Blogs;
using GS.Core.Domain.Blogs;
using GS.Data;
using GS.Services.Localization;
using GS.Services.Seo;
using GS.Web.Framework.Validators;

namespace GS.Web.Areas.Admin.Validators.Blogs
{
    public partial class BlogPostValidator : BaseGSValidator<BlogPostModel>
    {
        public BlogPostValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.ContentManagement.Blog.BlogPosts.Fields.Title.Required"));

            RuleFor(x => x.Body)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.ContentManagement.Blog.BlogPosts.Fields.Body.Required"));

            //blog tags should not contain dots
            //current implementation does not support it because it can be handled as file extension
            RuleFor(x => x.Tags)
                .Must(x => x == null || !x.Contains("."))
                .WithMessage(localizationService.GetResource("Admin.ContentManagement.Blog.BlogPosts.Fields.Tags.NoDots"));

            RuleFor(x => x.SeName).Length(0, GSSeoDefaults.SearchEngineNameLength)
                .WithMessage(string.Format(localizationService.GetResource("Admin.SEO.SeName.MaxLengthValidation"), GSSeoDefaults.SearchEngineNameLength));

            SetDatabaseValidationRules<BlogPost>(dbContext);
        }
    }
}