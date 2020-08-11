using GS.Web.Areas.Admin.Models.Customers;
using GS.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Models.Contracts
{
    public class ContractResourceModel:BaseGSEntityModel
    {
       public ContractResourceModel()
        {
            SelectedCustomerRoleIds = new List<int>(new int[] {9});
            RoleModels = new List<CustomerRoleModel>();
            AvailbleRoles = new List<SelectListItem>();
        }
        public IList<int> SelectedCustomerRoleIds { get; set; }
        public Int32 ContractId { get; set; }
        public Int32 CustomerId { get; set; }
        public string CustomerFullName { get; set; }
        public string CustomerAvatarUrl { get; set; }
        public Int32 RoleId { get; set; }
        public int[] RoleIds { get; set; }
        public List<CustomerRoleModel> RoleModels { get; set; }
        public List<SelectListItem> AvailbleRoles { get; set; }
        public String Note { get; set; }
        public int? UnitId { get; set; }
        public string UnitName { get; set; }
        public string CustomerName { get; set; }
        public int? GroupId { get; set; }
        public string GroupName { get; set; }
    }
}
