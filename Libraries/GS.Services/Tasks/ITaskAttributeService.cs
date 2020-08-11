using System.Collections.Generic;
using GS.Core.Domain.Tasks;

namespace GS.Services.Tasks
{
    public partial interface ITaskAttributeService
    {
        #region Task Attribute
        void DeleteTaskAttribute(TaskAttribute taskAttribute);

        IList<TaskAttribute> GetAllTaskAttributes();

        TaskAttribute GetTaskAttributeById(int taskAttributeId);

        void InsertTaskAttribute(TaskAttribute taskAttribute);

        void UpdateTaskAttribute(TaskAttribute taskAttribute);
        #endregion

        #region Task Attribute Value
        void DeleteTaskAttributeValue(TaskAttributeValue taskAttributeValue);

        IList<TaskAttributeValue> GetTaskAttributeValues(int taskAttributeId);

        TaskAttributeValue GetTaskAttributeValueById(int taskAttributeValueId);

        void InsertTaskAttributeValue(TaskAttributeValue taskAttributeValue);

        void UpdateTaskAttributeValue(TaskAttributeValue taskAttributeValue);
        #endregion
    }
}
