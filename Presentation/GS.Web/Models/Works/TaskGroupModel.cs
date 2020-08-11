using FluentValidation.Attributes;
using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Validators.Works;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Models.Works
{
    [Validator(typeof(TaskGroupValidator))]
    public class TaskGroupModel: BaseGSEntityModel
    {
        [GSResourceDisplayName("AppWork.Works.TaskGroup.Fields.Name")]
        public string Name { get; set; }
        [GSResourceDisplayName("AppWork.Works.TaskGroup.Fields.Description")]
        public string Description { get; set; }
        [GSResourceDisplayName("AppWork.Works.TaskGroup.Fields.Ratio")]
        public decimal Ratio { get; set; }
        [GSResourceDisplayName("AppWork.Works.TaskGroup.Fields.ParentId")]
        public int ParentId { get; set; }
        [GSResourceDisplayName("AppWork.Works.TaskGroup.Fields.TreeNode")]
        public string TreeNode { get; set; }
        [GSResourceDisplayName("AppWork.Works.TaskGroup.Fields.TreeLevel")]
        public int TreeLevel { get; set; }
        //add more
        public List<SelectListItem> AvaibleTaskGroupList { get; set; }
        public string ParentName { get; set; }
        public int CountChild { get; set; }
    }
    public partial class TaskGroupListModel : BasePagedListModel<TaskGroupModel>
    {

    }
    public partial class TaskGroupSearchModel : BaseSearchModel
    {
        [GSResourceDisplayName("AppWork.Works.TaskGroup.Fields.Name")]
        public string Name { get; set; }
        public int? ParentId { get; set; }
    }
    
}
