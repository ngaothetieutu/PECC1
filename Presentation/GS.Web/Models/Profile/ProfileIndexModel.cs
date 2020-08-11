using GS.Web.Framework.Models;

namespace GS.Web.Models.Profile
{
    public partial class ProfileIndexModel : BaseGSModel
    {
        public int CustomerProfileId { get; set; }
        public string ProfileTitle { get; set; }
        public int PostsPage { get; set; }
        public bool PagingPosts { get; set; }
        public bool ForumsEnabled { get; set; }
    }
}