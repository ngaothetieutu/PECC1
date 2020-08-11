using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Works
{
    public class TaskFile : BaseEntity
    {
        public Int32 TaskId { get; set; }
        public Int32 FileId { get; set; }
    }
}
