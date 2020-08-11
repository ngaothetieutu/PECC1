using GS.Web.Framework.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Web.Models.Works
{
    public class TaskLogModel : BaseGSEntityModel
    {
        public Int32 TaskId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int32 CreatorId { get; set; }
        public String Note { get; set; }
        public String TaskData { get; set; }
    }
    public partial class TaskLogListModel : BasePagedListModel<TaskLogModel>
    {
    }
    public partial class TaskLogSearchModel : BaseSearchModel
    {
    }
}
