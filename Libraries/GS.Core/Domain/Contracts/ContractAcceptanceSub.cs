using GS.Core.Domain.Customers;
using GS.Core.Domain.Directory;
using GS.Core.Domain.Works;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public enum ContractAcceptanceSubStatus
    {
        /// <summary>
        /// Su dung
        /// </summary>
        Use = 1,
        /// <summary>
        /// Xoa
        /// </summary>
        Destroy = 4
    }
    /// <summary>
    /// loai hop dong BB
    /// </summary>
    public enum ContractAcceptanceSubType
    {
        /// <summary>
        /// do don vi thi cong
        /// </summary>
        THI_CONG = 0,
        /// <summary>
        /// BB' cong ty thue
        /// </summary>
        BB_CONG_TY = 1,
        /// <summary>
        /// BB do don vi thu
        /// </summary>
        BB_DON_VI = 2,
    }
    public partial class ContractAcceptanceSub : BaseEntity
    {
        public ContractAcceptanceSub()
        {
            StatusId = (int)ContractAcceptanceSubStatus.Use;
        }
        public int AcceptanceId { get; set; }
        public int ContractId { get; set; }
        public int? UnitId { get; set; }
        public int? TaskId { get; set; }
        public decimal? Ratio { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TotalMoney { get; set; }
        public string Note { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int32? CurrencyId { get; set; }        
        public Int32 CreatorId { get; set; }
        public virtual Customer creator { get; set; }
        public virtual Contract contract { get; set; }
        public virtual Unit unit { get; set; }
        public virtual ContractAcceptance contractAcceptance { get; set; }
        public virtual Currency currency { get; set; }
        public virtual Task task { get; set; }
    }
}
