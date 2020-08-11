using FluentValidation;
using GS.Web.Areas.Admin.Models.Vendors;
using GS.Core.Domain.Vendors;
using GS.Data;
using GS.Services.Localization;
using GS.Services.Seo;
using GS.Web.Framework.Validators;

namespace GS.Web.Areas.Admin.Validators.Vendors
{
    public partial class VendorValidator : BaseGSValidator<VendorModel>
    {
        public VendorValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Vendors.Fields.Name.Required"));

            RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("Admin.Vendors.Fields.Email.Required"));
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Admin.Common.WrongEmail"));
            RuleFor(x => x.PageSizeOptions).Must(ValidatorUtilities.PageSizeOptionsValidator).WithMessage(localizationService.GetResource("Admin.Vendors.Fields.PageSizeOptions.ShouldHaveUniqueItems"));
            RuleFor(x => x.PageSize).Must((x, context) =>
            {
                if (!x.AllowCustomersToSelectPageSize && x.PageSize <= 0)
                    return false;

                return true;
            }).WithMessage(localizationService.GetResource("Admin.Vendors.Fields.PageSize.Positive"));
            RuleFor(x => x.SeName).Length(0, GSSeoDefaults.SearchEngineNameLength)
                .WithMessage(string.Format(localizationService.GetResource("Admin.SEO.SeName.MaxLengthValidation"), GSSeoDefaults.SearchEngineNameLength));

            SetDatabaseValidationRules<Vendor>(dbContext);
        }
    }
}