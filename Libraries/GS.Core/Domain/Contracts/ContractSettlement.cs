using GS.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public enum ContractSettlementStatusId
    {
        Use = 0,
        /// <summary>
        /// Sử dụng
        /// </summary>
        Delete = 4
        /// <summary>
        /// Xóa
        /// </summary>
    }
    public class ContractSettlement : BaseEntity
    {
        public virtual Contract contract { get; set; }
        public virtual Customer approver { get; set; }
        public virtual Customer creator { get; set; }
        public Int32 ContractId { get; set; }
        public String Code { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public Int32 ApproverId { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public Int32 CreatorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int32 StatusId { get; set; }
        public ContractSettlementStatusId contractSettlementStatus
        {
            get => (ContractSettlementStatusId)StatusId;
            set => StatusId = (int)value;
        }
    }
}
