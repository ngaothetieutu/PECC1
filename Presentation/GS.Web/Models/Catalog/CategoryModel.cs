﻿using System.Collections.Generic;
using GS.Web.Framework.Models;
using GS.Web.Models.Media;

namespace GS.Web.Models.Catalog
{
    public partial class CategoryModel : BaseGSEntityModel
    {
        public CategoryModel()
        {
            PictureModel = new PictureModel();
            PagingFilteringContext = new CatalogPagingFilteringModel();
            SubCategories = new List<SubCategoryModel>();
            CategoryBreadcrumb = new List<CategoryModel>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string SeName { get; set; }
        
        public PictureModel PictureModel { get; set; }

        public CatalogPagingFilteringModel PagingFilteringContext { get; set; }

        public bool DisplayCategoryBreadcrumb { get; set; }
        public IList<CategoryModel> CategoryBreadcrumb { get; set; }
        
        public IList<SubCategoryModel> SubCategories { get; set; }


		#region Nested Classes

        public partial class SubCategoryModel : BaseGSEntityModel
        {
            public SubCategoryModel()
            {
                PictureModel = new PictureModel();
            }

            public string Name { get; set; }

            public string SeName { get; set; }

            public string Description { get; set; }

            public PictureModel PictureModel { get; set; }
        }

		#endregion
    }
}