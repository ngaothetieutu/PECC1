using FluentValidation;
using GS.Web.Areas.Admin.Models.Directory;
using GS.Core.Domain.Directory;
using GS.Data;
using GS.Services.Localization;
using GS.Web.Framework.Validators;

namespace GS.Web.Areas.Admin.Validators.Directory
{
    public partial class MeasureWeightValidator : BaseGSValidator<MeasureWeightModel>
    {
        public MeasureWeightValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Shipping.Measures.Weights.Fields.Name.Required"));
            RuleFor(x => x.SystemKeyword).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Shipping.Measures.Weights.Fields.SystemKeyword.Required"));

            SetDatabaseValidationRules<MeasureWeight>(dbContext);
        }
    }
}