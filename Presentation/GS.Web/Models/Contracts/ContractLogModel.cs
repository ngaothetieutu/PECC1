using FluentValidation.Attributes;
using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Validators.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Models.Contracts
{
    [Validator(typeof(ContractLogValidator))]
    public class ContractLogModel : BaseGSEntityModel
    {
        [GSResourceDisplayName("AppWork.Contracts.ContractLog.Fields.ContractId")]
        public int ContractId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractLog.Fields.CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractLog.Fields.CreatorId")]
        public int CreatorId { get; set; }
        public string UserName { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractLog.Fields.Note")]
        public string Note { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractLog.Fields.ContractData")]
        public string ContractData { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractLog.Fields.PeriodId")]
        public int PeriodId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractLog.Fields.UserFullName")]
        public string UserFullName { get; set; }
        public string CustomerAvatar { get; set; }

    }
    public partial class ContractLogListModel : BasePagedListModel<ContractLogModel>
    {

    }
    public partial class ContractLogSearchModel : BaseSearchModel
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public string UserName { get; set; }
        public string Note { get; set; }
    }
}
