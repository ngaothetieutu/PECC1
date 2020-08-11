using FluentValidation;
using GS.Web.Areas.Admin.Models.Catalog;
using GS.Core.Domain.Catalog;
using GS.Data;
using GS.Services.Localization;
using GS.Services.Seo;
using GS.Web.Framework.Validators;

namespace GS.Web.Areas.Admin.Validators.Catalog
{
    public partial class ManufacturerValidator : BaseGSValidator<ManufacturerModel>
    {
        public ManufacturerValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Catalog.Manufacturers.Fields.Name.Required"));
            RuleFor(x => x.PageSizeOptions).Must(ValidatorUtilities.PageSizeOptionsValidator).WithMessage(localizationService.GetResource("Admin.Catalog.Manufacturers.Fields.PageSizeOptions.ShouldHaveUniqueItems"));
            RuleFor(x => x.PageSize).Must((x, context) =>
            {
                if (!x.AllowCustomersToSelectPageSize && x.PageSize <= 0)
                    return false;

                return true;
            }).WithMessage(localizationService.GetResource("Admin.Catalog.Manufacturers.Fields.PageSize.Positive"));
            RuleFor(x => x.SeName).Length(0, GSSeoDefaults.SearchEngineNameLength)
                .WithMessage(string.Format(localizationService.GetResource("Admin.SEO.SeName.MaxLengthValidation"), GSSeoDefaults.SearchEngineNameLength));

            SetDatabaseValidationRules<Manufacturer>(dbContext);
        }
    }
}