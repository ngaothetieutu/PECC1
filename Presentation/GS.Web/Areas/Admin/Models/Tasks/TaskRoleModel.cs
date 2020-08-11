using System.Collections.Generic;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using GS.Web.Areas.Admin.Validators.Tasks;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Framework.Models;

namespace GS.Web.Areas.Admin.Models.Tasks
{
    [Validator(typeof(TaskRoleValidator))]
    public partial class TaskRoleModel : BaseGSEntityModel
    {
        #region Ctor
        public TaskRoleModel()
        {
            this.TaxDisplayTypeValues = new List<SelectListItem>();
        }
        #endregion

        #region Properties
        [GSResourceDisplayName("Admin.Tasks.TaskRoles.Fields.Name")]
        public string Name { get; set; }

        [GSResourceDisplayName("Admin.Task.TaskRoles.Fields.FreeShipping")]
        public bool FreeShipping { get; set; }

        [GSResourceDisplayName("Admin.Tasks.TaskRoles.Fields.TaxExempt")]
        public bool TaxExempt { get; set; }

        [GSResourceDisplayName("Admin.Tasks.TaskRoles.Fields.Active")]
        public bool Active { get; set; }

        [GSResourceDisplayName("Admin.Tasks.TaskRoles.Fields.IsSystemRole")]
        public bool IsSystemRole { get; set; }

        [GSResourceDisplayName("Admin.Tasks.TaskRoles.Fields.SystemName")]
        public string SystemName { get; set; }

        [GSResourceDisplayName("Admin.Tasks.TaskRoles.Fields.EnablePasswordLifetime")]
        public bool EnablePasswordLifetime { get; set; }

        [GSResourceDisplayName("Admin.Tasks.TaskRoles.Fields.OverrideTaxDisplayType")]
        public bool OverrideTaxDisplayType { get; set; }

        [GSResourceDisplayName("Admin.Tasks.TaskRoles.Fields.DefaultTaxDisplayType")]
        public int DefaultTaxDisplayTypeId { get; set; }

        public IList<SelectListItem> TaxDisplayTypeValues { get; set; }

        [GSResourceDisplayName("Admin.Tasks.TaskRoles.Fields.PurchasedWithProduct")]
        public int PurchasedWithProductId { get; set; }

        [GSResourceDisplayName("Admin.Tasks.TaskRoles.Fields.PurchasedWithProduct")]
        public string PurchasedWithProductName { get; set; }
        #endregion
    }
}
