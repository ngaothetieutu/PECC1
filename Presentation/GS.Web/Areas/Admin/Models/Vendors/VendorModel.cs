﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using GS.Core.Domain.Catalog;
using GS.Web.Areas.Admin.Models.Common;
using GS.Web.Areas.Admin.Validators.Vendors;
using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;

namespace GS.Web.Areas.Admin.Models.Vendors
{
    /// <summary>
    /// Represents a vendor model
    /// </summary>
    [Validator(typeof(VendorValidator))]
    public partial class VendorModel : BaseGSEntityModel, ILocalizedModel<VendorLocalizedModel>
    {
        #region Ctor

        public VendorModel()
        {
            if (PageSize < 1)
                PageSize = 5;

            Address = new AddressModel();
            VendorAttributes = new List<VendorAttributeModel>();
            Locales = new List<VendorLocalizedModel>();
            AssociatedCustomers = new List<VendorAssociatedCustomerModel>();
            VendorNoteSearchModel = new VendorNoteSearchModel();
        }

        #endregion

        #region Properties

        [GSResourceDisplayName("Admin.Vendors.Fields.Name")]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [GSResourceDisplayName("Admin.Vendors.Fields.Email")]
        public string Email { get; set; }

        [GSResourceDisplayName("Admin.Vendors.Fields.Description")]
        public string Description { get; set; }

        [UIHint("Picture")]
        [GSResourceDisplayName("Admin.Vendors.Fields.Picture")]
        public int PictureId { get; set; }

        [GSResourceDisplayName("Admin.Vendors.Fields.AdminComment")]
        public string AdminComment { get; set; }

        public AddressModel Address { get; set; }

        [GSResourceDisplayName("Admin.Vendors.Fields.Active")]
        public bool Active { get; set; }

        [GSResourceDisplayName("Admin.Vendors.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }        

        [GSResourceDisplayName("Admin.Vendors.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [GSResourceDisplayName("Admin.Vendors.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [GSResourceDisplayName("Admin.Vendors.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [GSResourceDisplayName("Admin.Vendors.Fields.SeName")]
        public string SeName { get; set; }

        [GSResourceDisplayName("Admin.Vendors.Fields.PageSize")]
        public int PageSize { get; set; }

        [GSResourceDisplayName("Admin.Vendors.Fields.AllowCustomersToSelectPageSize")]
        public bool AllowCustomersToSelectPageSize { get; set; }

        [GSResourceDisplayName("Admin.Vendors.Fields.PageSizeOptions")]
        public string PageSizeOptions { get; set; }

        public List<VendorAttributeModel> VendorAttributes { get; set; }

        public IList<VendorLocalizedModel> Locales { get; set; }

        [GSResourceDisplayName("Admin.Vendors.Fields.AssociatedCustomerEmails")]
        public IList<VendorAssociatedCustomerModel> AssociatedCustomers { get; set; }

        //vendor notes
        [GSResourceDisplayName("Admin.Vendors.VendorNotes.Fields.Note")]
        public string AddVendorNoteMessage { get; set; }

        public VendorNoteSearchModel VendorNoteSearchModel { get; set; }

        #endregion

        #region Nested classes
        
        public partial class VendorAttributeModel : BaseGSEntityModel
        {
            public VendorAttributeModel()
            {
                Values = new List<VendorAttributeValueModel>();
            }

            public string Name { get; set; }

            public bool IsRequired { get; set; }

            /// <summary>
            /// Default value for textboxes
            /// </summary>
            public string DefaultValue { get; set; }

            public AttributeControlType AttributeControlType { get; set; }

            public IList<VendorAttributeValueModel> Values { get; set; }
        }

        public partial class VendorAttributeValueModel : BaseGSEntityModel
        {
            public string Name { get; set; }

            public bool IsPreSelected { get; set; }
        }

        #endregion
    }

    public partial class VendorLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [GSResourceDisplayName("Admin.Vendors.Fields.Name")]
        public string Name { get; set; }

        [GSResourceDisplayName("Admin.Vendors.Fields.Description")]
        public string Description { get; set; }

        [GSResourceDisplayName("Admin.Vendors.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [GSResourceDisplayName("Admin.Vendors.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [GSResourceDisplayName("Admin.Vendors.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [GSResourceDisplayName("Admin.Vendors.Fields.SeName")]
        public string SeName { get; set; }
    }
}