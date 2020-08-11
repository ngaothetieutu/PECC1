using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Framework.Models;
using GS.Web.Validators.Vendors;

namespace GS.Web.Models.Vendors
{
    [Validator(typeof(VendorInfoValidator))]
    public class VendorInfoModel : BaseGSModel
    {
        public VendorInfoModel()
        {
            this.VendorAttributes = new List<VendorAttributeModel>();
        }

        [GSResourceDisplayName("Account.VendorInfo.Name")]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [GSResourceDisplayName("Account.VendorInfo.Email")]
        public string Email { get; set; }

        [GSResourceDisplayName("Account.VendorInfo.Description")]
        public string Description { get; set; }

        [GSResourceDisplayName("Account.VendorInfo.Picture")]
        public string PictureUrl { get; set; }

        public IList<VendorAttributeModel> VendorAttributes { get; set; }
    }
}