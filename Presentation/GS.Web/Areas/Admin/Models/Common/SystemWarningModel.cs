using GS.Web.Framework.Models;

namespace GS.Web.Areas.Admin.Models.Common
{
    public partial class SystemWarningModel : BaseGSModel
    {
        public SystemWarningLevel Level { get; set; }

        public string Text { get; set; }

        public bool DontEncode { get; set; }
    }

    public enum SystemWarningLevel
    {
        Pass,
        Recommendation,
        CopyrightRemovalKey,
        Warning,
        Fail
    }
}