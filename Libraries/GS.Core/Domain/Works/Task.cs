using GS.Core.Domain.Contracts;
using GS.Core.Domain.Customers;
using GS.Core.Domain.Directory;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Works
{
    /// <summary>
    /// 1: draf, 2: approved, 4: destroy
    /// </summary>
    public enum TaskStatus
    {
        All=0,
        /// <summary>
        /// Nháp
        /// </summary>
        Draf = 1,
        /// <summary>
        /// Chờ duyệt
        /// </summary>
        Pending = 2,
        /// <summary>
        /// đang thục hiện
        /// </summary>
        Working = 3,
        /// <summary>
        /// Hủy
        /// </summary>
        Destroy = 4,
        /// <summary>
        /// Destroy by appendix
        /// </summary>
        Stop = 5,
        /// <summary>
        /// hoàn thành
        /// </summary>
        Complete = 6,             
        /// <summary>
        /// đã quyết toán
        /// </summary>
        Settlemented = 7,
        /// <summary>
        /// đã nghiem thu khoi luong
        /// </summary>
        Acceptance = 8,
    }
    /// <summary>
    /// cap dia hinh
    /// </summary>
    public enum ConTractTaskLeverId
    {
        NoLevel=0,
        /// <summary>
        /// cap I
        /// </summary>
        I = 1,
        /// <summary>
        /// Cap 2
        /// </summary>
        II = 2,
        /// <summary>
        /// Cap 3
        /// </summary>
        III = 3,
        /// <summary>
        /// cap 4
        /// </summary>
        IV = 4
    }
   
    public class Task : BaseEntity
    {
        private ICollection<TaskContract> _taskContracts;
        private ICollection<ContractSettlementTaskMapping> _contractSettlementTaskMappings;
        public Task()
        {
            this.TaskGuid = Guid.NewGuid();
            this.IsTemplate = false;
            this.taskStatus = TaskStatus.Draf;
            this.StoreId = 1;
            this.TypeId = 1;
            this.TreeLevel = 1;
            this.TreeNode = "1";
            //this.Code = "1";
        }
        public Guid TaskGuid { get; set; }
        public String Code { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public Int32 CreatorId { get; set; }
        public virtual Customer creator { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int32 ContractId { get; set; }
        public virtual Contract contract { get; set; }
        public Int32? ParentId { get; set; }
        public virtual Task parentTask { get; set; }
        public String TreeNode { get; set; }
        public Int32 TreeLevel { get; set; }
        public Int32? ApproverId { get; set; }
        public virtual Customer approver { get; set; }
        public Int32 StatusId { get; set; }
        public TaskStatus taskStatus
        {
            get => (TaskStatus)StatusId;
            set => StatusId = (int)value;
        }
        public Int32? PreTaskId { get; set; }
        public virtual Task preTask { get; set; }
        public DateTime? StartDate { get; set; }
        public Int32? DurationTime { get; set; }
        public Int32? DurationUnit { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? FinishedDate { get; set; }
        /// <summary>
        /// Nguoi chiu trach nhiem
        /// </summary>
        public Int32? ResponsibleId { get; set; }
        public virtual Customer responsible { get; set; }
        public Int32 StoreId { get; set; }
        public Int32? UnitId { get; set; }
        public virtual Unit unitInfo { get; set; }
        public Int32? MeasureId { get; set; }
        public virtual MeasureDimension measureDimension { get; set; }
        public Decimal? MeasureMass { get; set; }
        public Decimal? MeasurePrice { get; set; }
        public Decimal? TotalMoney { get; set; }
        public Decimal? PercentNum { get; set; }
        public String Note { get; set; }
        public Int32 TaskLevelId { get; set; }
        public ConTractTaskLeverId taskLevelId
        {
            get => (ConTractTaskLeverId)TaskLevelId;
            set => TaskLevelId = (int)value;
        }
        public Int32 TypeId { get; set; }
        public Boolean IsTemplate { get; set; }
        public Int32? CurrencyId { get; set; }
        public virtual Currency currency { get; set; }
        public Int32? TaskGroupId { get; set; }
        public virtual TaskGroup taskGroup { get; set; }
        public decimal? TaskGroupValue { get; set; }
        public decimal? TaskGroupSalary { get; set; }
        public decimal? TaskGroupCost { get; set; }
        public int? ContractTypeId { get; set; }
        public virtual ContractType contractType { get; set; }
        public decimal? TaxPercent { get; set; }
        public int? TaskProcuringAgencyId { get; set; }
        public virtual ProcuringAgency procuringAgency { get; set; }
        public virtual ICollection<TaskContract> TaskContracts
        {
            get => _taskContracts ?? (_taskContracts = new List<TaskContract>());
            protected set => _taskContracts = value;
        }
        public virtual ICollection<ContractSettlementTaskMapping> ContractSettlementTaskMappings
        {
            get => _contractSettlementTaskMappings ?? (_contractSettlementTaskMappings = new List<ContractSettlementTaskMapping>());
            protected set => _contractSettlementTaskMappings = value;
        }
    }
}
