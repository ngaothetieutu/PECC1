using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Framework.Models;
using GS.Web.Validators.Vendors;

namespace GS.Web.Models.Vendors
{
    [Validator(typeof(ApplyVendorValidator))]
    public partial class ApplyVendorModel : BaseGSModel
    {
        public ApplyVendorModel()
        {
            this.VendorAttributes = new List<VendorAttributeModel>();
        }

        [GSResourceDisplayName("Vendors.ApplyAccount.Name")]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [GSResourceDisplayName("Vendors.ApplyAccount.Email")]
        public string Email { get; set; }

        [GSResourceDisplayName("Vendors.ApplyAccount.Description")]
        public string Description { get; set; }

        public IList<VendorAttributeModel> VendorAttributes { get; set; }

        public bool DisplayCaptcha { get; set; }

        public bool TermsOfServiceEnabled { get; set; }
        public bool TermsOfServicePopup { get; set; }

        public bool DisableFormInput { get; set; }
        public string Result { get; set; }
    }
}