using GS.Web.Framework.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Models.Contracts
{
    public class ContractSettlementSubModel : BaseGSEntityModel
    {
        public Int32 SettlementId { get; set; }
        public int TaskId { get; set; }
        public int ContractId { get; set; }
        [UIHint("WorkCurrency")]
        public string TotalAmount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string TaskName { get; set; }
        public string TaskTotalMoney { get; set; }
        public string UnitName { get; set; }
    }
}
