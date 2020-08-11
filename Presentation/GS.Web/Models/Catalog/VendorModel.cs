using System.Collections.Generic;
using GS.Web.Framework.Models;
using GS.Web.Models.Media;

namespace GS.Web.Models.Catalog
{
    public partial class VendorModel : BaseGSEntityModel
    {
        public VendorModel()
        {
            PictureModel = new PictureModel();
            PagingFilteringContext = new CatalogPagingFilteringModel();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string SeName { get; set; }
        public bool AllowCustomersToContactVendors { get; set; }

        public PictureModel PictureModel { get; set; }

        public CatalogPagingFilteringModel PagingFilteringContext { get; set; }

    }
}