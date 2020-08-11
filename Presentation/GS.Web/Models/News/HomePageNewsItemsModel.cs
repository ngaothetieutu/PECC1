using System;
using System.Collections.Generic;
using GS.Web.Framework.Models;

namespace GS.Web.Models.News
{
    public partial class HomePageNewsItemsModel : BaseGSModel, ICloneable
    {
        public HomePageNewsItemsModel()
        {
            NewsItems = new List<NewsItemModel>();
        }

        public int WorkingLanguageId { get; set; }
        public IList<NewsItemModel> NewsItems { get; set; }

        public object Clone()
        {
            //we use a shallow copy (deep clone is not required here)
            return MemberwiseClone();
        }
    }
}