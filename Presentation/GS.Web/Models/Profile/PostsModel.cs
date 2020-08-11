using GS.Web.Framework.Models;

namespace GS.Web.Models.Profile
{
    public partial class PostsModel : BaseGSModel
    {
        public int ForumTopicId { get; set; }
        public string ForumTopicTitle { get; set; }
        public string ForumTopicSlug { get; set; }
        public string ForumPostText { get; set; }
        public string Posted { get; set; }
    }
}