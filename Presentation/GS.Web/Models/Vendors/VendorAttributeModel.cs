using System.Collections.Generic;
using GS.Core.Domain.Catalog;
using GS.Web.Framework.Models;

namespace GS.Web.Models.Vendors
{
    public partial class VendorAttributeModel : BaseGSEntityModel
    {
        public VendorAttributeModel()
        {
            Values = new List<VendorAttributeValueModel>();
        }

        public string Name { get; set; }

        public bool IsRequired { get; set; }

        /// <summary>
        /// Default value for textboxes
        /// </summary>
        public string DefaultValue { get; set; }

        public AttributeControlType AttributeControlType { get; set; }

        public IList<VendorAttributeValueModel> Values { get; set; }

    }

    public partial class VendorAttributeValueModel : BaseGSEntityModel
    {
        public string Name { get; set; }

        public bool IsPreSelected { get; set; }
    }
}