using GS.Web.Framework.Models;

namespace GS.Web.Models.PrivateMessages
{
    public partial class PrivateMessageIndexModel : BaseGSModel
    {
        public int InboxPage { get; set; }
        public int SentItemsPage { get; set; }
        public bool SentItemsTabSelected { get; set; }
    }
}