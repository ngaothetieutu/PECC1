using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.WidgetApps
{
    public partial class WidgetApp : BaseEntity
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String ViewId { get; set; }
    }
}
