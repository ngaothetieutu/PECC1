using FluentValidation.Attributes;
using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Validators.Unit;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Models.Unit
{
    [Validator(typeof(UnitValidator))]
    public class UnitModel : BaseGSEntityModel
    {
        public UnitModel()
        {
            subUnitModels = new List<UnitModel>();
        }
        [GSResourceDisplayName("AppWork.Customer.Unit.Fields.Code")]
        public string Code { get; set; }
        [GSResourceDisplayName("AppWork.Customer.Unit.Fields.Name")]
        public string Name { get; set; }
        [GSResourceDisplayName("AppWork.Customer.Unit.Fields.StoreId")]
        public int? StoreId { get; set; }
        [GSResourceDisplayName("AppWork.Customer.Unit.Fields.ParentId")]
        public int? ParentId { get; set; }
        [GSResourceDisplayName("AppWork.Customer.Unit.Fields.TreeNode")]
        public string TreeNode { get; set; }
        [GSResourceDisplayName("AppWork.Customer.Unit.Fields.Description")]
        public string Description { get; set; }
        public int? TreeLevel { get; set; }
        public string LevelSpace { get; set; }

        public string ParentName { get; set; }
        public IList<SelectListItem> ddlParentId { get; set; }

        public List<UnitModel> subUnitModels { get; set; }
    }
    public partial class UnitListModel : BasePagedListModel<UnitModel>
    {

    }
    public partial class UnitSearchModel : BaseSearchModel
    {
        public UnitSearchModel()
        {
            this.getUnitModel = new UnitModel();
        }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public UnitModel getUnitModel { get; set; }
    }
    public class CustomerUnitModel:BaseGSEntityModel
    {
        public int CustomerId { get; set; }
        public int UnitId { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }
    public partial class CustomerUnitListModel : BasePagedListModel<CustomerUnitModel>
    {
        
    }
    public partial class CustomerUnitSearchModel : BaseSearchModel
    {
        public CustomerUnitSearchModel()
        {
            ListCustomer = new List<CustomerUnitModel>();
            SelectedUnitIds = new List<int>();
        }
        public int UnitId { get; set; }
        public string srKeywordCustomer { get; set; }
        public List<CustomerUnitModel> ListCustomer { get; set; }
        public List<int> SelectedUnitIds { get; set; }
    }
}
