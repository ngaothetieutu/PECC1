using GS.Core.Domain.Customers;
using GS.Core.Domain.Directory;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Advance
{
    public enum PaymentAdvanceStatus
    {
        /// <summary>
        /// su dung
        /// </summary>
        Use = 1 ,
        /// <summary>
        /// Xoa
        /// </summary>
        Delete = 4
    }
   public  partial class ContractPaymentAdvance : BaseEntity
   {
        public ContractPaymentAdvance()
        {
          //  this.AdvanceGuid = Guid.NewGuid();
        }
        public Guid AdvanceGuid { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Int32? TypeId { get; set; }
        public string Description { get; set; }
        public Int32? UnitId { get; set; }
        public Int32? CurrencyId { get; set; }
        public DateTime? PayDate { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int32? CreatorId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? TotalReceive { get; set; }
        public virtual Unit unit { get; set; }
        public virtual Customer creator { get; set; }      
        public virtual Currency currency { get; set; }
    }
}