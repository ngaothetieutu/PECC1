using FluentValidation.Attributes;
using GS.Web.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Models.Works
{ 
    public class TaskContractModel : BaseGSEntityModel
    {
        public Int32 TaskId { get; set; }
        public Int32 ContractId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int32 CreatorId { get; set; }    
        public string Note { get; set; }
    }
}
