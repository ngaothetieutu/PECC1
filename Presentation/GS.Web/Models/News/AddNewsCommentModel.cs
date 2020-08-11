using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Framework.Models;

namespace GS.Web.Models.News
{
    public partial class AddNewsCommentModel : BaseGSModel
    {
        [GSResourceDisplayName("News.Comments.CommentTitle")]
        public string CommentTitle { get; set; }

        [GSResourceDisplayName("News.Comments.CommentText")]
        public string CommentText { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}