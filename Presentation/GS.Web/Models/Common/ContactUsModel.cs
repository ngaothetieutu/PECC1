using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Framework.Models;
using GS.Web.Validators.Common;

namespace GS.Web.Models.Common
{
    [Validator(typeof(ContactUsValidator))]
    public partial class ContactUsModel : BaseGSModel
    {
        [DataType(DataType.EmailAddress)]
        [GSResourceDisplayName("ContactUs.Email")]
        public string Email { get; set; }
        
        [GSResourceDisplayName("ContactUs.Subject")]
        public string Subject { get; set; }
        public bool SubjectEnabled { get; set; }

        [GSResourceDisplayName("ContactUs.Enquiry")]
        public string Enquiry { get; set; }

        [GSResourceDisplayName("ContactUs.FullName")]
        public string FullName { get; set; }

        public bool SuccessfullySent { get; set; }
        public string Result { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}