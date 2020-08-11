using System.Collections.Generic;
using GS.Web.Framework.Models;

namespace GS.Web.Models.Catalog
{
    public partial class CategoryNavigationModel : BaseGSModel
    {
        public CategoryNavigationModel()
        {
            Categories = new List<CategorySimpleModel>();
        }

        public int CurrentCategoryId { get; set; }
        public List<CategorySimpleModel> Categories { get; set; }

        #region Nested classes

        public class CategoryLineModel : BaseGSModel
        {
            public int CurrentCategoryId { get; set; }
            public CategorySimpleModel Category { get; set; }
        }

        #endregion
    }
}