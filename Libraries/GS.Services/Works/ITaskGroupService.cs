using GS.Core.Domain.Works;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Services.Works
{
    public interface ITaskGroupService
    {
        #region TaskGroup

        /// <summary>
        /// Delete the review type
        /// </summary>
        /// <param name="item"> Task Group</param>
        void DeleteTaskGroup(TaskGroup item);

        /// <summary>
        /// Get all review types
        /// </summary>
        /// <returns> Task Groups</returns>
        IList<TaskGroup> GetAllTaskGroups();

        IList<TaskGroup> GetAllTaskGroupsByName(string Name = "",int? ParentId =0);

        /// <summary>
        /// Get the review type 
        /// </summary>
        /// <param name="itemId"> Task Group identifier</param>
        /// <returns> Task Group</returns>
        TaskGroup GetTaskGroupById(int itemId);

        /// <summary>
        /// Insert the review type
        /// </summary>
        /// <param name="item">Task Group</param>
        void InsertTaskGroup(TaskGroup item);

        /// <summary>
        /// Update the review type
        /// </summary>
        /// <param name="item"> Task Group</param>
        void UpdateTaskGroup(TaskGroup item);
        int TaskGroupChildCount(int Id);
        #endregion
    }
}
