using GS.Core.Domain.Contracts;
using GS.Core.Domain.Works;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Services.Works
{
    public interface IWorkTaskService
    {
        #region Task
        /// <summary>
        /// Delete the review type
        /// </summary>
        /// <param name="item">Construction type</param>
        void DeleteTask(Task item);

        /// <summary>
        /// Get all review types
        /// </summary>
        /// <returns>Construction types</returns>
        IList<Task> GetAllTasks();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ContractId"></param>
        /// <returns></returns>
        IList<Task> getTasksByConTractId(int ContractId = 0, int ParentId = 0, bool includParent = false, bool isAll = false,int ContractTypeId =0);
        /// <summary>
        /// Get the review type 
        /// </summary>
        /// <param name="itemId">Construction type identifier</param>
        /// <returns>Construction type</returns>
        Task GetTaskById(int itemId);

        /// <summary>
        /// Insert the review type
        /// </summary>
        /// <param name="item">Construction type</param>
        void InsertTask(Task item);

        /// <summary>
        /// Update the review type
        /// </summary>
        /// <param name="item">Construction type</param>
        void UpdateTask(Task item);
        void UpdateTaskComplete(Task item);
        void UpdateTaskAmount(int TaskId);

        int CountTaskChild(int TaskId);
        int CountTask(int CustomerId, TaskStatus taskStatus = TaskStatus.All);
        IList<Task> GetTaskDeadLine(int contractId);
        /// <summary>
        /// Lay thong tin cac loai tien te trong hop dong
        /// </summary>
        /// <param name="ContractId"></param>
        /// <returns></returns>
        IList<ContractCurrency> GetContractCurrencies(int ContractId);
        IList<Task> GetTaskbyListTaskIds(IList<int> ListTaskIds);
        IList<Task> getAllTaskbyUnit(int UnitId, int ContractId = 0);
        IList<Task> GetAllTaskByContractIdAndStartdate(int ContractId);
        IList<Task> GetAllTasksByAprovalDatePaymentAcceptance(int unitId, int ContractId, string approvalDateString);
        IList<Task> GetAllTaskContact(int contractId);
        Task GetTaskByTaskProcuringAgencyId(int TaskProcuringAgencyId);
        IList<Task> GetAllTasksByContractDestroyId(int ContractId);
        #endregion
        #region TaskLog       
        /// <summary>
        /// Get all review types
        /// </summary>
        /// <returns>Construction types</returns>
        IList<TaskLog> GetAllTaskLogs();

        /// <summary>
        /// Get the review type 
        /// </summary>
        /// <param name="itemId">Construction type identifier</param>
        /// <returns>Construction type</returns>
        TaskLog GetTaskLogById(int itemId);

        /// <summary>
        /// Insert the review type
        /// </summary>
        /// <param name="item">Construction type</param>
        void InsertTaskLog(TaskLog item);

        /// <summary>
        /// Update the review type
        /// </summary>
        /// <param name="item">Construction type</param>
        #endregion
        #region ContractAcceptanceTask
        IList<Task> getallTaskHasContractAcceptance(int ContractId, int ContractAcceptanceId = 0);
        #endregion
        #region Task Resource
        void InsertTaskResource(TaskResource item);
        void DeleteTaskResource(int ItemId);
        IList<TaskResource> GetTaskResources(int TaskId, int CustomerId = 0);
        TaskResource GetTaskResourceById(int ItemId);
        void DeleteListTaskResource(int TaskId, int CustomerId = 0);
        void UpdateTaskResource(TaskResource item);
        #endregion
        #region TaskContract
        void UpdateContractAmountSummary(int ContractId);
        void InsertTaskContract(TaskContract item);
        void deleteTaskContract(TaskContract item);
        IList<TaskContract> getTaskContract(int TaskId =0,int ContractId = 0);
        TaskContract GetTaskContractById(int ItemId);
        TaskContract GetTaskContractByTaskId(int TaskId);
        TaskContract GetTaskContractByContractId(int ContractId);
        IList<TaskContract> GetAllTaskContractByTaskId(int TaskId);
        IList<TaskContract> GetListTaskContractByContractId(int ContractId);
        IList<Task> getTaskByContractIdForContractSettlement(int ContractId = 0);
        #endregion
    }
}
