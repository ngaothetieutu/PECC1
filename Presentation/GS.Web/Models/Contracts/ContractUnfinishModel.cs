using GS.Web.Framework.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Models.Contracts
{
    public class ContractUnfinishModel : BaseGSEntityModel
    {
        public Int32 ContractId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int32 ContractTypeId { get; set; }
        [UIHint("WorkCurrency")]
        public decimal AutoValue { get; set; }
        [UIHint("WorkCurrency")] 
        public string OptionValue { get; set; }
        public Int32 CreatorId { get; set; }
        public string DisplayValue { get; set;}
        public Int32 TypeId { get; set; }
        // add more
        public string ContractTypeName {get;set;}
        public virtual ContractModel contract { get; set; }
    }
    public class ContractUnfinishSearchModel 
    {
        [UIHint("DateNullable")]
        public DateTime DaySearch { get; set; }
        public int ContractId  { get; set; }
        public int ContractTypeId { get; set; }
    }
    public partial class ContractUnfinishListModel : BasePagedListModel<ContractUnfinishModel>
    {
      
    }
}
