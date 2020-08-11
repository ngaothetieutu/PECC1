using GS.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Services.Contracts
{
    public interface IReportService
    {
        IList<ContractReport> GetReport(string constructionCode = null, string contractNameCode = null,
            DateTime? startDateFrom = null, DateTime? startDateTo = null, string constructionType = null, Int32 contractFormId = 0, IList<int> SelectConstructionName = null, ContractStatus contractStatus = ContractStatus.All, IList<int> SelectConstructionType = null);
        IList<ContractAcceptanceReport> GetReportContractPaymentAcceptance(IList<int> SelectConstructionIds = null,
            string contractCodeName = null, DateTime? dateTimeFrom = null, DateTime? datetimeTo = null, Int32 unitCode = 0,
            int quarterId = 0, int yearId = 0);
        IList<ContractReportBB> GetContractReportBB(IList<int> SelectedConstructionIds = null, string contractCodeName = null,
            DateTime? datetimeFrom = null, DateTime? datetimeTo = null, string constructionType = null);
        IList<ContractReportAB> GetContractReportAB(IList<int> SelectedConstructionIds = null, string contractCodeName = null,
            DateTime? datetimeFrom = null, DateTime? datetimeTo = null, string constructionType = null);
        IList<ReportProcuringAgency> GetReportProcuringAgency(List<int> SelectedProcuringAgencyIds = null, DateTime? datetimeFrom = null, DateTime? datetimeTo = null);
        IList<ReportContractNotsigned> ReportContractNotsigneds(IList<int> SelectedConstructionIds = null, string contractCodeName = null);
        IList<ReportContractDealine> ReportContractDealine(IList<int> SelectedConstructionIds = null, DateTime? datetimeFrom = null, DateTime? datetimeTo = null);
        IList<ReportTaskByUnit> GetReportTaskByUnit(IList<int> SelectedConstructionIds = null, IList<int> SelectedUnitIds = null, DateTime? dateFrom = null, DateTime? dateTo = null);
        IList<ReportContractUnfinish> GetReportContractUnfinishes(DateTime? time = null, IList<int> ConstructionIds = null);
    }
}
