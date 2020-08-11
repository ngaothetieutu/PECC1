using GS.Core.Domain.Contracts;
using GS.Web.Framework.Models;
using GS.Web.Models.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Models.Works
{
    public class WorkModel : BaseGSEntityModel
    {
        public WorkModel() {}
        public decimal? toltalMoney { get; set; }
        public decimal toltalCategory { get; set; }
        public IList<ContractModel> DelayedPayment { get; set; }
        public IList<ContractModel> DelayedProcessing { get; set; }
    }
    public class SumaryTotalModel
    {
        public string TotalMoney { get; set; }
        public string TotalMoneyAcceptance { get; set; }
        public string TotalUnit { get; set; }
        public string TotalCustomer { get; set; }
        public string TotalRemainingAmount { get; set; }
        public string TotalUnfinish1 { get; set; }
        public string TotalUnfinish2 { get; set; }
    }

    public class SumaryTotalModel2
    {
        public string TotalTask { get; set; }
        public string TotalPaymentPaid { get; set; }
        public string TotalPaymentAdvance { get; set; }
        public string TotalReceivable { get; set; }
        public string TotalRequest { get; set; }
    }
    public class SumaryTotalModel3
    {
        public string TotalProcuringAgency { get; set; }
        public string TotalProcuringAgencyIsEVN { get; set; }
        public string TotalConstruction { get; set; }
        public string TotalContract { get; set; }
        public string totalCustomer { get; set; }
    }
    public class TypeContract
    {
        public string Name { get; set; }
        public int ContractCount { get; set; }
    }

    public class TopFiveConstruction
    {
        public IList<ConstructionModel> topFiveConstruction { get; set; }
    }

    public class TopContractDealine
    {
        public IList<ContractModel> topContractDealine { get; set; }
    }

    public class ContractDelayedProcesing
    {
        public IList<ContractModel> contractDelayedProcessing { get; set; }
    }
    public class ConstructionTypeTotalModel {
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public int TotalCount { get; set; }
    }
    public class SumaryTotalByYearModel
    {
        public string TotalMoney { get; set; }
        public string TotalMoneyAcceptance { get; set; }
        public string TotalPaymentAdvance { get; set; }
        public string TotalRemainingAmount { get; set; }
        public string TotalProcuringAgency { get; set; }
        public string TotalConstruction { get; set; }
        public string TotalContract { get; set; }
        public string TotalPaymentPaid { get; set; }
        public int Time { get; set; }
        public string TotalMoneyContractunfinish { get; set; }
        public string TotalMoneyContractUnfinish2 { get; set; }
    }
    public class BarCharts
    {
        /// <summary>
        /// Nhan hien thi o truc X
        /// </summary>
        public string AxisLabel { get; set; }
        /// <summary>
        /// Gia tri hien thi o truc Y
        /// </summary>
        public Decimal Val1 { get; set; }
        /// <summary>
        /// Gia tri hien thi o truc Y
        /// </summary>
        public Decimal Val2 { get; set; }
    }
}
