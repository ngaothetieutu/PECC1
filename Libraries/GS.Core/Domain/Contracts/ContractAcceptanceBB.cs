using GS.Core.Domain.Customers;
using GS.Core.Domain.Directory;
using GS.Core.Domain.Works;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public enum ContractAcceptanceBBStatus
    {
        /// <summary>
        /// su dung
        /// </summary>
        Use = 1,
        /// <summary>
        /// xoa
        /// </summary>
        Destroy = 4
    }
    public enum ContractAcceptanceBBType
    {
        /// <summary>
        /// nghiem thu BB
        /// </summary>
       BB = 1,      
    }
    public partial class ContractAcceptanceBB : BaseEntity
    {
        public ContractAcceptanceBB()
        {
            TypeId = 1;
            StatusId = (int)ContractAcceptanceBBStatus.Use;
        }
        public String Code { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public int? TypeId { get; set; }
        public Int32? CreatorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal? Ratio { get; set; }
        public decimal? TotalAmount { get; set; }
        public Int32 TaskId { get; set; }
        public Int32 ContractId { get; set; }
        public Int32 ContractIdBB { get; set; }
        public DateTime ApprovalDate { get; set; }
        public Int32? ApprovalId { get; set; }
        public int StatusId { get; set; }
        public Int32? UnitId { get; set; }
        public int? CurrencyId { get; set; }
        public virtual Customer creator { get; set; }
        public virtual Customer approver { get; set; }
        public virtual Contract contract { get; set; }
        public virtual Contract contractBB { get; set; }
        public virtual Currency currency { get; set; }
        public virtual Unit unit { get; set; }
        public virtual Task task { get; set; }
    }
}
