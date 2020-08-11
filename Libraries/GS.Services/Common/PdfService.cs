// RTL Support provided by Credo inc (www.credo.co.il  ||   info@credo.co.il)

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using GS.Core;
using GS.Core.Domain.Catalog;
using GS.Core.Domain.Common;
using GS.Core.Domain.Directory;
using GS.Core.Domain.Localization;
using GS.Core.Domain.Tax;
using GS.Core.Domain.Vendors;
using GS.Core.Html;
using GS.Core.Infrastructure;
using GS.Services.Catalog;
using GS.Services.Configuration;
using GS.Services.Directory;
using GS.Services.Helpers;
using GS.Services.Localization;
using GS.Services.Media;
using GS.Services.Stores;
using GS.Services.Vendors;

namespace GS.Services.Common
{
    /// <summary>
    /// PDF service
    /// </summary>
    public partial class PdfService : IPdfService
    {
        #region Fields

        private readonly AddressSettings _addressSettings;
        private readonly CatalogSettings _catalogSettings;
        private readonly CurrencySettings _currencySettings;
        private readonly IAddressAttributeFormatter _addressAttributeFormatter;
        private readonly ICurrencyService _currencyService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IMeasureService _measureService;
        private readonly IGSFileProvider _fileProvider;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly IVendorService _vendorService;
        private readonly IWorkContext _workContext;
        private readonly MeasureSettings _measureSettings;
        private readonly PdfSettings _pdfSettings;
        private readonly TaxSettings _taxSettings;
        private readonly VendorSettings _vendorSettings;

        #endregion

        #region Ctor

        public PdfService(AddressSettings addressSettings,
            CatalogSettings catalogSettings,
            CurrencySettings currencySettings,
            IAddressAttributeFormatter addressAttributeFormatter,
            ICurrencyService currencyService,
            IDateTimeHelper dateTimeHelper,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IMeasureService measureService,
            IGSFileProvider fileProvider,
            IPictureService pictureService,
            ISettingService settingService,
            IStoreContext storeContext,
            IStoreService storeService,
            IVendorService vendorService,
            IWorkContext workContext,
            MeasureSettings measureSettings,
            PdfSettings pdfSettings,
            TaxSettings taxSettings,
            VendorSettings vendorSettings)
        {
            this._addressSettings = addressSettings;
            this._catalogSettings = catalogSettings;
            this._currencySettings = currencySettings;
            this._addressAttributeFormatter = addressAttributeFormatter;
            this._currencyService = currencyService;
            this._dateTimeHelper = dateTimeHelper;
            this._languageService = languageService;
            this._localizationService = localizationService;
            this._measureService = measureService;
            this._fileProvider = fileProvider;
            this._pictureService = pictureService;
            this._settingService = settingService;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._vendorService = vendorService;
            this._workContext = workContext;
            this._measureSettings = measureSettings;
            this._pdfSettings = pdfSettings;
            this._taxSettings = taxSettings;
            this._vendorSettings = vendorSettings;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get font
        /// </summary>
        /// <returns>Font</returns>
        protected virtual Font GetFont()
        {
            //nopCommerce supports Unicode characters
            //nopCommerce uses Free Serif font by default (~/App_Data/Pdf/FreeSerif.ttf file)
            //It was downloaded from http://savannah.gnu.org/projects/freefont
            return GetFont(_pdfSettings.FontFileName);
        }

        /// <summary>
        /// Get font
        /// </summary>
        /// <param name="fontFileName">Font file name</param>
        /// <returns>Font</returns>
        protected virtual Font GetFont(string fontFileName)
        {
            if (fontFileName == null)
                throw new ArgumentNullException(nameof(fontFileName));

            var fontPath = _fileProvider.Combine(_fileProvider.MapPath("~/App_Data/Pdf/"), fontFileName);
            var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            var font = new Font(baseFont, 10, Font.NORMAL);
            return font;
        }

        /// <summary>
        /// Get font direction
        /// </summary>
        /// <param name="lang">Language</param>
        /// <returns>Font direction</returns>
        protected virtual int GetDirection(Language lang)
        {
            return lang.Rtl ? PdfWriter.RUN_DIRECTION_RTL : PdfWriter.RUN_DIRECTION_LTR;
        }

        /// <summary>
        /// Get element alignment
        /// </summary>
        /// <param name="lang">Language</param>
        /// <param name="isOpposite">Is opposite?</param>
        /// <returns>Element alignment</returns>
        protected virtual int GetAlignment(Language lang, bool isOpposite = false)
        {
            //if we need the element to be opposite, like logo etc`.
            if (!isOpposite)
                return lang.Rtl ? Element.ALIGN_RIGHT : Element.ALIGN_LEFT;

            return lang.Rtl ? Element.ALIGN_LEFT : Element.ALIGN_RIGHT;
        }

        /// <summary>
        /// Get PDF cell
        /// </summary>
        /// <param name="resourceKey">Locale</param>
        /// <param name="lang">Language</param>
        /// <param name="font">Font</param>
        /// <returns>PDF cell</returns>
        protected virtual PdfPCell GetPdfCell(string resourceKey, Language lang, Font font)
        {
            return new PdfPCell(new Phrase(_localizationService.GetResource(resourceKey, lang.Id), font));
        }

        /// <summary>
        /// Get PDF cell
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="font">Font</param>
        /// <returns>PDF cell</returns>
        protected virtual PdfPCell GetPdfCell(object text, Font font)
        {
            return new PdfPCell(new Phrase(text.ToString(), font));
        }

        /// <summary>
        /// Get paragraph
        /// </summary>
        /// <param name="resourceKey">Locale</param>
        /// <param name="lang">Language</param>
        /// <param name="font">Font</param>
        /// <param name="args">Locale arguments</param>
        /// <returns>Paragraph</returns>
        protected virtual Paragraph GetParagraph(string resourceKey, Language lang, Font font, params object[] args)
        {
            return GetParagraph(resourceKey, string.Empty, lang, font, args);
        }

        /// <summary>
        /// Get paragraph
        /// </summary>
        /// <param name="resourceKey">Locale</param>
        /// <param name="indent">Indent</param>
        /// <param name="lang">Language</param>
        /// <param name="font">Font</param>
        /// <param name="args">Locale arguments</param>
        /// <returns>Paragraph</returns>
        protected virtual Paragraph GetParagraph(string resourceKey, string indent, Language lang, Font font, params object[] args)
        {
            var formatText = _localizationService.GetResource(resourceKey, lang.Id);
            return new Paragraph(indent + (args.Any() ? string.Format(formatText, args) : formatText), font);
        }

        /// <summary>
        /// Print footer
        /// </summary>
        /// <param name="pdfSettingsByStore">PDF settings</param>
        /// <param name="pdfWriter">PDF writer</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="lang">Language</param>
        /// <param name="font">Font</param>
        protected virtual void PrintFooter(PdfSettings pdfSettingsByStore, PdfWriter pdfWriter, Rectangle pageSize, Language lang, Font font)
        {
            if (string.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn1) && string.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn2))
                return;

            var column1Lines = string.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn1)
                ? new List<string>()
                : pdfSettingsByStore.InvoiceFooterTextColumn1
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
            var column2Lines = string.IsNullOrEmpty(pdfSettingsByStore.InvoiceFooterTextColumn2)
                ? new List<string>()
                : pdfSettingsByStore.InvoiceFooterTextColumn2
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

            if (!column1Lines.Any() && !column2Lines.Any())
                return;

            var totalLines = Math.Max(column1Lines.Count, column2Lines.Count);
            const float margin = 43;

            //if you have really a lot of lines in the footer, then replace 9 with 10 or 11
            var footerHeight = totalLines * 9;
            var directContent = pdfWriter.DirectContent;
            directContent.MoveTo(pageSize.GetLeft(margin), pageSize.GetBottom(margin) + footerHeight);
            directContent.LineTo(pageSize.GetRight(margin), pageSize.GetBottom(margin) + footerHeight);
            directContent.Stroke();

            var footerTable = new PdfPTable(2)
            {
                WidthPercentage = 100f,
                RunDirection = GetDirection(lang)
            };
            footerTable.SetTotalWidth(new float[] { 250, 250 });

            //column 1
            if (column1Lines.Any())
            {
                var column1 = new PdfPCell(new Phrase())
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };

                foreach (var footerLine in column1Lines)
                {
                    column1.Phrase.Add(new Phrase(footerLine, font));
                    column1.Phrase.Add(new Phrase(Environment.NewLine));
                }

                footerTable.AddCell(column1);
            }
            else
            {
                var column = new PdfPCell(new Phrase(" ")) { Border = Rectangle.NO_BORDER };
                footerTable.AddCell(column);
            }

            //column 2
            if (column2Lines.Any())
            {
                var column2 = new PdfPCell(new Phrase())
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };

                foreach (var footerLine in column2Lines)
                {
                    column2.Phrase.Add(new Phrase(footerLine, font));
                    column2.Phrase.Add(new Phrase(Environment.NewLine));
                }

                footerTable.AddCell(column2);
            }
            else
            {
                var column = new PdfPCell(new Phrase(" ")) { Border = Rectangle.NO_BORDER };
                footerTable.AddCell(column);
            }

            footerTable.WriteSelectedRows(0, totalLines, pageSize.GetLeft(margin), pageSize.GetBottom(margin) + footerHeight, directContent);
        }

       

        
        /// <summary>
        /// Print products
        /// </summary>
        /// <param name="vendorId">Vendor identifier</param>
        /// <param name="lang">Language</param>
        /// <param name="titleFont">Title font</param>
        /// <param name="doc">Document</param>
        /// <param name="order">Order</param>
        /// <param name="font">Text font</param>
        /// <param name="attributesFont">Product attributes font</param>
        //protected virtual void PrintProducts(int vendorId, Language lang, Font titleFont, Document doc, Order order, Font font, Font attributesFont)
        //{
        //    var productsHeader = new PdfPTable(1)
        //    {
        //        RunDirection = GetDirection(lang),
        //        WidthPercentage = 100f
        //    };
        //    var cellProducts = GetPdfCell("PDFInvoice.Product(s)", lang, titleFont);
        //    cellProducts.Border = Rectangle.NO_BORDER;
        //    productsHeader.AddCell(cellProducts);
        //    doc.Add(productsHeader);
        //    doc.Add(new Paragraph(" "));

        //    var orderItems = order.OrderItems;

        //    var count = 4 + (_catalogSettings.ShowSkuOnProductDetailsPage ? 1 : 0)
        //                + (_vendorSettings.ShowVendorOnOrderDetailsPage ? 1 : 0);

        //    var productsTable = new PdfPTable(count)
        //    {
        //        RunDirection = GetDirection(lang),
        //        WidthPercentage = 100f
        //    };

        //    var widths = new Dictionary<int, int[]>
        //    {
        //        { 4, new[] { 50, 20, 10, 20 } },
        //        { 5, new[] { 45, 15, 15, 10, 15 } },
        //        { 6, new[] { 40, 13, 13, 12, 10, 12 } }
        //    };

        //    productsTable.SetWidths(lang.Rtl ? widths[count].Reverse().ToArray() : widths[count]);

        //    //product name
        //    var cellProductItem = GetPdfCell("PDFInvoice.ProductName", lang, font);
        //    cellProductItem.BackgroundColor = BaseColor.LightGray;
        //    cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
        //    productsTable.AddCell(cellProductItem);

        //    //SKU
        //    if (_catalogSettings.ShowSkuOnProductDetailsPage)
        //    {
        //        cellProductItem = GetPdfCell("PDFInvoice.SKU", lang, font);
        //        cellProductItem.BackgroundColor = BaseColor.LightGray;
        //        cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
        //        productsTable.AddCell(cellProductItem);
        //    }

        //    //Vendor name
        //    if (_vendorSettings.ShowVendorOnOrderDetailsPage)
        //    {
        //        cellProductItem = GetPdfCell("PDFInvoice.VendorName", lang, font);
        //        cellProductItem.BackgroundColor = BaseColor.LightGray;
        //        cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
        //        productsTable.AddCell(cellProductItem);
        //    }

        //    //price
        //    cellProductItem = GetPdfCell("PDFInvoice.ProductPrice", lang, font);
        //    cellProductItem.BackgroundColor = BaseColor.LightGray;
        //    cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
        //    productsTable.AddCell(cellProductItem);

        //    //qty
        //    cellProductItem = GetPdfCell("PDFInvoice.ProductQuantity", lang, font);
        //    cellProductItem.BackgroundColor = BaseColor.LightGray;
        //    cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
        //    productsTable.AddCell(cellProductItem);

        //    //total
        //    cellProductItem = GetPdfCell("PDFInvoice.ProductTotal", lang, font);
        //    cellProductItem.BackgroundColor = BaseColor.LightGray;
        //    cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
        //    productsTable.AddCell(cellProductItem);

        //    var vendors = _vendorSettings.ShowVendorOnOrderDetailsPage ? _vendorService.GetVendorsByIds(orderItems.Select(item => item.Product.VendorId).ToArray()) : new List<Vendor>();

        //    foreach (var orderItem in orderItems)
        //    {
        //        var p = orderItem.Product;

        //        //a vendor should have access only to his products
        //        if (vendorId > 0 && p.VendorId != vendorId)
        //            continue;

        //        var pAttribTable = new PdfPTable(1) { RunDirection = GetDirection(lang) };
        //        pAttribTable.DefaultCell.Border = Rectangle.NO_BORDER;

        //        //product name
        //        var name = _localizationService.GetLocalized(p, x => x.Name, lang.Id);
        //        pAttribTable.AddCell(new Paragraph(name, font));
        //        cellProductItem.AddElement(new Paragraph(name, font));
        //        //attributes
        //        if (!string.IsNullOrEmpty(orderItem.AttributeDescription))
        //        {
        //            var attributesParagraph =
        //                new Paragraph(HtmlHelper.ConvertHtmlToPlainText(orderItem.AttributeDescription, true, true),
        //                    attributesFont);
        //            pAttribTable.AddCell(attributesParagraph);
        //        }

        //        //rental info
        //        if (orderItem.Product.IsRental)
        //        {
        //            var rentalStartDate = orderItem.RentalStartDateUtc.HasValue
        //                ? _productService.FormatRentalDate(orderItem.Product, orderItem.RentalStartDateUtc.Value)
        //                : string.Empty;
        //            var rentalEndDate = orderItem.RentalEndDateUtc.HasValue
        //                ? _productService.FormatRentalDate(orderItem.Product, orderItem.RentalEndDateUtc.Value)
        //                : string.Empty;
        //            var rentalInfo = string.Format(_localizationService.GetResource("Order.Rental.FormattedDate"),
        //                rentalStartDate, rentalEndDate);

        //            var rentalInfoParagraph = new Paragraph(rentalInfo, attributesFont);
        //            pAttribTable.AddCell(rentalInfoParagraph);
        //        }

        //        productsTable.AddCell(pAttribTable);

        //        //SKU
        //        if (_catalogSettings.ShowSkuOnProductDetailsPage)
        //        {
        //            var sku = _productService.FormatSku(p, orderItem.AttributesXml);
        //            cellProductItem = GetPdfCell(sku ?? string.Empty, font);
        //            cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
        //            productsTable.AddCell(cellProductItem);
        //        }

        //        //Vendor name
        //        if (_vendorSettings.ShowVendorOnOrderDetailsPage)
        //        {
        //            var vendorName = vendors.FirstOrDefault(v => v.Id == p.VendorId)?.Name ?? string.Empty;
        //            cellProductItem = GetPdfCell(vendorName, font);
        //            cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
        //            productsTable.AddCell(cellProductItem);
        //        }

        //        //price
        //        string unitPrice;
        //        if (order.CustomerTaxDisplayType == TaxDisplayType.IncludingTax)
        //        {
        //            //including tax
        //            var unitPriceInclTaxInCustomerCurrency =
        //                _currencyService.ConvertCurrency(orderItem.UnitPriceInclTax, order.CurrencyRate);
        //            unitPrice = _priceFormatter.FormatPrice(unitPriceInclTaxInCustomerCurrency, true,
        //                order.CustomerCurrencyCode, lang, true);
        //        }
        //        else
        //        {
        //            //excluding tax
        //            var unitPriceExclTaxInCustomerCurrency =
        //                _currencyService.ConvertCurrency(orderItem.UnitPriceExclTax, order.CurrencyRate);
        //            unitPrice = _priceFormatter.FormatPrice(unitPriceExclTaxInCustomerCurrency, true,
        //                order.CustomerCurrencyCode, lang, false);
        //        }

        //        cellProductItem = GetPdfCell(unitPrice, font);
        //        cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
        //        productsTable.AddCell(cellProductItem);

        //        //qty
        //        cellProductItem = GetPdfCell(orderItem.Quantity, font);
        //        cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
        //        productsTable.AddCell(cellProductItem);

        //        //total
        //        string subTotal;
        //        if (order.CustomerTaxDisplayType == TaxDisplayType.IncludingTax)
        //        {
        //            //including tax
        //            var priceInclTaxInCustomerCurrency =
        //                _currencyService.ConvertCurrency(orderItem.PriceInclTax, order.CurrencyRate);
        //            subTotal = _priceFormatter.FormatPrice(priceInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode,
        //                lang, true);
        //        }
        //        else
        //        {
        //            //excluding tax
        //            var priceExclTaxInCustomerCurrency =
        //                _currencyService.ConvertCurrency(orderItem.PriceExclTax, order.CurrencyRate);
        //            subTotal = _priceFormatter.FormatPrice(priceExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode,
        //                lang, false);
        //        }

        //        cellProductItem = GetPdfCell(subTotal, font);
        //        cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
        //        productsTable.AddCell(cellProductItem);
        //    }

        //    doc.Add(productsTable);
        //}

        /// <summary>
        /// Print addresses
        /// </summary>
        /// <param name="vendorId">Vendor identifier</param>
        /// <param name="lang">Language</param>
        /// <param name="titleFont">Title font</param>
        /// <param name="order">Order</param>
        /// <param name="font">Text font</param>
        /// <param name="doc">Document</param>
        //protected virtual void PrintAddresses(int vendorId, Language lang, Font titleFont, Order order, Font font, Document doc)
        //{
        //    var addressTable = new PdfPTable(2) { RunDirection = GetDirection(lang) };
        //    addressTable.DefaultCell.Border = Rectangle.NO_BORDER;
        //    addressTable.WidthPercentage = 100f;
        //    addressTable.SetWidths(new[] { 50, 50 });

        //    //billing info
        //    PrintBillingInfo(vendorId, lang, titleFont, order, font, addressTable);

        //    //shipping info
        //    PrintShippingInfo(lang, order, titleFont, font, addressTable);

        //    doc.Add(addressTable);
        //    doc.Add(new Paragraph(" "));
        //}

        

        /// <summary>
        /// Print header
        /// </summary>
        /// <param name="pdfSettingsByStore">PDF settings</param>
        /// <param name="lang">Language</param>
        /// <param name="order">Order</param>
        /// <param name="font">Text font</param>
        /// <param name="titleFont">Title font</param>
        /// <param name="doc">Document</param>
        //protected virtual void PrintHeader(PdfSettings pdfSettingsByStore, Language lang, Order order, Font font, Font titleFont, Document doc)
        //{
        //    //logo
        //    var logoPicture = _pictureService.GetPictureById(pdfSettingsByStore.LogoPictureId);
        //    var logoExists = logoPicture != null;

        //    //header
        //    var headerTable = new PdfPTable(logoExists ? 2 : 1)
        //    {
        //        RunDirection = GetDirection(lang)
        //    };
        //    headerTable.DefaultCell.Border = Rectangle.NO_BORDER;

        //    //store info
        //    var store = _storeService.GetStoreById(order.StoreId) ?? _storeContext.CurrentStore;
        //    var anchor = new Anchor(store.Url.Trim('/'), font)
        //    {
        //        Reference = store.Url
        //    };

        //    var cellHeader = GetPdfCell(string.Format(_localizationService.GetResource("PDFInvoice.Order#", lang.Id), order.CustomOrderNumber), titleFont);
        //    cellHeader.Phrase.Add(new Phrase(Environment.NewLine));
        //    cellHeader.Phrase.Add(new Phrase(anchor));
        //    cellHeader.Phrase.Add(new Phrase(Environment.NewLine));
        //    cellHeader.Phrase.Add(GetParagraph("PDFInvoice.OrderDate", lang, font, _dateTimeHelper.ConvertToUserTime(order.CreatedOnUtc, DateTimeKind.Utc).ToString("D", new CultureInfo(lang.LanguageCulture))));
        //    cellHeader.Phrase.Add(new Phrase(Environment.NewLine));
        //    cellHeader.Phrase.Add(new Phrase(Environment.NewLine));
        //    cellHeader.HorizontalAlignment = Element.ALIGN_LEFT;
        //    cellHeader.Border = Rectangle.NO_BORDER;

        //    headerTable.AddCell(cellHeader);

        //    if (logoExists)
        //        headerTable.SetWidths(lang.Rtl ? new[] { 0.2f, 0.8f } : new[] { 0.8f, 0.2f });
        //    headerTable.WidthPercentage = 100f;

        //    //logo               
        //    if (logoExists)
        //    {
        //        var logoFilePath = _pictureService.GetThumbLocalPath(logoPicture, 0, false);
        //        var logo = Image.GetInstance(logoFilePath);
        //        logo.Alignment = GetAlignment(lang, true);
        //        logo.ScaleToFit(65f, 65f);

        //        var cellLogo = new PdfPCell { Border = Rectangle.NO_BORDER };
        //        cellLogo.AddElement(logo);
        //        headerTable.AddCell(cellLogo);
        //    }

        //    doc.Add(headerTable);
        //}

        #endregion

        #region Methods

        ///// <summary>
        ///// Print products to PDF
        ///// </summary>
        ///// <param name="stream">Stream</param>
        ///// <param name="products">Products</param>
        //public virtual void PrintProductsToPdf(Stream stream, IList<Contract> products)
        //{
        //    if (stream == null)
        //        throw new ArgumentNullException(nameof(stream));

        //    if (products == null)
        //        throw new ArgumentNullException(nameof(products));

        //    var lang = _workContext.WorkingLanguage;

        //    var pageSize = PageSize.A4;

        //    if (_pdfSettings.LetterPageSizeEnabled)
        //    {
        //        pageSize = PageSize.Letter;
        //    }

        //    var doc = new Document(pageSize);
        //    PdfWriter.GetInstance(doc, stream);
        //    doc.Open();

        //    //fonts
        //    var titleFont = GetFont();
        //    titleFont.SetStyle(Font.BOLD);
        //    titleFont.Color = BaseColor.Black;
        //    var font = GetFont();

        //    var productNumber = 1;
        //    var prodCount = products.Count;

        //    foreach (var product in products)
        //    {
        //        var productName = _localizationService.GetLocalized(product, x => x.Name, lang.Id);
        //        var productDescription = _localizationService.GetLocalized(product, x => x.FullDescription, lang.Id);

        //        var productTable = new PdfPTable(1) { WidthPercentage = 100f };
        //        productTable.DefaultCell.Border = Rectangle.NO_BORDER;
        //        if (lang.Rtl)
        //        {
        //            productTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
        //        }

        //        productTable.AddCell(new Paragraph($"{productNumber}. {productName}", titleFont));
        //        productTable.AddCell(new Paragraph(" "));
        //        productTable.AddCell(new Paragraph(HtmlHelper.StripTags(HtmlHelper.ConvertHtmlToPlainText(productDescription, decode: true)), font));
        //        productTable.AddCell(new Paragraph(" "));

        //        if (product.ProductType == ProductType.SimpleProduct)
        //        {
        //            //simple product
        //            //render its properties such as price, weight, etc
        //            var priceStr = $"{product.Price:0.00} {_currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode}";
        //            if (product.IsRental)
        //                priceStr = _priceFormatter.FormatRentalProductPeriod(product, priceStr);
        //            productTable.AddCell(new Paragraph($"{_localizationService.GetResource("PDFProductCatalog.Price", lang.Id)}: {priceStr}", font));
        //            productTable.AddCell(new Paragraph($"{_localizationService.GetResource("PDFProductCatalog.SKU", lang.Id)}: {product.Sku}", font));

        //            if (product.IsShipEnabled && product.Weight > decimal.Zero)
        //                productTable.AddCell(new Paragraph($"{_localizationService.GetResource("PDFProductCatalog.Weight", lang.Id)}: {product.Weight:0.00} {_measureService.GetMeasureWeightById(_measureSettings.BaseWeightId).Name}", font));

        //            if (product.ManageInventoryMethod == ManageInventoryMethod.ManageStock)
        //                productTable.AddCell(new Paragraph($"{_localizationService.GetResource("PDFProductCatalog.StockQuantity", lang.Id)}: {_productService.GetTotalStockQuantity(product)}", font));

        //            productTable.AddCell(new Paragraph(" "));
        //        }

        //        var pictures = _pictureService.GetPicturesByProductId(product.Id);
        //        if (pictures.Any())
        //        {
        //            var table = new PdfPTable(2) { WidthPercentage = 100f };
        //            if (lang.Rtl)
        //            {
        //                table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
        //            }

        //            foreach (var pic in pictures)
        //            {
        //                var picBinary = _pictureService.LoadPictureBinary(pic);
        //                if (picBinary == null || picBinary.Length <= 0)
        //                    continue;

        //                var pictureLocalPath = _pictureService.GetThumbLocalPath(pic, 200, false);
        //                var cell = new PdfPCell(Image.GetInstance(pictureLocalPath))
        //                {
        //                    HorizontalAlignment = Element.ALIGN_LEFT,
        //                    Border = Rectangle.NO_BORDER
        //                };
        //                table.AddCell(cell);
        //            }

        //            if (pictures.Count % 2 > 0)
        //            {
        //                var cell = new PdfPCell(new Phrase(" "))
        //                {
        //                    Border = Rectangle.NO_BORDER
        //                };
        //                table.AddCell(cell);
        //            }

        //            productTable.AddCell(table);
        //            productTable.AddCell(new Paragraph(" "));
        //        }

        //        if (product.ProductType == ProductType.GroupedProduct)
        //        {
        //            //grouped product. render its associated products
        //            var pvNum = 1;
        //            foreach (var associatedProduct in _productService.GetAssociatedProducts(product.Id, showHidden: true))
        //            {
        //                productTable.AddCell(new Paragraph($"{productNumber}-{pvNum}. {_localizationService.GetLocalized(associatedProduct, x => x.Name, lang.Id)}", font));
        //                productTable.AddCell(new Paragraph(" "));

        //                //uncomment to render associated product description
        //                //string apDescription = associated_localizationService.GetLocalized(product, x => x.ShortDescription, lang.Id);
        //                //if (!string.IsNullOrEmpty(apDescription))
        //                //{
        //                //    productTable.AddCell(new Paragraph(HtmlHelper.StripTags(HtmlHelper.ConvertHtmlToPlainText(apDescription)), font));
        //                //    productTable.AddCell(new Paragraph(" "));
        //                //}

        //                //uncomment to render associated product picture
        //                //var apPicture = _pictureService.GetPicturesByProductId(associatedProduct.Id).FirstOrDefault();
        //                //if (apPicture != null)
        //                //{
        //                //    var picBinary = _pictureService.LoadPictureBinary(apPicture);
        //                //    if (picBinary != null && picBinary.Length > 0)
        //                //    {
        //                //        var pictureLocalPath = _pictureService.GetThumbLocalPath(apPicture, 200, false);
        //                //        productTable.AddCell(Image.GetInstance(pictureLocalPath));
        //                //    }
        //                //}

        //                productTable.AddCell(new Paragraph($"{_localizationService.GetResource("PDFProductCatalog.Price", lang.Id)}: {associatedProduct.Price:0.00} {_currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode}", font));
        //                productTable.AddCell(new Paragraph($"{_localizationService.GetResource("PDFProductCatalog.SKU", lang.Id)}: {associatedProduct.Sku}", font));

        //                if (associatedProduct.IsShipEnabled && associatedProduct.Weight > decimal.Zero)
        //                    productTable.AddCell(new Paragraph($"{_localizationService.GetResource("PDFProductCatalog.Weight", lang.Id)}: {associatedProduct.Weight:0.00} {_measureService.GetMeasureWeightById(_measureSettings.BaseWeightId).Name}", font));

        //                if (associatedProduct.ManageInventoryMethod == ManageInventoryMethod.ManageStock)
        //                    productTable.AddCell(new Paragraph($"{_localizationService.GetResource("PDFProductCatalog.StockQuantity", lang.Id)}: {_productService.GetTotalStockQuantity(associatedProduct)}", font));

        //                productTable.AddCell(new Paragraph(" "));

        //                pvNum++;
        //            }
        //        }

        //        doc.Add(productTable);

        //        productNumber++;

        //        if (productNumber <= prodCount)
        //        {
        //            doc.NewPage();
        //        }
        //    }

        //    doc.Close();
        //}

        #endregion
    }
}