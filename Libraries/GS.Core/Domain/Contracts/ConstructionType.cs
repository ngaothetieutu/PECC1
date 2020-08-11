using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public class ConstructionType: BaseEntity
    {
        public int TypeId { get; set; }
        public string Name { get; set; }
        public int contractCount { get; set; }
    }
    public class ConstructionTypeResult
    {
        public int TypeId { get; set; }
        public int TotalCount { get; set; }
        public string TypeName { get; set; }
        public Decimal? TotalMoney { get; set; }
        public Decimal TotalPaymentAcceptance { get; set; }
        public Decimal TotalPaymentAdvance { get; set; }
        public Decimal TotalMoneyPaid { get; set; }
        public Decimal TotalMoneyContractUnfinish { get; set; }
        public Decimal TotalMoneyContractUnfinish2 { get; set; }
    }
    public class ConstructionTypeResultByYear
    {
        public int TypeId { get; set; }
        public int TotalCount { get; set; }
        public string TypeName { get; set; }
        public Decimal? TotalMoney { get; set; }
        public Decimal TotalPaymentAcceptance { get; set; }
        public Decimal TotalPaymentAdvance { get; set; }
        public Decimal TotalMoneyPaid { get; set; }
        public int StartYear { get; set; }
        public Decimal Unfinish { get; set; }
        public Decimal TotalMoneyContractUnfinish { get; set; }
        public Decimal TotalMoneyContractUnfinish2 { get; set; }
    }
}
