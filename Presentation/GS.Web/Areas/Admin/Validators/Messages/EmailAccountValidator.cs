using FluentValidation;
using GS.Web.Areas.Admin.Models.Messages;
using GS.Core.Domain.Messages;
using GS.Data;
using GS.Services.Localization;
using GS.Web.Framework.Validators;

namespace GS.Web.Areas.Admin.Validators.Messages
{
    public partial class EmailAccountValidator : BaseGSValidator<EmailAccountModel>
    {
        public EmailAccountValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Admin.Common.WrongEmail"));
            
            RuleFor(x => x.DisplayName).NotEmpty();

            SetDatabaseValidationRules<EmailAccount>(dbContext);
        }
    }
}