using GS.Web.Framework.Models;

namespace GS.Web.Models.Common
{
    public partial class LanguageModel : BaseGSEntityModel
    {
        public string Name { get; set; }

        public string FlagImageFileName { get; set; }
    }
}