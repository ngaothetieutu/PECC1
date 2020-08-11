using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Works
{
    public class TaskLog : BaseEntity
    {
        public Int32 TaskId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int32 CreatorId { get; set; }
        public String Note { get; set; }
        public String TaskData { get; set; }
    }
}
