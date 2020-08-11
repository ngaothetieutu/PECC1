using FluentValidation;
using GS.Core.Domain.Customers;
using GS.Services.Localization;
using GS.Web.Framework.Validators;
using GS.Web.Models.Customer;

namespace GS.Web.Validators.Customer
{
    public partial class LoginValidator : BaseGSValidator<LoginModel>
    {
        public LoginValidator(ILocalizationService localizationService, CustomerSettings customerSettings)
        {
            if (!customerSettings.UsernamesEnabled)
            {
                //login by email
                RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("Account.Login.Fields.Email.Required"));
                RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));
            }
        }
    }
}