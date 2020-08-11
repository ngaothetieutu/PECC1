using System.Collections.Generic;
using FluentValidation.Attributes;
using GS.Web.Areas.Admin.Validators.Tasks;
using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;

namespace GS.Web.Areas.Admin.Models.Tasks
{
    [Validator(typeof(TaskAttributeValidator))]
    public partial class TaskAttributeModel : BaseGSEntityModel, ILocalizedModel<TaskAttributeLocalizedModel>
    {
        #region Ctor
        public TaskAttributeModel()
        {
            Locales = new List<TaskAttributeLocalizedModel>();
            TaskAttributeValueSearchModel = new TaskAttributeValueSearchModel();
        }
        #endregion

        #region Properties
        [GSResourceDisplayName("Admin.Task.TaskAttributes.Fields.Name")]
        public string Name { get; set; }

        [GSResourceDisplayName("Admin.Task.TaskAttributes.Fields.IsRequired")]
        public bool IsRequired { get; set; }

        [GSResourceDisplayName("Admin.Task.TaskAttributes.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [GSResourceDisplayName("Admin.Task.TaskAttributes.Fields.TaskAttributeld")]
        public int AttributeControlTypeId { get; set; }

        [GSResourceDisplayName("Admin.Task.TaskAttributes.Fields.TaskAttributeName")]
        public string AttributeControlTypeName { get; set; }

        public IList<TaskAttributeLocalizedModel> Locales { get; set; }

        public TaskAttributeValueSearchModel TaskAttributeValueSearchModel { get; set; }
        #endregion
    }

    public partial class TaskAttributeLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [GSResourceDisplayName("Admin.Contract.ContractAttributes.Fields.Name")]
        public string Name { get; set; }
    }
}
