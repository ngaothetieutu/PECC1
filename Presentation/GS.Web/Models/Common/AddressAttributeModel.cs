using System.Collections.Generic;
using GS.Core.Domain.Catalog;
using GS.Web.Framework.Models;

namespace GS.Web.Models.Common
{
    public partial class AddressAttributeModel : BaseGSEntityModel
    {
        public AddressAttributeModel()
        {
            Values = new List<AddressAttributeValueModel>();
        }

        public string Name { get; set; }

        public bool IsRequired { get; set; }

        /// <summary>
        /// Default value for textboxes
        /// </summary>
        public string DefaultValue { get; set; }

        public AttributeControlType AttributeControlType { get; set; }

        public IList<AddressAttributeValueModel> Values { get; set; }
    }

    public partial class AddressAttributeValueModel : BaseGSEntityModel
    {
        public string Name { get; set; }

        public bool IsPreSelected { get; set; }
    }
}