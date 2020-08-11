using GS.Core.Domain.Works;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Contracts
{    
    public enum ContractFileType
    {
        /// <summary>
        /// tat cat
        /// </summary>
        All=0,
        Common=1,
        Assignment=2,
        /// <summary>
        /// nghiem thu cong viec
        /// </summary>
        TaskAcceptance=3,
        /// <summary>
        /// nghiem thu thanh toan
        /// </summary>
        PaymentAcceptance=4,
        /// <summary>
        /// thanh ly hop dong
        /// </summary>
        LiquidationContract = 5,
        /// <summary>
        /// ket thu hop dong 
        /// </summary>
        FinishContract = 6 ,
        /// <summary>
        /// nghiem thu BB
        /// </summary>
        BBAcceaptance = 7,
        /// <summary>
        /// quyet toan hop dong
        /// </summary>
        Settlement = 8,
        /// <summary>
        /// nghiem thu noi bo
        /// </summary>
        AcceptanceNoiBo = 9,
    }
    public class ContractFile : BaseEntity
    {
        public Int32 ContractId { get; set; }
        public Int32 FileId { get; set; }
        public virtual WorkFile workFile { get; set; }
        public int TypeId { get; set; }
        public ContractFileType contractFileType
        {
            get
            {
                return (ContractFileType)TypeId;
            }
            set
            {
                TypeId = (int)value;
            }
        }
        public string EntityId { get; set; }
    }
}
