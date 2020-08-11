using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using GS.Web.Framework.Models;

namespace GS.Web.Models.Boards
{
    public partial class TopicMoveModel : BaseGSEntityModel
    {
        public TopicMoveModel()
        {
            ForumList = new List<SelectListItem>();
        }

        public int ForumSelected { get; set; }
        public string TopicSeName { get; set; }
        
        public IEnumerable<SelectListItem> ForumList { get; set; }
    }
}