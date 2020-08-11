using GS.Core.Domain.Stores;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Customers
{
    public class Unit:BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int? StoreId { get; set; }
        public int? ParentId { get; set; }
        public virtual Unit parentUnit { get; set; }
        public string TreeNode { get; set; }
        public string Description { get; set; }
        public int? TreeLevel { get; set; }
        public string LevelSpace { get; set; }
        public virtual Store stores { get; set; }

    }
}
