using GS.Core.Domain.Contracts;
using GS.Web.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Models.Contracts
{
    public class ContractMonitorModel : BaseGSEntityModel
    {
        public int ContractId { get; set; }
        public int TypeId { get; set; }
        public String Description { get; set; }
        public String StatusIds { get; set; }
        public String AcceptanceSummary { get; set; }
        public String PaymentSummary { get; set; }
        public String PaymentAdvanceSummary { get; set; }
        /// <summary>
        /// Binh thuong
        /// </summary>
        public bool isNormal
        {
            get
            {
                return string.IsNullOrEmpty(StatusIds);
            }
        }
        public bool isOverduePayment
        {
            get
            {
                if (string.IsNullOrEmpty(StatusIds))
                    return false;
                return StatusIds.Contains(Convert.ToString((int)ContractMonitorStatus.OverduePayment));
            }
        }
        public bool isOverdueTask
        {
            get
            {
                if (string.IsNullOrEmpty(StatusIds))
                    return false;
                return StatusIds.Contains(Convert.ToString((int)ContractMonitorStatus.OverdueTask));
            }
        }
        public bool isTimeExpired
        {
            get
            {
                if (string.IsNullOrEmpty(StatusIds))
                    return false;
                return StatusIds.Contains(Convert.ToString((int)ContractMonitorStatus.TimeExpired));
            }
        }
        public bool isTimeEnding
        {
            get
            {
                if (string.IsNullOrEmpty(StatusIds))
                    return false;
                return StatusIds.Contains(Convert.ToString((int)ContractMonitorStatus.TimeEnding));
            }
        }
    }
    public class DelayPayment
    {
        public IList<ContractModel> contractDelayPayment { get; set; }
    }
}
