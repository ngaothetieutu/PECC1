using System.Collections.Generic;
using GS.Web.Framework.Models;
using GS.Web.Models.Common;

namespace GS.Web.Models.PrivateMessages
{
    public partial class PrivateMessageListModel : BaseGSModel
    {
        public IList<PrivateMessageModel> Messages { get; set; }
        public PagerModel PagerModel { get; set; }
    }
}