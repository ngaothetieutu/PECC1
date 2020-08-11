using System.Collections.Generic;
using GS.Web.Framework.Models;
using GS.Web.Models.Common;

namespace GS.Web.Models.Profile
{
    public partial class ProfilePostsModel : BaseGSModel
    {
        public IList<PostsModel> Posts { get; set; }
        public PagerModel PagerModel { get; set; }
    }
}