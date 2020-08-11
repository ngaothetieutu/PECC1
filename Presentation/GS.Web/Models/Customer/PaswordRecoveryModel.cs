using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Framework.Models;
using GS.Web.Validators.Customer;

namespace GS.Web.Models.Customer
{
    [Validator(typeof(PasswordRecoveryValidator))]
    public partial class PasswordRecoveryModel : BaseGSModel
    {
        [DataType(DataType.EmailAddress)]
        [GSResourceDisplayName("Account.PasswordRecovery.Email")]
        public string Email { get; set; }

        public string Result { get; set; }
    }
}