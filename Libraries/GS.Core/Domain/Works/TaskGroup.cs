using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Works
{
    public class TaskGroup:BaseEntity
    {
        public TaskGroup()
        {
            TreeLevel = 1;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Ratio { get; set; }
        public int? ParentId { get; set; }
        public string TreeNode { get; set; }
        public int TreeLevel { get; set; }

    }
}
