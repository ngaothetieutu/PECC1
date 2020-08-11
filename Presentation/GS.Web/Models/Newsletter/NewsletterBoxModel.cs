using System.ComponentModel.DataAnnotations;
using GS.Web.Framework.Models;

namespace GS.Web.Models.Newsletter
{
    public partial class NewsletterBoxModel : BaseGSModel
    {
        [DataType(DataType.EmailAddress)]
        public string NewsletterEmail { get; set; }
        public bool AllowToUnsubscribe { get; set; }
    }
}