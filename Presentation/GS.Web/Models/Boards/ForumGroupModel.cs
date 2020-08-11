using System.Collections.Generic;
using GS.Web.Framework.Models;

namespace GS.Web.Models.Boards
{
    public partial  class ForumGroupModel : BaseGSModel
    {
        public ForumGroupModel()
        {
            this.Forums = new List<ForumRowModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string SeName { get; set; }

        public IList<ForumRowModel> Forums { get; set; }
    }
}