using GS.Web.Framework.Models;

namespace GS.Web.Models.Common
{
    public partial class LogoModel : BaseGSModel
    {
        public string StoreName { get; set; }

        public string LogoPath { get; set; }
    }
}