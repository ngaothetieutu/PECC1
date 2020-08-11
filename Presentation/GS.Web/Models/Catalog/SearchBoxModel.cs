using GS.Web.Framework.Models;

namespace GS.Web.Models.Catalog
{
    public partial class SearchBoxModel : BaseGSModel
    {
        public bool AutoCompleteEnabled { get; set; }
        public bool ShowProductImagesInSearchAutoComplete { get; set; }
        public int SearchTermMinimumLength { get; set; }
    }
}