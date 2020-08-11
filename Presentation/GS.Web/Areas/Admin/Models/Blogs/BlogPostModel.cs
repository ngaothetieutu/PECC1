using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using GS.Web.Areas.Admin.Validators.Blogs;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Framework.Models;

namespace GS.Web.Areas.Admin.Models.Blogs
{
    /// <summary>
    /// Represents a blog post model
    /// </summary>
    [Validator(typeof(BlogPostValidator))]
    public partial class BlogPostModel : BaseGSEntityModel, IStoreMappingSupportedModel
    {
        #region Ctor

        public BlogPostModel()
        {
            AvailableLanguages = new List<SelectListItem>();
            SelectedStoreIds = new List<int>();
            AvailableStores = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [GSResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.Language")]
        public int LanguageId { get; set; }

        public IList<SelectListItem> AvailableLanguages { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.Language")]
        public string LanguageName { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.Title")]
        public string Title { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.Body")]
        public string Body { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.BodyOverview")]
        public string BodyOverview { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.AllowComments")]
        public bool AllowComments { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.Tags")]
        public string Tags { get; set; }

        public int ApprovedComments { get; set; }

        public int NotApprovedComments { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.StartDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? StartDate { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.EndDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? EndDate { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.SeName")]
        public string SeName { get; set; }

        [GSResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        //store mapping
        [GSResourceDisplayName("Admin.ContentManagement.Blog.BlogPosts.Fields.LimitedToStores")]
        public IList<int> SelectedStoreIds { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }

        #endregion
    }
}