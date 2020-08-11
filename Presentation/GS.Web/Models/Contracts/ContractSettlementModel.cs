using FluentValidation.Attributes;
using GS.Core.Domain.Contracts;
using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Validators.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Models.Contracts
{
    [Validator(typeof(ContractSettlementValidator))]
    public class ContractSettlementModel : BaseGSEntityModel
    {
        public ContractSettlementModel()
        {
            AvailableTaskList = new List<SelectListItem>();
            SelectedListTaskId = new List<int>();
            SelectedListFileId = new List<int>();
            lstSubModel = new List<ContractSettlementSubModel>();
        }
        [GSResourceDisplayName("AppWork.Contracts.ContractSettlement.Fields.Code")]
        public String Code { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractSettlement.Fields.Name")]
        public String Name { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractSettlement.Fields.Description")]
        public String Description { get; set; }
        public Int32 CreatorId { get; set; }
        public DateTime CreatedDate { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractSettlement.Fields.ContractId")]
        public Int32 ContractId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractSettlement.Fields.ApprovalId")]
        public Int32? ApproverId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractSettlement.Fields.ApprovalDate")]
        [UIHint("DateNullable")]
        public DateTime? ApprovalDate { get; set; }
        public Int32 StatusId { get; set; }
        public ContractSettlementStatusId contractAcceptanceStatus
        {
            get => (ContractSettlementStatusId)StatusId;
            set => StatusId = (int)value;
        }
        [GSResourceDisplayName("AppWork.Contracts.ContractSettlement.Fields.AvailableTaskList")]
        public List<SelectListItem> AvailableTaskList { get; set; }
        public IList<int> SelectedListTaskId { get; set; }
        public int TaskGroupId { get; set; }
        public Int32? TaskId { get; set; }
        [GSResourceDisplayName("AppWork.Contracts.ContractSettlement.Fields.File")]
        [UIHint("WorkFile")]
        public string WorkFileIds { get; set; }
        public IList<int> SelectedListFileId { get; set; }
        public string taskName { get; set; }
        public String taskTotalMoney { get; set; }
        public IList<ContractSettlementSubModel> lstSubModel { get; set; }
        ///<summary>
        ///tong tien cua quyet toan
        /// </summary>
        public Decimal TotalAmountSettlement { get; set; }
    }
}
