using System.Collections.Generic;
using GS.Web.Framework.Models;

namespace GS.Web.Models.Blogs
{
    public partial class BlogPostYearModel : BaseGSModel
    {
        public BlogPostYearModel()
        {
            Months = new List<BlogPostMonthModel>();
        }
        public int Year { get; set; }
        public IList<BlogPostMonthModel> Months { get; set; }
    }

    public partial class BlogPostMonthModel : BaseGSModel
    {
        public int Month { get; set; }

        public int BlogPostCount { get; set; }
    }
}