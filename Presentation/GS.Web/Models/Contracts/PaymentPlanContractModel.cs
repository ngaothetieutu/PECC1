using GS.Core.Domain.Contracts;
using GS.Web.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Models.Contracts
{
    public class PaymentPlanContractModel : BaseGSEntityModel
    {
        public int ContractId { get; set; }
        public int paymentPlanId { get; set; }
        public string Note { get; set; }
        public DateTime CreateDate { get; set; }
    }
    public partial class PaymentPlanNoteModel
    {
        public PaymentPlanNoteModel()
        {

        }
        public int Code { get; set; }
        public string Message { get; set; }
        public string IdRecord { get; set; }
        public ContractPaymentPlan ObjectInfo { get; set; }
    }
}
