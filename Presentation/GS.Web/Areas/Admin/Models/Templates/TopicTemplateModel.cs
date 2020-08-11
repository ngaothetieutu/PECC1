using FluentValidation.Attributes;
using GS.Web.Areas.Admin.Validators.Templates;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Framework.Models;

namespace GS.Web.Areas.Admin.Models.Templates
{
    /// <summary>
    /// Represents a topic template model
    /// </summary>
    [Validator(typeof(TopicTemplateValidator))]
    public partial class TopicTemplateModel : BaseGSEntityModel
    {
        #region Properties

        [GSResourceDisplayName("Admin.System.Templates.Topic.Name")]
        public string Name { get; set; }

        [GSResourceDisplayName("Admin.System.Templates.Topic.ViewPath")]
        public string ViewPath { get; set; }

        [GSResourceDisplayName("Admin.System.Templates.Topic.DisplayOrder")]
        public int DisplayOrder { get; set; }

        #endregion
    }
}