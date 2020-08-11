using FluentValidation;
using GS.Services.Localization;
using GS.Web.Framework.Validators;
using GS.Web.Models.Customer;

namespace GS.Web.Validators.Customer
{
    public partial class GiftCardValidator : BaseGSValidator<CheckGiftCardBalanceModel>
    {
        public GiftCardValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.GiftCardCode).NotEmpty().WithMessage(localizationService.GetResource("CheckGiftCardBalance.GiftCardCouponCode.Empty"));            
        }
    }
}
