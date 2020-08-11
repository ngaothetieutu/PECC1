using FluentValidation.Attributes;
using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Validators.Customer;
using System.ComponentModel.DataAnnotations;

namespace GS.Web.Models.Customer
{
    [Validator(typeof(GiftCardValidator))]
    public partial class CheckGiftCardBalanceModel : BaseGSModel
    {
        public string Result { get; set; }

        public string Message { get; set; }
        
        [GSResourceDisplayName("ShoppingCart.GiftCardCouponCode.Tooltip")]
        public string GiftCardCode { get; set; }
    }
}
