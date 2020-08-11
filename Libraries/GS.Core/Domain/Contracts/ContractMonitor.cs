using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public enum ContractMonitorStatus
    {
        /// <summary>
        /// Tat ca
        /// </summary>
        All = 0,
        /// <summary>
        /// Qua han thanh toan
        /// </summary>
        OverduePayment = 1,
        /// <summary>
        /// Qua han xu ly cong viec
        /// </summary>
        OverdueTask = 2,
        /// <summary>
        /// Hết hạn hợp đồng
        /// </summary>
        TimeExpired=3,
        /// <summary>
        /// Sắp hết hạn hợp đồng
        /// </summary>
        TimeEnding = 4,
        /// <summary>
        /// Sắp hết hạn thanh lý
        /// </summary>
        TimePayment = 5,
    }
    public partial class ContractMonitor : BaseEntity
    {
        public int ContractId { get; set; }
        public int? TypeId { get; set; }
        public String Description { get; set; }
        public String StatusIds { get; set; }
        public String AcceptanceSummary { get; set; }
        public String PaymentSummary { get; set; }
        public String PaymentAdvanceSummary { get; set; }
    }
}
