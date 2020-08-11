using System;
using System.Collections.Generic;
using System.IO;
using GS.Core.Domain.Catalog;
using GS.Core.Domain.Contracts;
using GS.Core.Domain.Customers;
using GS.Core.Domain.Directory;
using GS.Core.Domain.Messages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GS.Services.ExportImport
{
    /// <summary>
    /// Export manager interface
    /// </summary>
    public partial interface IExportManager
    {
        /// <summary>
        /// Export manufacturer list to XML
        /// </summary>
        /// <param name="manufacturers">Manufacturers</param>
        /// <returns>Result in XML format</returns>
        string ExportManufacturersToXml(IList<Manufacturer> manufacturers);

        /// <summary>
        /// Export manufacturers to XLSX
        /// </summary>
        /// <param name="manufacturers">Manufactures</param>
        byte[] ExportManufacturersToXlsx(IEnumerable<Manufacturer> manufacturers);

        /// <summary>
        /// Export category list to XML
        /// </summary>
        /// <returns>Result in XML format</returns>
        string ExportCategoriesToXml();

        /// <summary>
        /// Export categories to XLSX
        /// </summary>
        /// <param name="categories">Categories</param>
        byte[] ExportCategoriesToXlsx(IList<Category> categories);

       

        /// <summary>
        /// Export customer list to XLSX
        /// </summary>
        /// <param name="customers">Customers</param>
        byte[] ExportCustomersToXlsx(IList<Customer> customers);

        /// <summary>
        /// Export customer list to XML
        /// </summary>
        /// <param name="customers">Customers</param>
        /// <returns>Result in XML format</returns>
        string ExportCustomersToXml(IList<Customer> customers);

        /// <summary>
        /// Export newsletter subscribers to TXT
        /// </summary>
        /// <param name="subscriptions">Subscriptions</param>
        /// <returns>Result in TXT (string) format</returns>
        string ExportNewsletterSubscribersToTxt(IList<NewsLetterSubscription> subscriptions);

        /// <summary>
        /// Export states to TXT
        /// </summary>
        /// <param name="states">States</param>
        /// <returns>Result in TXT (string) format</returns>
        string ExportStatesToTxt(IList<StateProvince> states);

        /// <summary>
        /// Export customer info (GDPR request) to XLSX 
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Customer GDPR info</returns>
        byte[] ExportCustomerGdprInfoToXlsx(Customer customer, int storeId);
        void ExportReportToXlsx(IList<ContractReport> reports, Stream stream, DateTime timeFrom, DateTime timeTo);
        void ExportExcelContractPaymentAcceptance(IList<ContractAcceptanceReport> reports, Stream stream);
        void ExportExcelContractAB(IList<ContractReportAB> reports, Stream stream, DateTime timeFrom, DateTime timeTo);
        void ExportReportContractBB(IList<ContractReportBB> reports, Stream stream);
        void ExportContractProcuringAgency(IList<ReportProcuringAgency> reports, Stream stream, DateTime timeFrom, DateTime timeTo);
        void ExportContractDeadline(IList<ReportContractDealine> reports, Stream stream, DateTime timeFrom, DateTime timeTo);
        void ExportTaskByUnit(IList<ReportTaskByUnit> reports, Stream stream);
        void ExportContractUnfinish(IList<ReportContractUnfinish> reports, Stream stream, DateTime time);
    }
}
