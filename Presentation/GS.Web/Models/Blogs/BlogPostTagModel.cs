using GS.Web.Framework.Models;

namespace GS.Web.Models.Blogs
{
    public partial class BlogPostTagModel : BaseGSModel
    {
        public string Name { get; set; }

        public int BlogPostCount { get; set; }
    }
}