using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{
    public class ContractJoint : BaseEntity
    {
        public Int32 ContractId { get; set; }
        public virtual Contract contract { get; set; }
        public Int32 ProcuringAgencyId { get; set; }
        /// <summary>
        /// Thong tin chu dau tu tu danh muc
        /// </summary>
        public virtual ProcuringAgency curProcuringAgency { get; set; }

        public String ProcuringAgencyData { get; set; }
        
        public Boolean IsSideA { get; set; }
        public Int32 DisplayOrder { get; set; }
        public Boolean IsMain { get; set; }
    }
}
