using System.Collections.Generic;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using GS.Web.Areas.Admin.Validators.Topics;
using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;

namespace GS.Web.Areas.Admin.Models.Topics
{
    /// <summary>
    /// Represents a topic model
    /// </summary>
    [Validator(typeof(TopicValidator))]
    public partial class TopicModel : BaseGSEntityModel, IAclSupportedModel, ILocalizedModel<TopicLocalizedModel>, IStoreMappingSupportedModel
    {
        #region Ctor

        public TopicModel()
        {
            AvailableTopicTemplates = new List<SelectListItem>();
            Locales = new List<TopicLocalizedModel>();

            SelectedCustomerRoleIds = new List<int>();
            AvailableCustomerRoles = new List<SelectListItem>();

            SelectedStoreIds = new List<int>();
            AvailableStores = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.SystemName")]
        public string SystemName { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.IncludeInSitemap")]
        public bool IncludeInSitemap { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.IncludeInTopMenu")]
        public bool IncludeInTopMenu { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.IncludeInFooterColumn1")]
        public bool IncludeInFooterColumn1 { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.IncludeInFooterColumn2")]
        public bool IncludeInFooterColumn2 { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.IncludeInFooterColumn3")]
        public bool IncludeInFooterColumn3 { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.AccessibleWhenStoreClosed")]
        public bool AccessibleWhenStoreClosed { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.IsPasswordProtected")]
        public bool IsPasswordProtected { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.Password")]
        public string Password { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.URL")]
        public string Url { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.Title")]
        public string Title { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.Body")]
        public string Body { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.Published")]
        public bool Published { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.TopicTemplate")]
        public int TopicTemplateId { get; set; }

        public IList<SelectListItem> AvailableTopicTemplates { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.SeName")]
        public string SeName { get; set; }

        public IList<TopicLocalizedModel> Locales { get; set; }

        //store mapping
        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.LimitedToStores")]
        public IList<int> SelectedStoreIds { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }

        //ACL (customer roles)
        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.AclCustomerRoles")]
        public IList<int> SelectedCustomerRoleIds { get; set; }

        public IList<SelectListItem> AvailableCustomerRoles { get; set; }

        #endregion
    }

    public partial class TopicLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.Title")]
        public string Title { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.Body")]
        public string Body { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Topics.Fields.SeName")]
        public string SeName { get; set; }
    }
}