using GS.Core.Domain.Contracts;
using GS.Core.Domain.Works;
using GS.Web.Models.Contracts;
using GS.Web.Models.Works;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = GS.Core.Domain.Works.Task;

namespace GS.Web.Factories
{
    public interface ITaskModelFactory
    {
        #region Task Group
        TaskGroupSearchModel PrepareTaskGroupSearchModel(TaskGroupSearchModel searchModel);
        TaskGroupListModel PrepareTaskGroupListModel(TaskGroupSearchModel searchModel);
        TaskGroupModel PrepareTaskGroupModel(TaskGroupModel model, TaskGroup item, bool excludeProperties = false);
        #endregion
        #region Task
        TaskSearchModel PrepareTaskSearchModel(TaskSearchModel searchModel);
        TaskListModel PrepareTaskListModel(TaskSearchModel searchModel);       
        TaskModel PrepareTaskModel(TaskModel model, Task item, bool excludeProperties = false);       
        TaskModel PrepareTaskModelList(Task item);
        void PrepareTask(Task item, TaskModel model);
        TaskModel PrepareDelete(int TaskId);
        void InsertContractLog(Task item);
        void InsertContractLogDelete(Task item);
        void InsertTaskLog(TaskLogModel model);
        void PrepareDeadline(TaskModel model, Task item);
        void PrepareUnfinished(Task item, TaskModel model = null);
        bool CheckTaskEndDate(TaskModel model);
        bool CheckTaskStartDate(TaskModel model);
        #endregion
        #region ContractAcceptance
        ContractAcceptanceListModel PrepareListContractAcceptanceTasks(ContractAcceptanceSearchModel searchmodel);
        TaskListModel PrepareListTaskContractAcceptances(TaskSearchModel searchModel);
        #endregion
    }
}
