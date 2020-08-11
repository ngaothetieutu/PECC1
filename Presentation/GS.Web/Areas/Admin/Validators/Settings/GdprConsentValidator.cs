using FluentValidation;
using GS.Core.Domain.Gdpr;
using GS.Data;
using GS.Services.Localization;
using GS.Web.Areas.Admin.Models.Settings;
using GS.Web.Framework.Validators;

namespace GS.Web.Areas.Admin.Validators.Settings
{
    public partial class GdprConsentValidator : BaseGSValidator<GdprConsentModel>
    {
        public GdprConsentValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Message).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Settings.Gdpr.Consent.Message.Required"));
            RuleFor(x => x.RequiredMessage)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Configuration.Settings.Gdpr.Consent.RequiredMessage.Required"))
                .When(x => x.IsRequired);

            SetDatabaseValidationRules<GdprConsent>(dbContext);
        }
    }
}