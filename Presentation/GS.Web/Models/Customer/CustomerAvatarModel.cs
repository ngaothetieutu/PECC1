using GS.Web.Framework.Models;
using System;

namespace GS.Web.Models.Customer
{
    public partial class CustomerAvatarModel : BaseGSModel
    {
        public Guid CustomerGuid { get; set; }
        public string AvatarUrl { get; set; }
    }
}