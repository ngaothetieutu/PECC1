using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using GS.Web.Areas.Admin.Validators.Messages;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Framework.Models;

namespace GS.Web.Areas.Admin.Models.Messages
{
    /// <summary>
    /// Represents a newsletter subscription model
    /// </summary>
    [Validator(typeof(NewsLetterSubscriptionValidator))]
    public partial class NewsletterSubscriptionModel : BaseGSEntityModel
    {
        #region Properties

        [DataType(DataType.EmailAddress)]
        [GSResourceDisplayName("Admin.Promotions.NewsLetterSubscriptions.Fields.Email")]
        public string Email { get; set; }

        [GSResourceDisplayName("Admin.Promotions.NewsLetterSubscriptions.Fields.Active")]
        public bool Active { get; set; }

        [GSResourceDisplayName("Admin.Promotions.NewsLetterSubscriptions.Fields.Store")]
        public string StoreName { get; set; }

        [GSResourceDisplayName("Admin.Promotions.NewsLetterSubscriptions.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}