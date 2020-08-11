using System.Collections.Generic;
using GS.Core.Domain.Catalog;
using GS.Web.Framework.Models;

namespace GS.Web.Models.Customer
{
    public partial class CustomerAttributeModel : BaseGSEntityModel
    {
        public CustomerAttributeModel()
        {
            Values = new List<CustomerAttributeValueModel>();
        }

        public string Name { get; set; }

        public bool IsRequired { get; set; }

        /// <summary>
        /// Default value for textboxes
        /// </summary>
        public string DefaultValue { get; set; }

        public AttributeControlType AttributeControlType { get; set; }

        public IList<CustomerAttributeValueModel> Values { get; set; }

    }

    public partial class CustomerAttributeValueModel : BaseGSEntityModel
    {
        public string Name { get; set; }

        public bool IsPreSelected { get; set; }
    }
}