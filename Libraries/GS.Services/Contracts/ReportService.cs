using GS.Core.Data;
using GS.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using GS.Core.Domain.Customers;
using GS.Core.Domain.Works;
using GS.Services.Customers;
using GS.Services.Common;

namespace GS.Services.Contracts
{
    public class ReportService : IReportService
    {
        #region Fields
        private readonly IRepository<Construction> _itemRepository;
        private readonly IRepository<Contract> _contractRepository;
        private readonly IRepository<ConstructionType> _construtionTypeRepository;
        private readonly IRepository<ContractJoint> _contractJoinRepository;
        private readonly IRepository<ProcuringAgency> _procuringAgencyRepository;
        private readonly IRepository<ContractContractFormMapping> _contractContractFormMappingRepository;
        private readonly IRepository<ContractForm> _contractFormRepository;
        private readonly IRepository<ContractPayment> _contractPaymentRepository;
        private readonly IRepository<ContractPaymentAcceptance> _contractPaymentAcceptanceRepository;
        private readonly IRepository<ContractPaymentAcceptanceSub> _contractPaymentAcceptanceRepositorySub;
        private readonly IRepository<ContractAcceptance> _contractAcceptanceRepository;
        private readonly IRepository<ContractAcceptanceSub> _contractAcceptanceRepositorySub;
        private readonly IRepository<Unit> _UnitRepository;
        private readonly IRepository<Task> _taskRepository;
        private readonly IRepository<TaskContract> _taskContractRepository;
        private readonly IRepository<ContractPeriod> _contractPeriodRepository;
        private readonly IRepository<ContractRelate> _contractRelateRepository;
        private readonly IRepository<ContractPaymentPlan> _contractPaymentPlanRepository;
        private readonly IRepository<ContractPaymentRequest> _contractPaymentRequestRepository;
        private readonly IRepository<ContractAcceptanceTaskMapping> _contractAcceptanceTaskMappingRepository;
        private readonly IRepository<ContractAcceptanceSub> _contractAcceptanceSub;
        private readonly IRepository<ContractUnfinish> _contractUnfinishRepository;
        private readonly IContractService _contractService;
        private readonly IRepository<Customer> _customerRepository;
        private readonly ICustomerService _customerService;
        private readonly IGenericAttributeService _genericAttributeService;
        #endregion
        #region Ctor
        public ReportService(IRepository<Construction> itemRepository,
            IGenericAttributeService genericAttributeService,
            ICustomerService customerService,
            IContractService contractService,
            IRepository<Customer> customerRepository,
            IRepository<ContractUnfinish> contractUnfinishRepository,
            IRepository<ContractAcceptanceSub> contractAcceptanceSub,
            IRepository<TaskContract> taskContractRepository,
            IRepository<ContractPaymentRequest> contractPaymentRequestRepository,
            IRepository<ContractRelate> contractRelateRepository,
            IRepository<ContractPeriod> contractPeriodRepository,
            IRepository<Task> taskRepository,
            IRepository<Unit> UnitRepository,
            IRepository<ContractAcceptanceSub> contractAcceptanceRepositorySub,
            IRepository<ContractAcceptance> contractAcceptanceRepository,
            IRepository<ConstructionType> construtionTypeRepository,
            IRepository<ContractPaymentAcceptance> contractPaymentAcceptanceRepository,
            IRepository<ContractPaymentAcceptanceSub> contractPaymentAcceptanceRepositorySub,
            IRepository<ContractForm> contractFormRepository,
            IRepository<ProcuringAgency> procuringAgencyRepository,
            IRepository<ContractPaymentPlan> contractPaymentPlanRepository,
            IRepository<ContractPayment> contractPaymentRepository,
            IRepository<ContractJoint> contractJoinRepository,
            IRepository<ContractContractFormMapping> contractContractFormMappingRepository,
            IRepository<ContractAcceptanceTaskMapping> contractAcceptanceTaskMappingRepository,
            IRepository<Contract> contractRepository)
        {
            this._genericAttributeService = genericAttributeService;
            this._customerService = customerService;
            this._customerRepository = customerRepository;
            this._contractService = contractService;
            this._contractUnfinishRepository = contractUnfinishRepository;
            this._contractPaymentPlanRepository = contractPaymentPlanRepository;
            this._contractPaymentRequestRepository = contractPaymentRequestRepository;
            this._contractPeriodRepository = contractPeriodRepository;
            this._contractRelateRepository = contractRelateRepository;
            this._taskContractRepository = taskContractRepository;
            this._taskRepository = taskRepository;
            this._UnitRepository = UnitRepository;
            this._contractAcceptanceRepositorySub = contractAcceptanceRepositorySub;
            this._contractAcceptanceRepository = contractAcceptanceRepository;
            this._contractPaymentAcceptanceRepository = contractPaymentAcceptanceRepository;
            this._contractPaymentAcceptanceRepositorySub = contractPaymentAcceptanceRepositorySub;
            this._construtionTypeRepository = construtionTypeRepository;
            this._itemRepository = itemRepository;
            this._contractPaymentRepository = contractPaymentRepository;
            this._contractFormRepository = contractFormRepository;
            this._procuringAgencyRepository = procuringAgencyRepository;
            this._contractJoinRepository = contractJoinRepository;
            this._contractContractFormMappingRepository = contractContractFormMappingRepository;
            this._contractRepository = contractRepository;
            this._contractAcceptanceTaskMappingRepository = contractAcceptanceTaskMappingRepository;
            this._contractAcceptanceSub = contractAcceptanceSub;
        }
        #endregion
        #region ContractReport
        public virtual IList<ContractReport> GetReport(string constructionCode = null, string contractNameCode = null,
            DateTime? startDateFrom = null, DateTime? startDateTo = null, string constructionType = null, Int32 contractFormId = 0,
            IList<int> SelectConstructionName = null, ContractStatus contractStatus = ContractStatus.All, IList<int> SelectConstructionType = null)
        {
            var queryContract = _contractRepository.Table.Where(c => c.ClassificationId == (int)ContractClassification.AB)
                   .Join(_contractJoinRepository.Table, x => x.Id, y => y.ContractId,
                              (x, y) => new { Contract = x, ContractJoint = y })
                              .Where(t => t.ContractJoint.IsSideA == true)
                   .Select(t => new ContractReport {
                       constructionId = t.Contract.construction != null ? t.Contract.construction.Id : 0,
                       contractID = t.Contract.Id,
                       ConstructionCode = t.Contract.construction != null ? t.Contract.construction.Code : "",
                       ConstructionName = t.Contract.construction != null ? t.Contract.construction.Name : "",
                       ProcuringAgencyName = t.ContractJoint.curProcuringAgency != null ? t.ContractJoint.curProcuringAgency.Name : "",
                       Code = t.Contract.Code,
                       Name = t.Contract.Name,
                       ConstructionType = t.Contract.construction != null ? t.Contract.construction.constructionType.Name : "",
                       ProcuringAgencyIsEVN = t.ContractJoint.curProcuringAgency != null ? t.ContractJoint.curProcuringAgency.IsInEVN : false,
                       TotalAmount = t.Contract.TotalAmount,
                       ContractForm = String.Join(",", _contractFormRepository.Table
                                        .Join(_contractContractFormMappingRepository.Table, x => x.Id, y => y.ContractFormId,
                                        (x, y) => new { contractForm = x, mapping = y })
                                        .Where(c => c.mapping.ContractId == t.Contract.Id).Select(c => c.contractForm.Name)),
                       SignedDate = Convert.ToDateTime(t.Contract.SignedDate),
                       EndDate = Convert.ToDateTime(t.Contract.EndDate),
                       AdvanceAmount = _contractPaymentRepository.Table.Where(a => a.ContractId == t.Contract.Id && a.TypeId == 1).Sum(a => a.Amount),
                       StatusId = t.Contract.StatusId,
                       FinishDate = t.Contract.FinishedDate,
                       CodeP4 = t.Contract.Code1,
                       constructionTypeId = t.Contract.construction != null ? t.Contract.construction.TypeId : 0,
                   });
            if (!string.IsNullOrEmpty(constructionCode))
            {
                queryContract = queryContract.Where(c => c.ConstructionCode == constructionCode);
            }
            if (!string.IsNullOrEmpty(contractNameCode))
            {
                queryContract = queryContract.Where(c => (c.Code == contractNameCode) || (c.Name == contractNameCode));
            }
            if (startDateFrom.HasValue)
            {
                queryContract = queryContract.Where(c => c.SignedDate >= startDateFrom);
            }
            if (startDateTo.HasValue)
            {
                queryContract = queryContract.Where(c => c.SignedDate <= startDateTo);
            }
            if (!string.IsNullOrEmpty(constructionType))
            {
                queryContract = queryContract.Where(c => c.ConstructionType == constructionType);
            }
            if (contractFormId != 0)
            {
                queryContract = queryContract.Join(_contractContractFormMappingRepository.Table, x => x.contractID, y => y.ContractId,
                    (x, y) => new { queryContract = x, ContractForm = y })
                    .Where(c => c.ContractForm.ContractFormId == contractFormId)
                    .Select(c => c.queryContract);
            }
            if(SelectConstructionName != null && SelectConstructionName.Count() > 0)
            {
                queryContract = queryContract.Where(c => SelectConstructionName.Contains(c.constructionId));
            }
            if (contractStatus == ContractStatus.All)
            {
                queryContract = queryContract.Where(c => c.status != ContractStatus.Destroy && c.status != ContractStatus.Draf);
            }
            if (contractStatus != ContractStatus.All)
            {
                queryContract = queryContract.Where(c => c.status == contractStatus);
            }
            if (SelectConstructionType != null && SelectConstructionType.Count() > 0)
            {
                queryContract = queryContract.Where(c => SelectConstructionType.Contains(c.constructionTypeId));
            }

            return queryContract.ToList();
        }
        #endregion
        #region ContractPaymentAcceptance
        public virtual IList<ContractAcceptanceReport> GetReportContractPaymentAcceptance(IList<int> SelectConstructionIds = null,
            string contractCodeName = null, DateTime? dateTimeFrom = null, DateTime? datetimeTo = null, Int32 unitCode = 0,
            int quarterId = 0, int yearId = 0)
        {
            var queryContractAcceptanceSub = _contractAcceptanceRepositorySub.Table
                .Where(c => c.UnitId == unitCode)
                .Join(_contractAcceptanceRepository.Table, x => x.AcceptanceId, y => y.Id,
                (x, y) => new { acceptanceSub = x, acceptance = y })
                .Where(c => c.acceptance.StatusId != (int)ContractAcceptanceStatus.Destroy
                && c.acceptance.StatusId != (int)ContractAcceptanceStatus.Draf
                && c.acceptance.TypeId == (int)ContractAcceptancesType.NoiBo);
            if(quarterId == 0)
            {
                var date1 = new DateTime(yearId, 1, 1);
                var date2 = new DateTime(yearId, 3, 31);
                queryContractAcceptanceSub.Where(c => Convert.ToDateTime(c.acceptance.ApprovalDate).Date >= date1
                && Convert.ToDateTime(c.acceptance.ApprovalDate).Date <= date2);
            }
            if(quarterId == 1)
            {
                var date1 = new DateTime(yearId, 4, 1);
                var date2 = new DateTime(yearId, 6, 30);
                queryContractAcceptanceSub.Where(c => Convert.ToDateTime(c.acceptance.ApprovalDate).Date >= date1
                && Convert.ToDateTime(c.acceptance.ApprovalDate).Date <= date2);
            }
            if (quarterId == 2)
            {
                var date1 = new DateTime(yearId, 7, 1);
                var date2 = new DateTime(yearId, 9, 30);
                queryContractAcceptanceSub.Where(c => Convert.ToDateTime(c.acceptance.ApprovalDate).Date >= date1
                && Convert.ToDateTime(c.acceptance.ApprovalDate).Date <= date2);
            }
            if (quarterId == 3)
            {
                var date1 = new DateTime(yearId, 10, 1);
                var date2 = new DateTime(yearId, 12, 31);
                queryContractAcceptanceSub.Where(c => Convert.ToDateTime(c.acceptance.ApprovalDate).Date >= date1
                && Convert.ToDateTime(c.acceptance.ApprovalDate).Date <= date2);
            }
            var queryContract = _contractRepository.Table
                .Where(c => c.StatusId != (int)ContractStatus.Draf && c.StatusId != (int)ContractStatus.Destroy);
            var query = from c in queryContract
                        join p in _contractPaymentAcceptanceRepositorySub.Table on c.Id equals p.ContractId
                        join t in _taskRepository.Table on p.TaskId equals t.Id
                        where queryContractAcceptanceSub.Select(c => c.acceptanceSub.TaskId)
                        .Contains(p.TaskId)
                        select new ContractAcceptanceReport
                        {
                            constructionId = (int)c.ConstructionId,
                            ConstructionCode = c.construction != null ? c.construction.Code : "",
                            ConstructionName = c.construction != null ? c.construction.Name : "",
                            Code = c.Code,
                            Name = c.Name,
                            contractPeriod = c.ContractContractPeriodMappings.Select(x => x.contractPeriod.Name).FirstOrDefault(),
                            ContractPaymentAcceptance = p.TotalMoney * t.PercentNum,
                            ReduceAdvance = p.ReduceAdvance * t.PercentNum,
                            Depreciation = p.Depreciation * t.PercentNum,
                            ReduceKeep = p.ReduceKeep * t.PercentNum,
                            ReduceOrther = p.ReduceOther * t.PercentNum,
                            Ratio = p.Ratio * t.PercentNum,
                            TotalAmount = p.TotalAmount * t.PercentNum,
                            unitName = _UnitRepository.Table.Where(c => c.Id == unitCode).Select(c => c.Name).First(),
                            dateTime = "Quý " + quarterId + 1 + " năm " + yearId
                        };
            
            if (!string.IsNullOrEmpty(contractCodeName))
            {
                query = query.Where(c => c.Name == contractCodeName || c.Code == contractCodeName);
            }
            if (SelectConstructionIds != null && SelectConstructionIds.Count() > 0)
            {
                query = query.Where(c => SelectConstructionIds.Contains(c.constructionId));
            }

            return query.ToList();
        }
        #endregion
        #region ReportContractBB
        public virtual IList<ContractReportBB> GetContractReportBB(IList<int> SelectedConstructionIds = null, string contractCodeName = null,
            DateTime? datetimeFrom = null, DateTime? datetimeTo = null, string constructionType = null)
        {
            var queryContract = _contractRepository.Table.Where(c => c.ClassificationId == (int)ContractClassification.BB
            && c.StatusId != (int)ContractStatus.Destroy && c.StatusId != (int)ContractStatus.Draf);
            var query = from c in queryContract
                        join cj in _contractJoinRepository.Table on c.Id equals cj.ContractId
                        join p in _contractPeriodRepository.Table on c.PeriodId equals p.Id into pc from p in pc.DefaultIfEmpty()
                        join cr in _contractRelateRepository.Table on c.Id equals cr.Contract2Id
                        where cj.IsSideA == false
                        select new ContractReportBB
                        {
                            constructionId = (int)c.ConstructionId,
                             ConstructionCode = c.construction != null ? c.construction.Code : "",
                             ConstructionName = c.construction != null ? c.construction.Name : "",
                             ContractBBCode = c.Code,
                             ContractName = c.Name,
                             ContractPeriod = p != null ? p.Name : "",
                             ConstructionType = c.construction != null ? c.construction.constructionType.Name : "",
                             IsEVN = cj.curProcuringAgency != null ? cj.curProcuringAgency.IsInEVN : false,
                             TotalAmount = c.TotalAmount,
                             ContractABCode = _contractRepository.Table.Where(a => a.Id == cr.Contract1Id).Select(a => a.Code).First(),
                             SignedDate = Convert.ToDateTime(c.SignedDate),
                             Progress = c.FinishedDate,
                             ClientCode = _procuringAgencyRepository.Table.Where(pr => pr.Id == cj.ProcuringAgencyId).Select(pr => pr.TaxCode) != null ? _procuringAgencyRepository.Table.Where(pr => pr.Id == cj.ProcuringAgencyId).Select(pr => pr.TaxCode).First() : "",
                             ClientName = _procuringAgencyRepository.Table.Where(pr => pr.Id == cj.ProcuringAgencyId).Select(pr => pr.Name) != null ? _procuringAgencyRepository.Table.Where(pr => pr.Id == cj.ProcuringAgencyId).Select(pr => pr.Name).First() : "",
                             UnitName = string.Join("",from tc in _taskContractRepository.Table
                                         join t in _taskRepository.Table on tc.TaskId equals t.Id
                                         join u in _UnitRepository.Table on t.UnitId equals u.Id
                                         where t.ContractId == c.Id
                                         select u.Code),
                             ContractStatus = c.StatusId
                         };

            if (SelectedConstructionIds != null && SelectedConstructionIds.Count() > 0)
            {
                query = query.Where(c => SelectedConstructionIds.Contains(c.constructionId));
            }
            if (!string.IsNullOrEmpty(contractCodeName))
            {
                query = query.Where(c => c.ContractName == contractCodeName || c.ContractBBCode == contractCodeName);
            }
            if (datetimeFrom.HasValue)
            {
                query = query.Where(c => c.SignedDate >= datetimeFrom);
            }
            if (datetimeTo.HasValue)
            {
                query = query.Where(c => c.SignedDate <= datetimeTo);
            }
            if (!string.IsNullOrEmpty(constructionType))
            {
                query = query.Where(c => c.ConstructionType == constructionType);
            }

            return query.ToList();
        }
        #endregion
        #region ContractReportAB
        public virtual IList<ContractReportAB> GetContractReportAB(IList<int> SelectedConstructionIds = null, string contractCodeName = null,
            DateTime? datetimeFrom = null, DateTime? datetimeTo = null, string constructionType = null)
        {
            var queryContract = _contractRepository.Table.Where(c => c.ClassificationId == (int)ContractClassification.AB
            && c.StatusId != (int)ContractStatus.Destroy && c.StatusId != (int)ContractStatus.Draf);
            var query = from c in queryContract
                        join cj in _contractJoinRepository.Table on c.Id equals cj.ContractId
                        join p in _contractPeriodRepository.Table on c.PeriodId equals p.Id into pc
                        from p in pc.DefaultIfEmpty()
                        where cj.IsSideA == true
                        select new ContractReportAB
                        {
                            constructionId = c.construction != null ? c.construction.Id : 0,
                            ConstructionCode = c.construction != null ? c.construction.Code : "",
                            ConstructionName = c.construction != null ? c.construction.Name : "",
                            ContractCode = c.Code,
                            ContractName = c.Name,
                            ContractPeriod = p != null ? p.Name : "",
                            ConstructionType = c.construction != null ? c.construction.constructionType.Name : "",
                            IsEVN = cj.curProcuringAgency != null ? cj.curProcuringAgency.IsInEVN : false,
                            TotalAmount = c.TotalAmount,
                            ContractForms = String.Join(",", _contractFormRepository.Table
                                        .Join(_contractContractFormMappingRepository.Table, x => x.Id, y => y.ContractFormId,
                                        (x, y) => new { contractForm = x, mapping = y })
                                        .Where(t => t.mapping.ContractId == c.Id).Select(t => t.contractForm.Name)),
                            SignedDate = Convert.ToDateTime(c.SignedDate),
                            Progress = Convert.ToDateTime(c.FinishedDate),
                            PlanAdvance = _contractPaymentPlanRepository.Table.Where(a => a.ContractId == c.Id && a.TypeId == 1).Sum(a => a.Amount),
                            PlanPayment = _contractPaymentPlanRepository.Table.Where(a => a.ContractId == c.Id && a.TypeId == 2).Sum(a => a.Amount),
                            ClientCode = string.Join("",_procuringAgencyRepository.Table.Where(pr => pr.Id == cj.ProcuringAgencyId).Select(pr => pr.TaxCode)),
                            ClientName = string.Join("",_procuringAgencyRepository.Table.Where(pr => pr.Id == cj.ProcuringAgencyId).Select(pr => pr.Name)),
                            UnitCode = string.Join(", ",from t in _taskRepository.Table join u in _UnitRepository.Table on t.UnitId equals u.Id
                                       where t.ContractId == c.Id select u.Code.ToString()),
                            TotalAdvance = _contractPaymentRepository.Table.Where(a => a.ContractId == c.Id && a.TypeId == 1).Sum(a => a.Amount),
                            BillDate = string.Join("",_contractPaymentRepository.Table.Where(a => a.ContractId == c.Id && a.TypeId == 2).Select(a => a.PaymentDate)),
                            ContractStatus = c.StatusId
                        };

            if (SelectedConstructionIds != null && SelectedConstructionIds.Count() > 0)
            {
                query = query.Where(c => SelectedConstructionIds.Contains(c.constructionId));
            }
            if (!string.IsNullOrEmpty(contractCodeName))
            {
                query = query.Where(c => c.ContractName == contractCodeName || c.ContractCode == contractCodeName);
            }
            if (datetimeFrom.HasValue)
            {
                query = query.Where(c => c.SignedDate >= datetimeFrom);
            }
            if (datetimeTo.HasValue)
            {
                query = query.Where(c => c.SignedDate <= datetimeTo);
            }
            if (!string.IsNullOrEmpty(constructionType))
            {
                query = query.Where(c => c.ConstructionType == constructionType);
            }

            return query.ToList();
        }
        #endregion
        #region ReportProcuringAgency
        public virtual IList<ReportProcuringAgency> GetReportProcuringAgency(List<int> SelectedProcuringAgencyIds = null, DateTime? datetimeFrom = null, DateTime? datetimeTo = null)
        {
            var query = _contractRepository.Table.Where(c => c.ClassificationId == (int)ContractClassification.AB
            && c.StatusId != (int)ContractStatus.Destroy && c.StatusId != (int)ContractStatus.Draf)
            .Join(_contractJoinRepository.Table, x => x.Id, y => y.ContractId,
            (x, y) => new { contract = x, procuringAgency = y})
            .Where(t => t.procuringAgency.IsSideA == true)
            .Select(c => new ReportProcuringAgency {
                ProcuringAgencyId = c.procuringAgency.ProcuringAgencyId,
                ProcuringAgencyName = c.procuringAgency.curProcuringAgency != null ? c.procuringAgency.curProcuringAgency.Name : "",
                ConstructionCode = c.contract.construction != null ? c.contract.construction.Code : "",
                ConstructionName = c.contract.construction != null ? c.contract.construction.Name : "",
                ContractCode = c.contract.Code,
                IsEVN = c.procuringAgency.curProcuringAgency != null ? c.procuringAgency.curProcuringAgency.IsInEVN : false,
                ContractName = c.contract.Name,
                CodeP4 = c.contract.Code1,
                SignedDate = Convert.ToDateTime(c.contract.SignedDate),
                ContractStatus = c.contract.StatusId,
                TotalAmount = c.contract.TotalAmount,
                AmountRequest = _contractPaymentRequestRepository.Table.Where(a => a.ContractId == c.contract.Id && a.isDeleted == false).Sum(a => a.TotalAmount),
                AmountPayment = _contractPaymentRepository.Table.Where(a => a.StatusId != (int)ContractPaymentStatus.Destroy && a.ContractId == c.contract.Id && a.IsReceipts == true).Sum(a => a.Amount)
            });

            if (datetimeFrom.HasValue)
            {
                query = query.Where(c => c.SignedDate >= datetimeFrom);
            }
            if (datetimeTo.HasValue)
            {
                query = query.Where(c => c.SignedDate <= datetimeTo);
            }
            if((SelectedProcuringAgencyIds != null) && (SelectedProcuringAgencyIds.Count > 0))
            {
                query = query.Where(c => SelectedProcuringAgencyIds.Contains(c.ProcuringAgencyId));
            }

            return query.ToList();
        }
        #endregion
        #region ReportContractNotsigned
        public virtual IList<ReportContractNotsigned> ReportContractNotsigneds(IList<int> SelectedConstructionIds = null, string contractCodeName = null)
        {
            var query = _contractRepository.Table.Where(c => c.SignedDate == null && c.StatusId == (int)ContractStatus.Destroy && c.StatusId == (int)ContractStatus.Draf)
                .Join(_contractJoinRepository.Table, x => x.Id, y => y.ContractId,
                (x, y) => new { contract = x, procuringAgency = y })
                .Where(t => t.procuringAgency.IsSideA == true);
            var ls = query.Select(c => new ReportContractNotsigned {
                ConstructionId = c.contract.construction != null ? c.contract.construction.Id : 0,
                ConstructionName = c.contract.construction != null ? c.contract.construction.Name : "",
                ConstructionCode = c.contract.construction != null ? c.contract.construction.Code : "",
                ContractName = c.contract.Name,
                ContractCode = c.contract.Code,
                ProcuringAgency = c.procuringAgency.curProcuringAgency != null ? c.procuringAgency.curProcuringAgency.Name : "",
                isEvn = c.procuringAgency.curProcuringAgency != null ? c.procuringAgency.curProcuringAgency.IsInEVN : false,
                TotalAmount = (decimal)c.contract.TotalAmount,
                contractStatus = c.contract.StatusId,
                ContractForm = String.Join(",", _contractFormRepository.Table
                                        .Join(_contractContractFormMappingRepository.Table, x => x.Id, y => y.ContractFormId,
                                        (x, y) => new { contractForm = x, mapping = y })
                                        .Where(t => t.mapping.ContractId == c.contract.Id).Select(t => t.contractForm.Name)),
                ConstructionType = c.contract.construction.constructionType.Name,
            });
            if (SelectedConstructionIds != null && SelectedConstructionIds.Count() > 0)
            {
                ls = ls.Where(c => SelectedConstructionIds.Contains(c.ConstructionId));
            }
            if (!string.IsNullOrEmpty(contractCodeName))
            {
                ls = ls.Where(c => c.ContractName == contractCodeName || c.ContractCode == contractCodeName);
            }
            return ls.ToList();
        }
        #endregion
        #region ReportContractDealine
        public virtual IList<ReportContractDealine> ReportContractDealine(IList<int> SelectedConstructionIds = null, DateTime? datetimeFrom = null, DateTime? datetimeTo = null)
        {
            var query = _contractRepository.Table.Where(c =>
                Convert.ToDateTime(c.EndDate).Day <= DateTime.Now.Day + 180
                && c.EndDate > DateTime.Now
                 && c.StatusId != (int)ContractStatus.Destroy)
                 //&& c.StatusId != (int)ContractStatus.Draf)
                .Join(_contractJoinRepository.Table, x => x.Id, y => y.ContractId,
                (x, y) => new { contract = x, procuringAgency = y })
                .Where(t => t.procuringAgency.IsSideA == true);
            var ls = query.Select(c => new ReportContractDealine
            {
                ConstructionId = c.contract.construction != null ? c.contract.construction.Id : 0,
                ConstructionName = c.contract.construction != null ? c.contract.construction.Name : "",
                ConstructionCode = c.contract.construction != null ? c.contract.construction.Code : "",
                ContractName = c.contract.Name,
                ContractCode = c.contract.Code,
                ProcuringAgency = c.procuringAgency.curProcuringAgency != null ? c.procuringAgency.curProcuringAgency.Name : "",
                isEvn = c.procuringAgency.curProcuringAgency != null ? c.procuringAgency.curProcuringAgency.IsInEVN : false,
                TotalAmount = (decimal)c.contract.TotalAmount.GetValueOrDefault(0),
                ContractForm = String.Join(",", _contractFormRepository.Table
                                        .Join(_contractContractFormMappingRepository.Table, x => x.Id, y => y.ContractFormId,
                                        (x, y) => new { contractForm = x, mapping = y })
                                        .Where(t => t.mapping.ContractId == c.contract.Id).Select(t => t.contractForm.Name)),
                ConstructionType = c.contract.construction.constructionType != null ? c.contract.construction.constructionType.Name : "",
                FinishDate = Convert.ToDateTime(c.contract.EndDate)
            });
            ls = ls.OrderBy(c => c.FinishDate);
            if (SelectedConstructionIds != null && SelectedConstructionIds.Count() > 0)
            {
                ls = ls.Where(c => SelectedConstructionIds.Contains(c.ConstructionId));
            }
            if (datetimeFrom.HasValue)
            {
                ls = ls.Where(c => c.FinishDate >= datetimeFrom);
            }
            if (datetimeTo.HasValue)
            {
                ls = ls.Where(c => c.FinishDate <= datetimeTo);
            }
            return ls.ToList();
        }
        #endregion
        #region ReportTaskByUnit
        public virtual IList<ReportTaskByUnit> GetReportTaskByUnit(IList<int> SelectedConstructionIds = null, 
            IList<int> SelectedUnitIds = null, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var query = _contractRepository.Table.Where(c => c.StatusId != (int)ContractStatus.Destroy
                        && c.StatusId != (int)ContractStatus.Draf
                        && c.ClassificationId == (int)ContractClassification.AB);
            var ls = from c in query
                     join p in _contractJoinRepository.Table on c.Id equals p.ContractId
                     join t in _taskRepository.Table on c.Id equals t.ContractId
                     where p.IsSideA == true && t.ParentId == null && t.StatusId != (int)TaskStatus.Destroy && SelectedUnitIds.Contains((int)t.UnitId)
                     select new ReportTaskByUnit
                     {
                         UnitId = t.UnitId.GetValueOrDefault(0),
                         ConstructionId = c.construction != null ? c.construction.Id : 0,
                         ConstructionName = c.construction != null ? c.construction.Name : "",
                         ConstructionCode = c.construction != null ? c.construction.Code : "",
                         ContractName = c.Name,
                         ContractCode = c.Code,
                         UnitName = string.Join("", _UnitRepository.Table.Where(c => c.Id == t.UnitId).Select(c => c.Name)),
                         ProcuringAgencyName = p.curProcuringAgency != null ? p.curProcuringAgency.Name : "",
                         isEvn = p.curProcuringAgency != null ? p.curProcuringAgency.IsInEVN : false,
                         ContractTotalAmount = (decimal)c.TotalAmount.GetValueOrDefault(0),
                         ConstructionType = c.construction != null ? c.construction.constructionType.Name : "",
                         ContractForm = String.Join(", ", _contractFormRepository.Table
                                        .Join(_contractContractFormMappingRepository.Table, x => x.Id, y => y.ContractFormId,
                                        (x, y) => new { contractForm = x, mapping = y })
                                        .Where(m => m.mapping.ContractId == c.Id).Select(m => m.contractForm.Name)),
                         TaskName = t.Name,
                         TaskTotalAmount = t.TotalMoney.GetValueOrDefault(0),
                         tasktStatus = t.StatusId,
                         PaymentAdvance = _contractPaymentRepository.Table.Where(m => m.TaskId == t.Id && m.TypeId == (int)ContractPaymentType.Advance).Sum(m => m.Amount),
                         PaymentAcceptance = Convert.ToDecimal(_contractAcceptanceSub.Table.Where(m => m.TaskId == t.Id && SelectedUnitIds.Contains((int)m.UnitId) && _contractAcceptanceRepository.Table.Where(l => l.TypeId == (int)ContractAcceptancesType.KhoiLuong).Select(l => l.Id).Contains(m.AcceptanceId)).Sum(m => m.TotalAmount)),
                         time = t.StartDate,
                         ContractUnfinish1 = getTaskUnfinishByTaskId(t.Id, t.StartDate, t.EndDate, t.TotalMoney, 
                            Convert.ToDecimal(_contractPaymentAcceptanceRepositorySub.Table.Where(m => m.TaskId == t.Id && SelectedUnitIds.Contains((int)m.UnitId) && _contractPaymentAcceptanceRepository.Table.Where(l => l.ApprovalDate <= t.EndDate).Select(l => l.Id).Contains((int)m.AcceptanceId)).Sum(m => m.TotalMoney))),
                         ContractNoWork = getTaskNoWorkByTaskId(t.StartDate, t.TotalMoney)
                     };
            if (SelectedConstructionIds != null && SelectedConstructionIds.Count() > 0)
            {
                ls = ls.Where(c => SelectedConstructionIds.Contains(c.ConstructionId));
            }
            if (dateFrom.HasValue)
            {
                ls = ls.Where(c => c.time >= dateFrom);
            }
            if (dateTo.HasValue)
            {
                ls = ls.Where(c => c.time <= dateTo);
            }
            return ls.ToList();
        }
        public virtual Decimal getTaskUnfinishByTaskId(int taskId, DateTime? startDate, DateTime? endDate, Decimal? totalMoney, Decimal acceptance)
        {
            if (startDate < DateTime.Now)
            {
                DateTime? time = DateTime.Now;
                if (DateTime.Now > endDate)
                {
                    time = endDate;
                }
                var a1 = time - startDate;
                TimeSpan b1 = a1.Value;
                var percentTime1 = b1.Days;
                var a2 = endDate - startDate;
                TimeSpan b2 = a2.Value;
                var percentTime2 = b2.Days;
                var percent = percentTime1 / percentTime2;
                if (percent < 0)
                {
                    percent = 0;
                }
                if (percent > 1)
                {
                    percent = 1;
                }
                var ls = percent * totalMoney - acceptance;
                return ls.GetValueOrDefault(0);
            }
            else
            {
                return 0;
            }
        }
        public virtual Decimal getTaskNoWorkByTaskId(DateTime? startDate, Decimal? totalMoney)
        {
            if (startDate > DateTime.Now)
            {
                return totalMoney.GetValueOrDefault(0);
            }
            else
            {
                return 0;
            }
        }
        #endregion
        #region ContractUnfinish
        public virtual IList<ReportContractUnfinish> GetReportContractUnfinishes(DateTime? time = null, IList<int> ConstructionIds = null)
        {
            var ls = _contractUnfinishRepository.Table.Select(c => c.ContractId);
            var query = _contractRepository.Table.Where(c => c.StatusId != (int)ContractStatus.Destroy
                        && c.StatusId != (int)ContractStatus.Draf
                        && c.ClassificationId == (int)ContractClassification.AB
                        && ls.Contains(c.Id))
                        .Select(c => new ReportContractUnfinish {
                            ConstructionId = (int)c.ConstructionId,
                            ConstructionName = c.construction.Name,
                            ConstructionCode = c.construction.Code,
                            ContractCode = c.Code,
                            ContractName = c.Name,
                            SignedDate = Convert.ToDateTime(c.SignedDate),
                            TotalAmount = c.TotalAmount.GetValueOrDefault(0),
                            PaymentAmount = _contractPaymentAcceptanceRepositorySub.Table
                                            .Where(s => _taskRepository.Table.Where(t => t.ContractId == c.Id && t.StatusId != (int)TaskStatus.Destroy)
                                            .Select(t => t.Id).Contains(s.TaskId)).Sum(s => s.TotalMoney).GetValueOrDefault(0),
                            ContractFormDesign = _contractUnfinishRepository.Table.Where(t => t.ContractId == c.Id
                                                 && t.CreatedDate.Date == (time != null ? (Convert.ToDateTime(time).Date > Convert.ToDateTime(c.EndDate).Date ? Convert.ToDateTime(c.EndDate).Date : Convert.ToDateTime(time).Date) : Convert.ToDateTime(c.EndDate).Date)
                                                 && t.ContractTypeId == 5)
                                                 .Sum(t => t.OptionValue.GetValueOrDefault(0)),
                            ContractFormTested = _contractUnfinishRepository.Table.Where(t => t.ContractId == c.Id
                                                 && t.CreatedDate.Date == (time != null ? (Convert.ToDateTime(time).Date > Convert.ToDateTime(c.EndDate).Date ? Convert.ToDateTime(c.EndDate).Date : Convert.ToDateTime(time).Date) : Convert.ToDateTime(c.EndDate).Date)
                                                 && t.ContractTypeId == 4)
                                                 .Sum(t => t.OptionValue.GetValueOrDefault(0)),
                            ContractUnfinish2 = _contractUnfinishRepository.Table.Where(t => t.ContractId == c.Id
                                                 && t.CreatedDate.Date == (time != null ? (Convert.ToDateTime(time).Date > Convert.ToDateTime(c.EndDate).Date ? Convert.ToDateTime(c.EndDate).Date : Convert.ToDateTime(time).Date) : Convert.ToDateTime(c.EndDate).Date)
                                                 && t.ContractTypeId == 1000)
                                                 .Sum(t => t.OptionValue.GetValueOrDefault(0)),
                            isSettlemented = c.StatusId == (int)ContractStatus.Settlemented ? true : false,
                            UserMonitorId = c.CreatorId
                        });
            if (ConstructionIds != null && ConstructionIds.Count > 0)
            {
                query = query.Where(c => ConstructionIds.Contains(c.ConstructionId));
            }
            return query.ToList();
        }
        #endregion
    }
}
