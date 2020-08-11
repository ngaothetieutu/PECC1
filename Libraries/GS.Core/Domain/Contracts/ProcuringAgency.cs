using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public enum ProcuringAgencyType
    {
        All=0,
        /// <summary>
        /// PECC1
        /// </summary>
        PECC1=1,
        /// <summary>
        /// Chu dau tu
        /// </summary>
        Main=2,
        /// <summary>
        /// Lien danh
        /// </summary>
        JointVenture =3
    }    
    public enum  ProcuringAgencyId
    {
        Pecc1 = 1
    }
    public class ProcuringAgency : BaseEntity
    {
        public Int32 TypeId { get; set; }
        public ProcuringAgencyType procuringAgencyType
        {
            get => (ProcuringAgencyType)TypeId;
            set => TypeId = (int)value;
        }
        public String Name { get; set; }
        public String Representer { get; set; }
        public String Postion { get; set; }
        public String CompanyAddress { get; set; }
        public String TaxCode { get; set; }
        public String TaxInfo { get; set; }
        public String CompanyFax { get; set; }
        public String CompanyPhone { get; set; }
        public String CompanyEmail { get; set; }
        public String BankInfo { get; set; }
        public String Description { get; set; }
        public Boolean IsInEVN { get; set; }
    }
}
