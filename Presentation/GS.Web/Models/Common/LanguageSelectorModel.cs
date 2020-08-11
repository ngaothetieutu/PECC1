using System.Collections.Generic;
using GS.Web.Framework.Models;

namespace GS.Web.Models.Common
{
    public partial class LanguageSelectorModel : BaseGSModel
    {
        public LanguageSelectorModel()
        {
            AvailableLanguages = new List<LanguageModel>();
        }

        public IList<LanguageModel> AvailableLanguages { get; set; }

        public int CurrentLanguageId { get; set; }

        public bool UseImages { get; set; }
    }
}