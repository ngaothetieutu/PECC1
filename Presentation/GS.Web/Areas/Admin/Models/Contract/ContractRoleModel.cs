using System.Collections.Generic;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using GS.Web.Areas.Admin.Validators.Contract;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Framework.Models;

namespace GS.Web.Areas.Admin.Models.Contract
{
    [Validator(typeof(ContractRoleValidator))]
    public partial class ContractRoleModel : BaseGSEntityModel
    {
        #region Ctor
        public ContractRoleModel()
        {
            this.TaxDisplayTypeValues = new List<SelectListItem>();
        }
        #endregion

        #region Properties
        [GSResourceDisplayName("Admin.Contract.ContractRoles.Fields.Name")]
        public string Name { get; set; }

        [GSResourceDisplayName("Admin.Contract.ContractRoles.Fields.FreeShipping")]
        public bool FreeShipping { get; set; }

        [GSResourceDisplayName("Admin.Contract.ContractRoles.Fields.TaxExempt")]
        public bool TaxExempt { get; set; }

        [GSResourceDisplayName("Admin.Contract.ContractRoles.Fields.Active")]
        public bool Active { get; set; }

        [GSResourceDisplayName("Admin.Contract.ContractRoles.Fields.IsSystemRole")]
        public bool IsSystemRole { get; set; }

        [GSResourceDisplayName("Admin.Contract.ContractRoles.Fields.SystemName")]
        public string SystemName { get; set; }

        [GSResourceDisplayName("Admin.Contract.ContractRoles.Fields.EnablePasswordLifetime")]
        public bool EnablePasswordLifetime { get; set; }

        [GSResourceDisplayName("Admin.Contract.ContractRoles.Fields.OverrideTaxDisplayType")]
        public bool OverrideTaxDisplayType { get; set; }

        [GSResourceDisplayName("Admin.Contract.ContractRoles.Fields.DefaultTaxDisplayType")]
        public int DefaultTaxDisplayTypeId { get; set; }

        public IList<SelectListItem> TaxDisplayTypeValues { get; set; }

        [GSResourceDisplayName("Admin.Contract.ContractRoles.Fields.PurchasedWithProduct")]
        public int PurchasedWithProductId { get; set; }

        [GSResourceDisplayName("Admin.Contract.ContractRoles.Fields.PurchasedWithProduct")]
        public string PurchasedWithProductName { get; set; }

        #endregion
    }
}
