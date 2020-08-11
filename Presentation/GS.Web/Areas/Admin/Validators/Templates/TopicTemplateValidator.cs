using FluentValidation;
using GS.Web.Areas.Admin.Models.Templates;
using GS.Services.Localization;
using GS.Web.Framework.Validators;

namespace GS.Web.Areas.Admin.Validators.Templates
{
    public partial class TopicTemplateValidator : BaseGSValidator<TopicTemplateModel>
    {
        public TopicTemplateValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.System.Templates.Topic.Name.Required"));
            RuleFor(x => x.ViewPath).NotEmpty().WithMessage(localizationService.GetResource("Admin.System.Templates.Topic.ViewPath.Required"));
        }
    }
}