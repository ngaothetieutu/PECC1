using System.Collections.Generic;
using FluentValidation.Attributes;
using GS.Web.Areas.Admin.Validators.Tasks;
using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;

namespace GS.Web.Areas.Admin.Models.Tasks
{
    [Validator(typeof(TaskAttributeValueValidator))]
    public partial class TaskAttributeValueModel : BaseGSEntityModel, ILocalizedModel<TaskAttributeValueLocalizedModel>
    {
        #region Ctor
        public TaskAttributeValueModel()
        {
            Locales = new List<TaskAttributeValueLocalizedModel>();
        }
        #endregion

        #region Properties
        public int TaskAttributeId { get; set; }

        [GSResourceDisplayName("Admin.Task.TaskAttributes.Values.Fields.Name")]
        public string Name { get; set; }

        [GSResourceDisplayName("Admin.Task.TaskAttributes.Values.Fields.IsPreSelected")]
        public bool IsPreSelected { get; set; }

        [GSResourceDisplayName("Admin.Task.TaskAttributes.Values.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<TaskAttributeValueLocalizedModel> Locales { get; set; }
        #endregion
    }

    public partial class TaskAttributeValueLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [GSResourceDisplayName("Admin.Task.TaskAttributes.Values.Fields.Name")]
        public string Name { get; set; }
    }
}
