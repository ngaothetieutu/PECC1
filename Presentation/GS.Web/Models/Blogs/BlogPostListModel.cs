﻿using System.Collections.Generic;
using GS.Web.Framework.Models;

namespace GS.Web.Models.Blogs
{
    public partial class BlogPostListModel : BaseGSModel
    {
        public BlogPostListModel()
        {
            PagingFilteringContext = new BlogPagingFilteringModel();
            BlogPosts = new List<BlogPostModel>();
        }

        public int WorkingLanguageId { get; set; }
        public BlogPagingFilteringModel PagingFilteringContext { get; set; }
        public IList<BlogPostModel> BlogPosts { get; set; }
    }
}