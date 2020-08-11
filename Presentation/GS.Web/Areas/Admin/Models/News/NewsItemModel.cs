using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using GS.Web.Areas.Admin.Validators.News;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Framework.Models;

namespace GS.Web.Areas.Admin.Models.News
{
    /// <summary>
    /// Represents a news item model
    /// </summary>
    [Validator(typeof(NewsItemValidator))]
    public partial class NewsItemModel : BaseGSEntityModel, IStoreMappingSupportedModel
    {
        #region Ctor

        public NewsItemModel()
        {
            this.AvailableLanguages = new List<SelectListItem>();

            SelectedStoreIds = new List<int>();
            AvailableStores = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [GSResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.Language")]
        public int LanguageId { get; set; }

        public IList<SelectListItem> AvailableLanguages { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.Language")]
        public string LanguageName { get; set; }

        //store mapping
        [GSResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.LimitedToStores")]
        public IList<int> SelectedStoreIds { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.Title")]
        public string Title { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.Short")]
        public string Short { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.Full")]
        public string Full { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.AllowComments")]
        public bool AllowComments { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.StartDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? StartDate { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.EndDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? EndDate { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.SeName")]
        public string SeName { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.Published")]
        public bool Published { get; set; }

        public int ApprovedComments { get; set; }

        public int NotApprovedComments { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.News.NewsItems.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}