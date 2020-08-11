using System.Collections.Generic;
using GS.Web.Framework.Models;

namespace GS.Web.Models.Boards
{
    public partial class BoardsIndexModel : BaseGSModel
    {
        public BoardsIndexModel()
        {
            this.ForumGroups = new List<ForumGroupModel>();
        }
        
        public IList<ForumGroupModel> ForumGroups { get; set; }
    }
}