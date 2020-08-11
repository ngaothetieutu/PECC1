﻿using GS.Web.Framework.Models;

namespace GS.Web.Models.Common
{
    public partial class SocialModel : BaseGSModel
    {
        public string FacebookLink { get; set; }
        public string TwitterLink { get; set; }
        public string YoutubeLink { get; set; }
        public string GooglePlusLink { get; set; }
        public int WorkingLanguageId { get; set; }
        public bool NewsEnabled { get; set; }
    }
}