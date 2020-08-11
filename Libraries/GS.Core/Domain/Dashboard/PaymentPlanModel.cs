using GS.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Dashboard
{
    public class PaymentPlanModel
    {
        //tong so tien tam ung
        public Decimal totalhasPaymentPlan { get; set; }
        // tong so tien da tam ung
        public Decimal totalPaymentPlan { get; set; }
        // tong so tien da tam ung sau tru tam ung
        public Decimal totalPaymentAdvanced { get; set; }
        // tong so tien da thu
        public string totalAmountCollected { get; set; }
        // tong so tien phai chi
        public string totalAmountSpent { get; set; }
        // tong so tien phai thu
        public string totalAmountReceivable { get; set; }
        // tong so tien da chi
        public string totalAmountSpend { get; set; }
        public string totalAmountContract { get; set; }
        //Tong gia tri da nghiem thu khoi luong cua hop dong
        public string totalAmountAccepted { get; set; }
        //Gia tri con lai chua nghiem thu khoi luong cua hop dong
        public Decimal totalAmountNotAccepted { get; set; }
        public string totalAmountBB { get; set; }
        public Decimal totalAcceptanced { get; set; }
        public string noteSpend { get; set; }
        public string noteReceivable { get; set; }
        public string notePaymentPlan { get; set; }
        //Tong gia tri da nghiem thu thanh toan chua giam tru cua hop dong
        public Decimal totalMoneyPaymentAccepted { get; set; }
        //Tong gia tri da nghiem thu thanh toan da giam tru cua hop dong
        public Decimal totalAmountPaymentAccepted { get; set; }
        //Tong gia tri con lai chua nghiem thu thanh toan cua hop dong
        public Decimal totalAmountNotPaymentAccepted { get; set; }
        // tong gia tri da duoc thanh toan cua hop dong
        public Decimal totalAmountPayment { get; set; }
        // tong nghiem thu noi bo cua hop dong
        public Decimal totalAmountAcceptanceNoiBo { get; set; }
        // tong don vi da tam ung cua hop dong
        public Decimal totalAmountUnitAdvance { get; set; }
        // tong da quyet toan cua hop dong
        public Decimal totalAmountContractSettlement { get; set; }
        // khoan giu lai cua hop dong da quyet toan, thanh ly hoac ket thuc
        public Decimal totalAmountContractHolding { get; set; }
        public int contractStatus { get; set; }
    }  
}
