using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using GS.Core;
using GS.Core.Domain.Catalog;
using GS.Core.Domain.Common;
using GS.Core.Domain.Contracts;
using GS.Core.Domain.Customers;
using GS.Core.Domain.Directory;
using GS.Core.Domain.Forums;
using GS.Core.Domain.Gdpr;
using GS.Core.Domain.Messages;
using GS.Core.Domain.Tax;
using GS.Core.Domain.Vendors;
using GS.Services.Catalog;
using GS.Services.Common;
using GS.Services.Customers;
using GS.Services.Directory;
using GS.Services.ExportImport.Help;
using GS.Services.Forums;
using GS.Services.Gdpr;
using GS.Services.Helpers;
using GS.Services.Localization;
using GS.Services.Media;
using GS.Services.Messages;
using GS.Services.Seo;
using GS.Services.Stores;
using GS.Services.Vendors;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace GS.Services.ExportImport
{
    /// <summary>
    /// Export manager
    /// </summary>
    public partial class ExportManager : IExportManager
    {
        #region Fields

        private readonly AddressSettings _addressSettings;
        private readonly CatalogSettings _catalogSettings;
        private readonly CustomerSettings _customerSettings;
        private readonly ForumSettings _forumSettings;
        private readonly ICategoryService _categoryService;
        private readonly ICountryService _countryService;
        private readonly ICurrencyService _currencyService;
        private readonly ICustomerAttributeFormatter _customerAttributeFormatter;
        private readonly ICustomerService _customerService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IForumService _forumService;
        private readonly IGdprService _gdprService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IMeasureService _measureService;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly IPictureService _pictureService;
        private readonly ISpecificationAttributeService _specificationAttributeService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IStoreService _storeService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IVendorService _vendorService;
        private readonly IWorkContext _workContext;
        #endregion

        #region Ctor

        public ExportManager(AddressSettings addressSettings,
            CatalogSettings catalogSettings,
            CustomerSettings customerSettings,
            ForumSettings forumSettings,
            ICategoryService categoryService,
            ICountryService countryService,
            ICurrencyService currencyService,
            ICustomerAttributeFormatter customerAttributeFormatter,
            ICustomerService customerService,
            IDateTimeHelper dateTimeHelper,
            IForumService forumService,
            IGdprService gdprService,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            IManufacturerService manufacturerService,
            IMeasureService measureService,
            INewsLetterSubscriptionService newsLetterSubscriptionService,
            IPictureService pictureService,
            ISpecificationAttributeService specificationAttributeService,
            IStateProvinceService stateProvinceService,
            IStoreService storeService,
            IUrlRecordService urlRecordService,
            IVendorService vendorService,
            IWorkContext workContext
            )
        {
            this._addressSettings = addressSettings;
            this._catalogSettings = catalogSettings;
            this._customerSettings = customerSettings;
            this._forumSettings = forumSettings;
            this._categoryService = categoryService;
            this._countryService = countryService;
            this._currencyService = currencyService;
            this._customerAttributeFormatter = customerAttributeFormatter;
            this._customerService = customerService;
            this._dateTimeHelper = dateTimeHelper;
            this._forumService = forumService;
            this._gdprService = gdprService;
            this._genericAttributeService = genericAttributeService;
            this._localizationService = localizationService;
            this._manufacturerService = manufacturerService;
            this._measureService = measureService;
            this._newsLetterSubscriptionService = newsLetterSubscriptionService;
            this._pictureService = pictureService;
            this._specificationAttributeService = specificationAttributeService;
            this._stateProvinceService = stateProvinceService;
            this._storeService = storeService;
            this._urlRecordService = urlRecordService;
            this._vendorService = vendorService;
            this._workContext = workContext;
        }

        #endregion

        #region Utilities

        protected virtual void WriteCategories(XmlWriter xmlWriter, int parentCategoryId)
        {
            var categories = _categoryService.GetAllCategoriesByParentCategoryId(parentCategoryId, true);
            if (categories == null || !categories.Any())
                return;

            foreach (var category in categories)
            {
                xmlWriter.WriteStartElement("Category");

                xmlWriter.WriteString("Id", category.Id);

                xmlWriter.WriteString("Name", category.Name);
                xmlWriter.WriteString("Description", category.Description);
                xmlWriter.WriteString("CategoryTemplateId", category.CategoryTemplateId);
                xmlWriter.WriteString("MetaKeywords", category.MetaKeywords, IgnoreExportCategoryProperty());
                xmlWriter.WriteString("MetaDescription", category.MetaDescription, IgnoreExportCategoryProperty());
                xmlWriter.WriteString("MetaTitle", category.MetaTitle, IgnoreExportCategoryProperty());
                xmlWriter.WriteString("SeName", _urlRecordService.GetSeName(category, 0), IgnoreExportCategoryProperty());
                xmlWriter.WriteString("ParentCategoryId", category.ParentCategoryId);
                xmlWriter.WriteString("PictureId", category.PictureId);
                xmlWriter.WriteString("PageSize", category.PageSize, IgnoreExportCategoryProperty());
                xmlWriter.WriteString("AllowCustomersToSelectPageSize", category.AllowCustomersToSelectPageSize, IgnoreExportCategoryProperty());
                xmlWriter.WriteString("PageSizeOptions", category.PageSizeOptions, IgnoreExportCategoryProperty());
                xmlWriter.WriteString("PriceRanges", category.PriceRanges, IgnoreExportCategoryProperty());
                xmlWriter.WriteString("ShowOnHomePage", category.ShowOnHomePage, IgnoreExportCategoryProperty());
                xmlWriter.WriteString("IncludeInTopMenu", category.IncludeInTopMenu, IgnoreExportCategoryProperty());
                xmlWriter.WriteString("Published", category.Published, IgnoreExportCategoryProperty());
                xmlWriter.WriteString("Deleted", category.Deleted, true);
                xmlWriter.WriteString("DisplayOrder", category.DisplayOrder);
                xmlWriter.WriteString("CreatedOnUtc", category.CreatedOnUtc, IgnoreExportCategoryProperty());
                xmlWriter.WriteString("UpdatedOnUtc", category.UpdatedOnUtc, IgnoreExportCategoryProperty());

                xmlWriter.WriteStartElement("Products");
                //var productCategories = _categoryService.GetProductCategoriesByCategoryId(category.Id, showHidden: true);
                //foreach (var productCategory in productCategories)
                //{
                //    var product = productCategory.Product;
                //    if (product == null || product.Deleted)
                //        continue;

                //    xmlWriter.WriteStartElement("ProductCategory");
                //    xmlWriter.WriteString("ProductCategoryId", productCategory.Id);
                //    xmlWriter.WriteString("ProductId", productCategory.ProductId);
                //    xmlWriter.WriteString("ProductName", product.Name);
                //    xmlWriter.WriteString("IsFeaturedProduct", productCategory.IsFeaturedProduct);
                //    xmlWriter.WriteString("DisplayOrder", productCategory.DisplayOrder);
                //    xmlWriter.WriteEndElement();
                //}

                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("SubCategories");
                WriteCategories(xmlWriter, category.Id);
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
            }
        }

        /// <summary>
        /// Returns the path to the image file by ID
        /// </summary>
        /// <param name="pictureId">Picture ID</param>
        /// <returns>Path to the image file</returns>
        protected virtual string GetPictures(int pictureId)
        {
            var picture = _pictureService.GetPictureById(pictureId);
            return _pictureService.GetThumbLocalPath(picture);
        }



        protected virtual bool IgnoreExportCategoryProperty()
        {
            try
            {
                return !_genericAttributeService.GetAttribute<bool>(_workContext.CurrentCustomer, "category-advanced-mode");
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        protected virtual bool IgnoreExportManufacturerProperty()
        {
            try
            {
                return !_genericAttributeService.GetAttribute<bool>(_workContext.CurrentCustomer, "manufacturer-advanced-mode");
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        private PropertyManager<ExportProductAttribute> GetProductAttributeManager()
        {
            var attributeProperties = new[]
            {
                new PropertyByName<ExportProductAttribute>("AttributeId", p => p.AttributeId),
                new PropertyByName<ExportProductAttribute>("AttributeName", p => p.AttributeName),
                new PropertyByName<ExportProductAttribute>("AttributeTextPrompt", p => p.AttributeTextPrompt),
                new PropertyByName<ExportProductAttribute>("AttributeIsRequired", p => p.AttributeIsRequired),
                new PropertyByName<ExportProductAttribute>("AttributeControlType", p => p.AttributeControlTypeId)
                {
                    DropDownElements = AttributeControlType.TextBox.ToSelectList(useLocalization: false)
                },
                new PropertyByName<ExportProductAttribute>("AttributeDisplayOrder", p => p.AttributeDisplayOrder),
                new PropertyByName<ExportProductAttribute>("ProductAttributeValueId", p => p.Id),
                new PropertyByName<ExportProductAttribute>("ValueName", p => p.Name),
                new PropertyByName<ExportProductAttribute>("AttributeValueType", p => p.AttributeValueTypeId)
                {
                    DropDownElements = AttributeValueType.Simple.ToSelectList(useLocalization: false)
                },
                new PropertyByName<ExportProductAttribute>("AssociatedProductId", p => p.AssociatedProductId),
                new PropertyByName<ExportProductAttribute>("ColorSquaresRgb", p => p.ColorSquaresRgb),
                new PropertyByName<ExportProductAttribute>("ImageSquaresPictureId", p => p.ImageSquaresPictureId),
                new PropertyByName<ExportProductAttribute>("PriceAdjustment", p => p.PriceAdjustment),
                new PropertyByName<ExportProductAttribute>("PriceAdjustmentUsePercentage", p => p.PriceAdjustmentUsePercentage),
                new PropertyByName<ExportProductAttribute>("WeightAdjustment", p => p.WeightAdjustment),
                new PropertyByName<ExportProductAttribute>("Cost", p => p.Cost),
                new PropertyByName<ExportProductAttribute>("CustomerEntersQty", p => p.CustomerEntersQty),
                new PropertyByName<ExportProductAttribute>("Quantity", p => p.Quantity),
                new PropertyByName<ExportProductAttribute>("IsPreSelected", p => p.IsPreSelected),
                new PropertyByName<ExportProductAttribute>("DisplayOrder", p => p.DisplayOrder),
                new PropertyByName<ExportProductAttribute>("PictureId", p => p.PictureId)
            };

            return new PropertyManager<ExportProductAttribute>(attributeProperties, _catalogSettings);
        }

        private PropertyManager<ExportSpecificationAttribute> GetSpecificationAttributeManager()
        {
            var attributeProperties = new[]
            {
                new PropertyByName<ExportSpecificationAttribute>("AttributeType", p => p.AttributeTypeId)
                {
                    DropDownElements = SpecificationAttributeType.Option.ToSelectList(useLocalization: false)
                },
                new PropertyByName<ExportSpecificationAttribute>("SpecificationAttribute", p => p.SpecificationAttributeId)
                {
                    DropDownElements = _specificationAttributeService.GetSpecificationAttributes().Select(sa => sa as BaseEntity).ToSelectList(p => (p as SpecificationAttribute)?.Name ?? string.Empty)
                },
                new PropertyByName<ExportSpecificationAttribute>("CustomValue", p => p.CustomValue),
                new PropertyByName<ExportSpecificationAttribute>("SpecificationAttributeOptionId", p => p.SpecificationAttributeOptionId),
                new PropertyByName<ExportSpecificationAttribute>("AllowFiltering", p => p.AllowFiltering),
                new PropertyByName<ExportSpecificationAttribute>("ShowOnProductPage", p => p.ShowOnProductPage),
                new PropertyByName<ExportSpecificationAttribute>("DisplayOrder", p => p.DisplayOrder)
            };

            return new PropertyManager<ExportSpecificationAttribute>(attributeProperties, _catalogSettings);
        }


        private string GetCustomCustomerAttributes(Customer customer)
        {
            var selectedCustomerAttributes = _genericAttributeService.GetAttribute<string>(customer, GSCustomerDefaults.CustomCustomerAttributes);
            return _customerAttributeFormatter.FormatAttributes(selectedCustomerAttributes, ";");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Export manufacturer list to XML
        /// </summary>
        /// <param name="manufacturers">Manufacturers</param>
        /// <returns>Result in XML format</returns>
        public virtual string ExportManufacturersToXml(IList<Manufacturer> manufacturers)
        {
            var sb = new StringBuilder();
            var stringWriter = new StringWriter(sb);
            var xmlWriter = new XmlTextWriter(stringWriter);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Manufacturers");
            xmlWriter.WriteAttributeString("Version", GSVersion.CurrentVersion);

            foreach (var manufacturer in manufacturers)
            {
                xmlWriter.WriteStartElement("Manufacturer");

                xmlWriter.WriteString("ManufacturerId", manufacturer.Id);
                xmlWriter.WriteString("Name", manufacturer.Name);
                xmlWriter.WriteString("Description", manufacturer.Description);
                xmlWriter.WriteString("ManufacturerTemplateId", manufacturer.ManufacturerTemplateId);
                xmlWriter.WriteString("MetaKeywords", manufacturer.MetaKeywords, IgnoreExportManufacturerProperty());
                xmlWriter.WriteString("MetaDescription", manufacturer.MetaDescription, IgnoreExportManufacturerProperty());
                xmlWriter.WriteString("MetaTitle", manufacturer.MetaTitle, IgnoreExportManufacturerProperty());
                xmlWriter.WriteString("SEName", _urlRecordService.GetSeName(manufacturer, 0), IgnoreExportManufacturerProperty());
                xmlWriter.WriteString("PictureId", manufacturer.PictureId);
                xmlWriter.WriteString("PageSize", manufacturer.PageSize, IgnoreExportManufacturerProperty());
                xmlWriter.WriteString("AllowCustomersToSelectPageSize", manufacturer.AllowCustomersToSelectPageSize, IgnoreExportManufacturerProperty());
                xmlWriter.WriteString("PageSizeOptions", manufacturer.PageSizeOptions, IgnoreExportManufacturerProperty());
                xmlWriter.WriteString("PriceRanges", manufacturer.PriceRanges, IgnoreExportManufacturerProperty());
                xmlWriter.WriteString("Published", manufacturer.Published, IgnoreExportManufacturerProperty());
                xmlWriter.WriteString("Deleted", manufacturer.Deleted, true);
                xmlWriter.WriteString("DisplayOrder", manufacturer.DisplayOrder);
                xmlWriter.WriteString("CreatedOnUtc", manufacturer.CreatedOnUtc, IgnoreExportManufacturerProperty());
                xmlWriter.WriteString("UpdatedOnUtc", manufacturer.UpdatedOnUtc, IgnoreExportManufacturerProperty());

                xmlWriter.WriteStartElement("Products");
                //var productManufacturers = _manufacturerService.GetProductManufacturersByManufacturerId(manufacturer.Id, showHidden: true);
                //if (productManufacturers != null)
                //{
                //    foreach (var productManufacturer in productManufacturers)
                //    {
                //        var product = productManufacturer.Product;
                //        if (product == null || product.Deleted) 
                //            continue;

                //        xmlWriter.WriteStartElement("ProductManufacturer");
                //        xmlWriter.WriteString("ProductManufacturerId", productManufacturer.Id);
                //        xmlWriter.WriteString("ProductId", productManufacturer.ProductId);
                //        xmlWriter.WriteString("ProductName", product.Name);
                //        xmlWriter.WriteString("IsFeaturedProduct", productManufacturer.IsFeaturedProduct);
                //        xmlWriter.WriteString("DisplayOrder", productManufacturer.DisplayOrder);
                //        xmlWriter.WriteEndElement();
                //    }
                //}

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return stringWriter.ToString();
        }

        /// <summary>
        /// Export manufacturers to XLSX
        /// </summary>
        /// <param name="manufacturers">Manufactures</param>
        public virtual byte[] ExportManufacturersToXlsx(IEnumerable<Manufacturer> manufacturers)
        {
            //property manager 
            var manager = new PropertyManager<Manufacturer>(new[]
            {
                new PropertyByName<Manufacturer>("Id", p => p.Id),
                new PropertyByName<Manufacturer>("Name", p => p.Name),
                new PropertyByName<Manufacturer>("Description", p => p.Description),
                new PropertyByName<Manufacturer>("ManufacturerTemplateId", p => p.ManufacturerTemplateId),
                new PropertyByName<Manufacturer>("MetaKeywords", p => p.MetaKeywords, IgnoreExportManufacturerProperty()),
                new PropertyByName<Manufacturer>("MetaDescription", p => p.MetaDescription, IgnoreExportManufacturerProperty()),
                new PropertyByName<Manufacturer>("MetaTitle", p => p.MetaTitle, IgnoreExportManufacturerProperty()),
                new PropertyByName<Manufacturer>("SeName", p => _urlRecordService.GetSeName(p, 0), IgnoreExportManufacturerProperty()),
                new PropertyByName<Manufacturer>("Picture", p => GetPictures(p.PictureId)),
                new PropertyByName<Manufacturer>("PageSize", p => p.PageSize, IgnoreExportManufacturerProperty()),
                new PropertyByName<Manufacturer>("AllowCustomersToSelectPageSize", p => p.AllowCustomersToSelectPageSize, IgnoreExportManufacturerProperty()),
                new PropertyByName<Manufacturer>("PageSizeOptions", p => p.PageSizeOptions, IgnoreExportManufacturerProperty()),
                new PropertyByName<Manufacturer>("PriceRanges", p => p.PriceRanges, IgnoreExportManufacturerProperty()),
                new PropertyByName<Manufacturer>("Published", p => p.Published, IgnoreExportManufacturerProperty()),
                new PropertyByName<Manufacturer>("DisplayOrder", p => p.DisplayOrder)
            }, _catalogSettings);

            return manager.ExportToXlsx(manufacturers);
        }

        /// <summary>
        /// Export category list to XML
        /// </summary>
        /// <returns>Result in XML format</returns>
        public virtual string ExportCategoriesToXml()
        {
            var sb = new StringBuilder();
            var stringWriter = new StringWriter(sb);
            var xmlWriter = new XmlTextWriter(stringWriter);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Categories");
            xmlWriter.WriteAttributeString("Version", GSVersion.CurrentVersion);
            WriteCategories(xmlWriter, 0);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return stringWriter.ToString();
        }

        /// <summary>
        /// Export categories to XLSX
        /// </summary>
        /// <param name="categories">Categories</param>
        public virtual byte[] ExportCategoriesToXlsx(IList<Category> categories)
        {
            var parentCatagories = new List<Category>();
            if (_catalogSettings.ExportImportCategoriesUsingCategoryName)
            {
                //performance optimization, load all parent categories in one SQL request
                parentCatagories = _categoryService.GetCategoriesByIds(categories.Select(c => c.ParentCategoryId).Where(id => id != 0).ToArray());
            }

            //property manager 
            var manager = new PropertyManager<Category>(new[]
            {
                new PropertyByName<Category>("Id", p => p.Id),
                new PropertyByName<Category>("Name", p => p.Name),
                new PropertyByName<Category>("Description", p => p.Description),
                new PropertyByName<Category>("CategoryTemplateId", p => p.CategoryTemplateId),
                new PropertyByName<Category>("MetaKeywords", p => p.MetaKeywords, IgnoreExportCategoryProperty()),
                new PropertyByName<Category>("MetaDescription", p => p.MetaDescription, IgnoreExportCategoryProperty()),
                new PropertyByName<Category>("MetaTitle", p => p.MetaTitle, IgnoreExportCategoryProperty()),
                new PropertyByName<Category>("SeName", p => _urlRecordService.GetSeName(p, 0), IgnoreExportCategoryProperty()),
                new PropertyByName<Category>("ParentCategoryId", p => p.ParentCategoryId),
                new PropertyByName<Category>("ParentCategoryName", p =>
                {
                    var category = parentCatagories.FirstOrDefault(c => c.Id == p.ParentCategoryId);
                    return category != null ? _categoryService.GetFormattedBreadCrumb(category) : null;
                }, !_catalogSettings.ExportImportCategoriesUsingCategoryName),
                new PropertyByName<Category>("Picture", p => GetPictures(p.PictureId)),
                new PropertyByName<Category>("PageSize", p => p.PageSize, IgnoreExportCategoryProperty()),
                new PropertyByName<Category>("AllowCustomersToSelectPageSize", p => p.AllowCustomersToSelectPageSize, IgnoreExportCategoryProperty()),
                new PropertyByName<Category>("PageSizeOptions", p => p.PageSizeOptions, IgnoreExportCategoryProperty()),
                new PropertyByName<Category>("PriceRanges", p => p.PriceRanges, IgnoreExportCategoryProperty()),
                new PropertyByName<Category>("ShowOnHomePage", p => p.ShowOnHomePage, IgnoreExportCategoryProperty()),
                new PropertyByName<Category>("IncludeInTopMenu", p => p.IncludeInTopMenu, IgnoreExportCategoryProperty()),
                new PropertyByName<Category>("Published", p => p.Published, IgnoreExportCategoryProperty()),
                new PropertyByName<Category>("DisplayOrder", p => p.DisplayOrder)
            }, _catalogSettings);

            return manager.ExportToXlsx(categories);
        }


        /// <summary>
        /// Export customer list to XLSX
        /// </summary>
        /// <param name="customers">Customers</param>
        public virtual byte[] ExportCustomersToXlsx(IList<Customer> customers)
        {
            //property manager 
            var manager = new PropertyManager<Customer>(new[]
            {
                new PropertyByName<Customer>("CustomerId", p => p.Id),
                new PropertyByName<Customer>("CustomerGuid", p => p.CustomerGuid),
                new PropertyByName<Customer>("Email", p => p.Email),
                new PropertyByName<Customer>("Username", p => p.Username),
                new PropertyByName<Customer>("Password", p => _customerService.GetCurrentPassword(p.Id)?.Password),
                new PropertyByName<Customer>("PasswordFormatId", p => _customerService.GetCurrentPassword(p.Id)?.PasswordFormatId ?? 0),
                new PropertyByName<Customer>("PasswordSalt", p => _customerService.GetCurrentPassword(p.Id)?.PasswordSalt),
                new PropertyByName<Customer>("IsTaxExempt", p => p.IsTaxExempt),
                new PropertyByName<Customer>("AffiliateId", p => p.AffiliateId),
                new PropertyByName<Customer>("VendorId", p => p.VendorId),
                new PropertyByName<Customer>("Active", p => p.Active),
                new PropertyByName<Customer>("IsGuest", p => p.IsGuest()),
                new PropertyByName<Customer>("IsRegistered", p => p.IsRegistered()),
                new PropertyByName<Customer>("IsAdministrator", p => p.IsAdmin()),
                new PropertyByName<Customer>("IsForumModerator", p => p.IsForumModerator()),
                new PropertyByName<Customer>("CreatedOnUtc", p => p.CreatedOnUtc),
                //attributes
                new PropertyByName<Customer>("FirstName", p => _genericAttributeService.GetAttribute<string>(p, GSCustomerDefaults.FirstNameAttribute)),
                new PropertyByName<Customer>("LastName", p => _genericAttributeService.GetAttribute<string>(p, GSCustomerDefaults.LastNameAttribute)),
                new PropertyByName<Customer>("Gender", p => _genericAttributeService.GetAttribute<string>(p, GSCustomerDefaults.GenderAttribute)),
                new PropertyByName<Customer>("Company", p => _genericAttributeService.GetAttribute<string>(p, GSCustomerDefaults.CompanyAttribute)),
                new PropertyByName<Customer>("StreetAddress", p => _genericAttributeService.GetAttribute<string>(p, GSCustomerDefaults.StreetAddressAttribute)),
                new PropertyByName<Customer>("StreetAddress2", p => _genericAttributeService.GetAttribute<string>(p, GSCustomerDefaults.StreetAddress2Attribute)),
                new PropertyByName<Customer>("ZipPostalCode", p => _genericAttributeService.GetAttribute<string>(p, GSCustomerDefaults.ZipPostalCodeAttribute)),
                new PropertyByName<Customer>("City", p => _genericAttributeService.GetAttribute<string>(p, GSCustomerDefaults.CityAttribute)),
                new PropertyByName<Customer>("County", p => _genericAttributeService.GetAttribute<string>(p, GSCustomerDefaults.CountyAttribute)),
                new PropertyByName<Customer>("CountryId", p => _genericAttributeService.GetAttribute<int>(p, GSCustomerDefaults.CountryIdAttribute)),
                new PropertyByName<Customer>("StateProvinceId", p => _genericAttributeService.GetAttribute<int>(p, GSCustomerDefaults.StateProvinceIdAttribute)),
                new PropertyByName<Customer>("Phone", p => _genericAttributeService.GetAttribute<string>(p, GSCustomerDefaults.PhoneAttribute)),
                new PropertyByName<Customer>("Fax", p => _genericAttributeService.GetAttribute<string>(p, GSCustomerDefaults.FaxAttribute)),
                new PropertyByName<Customer>("VatNumber", p => _genericAttributeService.GetAttribute<string>(p, GSCustomerDefaults.VatNumberAttribute)),
                new PropertyByName<Customer>("VatNumberStatusId", p => _genericAttributeService.GetAttribute<int>(p, GSCustomerDefaults.VatNumberStatusIdAttribute)),
                new PropertyByName<Customer>("TimeZoneId", p => _genericAttributeService.GetAttribute<string>(p, GSCustomerDefaults.TimeZoneIdAttribute)),
                new PropertyByName<Customer>("AvatarPictureId", p => _genericAttributeService.GetAttribute<int>(p, GSCustomerDefaults.AvatarPictureIdAttribute)),
                new PropertyByName<Customer>("ForumPostCount", p => _genericAttributeService.GetAttribute<int>(p, GSCustomerDefaults.ForumPostCountAttribute)),
                new PropertyByName<Customer>("Signature", p => _genericAttributeService.GetAttribute<string>(p, GSCustomerDefaults.SignatureAttribute)),
                new PropertyByName<Customer>("CustomCustomerAttributes",  GetCustomCustomerAttributes)
            }, _catalogSettings);

            return manager.ExportToXlsx(customers);
        }
        /// <summary>
        /// Export report contract unfinish to XLSX
        /// </summary>
        /// <param name="report">Report</param>
        /// <summary>
        public void ExportContractUnfinish(IList<ReportContractUnfinish> reports, Stream stream, DateTime time)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("BaoCaoChung");
                worksheet.DefaultRowHeight = 20;
                worksheet.Cells["A1:D1"].Merge = true;
                worksheet.Cells[1, 1].Value = "BÁO CÁO SẢN LƯỢNG DỞ DANG";
                worksheet.Cells[1, 1].Style.Font.Size = 14;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells["A2:D2"].Merge = true;
                worksheet.Cells[2, 1].Value = time.toDateVNString();
                worksheet.Cells["A3:A4"].Merge = true;
                worksheet.Cells["A3"].Value = "STT";
                worksheet.Cells["B3:B4"].Merge = true;
                worksheet.Cells["B3"].Value = "Mã công trình";
                worksheet.Cells["C3:C4"].Merge = true;
                worksheet.Cells["C3"].Value = "Tên hợp đồng";
                worksheet.Cells["D3:D4"].Merge = true;
                worksheet.Cells["D3"].Value = "Tên hợp đồng";
                worksheet.Cells["E3:G3"].Merge = true;
                worksheet.Cells["E3"].Value = "Hợp đồng";
                worksheet.Cells["E4"].Value = "Số";
                worksheet.Cells["F4"].Value = "Ngày ký";
                worksheet.Cells["G4"].Value = "Giá trị";
                worksheet.Cells["H3:I3"].Merge = true;
                worksheet.Cells["H3"].Value = "Sản lượng dở dang";
                worksheet.Cells["H4"].Value = "Khảo sát";
                worksheet.Cells["I4"].Value = "Thiết kế";
                worksheet.Cells["J3:J4"].Merge = true;
                worksheet.Cells["J3"].Value = "Phần hợp đồng chưa thực hiện";
                worksheet.Cells["K3:K4"].Merge = true;
                worksheet.Cells["K3"].Value = "Sản lượng rủi ro";
                worksheet.Cells["L3:L4"].Merge = true;
                worksheet.Cells["L3"].Value = "CT đã quyết toán";
                worksheet.Cells["M3:N3"].Merge = true;
                worksheet.Cells["M3"].Value = "Điều độ theo dõi";
                worksheet.Cells["M4"].Value = "Khảo sát";
                worksheet.Cells["N4"].Value = "Thiết kế";
                int row = 5;
                int stt = 1;
                foreach (var item in reports)
                {
                    int col = 1;
                    var creator = _customerService.GetCustomerById(item.UserMonitorId);
                    var creatorName = _customerService.GetCustomerFullName(creator);

                    worksheet.Cells[row, col].Value = stt;
                    col++;
                    worksheet.Cells[row, col].Value = item.ConstructionCode;
                    col++;
                    worksheet.Cells[row, col].Value = item.ConstructionName;
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractName;
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractCode;
                    col++;
                    worksheet.Cells[row, col].Value = item.SignedDate.ToString("dd/MM/yyyy");
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    col++;
                    worksheet.Cells[row, col].Value = item.TotalAmount;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractFormTested;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractFormDesign;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractUnfinish2;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    col++;
                    worksheet.Cells[row, col].Value = "";
                    col++;
                    worksheet.Cells[row, col].Value = item.isSettlemented;
                    col++;
                    worksheet.Cells[row, col].Value = creatorName;
                    col++;
                    worksheet.Cells[row, col].Value = creatorName;
                    row++;
                    stt++;
                }
                worksheet.Column(4).Width = 50;
                string strMerge = "A" + row.ToString() + ":B" + row.ToString();
                worksheet.Cells[strMerge].Merge = true;
                worksheet.Cells[row, 1].Value = "Tổng cộng";
                worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                var colTong = 2;
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = reports.Sum(c => c.TotalAmount);
                worksheet.Cells[row, colTong].Style.Numberformat.Format = "#,###,##0.#0";
                worksheet.Cells[row, colTong].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                colTong++;
                worksheet.Cells[row, colTong].Value = reports.Sum(c => c.ContractFormTested);
                worksheet.Cells[row, colTong].Style.Numberformat.Format = "#,###,##0.#0";
                worksheet.Cells[row, colTong].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                colTong++;
                worksheet.Cells[row, colTong].Value = reports.Sum(c => c.ContractFormDesign);
                worksheet.Cells[row, colTong].Style.Numberformat.Format = "#,###,##0.#0";
                worksheet.Cells[row, colTong].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                colTong++;
                worksheet.Cells[row, colTong].Value = reports.Sum(c => c.ContractUnfinish2);
                worksheet.Cells[row, colTong].Style.Numberformat.Format = "#,###,##0.#0";
                worksheet.Cells[row, colTong].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //style tong cong
                worksheet.Cells[row, 1].Style.Font.Size = 12;
                worksheet.Cells[row, 1].Style.Font.Bold = true;

                string modelRangeheader = "A1:N4";
                var modelTableheader = worksheet.Cells[modelRangeheader];
                modelTableheader.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                modelTableheader.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                modelTableheader.Style.Font.Bold = true;
                //style body
                var modelRows = row;
                string modelRange = "A5:N" + modelRows.ToString();
                var modelTable = worksheet.Cells[modelRange];

                // Assign borders
                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Dashed;
                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.DashDot;

                // Fill worksheet with data to export
                string _modelRange = "A1:N" + modelRows.ToString();
                var _modelTable = worksheet.Cells[_modelRange];

                _modelTable.AutoFitColumns();
                xlPackage.Save();
            }
        }
        /// <summary>
        /// Export report task by unit to XLSX
        /// </summary>
        /// <param name="report">Report</param>
        /// <summary>
        public void ExportTaskByUnit(IList<ReportTaskByUnit> reports, Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            using (var xlPackage = new ExcelPackage(stream))
            {
                // uncomment this line if you want the XML written out to the outputDir
                //xlPackage.DebugMode = true; 

                // get handle to the existing worksheet
                var worksheet = xlPackage.Workbook.Worksheets.Add("BaoCaoCongVienTheoDonVi");
                //Create Headers and format them
                worksheet.DefaultRowHeight = 20;
                worksheet.Cells["A1:F1"].Merge = true;
                worksheet.Cells[1, 1].Value = " BÁO CÁO CÔNG VIỆC THEO ĐƠN VỊ";
                //worksheet.Cells["B2"].Value = " ";
                worksheet.Cells[1, 1].Style.Font.Size = 14;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //ten don vi
                worksheet.Cells["A2:F2"].Merge = true;
                worksheet.Cells[2, 1].Value = "ĐƠN VỊ : " + reports.FirstOrDefault().UnitName;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //worksheet.Cells["E2"].Style.Font.Bold = true;
                worksheet.Cells["A3:A4"].Merge = true;
                worksheet.Cells["A3"].Value = "STT";
                worksheet.Cells["B3:B4"].Merge = true;
                worksheet.Cells["B3"].Value = "Mã công trình";
                worksheet.Cells["C3:C4"].Merge = true;
                worksheet.Cells["C3"].Value = "Tên công trình";
                worksheet.Cells["D3:D4"].Merge = true;
                worksheet.Cells["D3"].Value = "Loại dự án";
                worksheet.Cells["E3:E4"].Merge = true;
                worksheet.Cells["E3"].Value = "Tên hợp đồng";
                worksheet.Cells["F3:F4"].Merge = true;
                worksheet.Cells["F3"].Value = "Mã hợp đồng";
                worksheet.Cells["G3:G4"].Merge = true;
                worksheet.Cells["G3"].Value = "Hình thức hợp đồng";
                worksheet.Cells["H3:H4"].Merge = true;
                worksheet.Cells["H3"].Value = "Giá trị hợp đồng";
                worksheet.Cells["I3:I4"].Merge = true;
                worksheet.Cells["I3"].Value = "Đơn vị";
                worksheet.Cells["J3:J4"].Merge = true;
                worksheet.Cells["J3"].Value = "Hạng mục công việc";
                worksheet.Cells["K3:K4"].Merge = true;
                worksheet.Cells["K3"].Value = "Giá trị hạng mục công việc";
                worksheet.Cells["L3:L4"].Merge = true;
                worksheet.Cells["L3"].Value = "Giá trị đã tạm ứng";
                worksheet.Cells["M3:M4"].Merge = true;
                worksheet.Cells["M3"].Value = "Giá trị đã nghiệm thu";
                worksheet.Cells["N3:N4"].Merge = true;
                worksheet.Cells["N3"].Value = "Trạng thái";
                worksheet.Cells["O3:P3"].Merge = true;
                worksheet.Cells["O3"].Value = "Sản lượng dở dang";
                worksheet.Cells["O4"].Value = "Chưa NT";
                worksheet.Cells["P4"].Value = "Chưa TT";
                int row = 5;
                int stt = 1;
                //var tongsonhap = 0m;
                //var tongsoxuat = 0m;
                foreach (var item in reports)
                {
                    int col = 1;
                    //tongsonhap += item.TongSoNhap;
                    //tongsoxuat += item.TongSoXuat;

                    worksheet.Cells[row, col].Value = stt;
                    col++;
                    worksheet.Cells[row, col].Value = item.ConstructionCode;
                    col++;

                    worksheet.Cells[row, col].Value = item.ConstructionName;
                    col++;

                    worksheet.Cells[row, col].Value = item.ConstructionType;
                    col++;

                    worksheet.Cells[row, col].Value = item.ContractName;
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractCode;
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractForm;
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractTotalAmount;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    col++;
                    worksheet.Cells[row, col].Value = item.UnitName;
                    col++;
                    worksheet.Cells[row, col].Value = item.TaskName;
                    col++;
                    worksheet.Cells[row, col].Value = item.TaskTotalAmount;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    col++;
                    worksheet.Cells[row, col].Value = item.PaymentAdvance;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    col++;
                    worksheet.Cells[row, col].Value = item.PaymentAcceptance;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    col++;
                    worksheet.Cells[row, col].Value = item.tasktStatus.ToStatusTaskVN();
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractUnfinish1;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractNoWork;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    row++;
                    stt++;
                }
                var colTong = 1;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "Tổng cộng";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;

                worksheet.Cells[row, colTong].Value = "";
                colTong++;

                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = reports.Sum(c => c.PaymentAdvance);
                worksheet.Cells[row, colTong].Style.Numberformat.Format = "#,###,##0.#0";
                worksheet.Cells[row, colTong].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                colTong++;
                worksheet.Cells[row, colTong].Value = reports.Sum(c => c.PaymentAcceptance);
                worksheet.Cells[row, colTong].Style.Numberformat.Format = "#,###,##0.#0";
                worksheet.Cells[row, colTong].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = reports.Sum(c => c.ContractUnfinish1);
                worksheet.Cells[row, colTong].Style.Numberformat.Format = "#,###,##0.#0";
                worksheet.Cells[row, colTong].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                colTong++;
                worksheet.Cells[row, colTong].Value = reports.Sum(c => c.ContractNoWork);
                worksheet.Cells[row, colTong].Style.Numberformat.Format = "#,###,##0.#0";
                worksheet.Cells[row, colTong].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                colTong++;
                //style tong cong
                worksheet.Cells[row, 2].Style.Font.Size = 12;
                worksheet.Cells[row, 2].Style.Font.Bold = true;
                //style header
                string modelRangeheader = "A1:P4";
                var modelTableheader = worksheet.Cells[modelRangeheader];
                modelTableheader.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                modelTableheader.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                modelTableheader.Style.Font.Bold = true;
                //style body
                var modelRows = row;
                string modelRange = "A4:P" + modelRows.ToString();
                var modelTable = worksheet.Cells[modelRange];

                // Assign borders
                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Dashed;
                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.DashDot;

                // Fill worksheet with data to export
                string _modelRange = "A1:P" + modelRows.ToString();
                var _modelTable = worksheet.Cells[_modelRange];

                _modelTable.AutoFitColumns();
                xlPackage.Save();
            }
        }
        /// <summary>
        /// Export report contract deadline to XLSX
        /// </summary>
        /// <param name="report">Report</param>
        /// <summary>
        public void ExportContractDeadline(IList<ReportContractDealine> reports, Stream stream, DateTime timeFrom, DateTime timeTo)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("HDsapDenHanNT");
                worksheet.DefaultRowHeight = 20;
                worksheet.Cells["A1:E1"].Merge = true;
                worksheet.Cells[1, 1].Value = "BÁO CÁO HỢP ĐỒNG SẮP ĐẾN HẠN NGHIỆM THU";
                worksheet.Cells[1, 1].Style.Font.Size = 14;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells["A2:E2"].Merge = true;
                worksheet.Cells[2, 1].Value = "Từ ngày " + timeFrom.toDateVNString() + " đến ngày " + timeTo.toDateVNString();
                worksheet.Cells["A3"].Value = "STT";
                worksheet.Cells["B3"].Value = "Mã CT";
                worksheet.Cells["C3"].Value = "Tên dự án";
                worksheet.Cells["D3"].Value = "Tên hợp đồng";
                worksheet.Cells["E3"].Value = "Mã hợp đồng";
                worksheet.Cells["F3"].Value = "Loại dự án";
                worksheet.Cells["G3"].Value = "Hình thức hợp đồng";
                worksheet.Cells["H3"].Value = "Chủ đầu tư";
                worksheet.Cells["I3"].Value = "Trong EVN";
                worksheet.Cells["J3"].Value = "Giá trị hợp đồng sau thuế (VNĐ)";
                worksheet.Cells["K3"].Value = "Ngày dự kiến kết thúc";
                int row = 4;
                int stt = 1;
                foreach (var item in reports)
                {
                    int col = 1;

                    worksheet.Cells[row, col].Value = stt;
                    col++;
                    worksheet.Cells[row, col].Value = item.ConstructionCode;
                    col++;
                    worksheet.Cells[row, col].Value = item.ConstructionName;
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractName;
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractCode;
                    col++;
                    worksheet.Cells[row, col].Value = item.ConstructionType;
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractForm;
                    col++;
                    worksheet.Cells[row, col].Value = item.ProcuringAgency;
                    col++;
                    worksheet.Cells[row, col].Value = item.isEvn;
                    col++;
                    worksheet.Cells[row, col].Value = item.TotalAmount;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    col++;
                    worksheet.Cells[row, col].Value = item.FinishDate.ToString("dd/MM/yyyy");
                    row++;
                    stt++;
                }
                string strMerge = "A" + row.ToString() + ":B" + row.ToString();
                worksheet.Cells[strMerge].Merge = true;
                worksheet.Cells[row, 1].Value = "Tổng cộng";
                worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                var colTong = 2;
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = reports.Sum(c => c.TotalAmount);
                worksheet.Cells[row, colTong].Style.Numberformat.Format = "#,###,##0.#0";
                worksheet.Cells[row, colTong].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //style tong cong
                worksheet.Cells[row, 1].Style.Font.Size = 12;
                worksheet.Cells[row, 1].Style.Font.Bold = true;

                string modelRangeheader = "A1:J4";
                var modelTableheader = worksheet.Cells[modelRangeheader];
                modelTableheader.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                modelTableheader.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                modelTableheader.Style.Font.Bold = true;

                var modelRows = row;
                string modelRange = "A4:J" + modelRows.ToString();
                var modelTable = worksheet.Cells[modelRange];

                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Dashed;
                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.DashDot;

                string _modelRange = "A1:J" + modelRows.ToString();
                var _modelTable = worksheet.Cells[_modelRange];

                _modelTable.AutoFitColumns();
                xlPackage.Save();
            }
        }
        /// Export report procuring agency list to XLSX
        /// </summary>
        /// <param name="report">Report</param>
        public void ExportContractProcuringAgency(IList<ReportProcuringAgency> reports, Stream stream, DateTime timeFrom, DateTime timeTo)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("LichSuHDvaChuDauTu");
                worksheet.DefaultRowHeight = 20;
                worksheet.Cells["A1:E1"].Merge = true;
                worksheet.Cells[1, 1].Value = "BÁO CÁO LỊCH SỬ HỢP ĐỒNG VÀ CHỦ ĐẦU TƯ";
                worksheet.Cells[1, 1].Style.Font.Size = 14;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells["A2:E2"].Merge = true;
                worksheet.Cells[2, 1].Value = "Từ ngày " + timeFrom.toDateVNString() + " đến ngày " + timeTo.toDateVNString();
                worksheet.Cells["A3"].Value = "STT";
                worksheet.Cells["B3"].Value = "Thông tin chủ đầu tư";
                worksheet.Cells["C3"].Value = "Tên công trình";
                worksheet.Cells["D3"].Value = "Mã công trình";
                worksheet.Cells["E3"].Value = "Tên hợp đồng";
                worksheet.Cells["F3"].Value = "Số hợp đồng";
                worksheet.Cells["G3"].Value = "Trong EVN";
                worksheet.Cells["H3"].Value = "Mã hợp đồng P4";
                worksheet.Cells["I3"].Value = "Ngày ký";
                worksheet.Cells["J3"].Value = "Trạng thái";
                worksheet.Cells["K3"].Value = "Giá trị HĐ sau thuế";
                worksheet.Cells["L3"].Value = "Công nợ phải thu";
                int row = 4;
                int stt = 1;
                foreach (var item in reports)
                {
                    int col = 1;

                    worksheet.Cells[row, col].Value = stt;
                    col++;
                    worksheet.Cells[row, col].Value = item.ProcuringAgencyName;
                    col++;
                    worksheet.Cells[row, col].Value = item.ConstructionName;
                    col++;
                    worksheet.Cells[row, col].Value = item.ConstructionCode;
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractName;
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractCode;
                    col++;
                    worksheet.Cells[row, col].Value = item.IsEVN;
                    col++;
                    worksheet.Cells[row, col].Value = item.CodeP4;
                    col++;
                    worksheet.Cells[row, col].Value = item.SignedDate.ToString("dd/MM/yyyy");
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractStatus.ToStatusContractVN();
                    col++;
                    worksheet.Cells[row, col].Value = item.TotalAmount;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    col++;
                    worksheet.Cells[row, col].Value = item.AmountRequest - item.AmountPayment;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    row++;
                    stt++;
                }

                string strMerge = "A" + row.ToString() + ":B" + row.ToString();
                worksheet.Cells[strMerge].Merge = true;
                worksheet.Cells[row, 1].Value = "Tổng cộng";
                worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                var colTong = 2;
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = reports.Count();
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = reports.Sum(c => c.TotalAmount);
                colTong++;
                worksheet.Cells[row, colTong].Value = reports.Sum(c => c.AmountRequest - c.AmountPayment);
                colTong++;
                //style tong cong
                worksheet.Cells[row, 1].Style.Font.Size = 12;
                worksheet.Cells[row, 1].Style.Font.Bold = true;

                string modelRangeheader = "A1:K4";
                var modelTableheader = worksheet.Cells[modelRangeheader];
                modelTableheader.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                modelTableheader.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                modelTableheader.Style.Font.Bold = true;

                var modelRows = row;
                string modelRange = "A4:K" + modelRows.ToString();
                var modelTable = worksheet.Cells[modelRange];

                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Dashed;
                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.DashDot;

                string _modelRange = "A1:K" + modelRows.ToString();
                var _modelTable = worksheet.Cells[_modelRange];

                _modelTable.AutoFitColumns();
                xlPackage.Save();
            }
        }
        /// <summary>
        /// Export report contract BB list to XLSX
        /// </summary>
        /// <param name="report">Report</param>
        public void ExportReportContractBB(IList<ContractReportBB> reports, Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("HopDongBB");
                worksheet.DefaultRowHeight = 20;
                worksheet.Cells["A1:E1"].Merge = true;
                worksheet.Cells[1, 1].Value = "BÁO CÁO HỢP ĐỒNG BB'";
                worksheet.Cells[1, 1].Style.Font.Size = 14;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells["A3"].Value = "STT";
                worksheet.Cells["B3"].Value = "Mã công trình";
                worksheet.Cells["C3"].Value = "Tên dự án";
                worksheet.Cells["D3"].Value = "Tên hợp đồng";
                worksheet.Cells["E3"].Value = "Giai đoạn";
                worksheet.Cells["F3"].Value = "Loại dự án";
                worksheet.Cells["G3"].Value = "Trong EVN";
                worksheet.Cells["H3"].Value = "Giá trị hợp đồng";
                worksheet.Cells["I3"].Value = "Mã hợp đồng AB";
                worksheet.Cells["J3"].Value = "Mã hợp đồng BB";
                worksheet.Cells["K3"].Value = "Ngày ký";
                worksheet.Cells["L3"].Value = "Tiến độ";
                worksheet.Cells["M3"].Value = "Mã KH";
                worksheet.Cells["N3"].Value = "Đối tác";
                worksheet.Cells["O3"].Value = "ĐVTH";
                worksheet.Cells["P3"].Value = "Trạng thái";
                int row = 4;
                int stt = 1;
                foreach (var item in reports)
                {
                    int col = 1;

                    worksheet.Cells[row, col].Value = stt;
                    col++;
                    worksheet.Cells[row, col].Value = item.ConstructionCode;
                    col++;
                    worksheet.Cells[row, col].Value = item.ConstructionName;
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractName;
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractPeriod;
                    col++;
                    worksheet.Cells[row, col].Value = item.ConstructionType;
                    col++;
                    worksheet.Cells[row, col].Value = item.IsEVN;
                    col++;
                    worksheet.Cells[row, col].Value = item.TotalAmount;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractABCode;
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractBBCode;
                    col++;
                    worksheet.Cells[row, col].Value = item.SignedDate.ToString("dd/MM/yyyy");
                    col++;
                    worksheet.Cells[row, col].Value = item.Progress != null ? Convert.ToDateTime(item.Progress).ToString("dd/MM/yyyy") : "";
                    col++;
                    worksheet.Cells[row, col].Value = item.ClientCode;
                    col++;
                    worksheet.Cells[row, col].Value = item.ClientName;
                    col++;
                    worksheet.Cells[row, col].Value = item.UnitName;
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractStatus.ToStatusContractVN();
                    row++;
                    stt++;
                }
                string strMerge = "A" + row.ToString() + ":B" + row.ToString();
                worksheet.Cells[strMerge].Merge = true;
                worksheet.Cells[row, 1].Value = "Tổng cộng";
                worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                var colTong = 2;
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = reports.Sum(c => c.TotalAmount);
                worksheet.Cells[row, colTong].Style.Numberformat.Format = "#,###,##0.#0";
                worksheet.Cells[row, colTong].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //style tong cong
                worksheet.Cells[row, 1].Style.Font.Size = 12;
                worksheet.Cells[row, 1].Style.Font.Bold = true;

                string modelRangeheader = "A1:O3";
                var modelTableheader = worksheet.Cells[modelRangeheader];
                modelTableheader.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                modelTableheader.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                modelTableheader.Style.Font.Bold = true;

                var modelRows = row;
                string modelRange = "A4:O" + modelRows.ToString();
                var modelTable = worksheet.Cells[modelRange];

                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Dashed;
                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.DashDot;

                string _modelRange = "A1:O" + modelRows.ToString();
                var _modelTable = worksheet.Cells[_modelRange];

                _modelTable.AutoFitColumns();
                xlPackage.Save();
            }
        }
        /// <summary>
        /// Export report contract AB list to XLSX
        /// </summary>
        /// <param name="report">Report</param>
        public void ExportExcelContractAB(IList<ContractReportAB> reports, Stream stream, DateTime timeFrom, DateTime timeTo)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("HopDongAB");
                worksheet.DefaultRowHeight = 20;
                worksheet.Cells["A1:E1"].Merge = true;
                worksheet.Cells[1, 1].Value = "BÁO CÁO HỢP ĐỒNG AB";
                worksheet.Cells["A2:E2"].Merge = true;
                worksheet.Cells[2, 1].Value = "Từ ngày " + timeFrom.toDateVNString() + " đến ngày " + timeTo.toDateVNString();
                worksheet.Cells[1, 1].Style.Font.Size = 14;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells["A3"].Value = "STT";
                worksheet.Cells["B3"].Value = "Mã công trình";
                worksheet.Cells["C3"].Value = "Tên công trình";
                worksheet.Cells["D3"].Value = "Tên hợp đồng";
                worksheet.Cells["E3"].Value = "Giai đoạn";
                worksheet.Cells["F3"].Value = "Loại dự án";
                worksheet.Cells["G3"].Value = "Trong EVN";
                worksheet.Cells["H3"].Value = "Giá trị hợp đồng";
                worksheet.Cells["I3"].Value = "Hình thức hợp đồng";
                worksheet.Cells["J3"].Value = "Mã hợp đồng";
                worksheet.Cells["K3"].Value = "Ngày ký";
                worksheet.Cells["L3"].Value = "Tiến độ thực hiện";
                worksheet.Cells["M3"].Value = "Tiến độ thu vốn";
                worksheet.Cells["N3"].Value = "Mã KH";
                worksheet.Cells["O3"].Value = "Đối tác";
                worksheet.Cells["P3"].Value = "ĐVTH";
                worksheet.Cells["Q3"].Value = "Điều độ";
                worksheet.Cells["R3"].Value = "Giá trị tạm ứng";
                worksheet.Cells["S3"].Value = "Ngày hóa đơn";
                worksheet.Cells["T3"].Value = "Kế hoạch thu";
                worksheet.Cells["U3"].Value = "Trạng thái";
                int row = 4;
                int stt = 1;
                foreach (var item in reports)
                {
                    int col = 1;

                    worksheet.Cells[row, col].Value = stt;
                    col++;
                    worksheet.Cells[row, col].Value = item.ConstructionCode;
                    col++;
                    worksheet.Cells[row, col].Value = item.ConstructionName;
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractName;
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractPeriod;
                    col++;
                    worksheet.Cells[row, col].Value = item.ConstructionType;
                    col++;
                    worksheet.Cells[row, col].Value = item.IsEVN;
                    col++;
                    worksheet.Cells[row, col].Value = item.TotalAmount;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractForms;
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractCode;
                    col++;
                    worksheet.Cells[row, col].Value = item.SignedDate.ToString("dd/MM/yyyy");
                    col++;
                    worksheet.Cells[row, col].Value = item.Progress.ToString("dd/MM/yyyy");
                    col++;
                    worksheet.Cells[row, col].Value = item.PlanAdvance + " / " + item.PlanPayment;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    col++;
                    worksheet.Cells[row, col].Value = item.ClientCode;
                    col++;
                    worksheet.Cells[row, col].Value = item.ClientName;
                    col++;
                    worksheet.Cells[row, col].Value = item.UnitCode;
                    col++;
                    worksheet.Cells[row, col].Value = "";
                    col++;
                    worksheet.Cells[row, col].Value = item.TotalAdvance;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    col++;
                    worksheet.Cells[row, col].Value = item.BillDate;
                    col++;
                    worksheet.Cells[row, col].Value = "";
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractStatus.ToStatusContractVN();
                    row++;
                    stt++;
                }
                string strMerge = "A" + row.ToString() + ":B" + row.ToString();
                worksheet.Cells[strMerge].Merge = true;
                worksheet.Cells[row, 1].Value = "Tổng cộng";
                worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                var colTong = 2;
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = reports.Sum(c => c.TotalAmount);
                worksheet.Cells[row, colTong].Style.Numberformat.Format = "#,###,##0.#0";
                worksheet.Cells[row, colTong].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = reports.Sum(c => c.PlanAdvance) + " / " + reports.Sum(c => c.PlanPayment);
                worksheet.Cells[row, colTong].Style.Numberformat.Format = "#,###,##0.#0";
                worksheet.Cells[row, colTong].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = reports.Sum(c => c.TotalAdvance);
                worksheet.Cells[row, colTong].Style.Numberformat.Format = "#,###,##0.#0";
                worksheet.Cells[row, colTong].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //style tong cong
                worksheet.Cells[row, 1].Style.Font.Size = 12;
                worksheet.Cells[row, 1].Style.Font.Bold = true;

                string modelRangeheader = "A1:U3";
                var modelTableheader = worksheet.Cells[modelRangeheader];
                modelTableheader.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                modelTableheader.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                modelTableheader.Style.Font.Bold = true;

                var modelRows = row;
                string modelRange = "A4:U" + modelRows.ToString();
                var modelTable = worksheet.Cells[modelRange];

                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Dashed;
                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.DashDot;
                modelTable.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                string _modelRange = "A1:U" + modelRows.ToString();
                var _modelTable = worksheet.Cells[_modelRange];

                _modelTable.AutoFitColumns();
                xlPackage.Save();
            }
        }
        /// <summary>
        /// Export report contract payment acceptance list to XLSX
        /// </summary>
        /// <param name="report">Report</param>
        public void ExportExcelContractPaymentAcceptance(IList<ContractAcceptanceReport> reports, Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("NghiemThuNoiBo");
                worksheet.DefaultRowHeight = 20;
                worksheet.Cells["A1:E1"].Merge = true;
                worksheet.Cells[1, 1].Value = "BÁO CÁO NGHIỆM THU NỘI BỘ";
                worksheet.Cells["A2:E2"].Merge = true;
                worksheet.Cells[2, 1].Value = reports != null && reports.Count() > 0 ? reports.Select(c => c.dateTime).First() + " Đơn vị: " + reports.Select(c => c.unitName).First() : "";
                worksheet.Cells[1, 1].Style.Font.Size = 14;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells["A3:A4"].Merge = true;
                worksheet.Cells["A3"].Value = "STT";
                worksheet.Cells["B3:B4"].Merge = true;
                worksheet.Cells["B3"].Value = "Mã công trình";
                worksheet.Cells["C3:C4"].Merge = true;
                worksheet.Cells["C3"].Value = "Tên công trình";
                worksheet.Cells["D3:D4"].Merge = true;
                worksheet.Cells["D3"].Value = "Mã hợp đồng";
                worksheet.Cells["E3:E4"].Merge = true;
                worksheet.Cells["E3"].Value = "Tên hợp đồng";
                worksheet.Cells["F3:F4"].Merge = true;
                worksheet.Cells["F3"].Value = "Giai đoạn";
                worksheet.Cells["G3:G4"].Merge = true;
                worksheet.Cells["G3"].Value = "Nghiệm thu trong kỳ";
                worksheet.Cells["H3:H4"].Merge = true;
                worksheet.Cells["H3"].Value = "Sản lượng đã tạm ứng";
                worksheet.Cells["I3:I4"].Merge = true;
                worksheet.Cells["I3"].Value = "Sản lượng nghiệm thu kỳ này";
                worksheet.Cells["J3:L3"].Merge = true;
                worksheet.Cells["J3"].Value = "Giảm trừ";
                worksheet.Cells["J4"].Value = "Trừ khấu hao";
                worksheet.Cells["K4"].Value = "Khoản giữ lại";
                worksheet.Cells["L4"].Value = "Giảm trừ khác";
                worksheet.Cells["M3:M4"].Merge = true;
                worksheet.Cells["M3"].Value = "Sản lượng NT còn lại";
                worksheet.Cells["N3:N4"].Merge = true;
                worksheet.Cells["N3"].Value = "Tỉ lệ khoán";
                worksheet.Cells["O3:Q3"].Merge = true;
                worksheet.Cells["O3"].Value = "Sản lượng khoán cho đơn vị";
                worksheet.Cells["O4"].Value = "Tổng cộng";
                worksheet.Cells["P4"].Value = "Tự làm";
                worksheet.Cells["Q4"].Value = "Thuê ngoài";
                int row = 5;
                int stt = 1;
                foreach (var item in reports)
                {
                    int col = 1;

                    worksheet.Cells[row, col].Value = stt;
                    col++;
                    worksheet.Cells[row, col].Value = item.ConstructionCode;
                    col++;
                    worksheet.Cells[row, col].Value = item.ConstructionName;
                    col++;
                    worksheet.Cells[row, col].Value = item.Code;
                    col++;
                    worksheet.Cells[row, col].Value = item.Name;
                    col++;
                    worksheet.Cells[row, col].Value = item.contractPeriod;
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractPaymentAcceptance;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    col++;
                    worksheet.Cells[row, col].Value = item.ReduceAdvance;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    col++;
                    worksheet.Cells[row, col].Value = (item.ContractPaymentAcceptance != null ? item.ContractPaymentAcceptance : 0) - (item.ReduceAdvance != null ? item.ReduceAdvance : 0);
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    col++;
                    worksheet.Cells[row, col].Value = item.Depreciation;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    col++;
                    worksheet.Cells[row, col].Value = item.ReduceKeep;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    col++;
                    worksheet.Cells[row, col].Value = item.ReduceOrther;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    col++;
                    worksheet.Cells[row, col].Value = item.TotalAmount;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    col++;
                    worksheet.Cells[row, col].Value = item.Ratio;
                    col++;
                    worksheet.Cells[row, col].Value = item.TotalAmount * (item.Ratio != null ? item.Ratio : 0);
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    col++;
                    worksheet.Cells[row, col].Value = "";
                    col++;
                    worksheet.Cells[row, col].Value = "";
                    row++;
                    stt++;
                }
                string strMerge = "A" + row.ToString() + ":B" + row.ToString();
                worksheet.Cells[strMerge].Merge = true;
                worksheet.Cells[row, 1].Value = "Tổng cộng";
                worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                var colTong = 2;
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = reports.Sum(c => c.ContractPaymentAcceptance);
                colTong++;
                worksheet.Cells[row, colTong].Value = reports.Sum(c => c.ReduceAdvance);
                colTong++;
                worksheet.Cells[row, colTong].Value = reports.Sum(c => c.ContractPaymentAcceptance) - reports.Sum(c => c.ReduceAdvance);
                colTong++;
                worksheet.Cells[row, colTong].Value = reports.Sum(c => c.Depreciation);
                colTong++;
                worksheet.Cells[row, colTong].Value = reports.Sum(c => c.ReduceKeep);
                colTong++;
                worksheet.Cells[row, colTong].Value = reports.Sum(c => c.ReduceOrther);
                colTong++;
                worksheet.Cells[row, colTong].Value = reports.Sum(c => c.TotalAmount);
                //style tong cong
                worksheet.Cells[row, 1].Style.Font.Size = 12;
                worksheet.Cells[row, 1].Style.Font.Bold = true;

                string modelRangeheader = "A1:Q4";
                var modelTableheader = worksheet.Cells[modelRangeheader];
                modelTableheader.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                modelTableheader.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                modelTableheader.Style.Font.Bold = true;

                var modelRows = row;
                string modelRange = "A5:Q" + modelRows.ToString();
                var modelTable = worksheet.Cells[modelRange];

                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Dashed;
                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.DashDot;

                string _modelRange = "A5:Q" + modelRows.ToString();
                var _modelTable = worksheet.Cells[_modelRange];

                _modelTable.AutoFitColumns();
                xlPackage.Save();
            }
        }
        /// <summary>
        /// Export report list to XLSX
        /// </summary>
        /// <param name="report">Report</param>
        public void ExportReportToXlsx(IList<ContractReport> reports, Stream stream, DateTime timeFrom, DateTime timeTo)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("BaoCaoChung");
                worksheet.DefaultRowHeight = 20;
                worksheet.Cells["A1:E1"].Merge = true;
                worksheet.Cells[1, 1].Value = "BÁO CÁO HỢP ĐỒNG";
                worksheet.Cells["A2:E2"].Merge = true;
                worksheet.Cells[2, 1].Value = "Từ ngày " + timeFrom.toDateVNString() + " đến ngày " + timeTo.toDateVNString();
                worksheet.Cells[1, 1].Style.Font.Size = 14;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells["A3"].Value = "STT";
                worksheet.Cells["B3"].Value = "Mã CT";
                worksheet.Cells["C3"].Value = "Tên dự án";
                worksheet.Cells["D3"].Value = "Chủ đầu tư";
                worksheet.Cells["E3"].Value = "Tên hợp đồng";
                worksheet.Cells["F3"].Value = "Loại công trình";
                worksheet.Cells["G3"].Value = "Trong EVN";
                worksheet.Cells["H3"].Value = "Giá trị";
                worksheet.Cells["I3"].Value = "Hình thức hợp đồng";
                worksheet.Cells["J3"].Value = "Mã hợp đồng";
                worksheet.Cells["K3"].Value = "Ngày ký";
                worksheet.Cells["L3"].Value = "Ngày dự kiến kết thúc";
                worksheet.Cells["M3"].Value = "Giá trị tạm ứng";
                worksheet.Cells["N3"].Value = "Trạng thái";
                worksheet.Cells["O3"].Value = "Ngày kết thúc";
                int row = 4;
                int stt = 1;
                foreach (var item in reports)
                {
                    int col = 1;

                    worksheet.Cells[row, col].Value = stt;
                    col++;
                    worksheet.Cells[row, col].Value = item.ConstructionCode;
                    col++;
                    worksheet.Cells[row, col].Value = item.ConstructionName;
                    col++;
                    worksheet.Cells[row, col].Value = item.ProcuringAgencyName;
                    col++;
                    worksheet.Cells[row, col].Value = item.Name;
                    col++;
                    worksheet.Cells[row, col].Value = item.ConstructionType;
                    col++;
                    worksheet.Cells[row, col].Value = item.ProcuringAgencyIsEVN;
                    col++;
                    worksheet.Cells[row, col].Value = item.TotalAmount;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    col++;
                    worksheet.Cells[row, col].Value = item.ContractForm;
                    col++;
                    worksheet.Cells[row, col].Value = item.Code;
                    col++;
                    worksheet.Cells[row, col].Value = item.SignedDate.ToString("dd/MM/yyyy");
                    col++;
                    worksheet.Cells[row, col].Value = item.EndDate.ToString("dd/MM/yyyy");
                    col++;
                    worksheet.Cells[row, col].Value = item.AdvanceAmount;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "#,###,##0.#0";
                    worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    col++;
                    worksheet.Cells[row, col].Value = item.StatusId.ToStatusContractVN();
                    col++;
                    worksheet.Cells[row, col].Value = item.FinishDate != null ? Convert.ToDateTime(item.FinishDate).ToString("dd/MM/yyyy") : "";
                    row++;
                    stt++;
                }
                string strMerge = "A" + row.ToString() + ":B" + row.ToString();
                worksheet.Cells[strMerge].Merge = true;
                worksheet.Cells[row, 1].Value = "Tổng cộng";
                worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                var colTong = 2;
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = reports.Sum(c => c.TotalAmount);
                worksheet.Cells[row, colTong].Style.Numberformat.Format = "#,###,##0.#0";
                worksheet.Cells[row, colTong].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = "";
                colTong++;
                worksheet.Cells[row, colTong].Value = reports.Sum(c => c.AdvanceAmount);
                worksheet.Cells[row, colTong].Style.Numberformat.Format = "#,###,##0.#0";
                worksheet.Cells[row, colTong].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //style tong cong
                worksheet.Cells[row, 1].Style.Font.Size = 12;
                worksheet.Cells[row, 1].Style.Font.Bold = true;

                string modelRangeheader = "A1:O3";
                var modelTableheader = worksheet.Cells[modelRangeheader];
                modelTableheader.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                modelTableheader.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                modelTableheader.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                modelTableheader.Style.Font.Bold = true;

                var modelRows = row;
                string modelRange = "A4:O" + modelRows.ToString();
                var modelTable = worksheet.Cells[modelRange];

                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Dashed;
                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.DashDot;

                string _modelRange = "A1:O" + modelRows.ToString();
                var _modelTable = worksheet.Cells[_modelRange];

                _modelTable.AutoFitColumns();
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Export customer list to XML
        /// </summary>
        /// <param name="customers">Customers</param>
        /// <returns>Result in XML format</returns>
        public virtual string ExportCustomersToXml(IList<Customer> customers)
        {
            var sb = new StringBuilder();
            var stringWriter = new StringWriter(sb);
            var xmlWriter = new XmlTextWriter(stringWriter);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Customers");
            xmlWriter.WriteAttributeString("Version", GSVersion.CurrentVersion);

            foreach (var customer in customers)
            {
                xmlWriter.WriteStartElement("Customer");
                xmlWriter.WriteElementString("CustomerId", null, customer.Id.ToString());
                xmlWriter.WriteElementString("CustomerGuid", null, customer.CustomerGuid.ToString());
                xmlWriter.WriteElementString("Email", null, customer.Email);
                xmlWriter.WriteElementString("Username", null, customer.Username);

                var customerPassword = _customerService.GetCurrentPassword(customer.Id);
                xmlWriter.WriteElementString("Password", null, customerPassword?.Password);
                xmlWriter.WriteElementString("PasswordFormatId", null, (customerPassword?.PasswordFormatId ?? 0).ToString());
                xmlWriter.WriteElementString("PasswordSalt", null, customerPassword?.PasswordSalt);

                xmlWriter.WriteElementString("IsTaxExempt", null, customer.IsTaxExempt.ToString());
                xmlWriter.WriteElementString("AffiliateId", null, customer.AffiliateId.ToString());
                xmlWriter.WriteElementString("VendorId", null, customer.VendorId.ToString());
                xmlWriter.WriteElementString("Active", null, customer.Active.ToString());

                xmlWriter.WriteElementString("IsGuest", null, customer.IsGuest().ToString());
                xmlWriter.WriteElementString("IsRegistered", null, customer.IsRegistered().ToString());
                xmlWriter.WriteElementString("IsAdministrator", null, customer.IsAdmin().ToString());
                xmlWriter.WriteElementString("IsForumModerator", null, customer.IsForumModerator().ToString());
                xmlWriter.WriteElementString("CreatedOnUtc", null, customer.CreatedOnUtc.ToString(CultureInfo.InvariantCulture));

                xmlWriter.WriteElementString("FirstName", null, _genericAttributeService.GetAttribute<string>(customer, GSCustomerDefaults.FirstNameAttribute));
                xmlWriter.WriteElementString("LastName", null, _genericAttributeService.GetAttribute<string>(customer, GSCustomerDefaults.LastNameAttribute));
                xmlWriter.WriteElementString("Gender", null, _genericAttributeService.GetAttribute<string>(customer, GSCustomerDefaults.GenderAttribute));
                xmlWriter.WriteElementString("Company", null, _genericAttributeService.GetAttribute<string>(customer, GSCustomerDefaults.CompanyAttribute));

                xmlWriter.WriteElementString("CountryId", null, _genericAttributeService.GetAttribute<int>(customer, GSCustomerDefaults.CountryIdAttribute).ToString());
                xmlWriter.WriteElementString("StreetAddress", null, _genericAttributeService.GetAttribute<string>(customer, GSCustomerDefaults.StreetAddressAttribute));
                xmlWriter.WriteElementString("StreetAddress2", null, _genericAttributeService.GetAttribute<string>(customer, GSCustomerDefaults.StreetAddress2Attribute));
                xmlWriter.WriteElementString("ZipPostalCode", null, _genericAttributeService.GetAttribute<string>(customer, GSCustomerDefaults.ZipPostalCodeAttribute));
                xmlWriter.WriteElementString("City", null, _genericAttributeService.GetAttribute<string>(customer, GSCustomerDefaults.CityAttribute));
                xmlWriter.WriteElementString("County", null, _genericAttributeService.GetAttribute<string>(customer, GSCustomerDefaults.CountyAttribute));
                xmlWriter.WriteElementString("StateProvinceId", null, _genericAttributeService.GetAttribute<int>(customer, GSCustomerDefaults.StateProvinceIdAttribute).ToString());
                xmlWriter.WriteElementString("Phone", null, _genericAttributeService.GetAttribute<string>(customer, GSCustomerDefaults.PhoneAttribute));
                xmlWriter.WriteElementString("Fax", null, _genericAttributeService.GetAttribute<string>(customer, GSCustomerDefaults.FaxAttribute));
                xmlWriter.WriteElementString("VatNumber", null, _genericAttributeService.GetAttribute<string>(customer, GSCustomerDefaults.VatNumberAttribute));
                xmlWriter.WriteElementString("VatNumberStatusId", null, _genericAttributeService.GetAttribute<int>(customer, GSCustomerDefaults.VatNumberStatusIdAttribute).ToString());
                xmlWriter.WriteElementString("TimeZoneId", null, _genericAttributeService.GetAttribute<string>(customer, GSCustomerDefaults.TimeZoneIdAttribute));

                foreach (var store in _storeService.GetAllStores())
                {
                    var newsletter = _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(customer.Email, store.Id);
                    var subscribedToNewsletters = newsletter != null && newsletter.Active;
                    xmlWriter.WriteElementString($"Newsletter-in-store-{store.Id}", null, subscribedToNewsletters.ToString());
                }

                xmlWriter.WriteElementString("AvatarPictureId", null, _genericAttributeService.GetAttribute<int>(customer, GSCustomerDefaults.AvatarPictureIdAttribute).ToString());
                xmlWriter.WriteElementString("ForumPostCount", null, _genericAttributeService.GetAttribute<int>(customer, GSCustomerDefaults.ForumPostCountAttribute).ToString());
                xmlWriter.WriteElementString("Signature", null, _genericAttributeService.GetAttribute<string>(customer, GSCustomerDefaults.SignatureAttribute));

                var selectedCustomerAttributesString = _genericAttributeService.GetAttribute<string>(customer, GSCustomerDefaults.CustomCustomerAttributes);

                if (!string.IsNullOrEmpty(selectedCustomerAttributesString))
                {
                    var selectedCustomerAttributes = new StringReader(selectedCustomerAttributesString);
                    var selectedCustomerAttributesXmlReader = XmlReader.Create(selectedCustomerAttributes);
                    xmlWriter.WriteNode(selectedCustomerAttributesXmlReader, false);
                }

                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return stringWriter.ToString();
        }

        /// <summary>
        /// Export newsletter subscribers to TXT
        /// </summary>
        /// <param name="subscriptions">Subscriptions</param>
        /// <returns>Result in TXT (string) format</returns>
        public virtual string ExportNewsletterSubscribersToTxt(IList<NewsLetterSubscription> subscriptions)
        {
            if (subscriptions == null)
                throw new ArgumentNullException(nameof(subscriptions));

            const string separator = ",";
            var sb = new StringBuilder();
            foreach (var subscription in subscriptions)
            {
                sb.Append(subscription.Email);
                sb.Append(separator);
                sb.Append(subscription.Active);
                sb.Append(separator);
                sb.Append(subscription.StoreId);
                sb.Append(Environment.NewLine); //new line
            }

            return sb.ToString();
        }

        /// <summary>
        /// Export states to TXT
        /// </summary>
        /// <param name="states">States</param>
        /// <returns>Result in TXT (string) format</returns>
        public virtual string ExportStatesToTxt(IList<StateProvince> states)
        {
            if (states == null)
                throw new ArgumentNullException(nameof(states));

            const string separator = ",";
            var sb = new StringBuilder();
            foreach (var state in states)
            {
                sb.Append(state.Country.TwoLetterIsoCode);
                sb.Append(separator);
                sb.Append(state.Name);
                sb.Append(separator);
                sb.Append(state.Abbreviation);
                sb.Append(separator);
                sb.Append(state.Published);
                sb.Append(separator);
                sb.Append(state.DisplayOrder);
                sb.Append(Environment.NewLine); //new line
            }

            return sb.ToString();
        }

        /// <summary>
        /// Export customer info (GDPR request) to XLSX 
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Customer GDPR info</returns>
        public virtual byte[] ExportCustomerGdprInfoToXlsx(Customer customer, int storeId)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            //customer info and customer attributes
            var customerManager = new PropertyManager<Customer>(new[]
            {
                new PropertyByName<Customer>("Email", p => p.Email, _customerSettings.UsernamesEnabled),
                new PropertyByName<Customer>("Username", p => p.Username, !_customerSettings.UsernamesEnabled), 
                //attributes
                new PropertyByName<Customer>("First name", p => _genericAttributeService.GetAttribute<string>(p, GSCustomerDefaults.FirstNameAttribute)),
                new PropertyByName<Customer>("Last name", p => _genericAttributeService.GetAttribute<string>(p, GSCustomerDefaults.LastNameAttribute)),
                new PropertyByName<Customer>("Gender", p => _genericAttributeService.GetAttribute<string>(p, GSCustomerDefaults.GenderAttribute), !_customerSettings.GenderEnabled),
                new PropertyByName<Customer>("Date of birth", p => _genericAttributeService.GetAttribute<string>(p, GSCustomerDefaults.DateOfBirthAttribute), !_customerSettings.DateOfBirthEnabled),
                new PropertyByName<Customer>("Company", p => _genericAttributeService.GetAttribute<string>(p, GSCustomerDefaults.CompanyAttribute), !_customerSettings.CompanyEnabled),
                new PropertyByName<Customer>("Street address", p => _genericAttributeService.GetAttribute<string>(p, GSCustomerDefaults.StreetAddressAttribute), !_customerSettings.StreetAddressEnabled),
                new PropertyByName<Customer>("Street address 2", p => _genericAttributeService.GetAttribute<string>(p, GSCustomerDefaults.StreetAddress2Attribute), !_customerSettings.StreetAddress2Enabled),
                new PropertyByName<Customer>("Zip / postal code", p => _genericAttributeService.GetAttribute<string>(p, GSCustomerDefaults.ZipPostalCodeAttribute), !_customerSettings.ZipPostalCodeEnabled),
                new PropertyByName<Customer>("City", p => _genericAttributeService.GetAttribute<string>(p, GSCustomerDefaults.CityAttribute), !_customerSettings.CityEnabled),
                new PropertyByName<Customer>("County", p => _genericAttributeService.GetAttribute<string>(p, GSCustomerDefaults.CountyAttribute), !_customerSettings.CountyEnabled),
                new PropertyByName<Customer>("Country", p => _countryService.GetCountryById(_genericAttributeService.GetAttribute<int>(p, GSCustomerDefaults.CountryIdAttribute))?.Name ?? string.Empty, !_customerSettings.CountryEnabled),
                new PropertyByName<Customer>("State province", p => _stateProvinceService.GetStateProvinceById(_genericAttributeService.GetAttribute<int>(p, GSCustomerDefaults.StateProvinceIdAttribute))?.Name ?? string.Empty, !(_customerSettings.StateProvinceEnabled && _customerSettings.CountryEnabled)),
                new PropertyByName<Customer>("Phone", p => _genericAttributeService.GetAttribute<string>(p, GSCustomerDefaults.PhoneAttribute), !_customerSettings.PhoneEnabled),
                new PropertyByName<Customer>("Fax", p => _genericAttributeService.GetAttribute<string>(p, GSCustomerDefaults.FaxAttribute), !_customerSettings.FaxEnabled),
                new PropertyByName<Customer>("Customer attributes",  GetCustomCustomerAttributes)
            }, _catalogSettings);



            //customer addresses
            var addressManager = new PropertyManager<Address>(new[]
            {
                new PropertyByName<Address>("First name", p => p.FirstName),
                new PropertyByName<Address>("Last name", p => p.LastName),
                new PropertyByName<Address>("Email", p => p.Email),
                new PropertyByName<Address>("Company", p => p.Company, !_addressSettings.CompanyEnabled),
                new PropertyByName<Address>("Country", p => p.Country != null ? _localizationService.GetLocalized(p.Country, c => c.Name) : string.Empty, !_addressSettings.CountryEnabled),
                new PropertyByName<Address>("State province", p => p.StateProvince != null ? _localizationService.GetLocalized(p.StateProvince, sp => sp.Name) : string.Empty, !_addressSettings.StateProvinceEnabled),
                new PropertyByName<Address>("County", p => p.County, !_addressSettings.CountyEnabled),
                new PropertyByName<Address>("City", p => p.City, !_addressSettings.CityEnabled),
                new PropertyByName<Address>("Address 1", p => p.Address1, !_addressSettings.StreetAddressEnabled),
                new PropertyByName<Address>("Address 2", p => p.Address2, !_addressSettings.StreetAddress2Enabled),
                new PropertyByName<Address>("Zip / postal code", p => p.ZipPostalCode, !_addressSettings.ZipPostalCodeEnabled),
                new PropertyByName<Address>("Phone number", p => p.PhoneNumber, !_addressSettings.PhoneEnabled),
                new PropertyByName<Address>("Fax number", p => p.FaxNumber, !_addressSettings.FaxEnabled),
                new PropertyByName<Address>("Custom attributes", p => _customerAttributeFormatter.FormatAttributes(p.CustomAttributes, ";"))
            }, _catalogSettings);

            //customer private messages
            var privateMessageManager = new PropertyManager<PrivateMessage>(new[]
            {
                new PropertyByName<PrivateMessage>("From", pm => _customerSettings.UsernamesEnabled ? pm.FromCustomer.Username : pm.FromCustomer.Email),
                new PropertyByName<PrivateMessage>("To", pm => _customerSettings.UsernamesEnabled ? pm.ToCustomer.Username : pm.ToCustomer.Email),
                new PropertyByName<PrivateMessage>("Subject", pm => pm.Subject),
                new PropertyByName<PrivateMessage>("Text", pm => pm.Text),
                new PropertyByName<PrivateMessage>("Created on", pm => _dateTimeHelper.ConvertToUserTime(pm.CreatedOnUtc, DateTimeKind.Utc).ToString("D"))
            }, _catalogSettings);

            List<PrivateMessage> pmList = null;
            if (_forumSettings.AllowPrivateMessages)
            {
                pmList = _forumService.GetAllPrivateMessages(storeId, customer.Id, 0, null, null, null, null).ToList();
                pmList.AddRange(_forumService.GetAllPrivateMessages(storeId, 0, customer.Id, null, null, null, null).ToList());
            }

            //customer GDPR logs
            var gdprLogManager = new PropertyManager<GdprLog>(new[]
            {
                new PropertyByName<GdprLog>("Request type", log => _localizationService.GetLocalizedEnum(log.RequestType)),
                new PropertyByName<GdprLog>("Request details", log => log.RequestDetails),
                new PropertyByName<GdprLog>("Created on", log => _dateTimeHelper.ConvertToUserTime(log.CreatedOnUtc, DateTimeKind.Utc).ToString("D"))
            }, _catalogSettings);

            var gdprLog = _gdprService.GetAllLog(customer.Id);

            using (var stream = new MemoryStream())
            {
                // ok, we can run the real code of the sample now
                using (var xlPackage = new ExcelPackage(stream))
                {
                    // uncomment this line if you want the XML written out to the outputDir
                    //xlPackage.DebugMode = true; 

                    // get handles to the worksheets
                    var customerInfoWorksheet = xlPackage.Workbook.Worksheets.Add("Customer info");
                    var fWorksheet = xlPackage.Workbook.Worksheets.Add("DataForFilters");
                    fWorksheet.Hidden = eWorkSheetHidden.VeryHidden;

                    //customer info and customer attributes
                    var customerInfoRow = 2;
                    customerManager.CurrentObject = customer;
                    customerManager.WriteCaption(customerInfoWorksheet);
                    customerManager.WriteToXlsx(customerInfoWorksheet, customerInfoRow);

                    //customer addresses
                    if (customer.Addresses.Any())
                    {
                        customerInfoRow += 2;

                        var cell = customerInfoWorksheet.Cells[customerInfoRow, 1];
                        cell.Value = "Address List";
                        customerInfoRow += 1;
                        addressManager.SetCaptionStyle(cell);
                        addressManager.WriteCaption(customerInfoWorksheet, customerInfoRow);

                        foreach (var customerAddress in customer.Addresses)
                        {
                            customerInfoRow += 1;
                            addressManager.CurrentObject = customerAddress;
                            addressManager.WriteToXlsx(customerInfoWorksheet, customerInfoRow);
                        }
                    }



                    //customer private messages
                    if (pmList?.Any() ?? false)
                    {
                        var privateMessageWorksheet = xlPackage.Workbook.Worksheets.Add("Private messages");
                        privateMessageManager.WriteCaption(privateMessageWorksheet);

                        var privateMessageRow = 1;

                        foreach (var privateMessage in pmList)
                        {
                            privateMessageRow += 1;

                            privateMessageManager.CurrentObject = privateMessage;
                            privateMessageManager.WriteToXlsx(privateMessageWorksheet, privateMessageRow);
                        }
                    }

                    //customer GDPR logs
                    if (gdprLog.Any())
                    {
                        var gdprLogWorksheet = xlPackage.Workbook.Worksheets.Add("GDPR requests (log)");
                        gdprLogManager.WriteCaption(gdprLogWorksheet);

                        var gdprLogRow = 1;

                        foreach (var log in gdprLog)
                        {
                            gdprLogRow += 1;

                            gdprLogManager.CurrentObject = log;
                            gdprLogManager.WriteToXlsx(gdprLogWorksheet, gdprLogRow);
                        }
                    }

                    xlPackage.Save();
                }

                return stream.ToArray();
            }
        }

        #endregion
    }
}