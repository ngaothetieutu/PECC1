using GS.Core.Domain.Contracts;
using GS.Core.Data;
using GS.Services.Events;
using System;
using System.Collections.Generic;
using GS.Core.Caching;
using GS.Services.Catalog;
using System.Linq;
using GS.Core;
using GS.Core.Domain.Works;
using System.Data.SqlClient;
using GS.Core.Domain.Customers;
using GS.Services.Customers;
using System.Globalization;
using GS.Core.Domain.Dashboard;

namespace GS.Services.Contracts
{
    public class ContractService : IContractService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;

        private readonly IProcuringAgencyService _procuringAgencyService;
        private readonly IRepository<Contract> _itemRepository;
        private readonly IRepository<ContractJoint> _contractJointRepository;
        private readonly IRepository<ContractFile> _contractFileRepository;
        private readonly IRepository<ContractResource> _contractResourceRepository;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IWorkContext _workContext;
        private readonly IRepository<ContractView> _contractViewRepository;
        private readonly IRepository<ContractPaymentPlan> _contractPaymentPlanRepository;
        private readonly IRepository<ContractPaymentRequest> _contractPaymentRequestRepository;
        private readonly IRepository<ContractAcceptance> _contractAcceptanceRepository;
        private readonly IRepository<ContractAcceptanceTaskMapping> _contractAcceptanceTaskMappingRepository;
        private readonly IRepository<ContractPaymentAcceptance> _contractPaymentAcceptanceRepository;
        private readonly IRepository<ContractPaymentTask> _contractPaymentTaskRepository;
        private readonly IRepository<ContractRelate> _contractRelateRepository;
        private readonly IRepository<ContractPayment> _contractPaymentRepository;
        private readonly IRepository<PaymentPlanContract> _paymentPlanContractRepository;
        private readonly IRepository<Construction> _constructionRepository;
        private readonly IRepository<ConstructionType> _constructionTypeRepository;
        private readonly IRepository<ContractAcceptanceSub> _contractAcceptanceSubRepository;
        private readonly IRepository<Task> _taskRepository;
        private readonly IRepository<ContractPaymentAcceptanceSub> _contractPaymentAcceptanceSubRepository;
        private readonly IRepository<ContractContractFormMapping> _contractContractFormMappingRepository;
        private readonly IRepository<ContractMonitor> _contractMonitorRepository;
        private readonly IContractMonitorService _contractMonitorService;
        private readonly IRepository<CustomerUnitMapping> _customerCustomerUnitMappingRepository;
        private readonly IRepository<CustomerCustomerRoleMapping> _customerCustomerRoleMappingRepository;
        private readonly IRepository<ContractAcceptanceBB> _contractAcceptanceBBRepository;
        private readonly IRepository<ContractSettlement> _contractSettlementRepository;
        private readonly IRepository<ContractSettlementTaskMapping> _contractSettlementTaskMappingRepository;
        private readonly IRepository<ContractSettlementSub> _contractSettlementSubRepository;
        private readonly IRepository<PaymentExpenditure> _paymentExpenditureRepository;
        private readonly IRepository<ContractUnfinish> _contractUnfinishRepository;
        #endregion
        #region Ctor
        public ContractService(IEventPublisher eventPublisher,
            IRepository<PaymentExpenditure> paymentExpenditureRepository,
            IRepository<ContractSettlementSub> contractSettlementSubRepository,
            IContractMonitorService contractMonitorService,
            IRepository<ContractSettlementTaskMapping> contractSettlementTaskMappingRepository,
            IRepository<ContractSettlement> contractSettlementRepository,
            IRepository<ContractPayment> contractPaymentRepository,
            IRepository<PaymentPlanContract> paymentPlanContractRepository,
            IRepository<ContractContractFormMapping> contractContractFormMappingRepository,
            IRepository<Construction> constructionRepository,
            IRepository<ConstructionType> constructionTypeRepository,
            IProcuringAgencyService procuringAgencyService,
            IRepository<ContractRelate> contractRelateRepository,
            IRepository<ContractResource> contractResourceRepository,
            IRepository<Contract> itemRepository,
            IRepository<ContractJoint> contractJointRepository,
            IRepository<ContractFile> contractFileRepository,
            IStaticCacheManager cacheManager,
            IWorkContext workContext,
            IRepository<ContractView> contractViewRepository,
            IRepository<ContractPaymentPlan> contractPaymentPlanRepository,
            IRepository<ContractPaymentRequest> contractPaymentRequestRepository,
            IRepository<ContractAcceptance> contractAcceptanceRepository,
            IRepository<ContractAcceptanceTaskMapping> contractAcceptanceTaskMappingRepository,
            IRepository<ContractPaymentAcceptance> contractPaymentAcceptanceRepository,
            IRepository<ContractPaymentTask> contractPaymentTaskRepository,
            IRepository<ContractAcceptanceSub> contractAcceptanceSubRepository,
            IRepository<ContractPaymentAcceptanceSub> contractPaymentAcceptanceSubRepository,
            IRepository<ContractMonitor> contractMonitorRepository,
            IRepository<Task> taskRepository,
            IRepository<CustomerUnitMapping> customerCustomerUnitMappingRepository,
            IRepository<CustomerCustomerRoleMapping> customerCustomerRoleMappingRepository,
            IRepository<ContractAcceptanceBB> contractAcceptanceBBRepository,
            IRepository<ContractUnfinish> contractUnfinishRepository
            )

        {
            this._paymentExpenditureRepository = paymentExpenditureRepository;
            this._contractSettlementSubRepository = contractSettlementSubRepository;
            this._contractSettlementTaskMappingRepository = contractSettlementTaskMappingRepository;
            this._customerCustomerUnitMappingRepository = customerCustomerUnitMappingRepository;
            this._customerCustomerRoleMappingRepository = customerCustomerRoleMappingRepository;
            this._contractSettlementRepository = contractSettlementRepository;
            this._contractMonitorService = contractMonitorService;
            this._contractMonitorRepository = contractMonitorRepository;
            this._constructionRepository = constructionRepository;
            this._contractContractFormMappingRepository = contractContractFormMappingRepository;
            this._constructionTypeRepository = constructionTypeRepository;
            this._contractPaymentRepository = contractPaymentRepository;
            this._eventPublisher = eventPublisher;
            this._contractRelateRepository = contractRelateRepository;
            this._procuringAgencyService = procuringAgencyService;
            this._contractResourceRepository = contractResourceRepository;
            this._itemRepository = itemRepository;
            this._contractJointRepository = contractJointRepository;
            this._contractFileRepository = contractFileRepository;
            this._cacheManager = cacheManager;
            this._workContext = workContext;
            this._contractViewRepository = contractViewRepository;
            this._contractPaymentPlanRepository = contractPaymentPlanRepository;
            this._contractPaymentRequestRepository = contractPaymentRequestRepository;
            this._contractAcceptanceRepository = contractAcceptanceRepository;
            this._contractAcceptanceTaskMappingRepository = contractAcceptanceTaskMappingRepository;
            this._contractPaymentAcceptanceRepository = contractPaymentAcceptanceRepository;
            this._taskRepository = taskRepository;
            this._contractPaymentTaskRepository = contractPaymentTaskRepository;
            this._contractAcceptanceSubRepository = contractAcceptanceSubRepository;
            this._contractPaymentAcceptanceSubRepository = contractPaymentAcceptanceSubRepository;
            this._paymentPlanContractRepository = paymentPlanContractRepository;
            this._contractAcceptanceBBRepository = contractAcceptanceBBRepository;
            this._contractUnfinishRepository = contractUnfinishRepository;
        }
        #endregion
        #region Contract

        public IPagedList<Contract> GetContracts(string keySearch = null, string Code = null, string Name = null
            , DateTime? FromDate = null, DateTime? ToDate = null
            , int CustomerId = 0, ContractStatus contractStatus = ContractStatus.All
            , int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false, int ContractListOrder = 0, List<int> SelectedConstructionIds = null
            , ContractClassification classificationId = ContractClassification.All, int ConstructionTypeId = 0, int contractType = 0
            , ContractMonitorStatus contractMonitorStatus = ContractMonitorStatus.All, DateTime? signedateFrom = null, DateTime? signedateTo = null, bool isGetRecently = false, List<int> SelectedProcuringAgencyIds = null
            , int StartYear = 0, DateTime? acceptanceFrom = null, DateTime? acceptanceTo = null)
        {
            var query = _itemRepository.Table;
            if (contractStatus == ContractStatus.All)
                query = query.Where(c => c.StatusId != (int)ContractStatus.Destroy);
            else if (contractStatus == ContractStatus.Settlemented)
            {
                query = query.Where(c => _contractSettlementRepository.Table.Any(x => x.StatusId == (int)ContractSettlementStatusId.Use && x.ContractId == c.Id)
                   && c.StatusId != (int)ContractStatus.Destroy && c.StatusId != (int)ContractStatus.Draf);
            }
            else if (contractStatus == ContractStatus.Acceptance)
            {
                var a = _contractAcceptanceRepository.Table
                    .Where(c => c.StatusId != (int)ContractAcceptanceStatus.Destroy && c.StatusId != (int)ContractAcceptanceStatus.Draf
                    && c.TypeId == (int)ContractAcceptancesType.KhoiLuong).Select(c => c.ContractId);
                query = query.Where(c => a.Contains(c.Id) && c.StatusId != (int)ContractStatus.Destroy && c.StatusId != (int)ContractStatus.Draf);
            }
            else
                query = query.Where(c => c.StatusId == (int)contractStatus);
            //check quyen xem hop dong, kiem tra xem co role Admin ko, neu la Admin thi dc full quyen
            if (CustomerId > 0 && !_customerCustomerRoleMappingRepository.Table.Any(c => c.CustomerId == CustomerId && c.CustomerRoleId == GSCustomerDefaults.AdministratorsRoleId))
            {

                //kiem tra trong contract resource 
                // && c.CustomerRoleId == GSCustomerDefaults.GSContractViewerId
                //lay tat ca thong tin role theo user hien tai
                var queryCustomerRole = _customerCustomerRoleMappingRepository.Table.Where(c => c.CustomerId == CustomerId).ToList();

                //tao thong tin ContractResource Temp theo user hien tai
                var queryCustomerResourceTemp = (from r in queryCustomerRole
                                                 join u in _customerCustomerUnitMappingRepository.Table.Where(uc => uc.CustomerId == CustomerId).ToList() on r.CustomerId equals u.CustomerId into ru
                                                 from subru in ru.DefaultIfEmpty()
                                                 select new ContractResource
                                                 {
                                                     CustomerId = CustomerId,
                                                     RoleId = GSCustomerDefaults.GSContractViewerId,
                                                     UnitId = subru?.UnitId ?? null,
                                                     GroupId = r.CustomerRoleId
                                                 }
                                       ).ToList();
                //lien ket toi bang ContractResource de lay them thong tin ContractId
                var queryContractResourceTemp = (from rc in _contractResourceRepository.Table
                                                 join tmp in queryCustomerResourceTemp on rc.RoleId equals tmp.RoleId
                                                 where (
                                                 rc.CustomerId == tmp.CustomerId
                                                 || (rc.UnitId == tmp.UnitId && rc.GroupId == tmp.GroupId)
                                                 )
                                                 select rc.ContractId
                                               );
                //nhung user tao or approved deu co the thay 
                query = query.Where(c => c.CreatorId == CustomerId
                    || c.ApproverId == CustomerId
                    || queryContractResourceTemp.Contains(c.Id) //hoac trong resource phai co id
                    );
            }

            if (classificationId != ContractClassification.All)
            {
                query = query.Where(c => c.ClassificationId == (int)classificationId);
            }
            if (contractMonitorStatus != ContractMonitorStatus.All)
            {
                query = query.Join(_contractMonitorRepository.Table, x => x.Id, y => y.ContractId,
                    (x, y) => new { contract = x, contractMonitor = y })
                    .Where(c => c.contractMonitor.StatusIds.Contains(Convert.ToString((int)contractMonitorStatus)))
                    .Select(c => c.contract);
            }
            if (!string.IsNullOrEmpty(keySearch))
            {
                query = query.Where(c => c.SearchFullText.Contains(keySearch, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(Code))
            {
                query = query.Where(c => c.Code == Code);
            }
            if (!string.IsNullOrEmpty(Name))
            {
                query = query.Where(c => c.Name.Contains(Name));
            }
            if (FromDate.HasValue)
            {
                query = query.Where(c => c.StartDate >= FromDate);

            }
            if (ToDate.HasValue)
            {
                query = query.Where(c => c.StartDate <= ToDate);
            }
            if ((SelectedConstructionIds != null) && (SelectedConstructionIds.Count > 0))
            {
                query = query.Where(c => SelectedConstructionIds.Contains((int)c.ConstructionId));
            }
            if (ContractListOrder == 0)
            {
                query = query.OrderBy(c => c.CreatedDate);
            }
            if (ContractListOrder == 1)
            {
                query = query.OrderByDescending(c => c.StartDate);
            }
            if (ContractListOrder == 2)
            {
                query = query.OrderBy(c => c.TotalAmount);
            }
            if (ContractListOrder == 3)
            {
                query = query.OrderByDescending(c => c.TotalAmount);
            }
            if (ConstructionTypeId > 0)
            {
                query = query.Join(_constructionRepository.Table, x => x.ConstructionId, y => y.Id,
                    (x, y) => new { contract = x, constructionType = y })
                    .Where(c => c.constructionType.TypeId == ConstructionTypeId && c.contract.ClassificationId == (int)ContractClassification.AB)
                    .Select(c => c.contract);
            }
            if (contractType > 0)
            {
                query = query.Join(_contractContractFormMappingRepository.Table, x => x.Id, y => y.ContractId,
                    (x, y) => new { queryContract = x, ContractForm = y })
                    .Where(c => c.ContractForm.ContractFormId == contractType)
                    .Select(c => c.queryContract);
            }
            if (signedateFrom.HasValue)
            {
                query = query.Where(c => c.SignedDate >= signedateFrom);
            }
            if (signedateTo.HasValue)
            {
                query = query.Where(c => c.SignedDate <= signedateTo);
            }
            //Lay hop dong xem gan day, thay the cho viec lay hop dong xem gan day cua Tai
            if (isGetRecently)
            {
                query = query.Join(_contractViewRepository.Table.OrderByDescending(c=>c.ViewDate), x => x.Id, y => y.ContractId,
                    (x, y) => new { queryContract = x, ContractView = y })
                    .Where(c => c.ContractView.CustomerId == CustomerId)
                    .Select(c => c.queryContract);
            }
            if ((SelectedProcuringAgencyIds != null) && (SelectedProcuringAgencyIds.Count > 0))
            {
                query = query.Join(_contractJointRepository.Table, x => x.Id, y => y.ContractId,
                    (x, y) => new { contract = x, contractJoin = y })
                    .Where(c => SelectedProcuringAgencyIds.Contains((int)c.contractJoin.ProcuringAgencyId))
                    .Select(c => c.contract);
            }
            if (StartYear != 0)
            {
                query = query.Where(c => Convert.ToDateTime(c.StartDate).Year == StartYear && c.StatusId != (int)ContractStatus.Draf);
            }
            if (acceptanceFrom.HasValue)
            {
                var acceptance = _contractAcceptanceRepository.Table
                    .Where(c => c.TypeId == (int)ContractAcceptancesType.KhoiLuong && c.ApprovalDate >= acceptanceFrom
                    && c.StatusId != (int)ContractAcceptanceStatus.Destroy && c.StatusId != (int)ContractAcceptanceStatus.Draf)
                    .Select(c => c.ContractId);
                query = query.Where(c => acceptance.Contains(c.Id));
            }
            if (acceptanceTo.HasValue)
            {
                var acceptance = _contractAcceptanceRepository.Table
                    .Where(c => c.TypeId == (int)ContractAcceptancesType.KhoiLuong && c.ApprovalDate <= acceptanceTo
                    && c.StatusId != (int)ContractAcceptanceStatus.Destroy && c.StatusId != (int)ContractAcceptanceStatus.Draf)
                    .Select(c => c.ContractId);
                query = query.Where(c => acceptance.Contains(c.Id));
            }
            return new PagedList<Contract>(query, pageIndex, pageSize, getOnlyTotalCount);
        }

        /// <summary>
        /// Lay tat ca hop dong lien quan den hop dong goc
        /// </summary>
        /// <param name="ContractId"></param>
        /// <param name="classificationId"></param>
        /// <returns></returns>
        public virtual IList<Contract> GetContractRelates(int ContractId, ContractClassification classificationId)
        {
            var query = _itemRepository.Table
                    .Where(c => c.StatusId != (int)ContractStatus.Destroy && c.ClassificationId == (int)classificationId);
            query = query.Join(_contractRelateRepository.Table, x => x.Id, y => y.Contract2Id,
                       (x, y) => new { Contract = x, Mapping = y.Contract1Id })
                   .Where(z => z.Mapping == ContractId)
                   .Select(z => z.Contract);
            query = query.OrderBy(c => c.Name);
            return query.ToList();
        }
        public virtual ContractRelate GetContractRelateByContract2Id(int Contract2Id)
        {
            var query = _contractRelateRepository.Table.Where(c => c.Contract2Id == Contract2Id).FirstOrDefault();
            return query;
        }
        public virtual IList<Contract> GetAllContract()
        {
            var query = _itemRepository.Table.Where(c => c.status != ContractStatus.Destroy).ToList();
            return query;
        }
        /// <summary>
        /// Gets a review type 
        /// </summary>
        /// <param name="itemId">Review type identifier</param>
        /// <returns>Review type</returns>
        public virtual Contract GetContractById(int itemId)
        {
            if (itemId == 0)
                return null;

            return _itemRepository.GetById(itemId);
        }
        public virtual Contract GetContractByCode1(string code1)
        {
            if (code1 == null)
                return null;
            var query = _itemRepository.Table.Where(c => c.Code1 == code1 && c.StatusId != (int)ContractStatus.Destroy).FirstOrDefault();
            return query;
        }
        public IList<Contract> GetListContractbylistId(List<int> listId)
        {
            return _itemRepository.Table.Where(c => c.status != ContractStatus.Destroy && listId.Contains(c.Id)).ToList();
        }
        /// <summary>
        /// Gets a customer by GUID
        /// </summary>
        /// <param name="customerGuid">Customer GUID</param>
        /// <returns>A customer</returns>
        public virtual Contract GetContractByGuid(Guid contractGuid)
        {
            if (contractGuid == Guid.Empty)
                return null;

            var query = from c in _itemRepository.Table
                        where c.ContractGuid == contractGuid
                        orderby c.Id
                        select c;
            var contract = query.FirstOrDefault();
            return contract;
        }
        private string CreateFullTextString(Contract item)
        {
            //lay thong tin cong trinh
            string _constructionText = "";
            if (item.construction != null)
            {
                _constructionText = item.construction.Name;
            }
            else if (item.ConstructionId > 0)
            {
                var _construction = _constructionRepository.GetById(item.ConstructionId);
                if (_construction != null)
                {
                    _constructionText = _construction.Name;
                }
            }
            //lay thong tin chu dau tu
            string _sideAText = "";
            if (item.Id > 0)
            {
                var _sideAItem = _contractJointRepository.Table.Where(c => c.ContractId == item.Id && c.IsSideA == true).Select(s => s.curProcuringAgency).FirstOrDefault();
                if (_sideAItem != null)
                    _sideAText = _sideAItem.Name;
            }

            string _val = string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8}", item.Code, item.Name, item.Description
                , item.Note, item.PaymentInfo, item.SignedDate.toDateVNString()
                , item.Code1, _constructionText, _sideAText);
            _val = _val + " " + _val.ToNoSign();
            return _val;
        }
        /// <summary>
        /// Inserts a review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual void InsertContract(Contract item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            item.CreatedDate = DateTime.Now;
            item.UpdatedDate = DateTime.Now;
            item.StoreId = 1;//set default 
            item.status = ContractStatus.Draf;
            item.CreatorId = _workContext.CurrentCustomer.Id;
            item.SearchFullText = CreateFullTextString(item);
            _itemRepository.Insert(item);


            if (item.classification == ContractClassification.AB)
            {
                var procuringAgency = _procuringAgencyService.GetProcuringAgencyByPECC1();
                var contractJoint = new ContractJoint();
                contractJoint.ContractId = item.Id;
                contractJoint.ProcuringAgencyId = procuringAgency.Id;
                contractJoint.ProcuringAgencyData = procuringAgency.toStringJson();
                contractJoint.IsMain = true;
                contractJoint.DisplayOrder = 1;
                InsertContractJoint(contractJoint);

            }
            else if (item.classification == ContractClassification.BB)
            {
                var procuringAgency = _procuringAgencyService.GetProcuringAgencyByPECC1();
                var contractJoint = new ContractJoint();
                contractJoint.ContractId = item.Id;
                contractJoint.ProcuringAgencyId = procuringAgency.Id;
                contractJoint.ProcuringAgencyData = procuringAgency.toStringJson();
                contractJoint.IsMain = true;
                contractJoint.IsSideA = true;
                contractJoint.DisplayOrder = 1;
                InsertContractJoint(contractJoint);
            }
            //tao thong tin resource mac dinh cho hop dong
            //thiet dat ban lanh dao co the vao xem thong tin hop dong
            UpdateViewContractResources(item.Id, GSDataSettingsDefaults.ManagerUnitId, GSCustomerDefaults.PECC1LeaderShipId);
            //event notification
            _eventPublisher.EntityInserted(item);
        }
        /// <summary>
        /// tong gia tri tren phieu nghiem thu
        /// </summary>
        public virtual Decimal GetTotalRequest()
        {
            var listContractId = _itemRepository.Table.Where(c => c.StatusId != (int)ContractStatus.Destroy && c.StatusId != (int)ContractStatus.Draf && c.StatusId != (int)ContractStatus.Completed).Select(c => c.Id);
            var query = _contractPaymentRequestRepository.Table;
            var ls = query.Where(c => c.TypeId == 2 && listContractId.Contains(c.ContractId) && c.isDeleted == false);
            return ls.Sum(c => c.TotalAmount);
        }
        /// <summary>
        /// tong gia tri HD
        /// </summary>
        public virtual Decimal GetTotalMoney(int year = 0)
        {
            var query = _itemRepository.Table;
            var ls = query.Where(c => c.ClassificationId == (int)ContractClassification.AB
                && c.StatusId != (int)ContractStatus.Destroy
                && c.StatusId != (int)ContractStatus.Draf);
            if (year != 0)
            {
                ls = ls.Where(c => Convert.ToDateTime(c.StartDate).Year == year);
            }
            decimal totalMoney = 0;
            foreach (var ct in ls)
            {
                var value = GetTotalAmountContractByTask(ct.Id, true);
                totalMoney = totalMoney + value;
            }
            return totalMoney;
        }
        /// <summary>
        /// do dang theo nam
        /// </summary>
        public virtual Decimal GetTotalMoneyContractunfinishByYear(int year)
        {
            var query = _contractUnfinishRepository.Table.Where(c => c.ContractTypeId != 1000);
            if(year == DateTime.Now.Year)
            {
                query = query.Where(c => c.CreatedDate.Date == DateTime.Now.Date);
            }
            else
            {
                var lastDate = new DateTime(year, 12, 31);
                query = query.Where(c => c.CreatedDate.Date == lastDate);
            }
            return query.Sum(c => c.OptionValue.GetValueOrDefault(0));
        }
        /// <summary>
        /// do dang theo nam
        /// </summary>
        public List<ContractUnfinish> GetAllContractunfinishByYear(int year)
        {
            var query = _contractUnfinishRepository.Table.Where(c => c.ContractTypeId != 1000);
            if (year == DateTime.Now.Year)
            {
                query = query.Where(c => c.CreatedDate.Date == DateTime.Now.Date);
            }
            else
            {
                var lastDate = new DateTime(year, 12, 31);
                query = query.Where(c => c.CreatedDate.Date == lastDate);
            }
            return query.ToList();
        }
        /// <summary>
        /// tong gia tri nghiem thu khoi luong
        /// </summary>
        public virtual Decimal GetTotalMoneyAcceptance(int year = 0)
        {
            var contract = _itemRepository.Table
                .Where(c => c.StatusId != (int)ContractStatus.Destroy && c.StatusId != (int)ContractStatus.Draf)
                .Select(c => c.Id);
            var query = _contractAcceptanceRepository.Table.Where(c => c.TypeId == (int)ContractAcceptancesType.KhoiLuong
            && c.StatusId != (int)ContractAcceptanceStatus.Destroy && contract.Contains(c.ContractId))
            .Where(c => year != 0 ? Convert.ToDateTime(c.ApprovalDate).Year == year : Convert.ToDateTime(c.ApprovalDate).Year != -1)
            .Join(_contractAcceptanceSubRepository.Table, x => x.Id, y => y.AcceptanceId,
            (x, y) => new { acceptance = x, sub = y })
            .Sum(c => c.sub.TotalAmount);

            return (decimal)query;
        }
        /// <summary>
        /// tong gia tri tam ung
        /// </summary>
        public virtual Decimal GetSumPaymentAdvance(int year = 0)
        {
            var query = _itemRepository.Table.Where(c => c.StatusId != (int)ContractStatus.Destroy).Select(c => c.Id);
            var ls = _contractPaymentRepository.Table.Where(c => c.StatusId != (int)ContractPaymentStatus.Destroy
                && c.IsReceipts == true && c.TypeId == (int)ContractPaymentType.Advance
                && query.Contains((int)c.ContractId));
            if (year != 0)
            {
                ls = ls.Where(c => Convert.ToDateTime(c.PaymentDate).Year == year);
            }
            return ls.Sum(c => c.Amount);
        }
        /// <summary>
        /// tong gia tri da thanh toan
        /// </summary>
        public virtual Decimal GetSumPaymentPaid(int year = 0)
        {
            var query = _itemRepository.Table.Where(c => c.StatusId != (int)ContractStatus.Destroy).Select(c => c.Id);
            var ls = _contractPaymentRepository.Table.Where(c => c.StatusId != (int)ContractPaymentStatus.Destroy
                && c.IsReceipts == true && c.TypeId == (int)ContractPaymentType.Payment
                && query.Contains((int)c.ContractId));
            if (year != 0)
            {
                ls = ls.Where(c => Convert.ToDateTime(c.PaymentDate).Year == year);
            }
            return ls.Sum(c => c.Amount);
        }
        /// <summary>
        /// tong gia tri dở dang
        /// </summary>
        public virtual Decimal GetUnfinish(int year)
        {
            var query = _contractUnfinishRepository.Table
                .Where(c => c.ContractTypeId == 1000);
            if (year == DateTime.Now.Year)
            {
                query = query.Where(c => c.CreatedDate.Date == DateTime.Now.Date);
            }
            else
            {
                var lastDate = new DateTime(year, 12, 31);
                query = query.Where(c => c.CreatedDate.Date == lastDate);
            }
            return query.Sum(c => c.OptionValue.GetValueOrDefault(0));
        }
        public virtual IList<Construction> GetTopConstructionContract()
        {
            var query = _itemRepository.Table;
            var ls = query.Where(c => c.ClassificationId == (int)ContractClassification.AB
                && c.StatusId != (int)ContractStatus.Destroy && c.StatusId != (int)ContractStatus.Draf)
                .Select(c => new { c.construction, c.ConstructionId, c.TotalAmount }).ToList()
                .GroupBy(
                    p => p.ConstructionId
                )
                .OrderByDescending(g => g.Sum(c => c.TotalAmount))
                .Take(5)
                .Select(x =>
                {
                    var item = x.FirstOrDefault().construction;
                    item.ContractCount = x.Count();
                    item.TotalMoneyContract = Convert.ToString(x.Sum(c => c.TotalAmount).GetValueOrDefault(0));
                    return item;
                }).ToList();

            return ls;
        }
        /// <summary>
        /// lay thong tin theo loai cong trinh
        /// </summary>
        public virtual IList<ConstructionTypeResult> GetTotalTypeContract(int constructionType = 0)
        {
            var queryContract = _itemRepository.Table
                .Where(c => c.StatusId != (int)ContractStatus.Destroy && c.ClassificationId == (int)ContractClassification.AB
                && c.StatusId != (int)ContractStatus.Draf);
            var queryConstruction = _constructionRepository.Table;
            //lay thong tin giua cong trinh va hop dong, group theo loai cong trinh
            var queryCC = (from c in queryConstruction
                           join con in queryContract on c equals con.construction
                           select c
                         )
                         .GroupBy(g => g.TypeId)
                         .Select(ccon => new { TypeId = ccon.Key, TotalContract = ccon.Count() }
                         ).ToList();

            //tao left join voi bang loai cong trinh, de lay thong tin 
            var query = (from ct in _constructionTypeRepository.Table.ToList()
                         join cc in queryCC on ct.Id equals cc.TypeId into ctcc
                         from suctcc in ctcc.DefaultIfEmpty()
                         where constructionType != 0 ? ct.Id == constructionType : ct.Id > -1
                         select new { constructionType = ct, TotalCount = suctcc?.TotalContract ?? 0 }
                       )
                       .GroupBy(g => g.constructionType).Select(x => new ConstructionTypeResult
                       {
                           TypeId = x.Key.Id,
                           TypeName = x.Key.Name,
                           TotalCount = x.Sum(s => s.TotalCount),
                           TotalMoney = GetTotalMoneyByConstructionType(x.Key.Id, 0),
                           TotalPaymentAcceptance = GetTotalMoneyAcceptanceByConstructiontype(x.Key.Id, 0),
                           TotalPaymentAdvance = GetSumPaymentAdvanceByConstructiontype(x.Key.Id, 0),
                           TotalMoneyPaid = GetSumPaymentPaidByConstructionType(x.Key.Id, 0),
                           TotalMoneyContractUnfinish = GetTotalMoneyContractUnfinish(x.Key.Id, 0),
                           TotalMoneyContractUnfinish2 = GetTotalMoneyContractUnfinish2ByConstructionType(x.Key.Id, 0)
                       }
                );

            return query.OrderByDescending(c => c.TypeName).ToList();
        }
        /// <summary>
        /// lay thong tin theo loai cong trinh thong ke theo nam
        /// </summary>
        public virtual IList<ConstructionTypeResultByYear> GetTotalTypeContractByYear(int year = 0, int constructionType = 0)
        {
            var queryContract = _itemRepository.Table.Where(c => c.StatusId != (int)ContractStatus.Destroy && c.StatusId != (int)ContractStatus.Draf
            && c.ClassificationId == (int)ContractClassification.AB && Convert.ToDateTime(c.StartDate).Year == year);
            var queryConstruction = _constructionRepository.Table;
            //lay thong tin giua cong trinh va hop dong, group theo loai cong trinh
            var queryCC = (from c in queryConstruction
                           join con in queryContract on c equals con.construction
                           select c
                         )
                         .GroupBy(g => g.TypeId)
                         .Select(ccon => new { TypeId = ccon.Key, TotalContract = ccon.Count() }
                         ).ToList();

            //tao left join voi bang loai cong trinh, de lay thong tin 
            var query = (from ct in _constructionTypeRepository.Table.ToList()
                         join cc in queryCC on ct.Id equals cc.TypeId into ctcc
                         from suctcc in ctcc.DefaultIfEmpty()
                         where constructionType != 0 ? ct.Id == constructionType : ct.Id > -1
                         select new { constructionType = ct, TotalCount = suctcc?.TotalContract ?? 0 }
                       )
                       .GroupBy(g => g.constructionType).Select(x => new ConstructionTypeResultByYear
                       {
                           TypeId = x.Key.Id,
                           TypeName = x.Key.Name,
                           TotalCount = x.Sum(s => s.TotalCount),
                           TotalMoney = GetTotalMoneyByConstructionType(x.Key.Id, year),
                           TotalPaymentAcceptance = GetTotalMoneyAcceptanceByConstructiontype(x.Key.Id, year),
                           TotalPaymentAdvance = GetSumPaymentAdvanceByConstructiontype(x.Key.Id, year),
                           TotalMoneyPaid = GetSumPaymentPaidByConstructionType(x.Key.Id, year),
                           StartYear = year,
                           TotalMoneyContractUnfinish = GetTotalMoneyContractUnfinish(x.Key.Id, year),
                           TotalMoneyContractUnfinish2 = GetTotalMoneyContractUnfinish2ByConstructionType(x.Key.Id, year)
                       }
                );

            return query.OrderByDescending(c => c.TypeName).ToList();
        }
        /// <summary>
        /// tong thanh toan theo loai cong trinh
        /// </summary>
        public virtual Decimal GetSumPaymentPaidByConstructionType(int constructionType, int year = 0)
        {
            var queryContract = _constructionRepository.Table
                .Join(_itemRepository.Table, x => x.Id, y => y.ConstructionId,
                (x, y) => new { construction = x, contract = y })
                .Where(c => c.contract.StatusId != (int)ContractStatus.Destroy
                && c.construction.TypeId == constructionType)
                .Select(c => c.contract.Id);
            var ls = _contractPaymentRepository.Table.Where(c => c.StatusId != (int)ContractPaymentStatus.Destroy
                && c.IsReceipts == true && c.TypeId == (int)ContractPaymentType.Payment
                && queryContract.Contains((int)c.ContractId));
            if (year != 0)
            {
                ls = ls.Where(c => Convert.ToDateTime(c.PaymentDate).Year == year);
            }

            return ls.Sum(c => c.Amount);
        }
        /// <summary>
        /// tong tam ung theo loai cong trinh
        /// </summary>
        public virtual Decimal GetSumPaymentAdvanceByConstructiontype(int constructionType, int year = 0)
        {
            var queryContract = _constructionRepository.Table
                .Join(_itemRepository.Table, x => x.Id, y => y.ConstructionId,
                (x, y) => new { construction = x, contract = y })
                .Where(c => c.contract.StatusId != (int)ContractStatus.Destroy
                && c.construction.TypeId == constructionType)
                .Select(c => c.contract.Id);
            var ls = _contractPaymentRepository.Table.Where(c => c.StatusId != (int)ContractPaymentStatus.Destroy
                && c.IsReceipts == true && c.TypeId == (int)ContractPaymentType.Advance
                && queryContract.Contains((int)c.ContractId));
            if (year != 0)
            {
                ls = ls.Where(c => Convert.ToDateTime(c.PaymentDate).Year == year);
            }

            return ls.Sum(c => c.Amount);
        }
        /// <summary>
        /// tong nghiem thu KL theo loai cong trinh
        /// </summary>
        public virtual Decimal GetTotalMoneyAcceptanceByConstructiontype(int constructionType, int year = 0)
        {
            var queryContract = _constructionRepository.Table.Where(c => c.TypeId == constructionType)
                .Join(_itemRepository.Table, x => x.Id, y => y.ConstructionId,
                (x, y) => new { construction = x, contract = y })
                .Where(c => c.contract.StatusId != (int)ContractStatus.Destroy)
                .Select(c => c.contract.Id);
            var query = _contractAcceptanceRepository.Table.Where(c => c.TypeId == (int)ContractAcceptancesType.KhoiLuong
            && c.StatusId != (int)ContractAcceptanceStatus.Destroy && queryContract.Contains(c.ContractId))
            .Where(c => year != 0 ? Convert.ToDateTime(c.ApprovalDate).Year == year : Convert.ToDateTime(c.ApprovalDate).Year != -1)
            .Join(_contractAcceptanceSubRepository.Table, x => x.Id, y => y.AcceptanceId,
            (x, y) => new { acceptance = x, sub = y })
            .Sum(c => c.sub.TotalAmount);

            return (decimal)query;
        }
        /// <summary>
        /// tong so do dang chua thuc hien theo loai cong trinh
        /// </summary>
        public virtual Decimal GetTotalMoneyContractUnfinish2ByConstructionType(int constructionTypeId, int year = 0)
        {
            var query = _constructionRepository.Table
                .Join(_itemRepository.Table, x => x.Id, y => y.ConstructionId,
                (x, y) => new { construction = x, contract = y })
                .Where(c => c.contract.StatusId != (int)ContractStatus.Destroy && c.contract.StatusId != (int)ContractStatus.Draf
                    && c.contract.ClassificationId == (int)ContractClassification.AB
                    && c.construction.TypeId == constructionTypeId)
                .Select(c => c.contract.Id).ToList().Distinct();
            var ls = _contractUnfinishRepository.Table.Where(c => c.ContractTypeId == 1000);
            if (year == DateTime.Now.Year || year == 0)
            {
                ls = ls.Where(c => query.Contains(c.ContractId) && c.CreatedDate.Date == DateTime.Now.Date);
            }
            else
            {
                var lastDay = new DateTime(year, 12, 31);
                ls = ls.Where(c => query.Contains(c.ContractId) && c.CreatedDate.Date == lastDay);
            }

            return ls.Sum(c => c.OptionValue.GetValueOrDefault(0));
        }
        /// <summary>
        /// tong so do dang theo loai cong trinh
        /// </summary>
        public virtual Decimal GetTotalMoneyContractUnfinish(int constructionTypeId, int year = 0)
        {
            var query = _constructionRepository.Table
                .Join(_itemRepository.Table, x => x.Id, y => y.ConstructionId,
                (x, y) => new { construction = x, contract = y })
                .Where(c => c.contract.StatusId != (int)ContractStatus.Destroy && c.contract.StatusId != (int)ContractStatus.Draf
                    && c.contract.ClassificationId == (int)ContractClassification.AB
                    && c.construction.TypeId == constructionTypeId)
                .Select(c => c.contract.Id).ToList().Distinct();
            var ls = _contractUnfinishRepository.Table.Where(c => c.ContractTypeId != 1000);
            if (year == DateTime.Now.Year || year == 0)
            {
                ls = ls.Where(c => query.Contains(c.ContractId) && c.CreatedDate.Date == DateTime.Now.Date);
            }
            else
            {
                var lastDay = new DateTime(year, 12, 31);
                ls = ls.Where(c => query.Contains(c.ContractId) && c.CreatedDate.Date == lastDay);
            }

            return ls.Sum(c => c.OptionValue.GetValueOrDefault(0));
        }
        /// <summary>
        /// tong so tien theo loai cong trinh
        /// </summary>
        public virtual Decimal GetTotalMoneyByConstructionType(int constructionTypeId, int year = 0)
        {
            var query = _constructionRepository.Table
                .Join(_itemRepository.Table, x => x.Id, y => y.ConstructionId,
                (x, y) => new { construction = x, contract = y })
                .Where(c => c.contract.StatusId != (int)ContractStatus.Destroy
                    && c.contract.ClassificationId == (int)ContractClassification.AB
                    && c.contract.StatusId != (int)ContractStatus.Draf
                    && c.construction.TypeId == constructionTypeId)
                .Select(c => c.contract).ToList().Distinct();
            if (year != 0)
            {
                query = query.Where(c => Convert.ToDateTime(c.StartDate).Year == year);
            }
            var ls = query.Sum(c => c.TotalAmount);

            return ls.GetValueOrDefault(0);
        }
        public virtual IList<Contract> GetTopContractDealine(int Top = 5)
        {
            var query = _contractMonitorRepository.Table
                .Where(c => c.StatusIds.Contains(((int)ContractMonitorStatus.TimeEnding).ToString()))
                .Select(c => c.ContractId);
            var ls = _itemRepository.Table.Where(c => query.Contains(c.Id));

            return ls.ToList();
        }
        public virtual IList<Contract> GetContractDelayedProcessing(int Top = 5)
        {
            var query = _contractMonitorRepository.Table
                .Where(c => c.StatusIds.Contains(((int)ContractMonitorStatus.OverdueTask).ToString()))
                .Select(c => c.ContractId);
            var ls = _itemRepository.Table.Where(c => query.Contains(c.Id));

            return ls.ToList();
        }
        /// <summary>
        /// tong so tien tam ung
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public PaymentPlanModel GetTotalAdvancePayment(int contractId)
        {
            var model = new PaymentPlanModel();
            var total = GetallPaymentAcceptanceSubs(ContractId: contractId).Sum(c => c.ReduceAdvance.GetValueOrDefault(0));
            var query = _contractPaymentRepository.Table;
            var ls = query.Where(c => c.StatusId != (int)ContractPaymentStatus.Destroy
                && c.TypeId == (int)ContractPaymentType.Advance
                && c.IsReceipts == true
                && c.ContractId == contractId).ToList();
            model.totalPaymentAdvanced = (ls.Sum(c => c.Amount) - total);
            model.totalhasPaymentPlan = ls.Sum(c => c.Amount);
            return model;
        }
        public IList<ContractPayment> GetAllContractPaymentByContractId(int contractId)
        {
            var query = _contractPaymentRepository.Table;
            var ls = query.Where(c => c.StatusId != (int)ContractPaymentStatus.Destroy
                && c.ContractId == contractId).ToList();
            return ls;
        }
        /// <summary>
        /// tong so tien da tam ung, loai tien thu
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public virtual Decimal GetTotalAdvancedHasPayment(int contractId)
        {
            var query = _contractPaymentPlanRepository.Table;
            var ls = query.Where(c => c.TypeId == (int)ContractPaymentType.Advance
                && c.ContractId == contractId).ToList();
            return ls.Sum(c => c.Amount);
        }
        // tong so tien da thu, loai thanh toan
        public virtual Decimal GetTotalAmountCollected(int contractId)
        {
            var query = _contractPaymentRepository.Table;
            var ls = query.Where(c => c.StatusId == (int)ContractPaymentStatus.Approved
                && c.IsReceipts == true
                && c.ContractId == contractId);

            return ls.Sum(c => c.Amount);
        }
        // tong so tien da chi
        public virtual Decimal GetTotalAmountSpent(int contractId)
        {
            var query = _contractPaymentRepository.Table;
            var ls = query.Where(c => c.StatusId == (int)ContractPaymentStatus.Approved
                && c.IsReceipts == false
                && c.ContractId == contractId);

            return ls.Sum(c => c.Amount);
        }
        // tong so tien phai thu
        public virtual Decimal GetTotalAmountReceivable(int contractId)
        {
            var query = _contractPaymentRequestRepository.Table;
            var ls = query.Where(c => c.ContractId == contractId);
            return ls.Sum(c => c.TotalAmount);
        }
        //tong so tien phai chi
        public virtual Decimal GetTotalAmountSpend(int contractId)
        {
            //tam thoi de trang thai khac destroy
            var query = _contractAcceptanceRepository.Table;
            query = query.Where(c => c.ContractId == contractId
                && c.StatusId != (int)ContractAcceptanceStatus.Destroy
                && c.TypeId == (int)ContractAcceptancesType.NoiBo);
            return query.Sum(c => (Decimal)c.TotalAmount * c.currency.Rate);

        }
        // tong gia tri hop dong
        public virtual Decimal GetTotalAmountContract(int contractId)
        {
            var query = _taskRepository.Table;
            var ls = query.Where(c => c.ContractId == contractId
            && c.StatusId != (int)TaskStatus.Destroy
            && c.ParentId == null
            && c.TaskProcuringAgencyId == null);
            return ls.Sum(c => (decimal)c.TotalMoney * c.currency.Rate);
        }
        // tong nghiem thu
        public virtual Decimal GetTotalAmountAccepted(int contractId)
        {
            var query = _contractPaymentAcceptanceRepository.Table;
            var ls = query.Where(c => c.ContractId == contractId
            && c.StatusId != (int)ContractAcceptanceStatus.Destroy);
            return ls.Sum(c => (decimal)c.TotalAmount);
        }
        // tong gia trị BB'
        public virtual Decimal GetTotalAmountBB(int contractId)
        {
            var query = _itemRepository.Table
                    .Where(c => c.StatusId != (int)ContractStatus.Destroy);
            query = query.Join(_contractRelateRepository.Table, x => x.Id, y => y.Contract2Id,
                       (x, y) => new { Contract = x, Mapping = y.Contract1Id })
                   .Where(z => z.Mapping == contractId)
                   .Select(z => z.Contract);
            return query.Sum(c => (decimal)c.TotalAmount);
        }
        /// <summary>
        /// danh sach hop dong BB da duoc duyet cua hop dong AB
        /// </summary>
        /// <param name="contractId"></param>
        public IList<Contract> GetAllContractBBAproval(int contractId)
        {
            var query = _itemRepository.Table.Where(c => c.StatusId != (int)ContractStatus.Destroy && c.StatusId == (int)ContractStatus.Building);
            query = query.Join(_contractRelateRepository.Table, x => x.Id, y => y.Contract2Id,
                (x, y) => new { Contract = x, Mapping = y.Contract1Id })
                .Where(m => m.Mapping == contractId)
                .Select(z => z.Contract);
            return query.ToList();
        }
        //  gia tri da nghiem thu khoi luong cua hop dong
        public virtual Decimal GetTotalAmountAcceptanced(int contractId)
        {
            var query = _contractAcceptanceRepository.Table.Where(c => c.TypeId == (int)ContractAcceptancesType.KhoiLuong
            && c.ContractId == contractId
            && c.StatusId != (int)ContractAcceptanceStatus.Destroy);
            var lstAcceptanceSub = query.Join(_contractAcceptanceSubRepository.Table, x => x.Id, y => y.AcceptanceId,
                (x, y) => new { acceptance = x, acceptanceSub = y })
                .Select(z => z.acceptanceSub)
                .ToList();
            return lstAcceptanceSub.Sum(n => (decimal)n.TotalAmount);
        }
        ///<sumary>
        ///Tong gia tri da nghiem thu thanh toan chua giam tru cua hop dong
        /// </sumary>
        public virtual Decimal GetTotalMoneyPaymentAcceptance(int contractId)
        {
            var query = getAllContractPaymentAcceptance(ContractId: contractId);
            var paymentAcceptanceSub = query.Join(_contractPaymentAcceptanceSubRepository.Table, x => x.Id, y => y.PaymentAcceptanceId,
                (x, y) => new { acceptance = x, acceptanceSub = y })
                .Where(c => c.acceptanceSub.StatusId != (int)PaymentAcceptanceSubStatus.Destroy)
                .Select(m => m.acceptanceSub).ToList();
            return paymentAcceptanceSub.Sum(o =>(Decimal)o.TotalMoney);
        }
        ///<sumary>
        ///Tong gia tri da nghiem thu thanh toan da giam tru cua hop dong
        /// </sumary>
        public virtual Decimal GetTotalAmountPaymentAcceptance(int contractId)
        {
            var query = getAllContractPaymentAcceptance(ContractId: contractId);
            var paymentAcceptanceSub = query.Join(_contractPaymentAcceptanceSubRepository.Table, x => x.Id, y => y.PaymentAcceptanceId,
                (x, y) => new { acceptance = x, acceptanceSub = y })
                .Where(c => c.acceptanceSub.StatusId != (int)PaymentAcceptanceSubStatus.Destroy)
                .Select(m => m.acceptanceSub).ToList();
            return paymentAcceptanceSub.Sum(o => (Decimal)o.TotalAmount);
        }
        ///<sumary>
        ///Tong gia tri da duoc thanh toan cua hop dong
        /// </sumary>
        public virtual Decimal GetTotalAmountPayment(int contractId)
        {
            var query = _contractPaymentRepository.Table;
            var ls = query.Where(c => c.StatusId != (int)ContractPaymentStatus.Destroy
                && c.TypeId == (int)ContractPaymentType.Payment
                && c.IsReceipts == true
                && c.ContractId == contractId).ToList();
            return ls.Sum(c => c.Amount);
        }
        ///<sumary>
        ///Tong gia tri nghiem thu noi bo cua hop dong
        /// </sumary>
        public virtual Decimal GetTotalAmountAcceptanceNoiBo(int contractId)
        {
            var _CaSubs = _contractAcceptanceRepository.Table.Where(c => c.StatusId != (int)ContractAcceptanceStatus.Destroy
            && c.ContractId == contractId && c.TypeId == (int)ContractAcceptancesType.NoiBo)
            .Join(_contractAcceptanceSubRepository.Table, x => x.Id, y => y.AcceptanceId, (x, y) => new { caNoiBo = x, caSub = y })
            .Where(m => m.caSub.StatusId != (int)ContractAcceptanceSubStatus.Destroy)
            .Select(o => o.caSub)
            .ToList();
            return _CaSubs.Sum(c => (Decimal)c.TotalAmount);
        }
        ///<sumary>
        ///Tong gia tri da chi cho don vi cua hop dong
        /// </sumary>
        public virtual Decimal GetTotalAmountPaymentAdvanceForUnit(int contractId)
        {
            var query = _contractPaymentRepository.Table;
            var ls = query.Where(c => c.StatusId != (int)ContractPaymentStatus.Destroy
                && c.TypeId == (int)ContractPaymentType.Advance
                && c.IsReceipts == false
                && c.ContractId == contractId).ToList();
            return ls.Sum(c => c.Amount * c.CurrencyRatio);
        }
        public PaymentPlanModel GetPaymentPlan(int ContractId)
        {
            var contract = GetContractById(ContractId);
            var lstContractSettlement = GetAllContractSettlementSub(contractId: ContractId);
            var model = new PaymentPlanModel()
            {
                //Tong gia tri da nghiem thu thanh toan chua giam tru toan cua hop dong
                totalMoneyPaymentAccepted = GetTotalMoneyPaymentAcceptance(ContractId),
                //Tong gia tri da nghiem thu thanh da giam tru toan cua hop dong
                totalAmountPaymentAccepted = GetTotalAmountPaymentAcceptance(ContractId),
                totalAcceptanced = GetTotalAmountAcceptanced(ContractId),
                //Gia tri con lai chua nghiem thu khoi luong cua hop dong
                totalAmountNotAccepted = (GetTotalAmountContract(ContractId) - GetTotalAmountAcceptanced(ContractId)),
                //Tong gia tri con lai chua nghiem thu thanh toan cua hop dong
                totalAmountNotPaymentAccepted = (GetTotalAmountContract(ContractId) - GetTotalMoneyPaymentAcceptance(ContractId)),
                totalPaymentPlan = GetTotalAdvancePayment(ContractId).totalhasPaymentPlan,
                //tong tien chu dau tu tam ung sau tru tam ung
                totalPaymentAdvanced = GetTotalAdvancePayment(ContractId).totalPaymentAdvanced,
                // tong so tien da duoc thanh toan cua hop dong
                totalAmountPayment = GetTotalAmountPayment(ContractId),
                // tong nghiem thu noi bo cua hop dong
                totalAmountAcceptanceNoiBo = GetTotalAmountAcceptanceNoiBo(ContractId),
                // tong don vi da tam ung cua hop dong
                totalAmountUnitAdvance = GetTotalAmountPaymentAdvanceForUnit(ContractId),
                contractStatus = contract.StatusId,
                // tong da quyet toan cua hop dong
                totalAmountContractSettlement = lstContractSettlement.Sum(c => c.TotalAmount),
                //khoan giu lai cua hop dong
                totalAmountContractHolding = (GetTotalAmountContract(ContractId) - lstContractSettlement.Sum(c => c.TotalAmount))
            };
            return model;
        }
        /// <summary>
        /// Updates a review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual void UpdateContract(Contract item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            item.UpdatedDate = DateTime.Now;
            item.SearchFullText = CreateFullTextString(item);
            _itemRepository.Update(item);

            //event notification
            _eventPublisher.EntityUpdated(item);
        }
        public virtual IList<ContractJoint> GetContractJointsByContractId(int Id)
        {
            if (Id == 0)
            {
                return null;
            }
            return _contractJointRepository.Table
                   .Where(item => item.ContractId == Id)
                   .ToList();
        }
        /// <summary>
        /// Delete review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual void DeleteContract(Contract item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            item.status = ContractStatus.Destroy;
            item.Code1 = item.Code1 + "-deleted";
            item.Code = item.Code + "-deleted";
            _itemRepository.Update(item);

            //event notification
            _eventPublisher.EntityDeleted(item);
        }
        public IList<Contract> getAllContractbyUnitId(int UniId)
        {
            var ListId = _taskRepository.Table.Where(c => c.UnitId == UniId && c.StatusId != (int)TaskStatus.Destroy).Select(c => c.ContractId).Distinct();

            return GetListContractbylistId(ListId.ToList());
        }
        /// <summary>
        /// Lay danh sach hop dong theo unitID co cong viec da duoc nghiem thu thanh toan
        /// </summary>
        public IList<Contract> GetAllContractPaymentAcceptanceByUnitId(int UniId, string approvaldateString)
        {
            var fromDate = new DateTime();
            var approvaldate = DateTime.ParseExact(approvaldateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var year = approvaldate.Year;
            
            if ( approvaldate.Month >= 1 && approvaldate.Month <= 3)
            {
                fromDate = DateTime.ParseExact(year.ToString() + "0101", "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            if (approvaldate.Month >3 && approvaldate.Month <= 6)
            {
                fromDate = DateTime.ParseExact(year.ToString() + "0401", "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            if (approvaldate.Month >6  && approvaldate.Month <= 9)
            { 
                fromDate = DateTime.ParseExact(year.ToString() + "0701", "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            if (approvaldate.Month >9)
            {
                fromDate = DateTime.ParseExact(year.ToString() + "1001", "yyyyMMdd", CultureInfo.InvariantCulture);
            }
           return  _contractPaymentAcceptanceSubRepository.Table.Where(c => c.contractPaymentAcceptance.ApprovalDate >= fromDate 
           && c.contractPaymentAcceptance.ApprovalDate <= approvaldate 
           && c.task.UnitId == UniId).GroupBy(c => c.ContractId).Select(group => group.First()).Select(c => c.contract).ToList();
        }
        /// <summary>
        /// Lay danh sach hop dong trong nam(Where  Year )
        /// </summary>
        public IList<Contract> GetAllContractInYear(int year = 0)
        {
            var query = _itemRepository.Table.Where(c => c.ClassificationId == (int)ContractClassification.AB
                && c.StatusId != (int)ContractStatus.Destroy && c.StatusId != (int)ContractStatus.Draf);
            if (year != 0 )
            {
                query = query.Where(c => Convert.ToDateTime(c.StartDate).Year == year);
            }
            return query.ToList();
        }
        /// <summary>
        /// Lay danh sach hop dong trong nam co nghiem thu khoi luong(Where  Year )
        /// </summary>
        public IList<Contract> GetAllContractInYearAcceptance(int year = 0)
        {
            var query = _contractAcceptanceRepository.Table.Where(c => c.TypeId == (int)ContractAcceptancesType.KhoiLuong
            && c.StatusId != (int)ContractAcceptanceStatus.Destroy
            && Convert.ToDateTime(c.ApprovalDate).Year == year)
            .Select(c => c.ContractId).Distinct().ToList();
            var _lstContract = _itemRepository.Table.Where(c => query.Contains(c.Id) && c.ClassificationId == (int)ContractClassification.AB
                && c.StatusId != (int)ContractStatus.Destroy && c.StatusId != (int)ContractStatus.Draf).ToList();
            return _lstContract;
        }
        /// <summary>
        /// Lay danh sach hop dong trong nam co tam ung(Where  Year )
        /// </summary>
        public IList<Contract> GetAllContractInYearPaymentAdvance(int year = 0)
        {
            var query = _contractPaymentRepository.Table.Where(c => c.TypeId == (int)ContractPaymentType.Advance
            && c.StatusId != (int)ContractPaymentStatus.Destroy
            && Convert.ToDateTime(c.PaymentDate).Year == year
            && c.IsReceipts == true)
            .Select(c => c.ContractId).Distinct().ToList();
            var _lstContract = _itemRepository.Table.Where(c => query.Contains(c.Id) && c.ClassificationId == (int)ContractClassification.AB
                && c.StatusId != (int)ContractStatus.Destroy && c.StatusId != (int)ContractStatus.Draf).ToList();
            return _lstContract;
        }
        /// <summary>
        /// Lay danh sach hop dong theo loai cong trinh
        /// </summary>
        public IList<Contract> GetAllContractByConstructionId(int constructionTypeId)
        {
            var query = _constructionRepository.Table.Where(c => c.TypeId == constructionTypeId)
                .Join(_itemRepository.Table, x => x.Id, y => y.ConstructionId,
                (x,y) => new { construction = x, contract = y})                
                .Select(c => c.contract)
                .ToList();
            return query;
        }
        /// <summary>
        /// Lay danh sach hop dong theo nam va loai cong trinh, 
        /// </summary>
        public IList<Contract>GetAllContractByConstructionInYear(int constructionTypeId, int year)
        {
            var query = GetAllContractByConstructionId(constructionTypeId);
            var lstContract = query.Where(m => Convert.ToDateTime(m.StartDate).Year == year && m.StatusId != (int)ContractStatus.Destroy 
            && m.ClassificationId == (int)ContractClassification.AB
            && m.StatusId !=(int)ContractStatus.Draf).ToList();
            return lstContract;
        }
        /// <summary>
        /// Lay danh sach hop dong trong nam co thanh toan(Where  Year )
        /// </summary>
        public IList<Contract> GetAllContractInYearPaymentPaid(int year = 0)
        {
            var query = _contractPaymentRepository.Table.Where(c => c.TypeId == (int)ContractPaymentType.Payment
            && c.StatusId != (int)ContractPaymentStatus.Destroy
            && Convert.ToDateTime(c.PaymentDate).Year == year
            && c.IsReceipts == true)
            .Select(c => c.ContractId).Distinct().ToList();
            var _lstContract = _itemRepository.Table.Where(c => query.Contains(c.Id) && c.ClassificationId == (int)ContractClassification.AB
                && c.StatusId != (int)ContractStatus.Destroy && c.StatusId != (int)ContractStatus.Draf).ToList();
            return _lstContract;
        }
        ///<summary>
        ///Lay danh sach hop dong trong nam co nghiem thu va theo loai cong trinh
        /// </summary>
        public IList<Contract>GetAllContractInYearPaymentByConstructionTypeId(int constructionTypeId, int year)
        {
            var lstContractInYearAcceptance = GetAllContractInYearAcceptance(year);
            var lstContractByConstructionId = GetAllContractByConstructionId(constructionTypeId);
            var _lstContract = lstContractInYearAcceptance.Join(lstContractByConstructionId, x => x.Id, y => y.Id,
                (x, y) => new { contractInYear = x, contractByConstructionId = y }).Select(m => m.contractByConstructionId).ToList();
            return _lstContract;
        }
        ///<summary>
        ///Lay danh sach hop dong trong nam co tam ung va theo loai cong trinh
        /// </summary>
        public IList<Contract> GetAllContractInYearPaymentAdvanceByConstructionTypeId(int constructionTypeId, int year)
        {
            var lstContractInYearAdvance = GetAllContractInYearPaymentAdvance(year);
            var lstContractByConstructionId = GetAllContractByConstructionId(constructionTypeId);
            var _lstContract = lstContractInYearAdvance.Join(lstContractByConstructionId, x => x.Id, y => y.Id,
                (x, y) => new { contractInYearAdvance = x, contractByConstructionId = y }).Select(m => m.contractByConstructionId).ToList();
            return _lstContract;
        }
        ///<summary>
        ///Lay danh sach hop dong trong nam co thanh toan va theo loai cong trinh
        /// </summary>
        public IList<Contract> GetAllContractInYearPaymentPaidByConstructionTypeId(int constructionTypeId, int year)
        {
            var lstContractInYearPaymentPaid = GetAllContractInYearPaymentPaid(year);
            var lstContractByConstructionId = GetAllContractByConstructionId(constructionTypeId);
            var _lstContract = lstContractInYearPaymentPaid.Join(lstContractByConstructionId, x => x.Id, y => y.Id,
                (x, y) => new { contractInYearPaid = x, contractByConstructionId = y }).Select(m => m.contractByConstructionId).ToList();
            return _lstContract;
        }
        /// <summary>
        /// hàm tính giá trị hợp đồng trước thuế hoặc sau thuế
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="beforeTax"></param>
        /// <returns></returns>
        public Decimal GetTotalAmountContractByTask(int contractId, bool beforeTax = true)
        {
            var listTask = _taskRepository.Table
                .Where(c => c.ContractId == contractId && c.StatusId != (int)TaskStatus.Destroy && c.StatusId != (int)TaskStatus.Draf);
            decimal valueTemp = 0;
            foreach (var task in listTask)
            {
                var totalMoney = task.TotalMoney != null ? (decimal)task.TotalMoney : 0 / task.TaxPercent != null ? ((decimal)task.TaxPercent + 1) : 0;
                valueTemp = valueTemp + totalMoney;
            }
            return valueTemp;
        }
        #endregion
        #region ContractJoint
        /// <summary>
        /// Inserts a review type
        /// </summary>
        /// <param name="item">Review type</param>
        public virtual void InsertContractJoint(ContractJoint item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            item.IsMain = true;
            item.DisplayOrder = 1;
            _contractJointRepository.Insert(item);

            //event notification
            _eventPublisher.EntityInserted(item);
        }
        public virtual ContractJoint GetContractJointById(int _contractId, int _procuringAgencyId)
        {
            if (_contractId == 0 && _procuringAgencyId == 0)
                return null;
            var query = from c in _contractJointRepository.Table
                        where (c.ContractId == _contractId && c.ProcuringAgencyId == _procuringAgencyId)
                        orderby c.Id
                        select c;
            var contractJoint = query.FirstOrDefault();
            return contractJoint;
            //return _contractJointRepository.GetById(itemId);
        }
        public void DeleteContractJoint(int _ContractId, int _ProCuringAgencyId)
        {
            var item = _contractJointRepository.Table.Where(c => c.ContractId == _ContractId && c.ProcuringAgencyId == _ProCuringAgencyId).FirstOrDefault();
            if (item != null)
                _contractJointRepository.Delete(item);
        }
        ///<summary>
        ///lay danh sach lien danh cua hop dong
        /// </summary>
        public IList<ProcuringAgency> GetallProcuringAgencyContract(int contractId)
        {
            var query = _contractJointRepository.Table.Where(c => !c.IsSideA
            && c.ContractId == contractId).Select(c=>c.curProcuringAgency).ToList();
            return query;
        }
        #endregion
        #region  Contract File
        public void InsertContractFile(ContractFile item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (_contractFileRepository.Table.Any(c => c.ContractId == item.ContractId && c.FileId == item.FileId))
                return;
            _contractFileRepository.Insert(item);
        }
        public void DeleteMultiContractFile(int ContractId, int TypeId = 0, int FileId = 0, string entityId = null)
        {
            var query = _contractFileRepository.Table.Where(c => c.ContractId == ContractId);
            if (TypeId > 0)
            {
                query = query.Where(c => c.TypeId == TypeId);
            }
            if (TypeId > 0)
            {
                query = query.Where(c => c.FileId == FileId);
            }
            if (!string.IsNullOrEmpty(entityId))
            {
                query = query.Where(c => c.EntityId == entityId);
            }
            var files = query.ToList();
            foreach (ContractFile file in files)
            {
                _contractFileRepository.Delete(file);
            }
        }
        public void DeleteContractFile(int FileId)
        {
            var item = _contractFileRepository.Table.Where(c => c.FileId == FileId).FirstOrDefault();
            if (item != null)
                _contractFileRepository.Delete(item);
        }
        public IList<ContractFile> GetallContractFiles(int ContractId, int TypeId, int EntityId = 0)
        {
            var query = _contractFileRepository.Table;
            if (ContractId > 0)
            {
                query = query.Where(c => c.ContractId == ContractId);
            }
            if (TypeId > 0)
            {
                query = query.Where(c => c.TypeId == TypeId);
            }
            if (EntityId > 0)
            {
                query = query.Where(c => c.EntityId.ToNumber() == EntityId);
            }
            return query.ToList();
        }
        /// <summary>
        /// get file thuoc nhieu contract
        /// </summary>
        /// <param name="TypeId"></param>
        /// <param name="ContractId"></param>
        /// <param name="EntityId"></param>
        /// <returns></returns>
        public IList<ContractFile> GetContractFiles(int TypeId, int ContractId = 0, int EntityId = 0)
        {
            var query = _contractFileRepository.Table.Where(c => c.TypeId == TypeId);
            if (ContractId > 0)
            {
                query = query.Where(c => c.ContractId == ContractId);
            }
            if (EntityId > 0)
            {
                query = query.Where(c => c.EntityId.ToNumber() == EntityId);
            }
            return query.GroupBy(c => c.FileId).Select(group => group.First()).ToList();
        }

        public IList<WorkFile> GetContractFiles(Guid ContractGuid)
        {
            var contract = GetContractByGuid(ContractGuid);
            if (contract == null)
                return new List<WorkFile>();
            return GetContractFiles(contract.Id);
        }
        public IList<WorkFile> GetContractFiles(int ContractId)
        {
            return _contractFileRepository.Table.Where(c => c.ContractId == ContractId).Select(f => f.workFile).ToList();
        }
        #endregion
        #region ContractView
        public void InsertContractView(ContractView item)
        {
            if (item == null || item.ContractId == 0)
                throw new ArgumentNullException(nameof(item));
            var ent = _contractViewRepository.Table.Where(c => c.CustomerId == item.CustomerId && c.ContractId == item.ContractId).FirstOrDefault();
            if (ent != null)
            {
                ent.ViewDate = DateTime.Now;
                UpdateContractView(ent);
            }
            else
            {
                item.ViewDate = DateTime.Now;
                _contractViewRepository.Insert(item);
                //event notification
                _eventPublisher.EntityInserted(item);
            }
        }
        public void UpdateContractView(ContractView item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            item.ViewDate = DateTime.Now;
            _contractViewRepository.Update(item);
            //event notification
            _eventPublisher.EntityUpdated(item);
        }
        public IList<ContractView> getallContractView(int CustomerId) {            
          return _contractViewRepository.Table.Where(c=>c.CustomerId == CustomerId).OrderByDescending(c=>c.ViewDate).Take(4).ToList();
        }
        public void DeleteContractView(int ContractId)
        {
            var items = _contractViewRepository.Table.Where(c => c.ContractId == ContractId);
            _contractViewRepository.Delete(items);
        }
        #endregion
        #region Contract resource
        public void InsertContractResource(ContractResource item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (item.CustomerId == 0)
            {
                item.CustomerId = null;
            }
            if (item.UnitId == 0)
            {
                item.UnitId = null;
            }
            if (item.GroupId == 0)
            {
                item.GroupId = null;
            }
            //truong hop chi truyen vao unit
            //thi lay ong truong phong de tao
            //code them o day...
            //item.CustomerId = GetCustomerId(UnitId, RoleId);
            //_contractResourceRepository
            //kiem tra da ton tai chua
            if (_contractResourceRepository.Table.Any(c => c.ContractId == item.ContractId
                && c.RoleId == item.RoleId
                && (
                (c.CustomerId == item.CustomerId)
                && (c.UnitId == item.UnitId)
                && (c.GroupId == item.GroupId)
                )
                ))
                return;
            _contractResourceRepository.Insert(item);
            //_eventPublisher.EntityInserted(item);
        }
        public void DeleteContractResource(int ItemId)
        {
            var item = _contractResourceRepository.GetById(ItemId);
            if (item != null)
                _contractResourceRepository.Delete(item);
            _eventPublisher.EntityDeleted(item);
        }
        public IList<ContractResource> GetContractResources(int ContractId, int CustomerId = 0)
        {
            var query = _contractResourceRepository.Table;
            if (ContractId > 0)
                query = query.Where(c => c.ContractId == ContractId);
            if (CustomerId > 0)
                query = query.Where(c => c.CustomerId == CustomerId);
            return query.ToList();
        }
        public ContractResource GetContractResourceById(int ItemId)
        {
            if (ItemId == 0)
                return null;

            return _contractResourceRepository.GetById(ItemId);
        }

        public void DeleteListContractResource(int ContractId, int CustomerId = 0)
        {
            var lst = GetContractResources(ContractId, CustomerId).ToList();
            foreach (ContractResource Cr in lst)
            {
                DeleteContractResource(Cr.Id);
            }
        }
        public void UpdateContractResource(ContractResource item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _contractResourceRepository.Update(item);
            _eventPublisher.EntityUpdated(item);
        }
        public void UpdateContractResources(int ContractId, int[] RoleIds, int UnitId = 0, int GroupId = 0, int CustomerId = 0)
        {
            foreach (var roleId in RoleIds)
            {
                var item = new ContractResource();
                item.RoleId = roleId;
                item.ContractId = ContractId;
                item.UnitId = UnitId;
                item.CustomerId = CustomerId;
                item.GroupId = GroupId;
                InsertContractResource(item);
            }
        }
        public void UpdateViewContractResources(int ContractId, int UnitId = 0, int GroupId = 0, int CustomerId = 0)
        {
            var RoleIds = new int[] { GSCustomerDefaults.GSContractViewerId };
            UpdateContractResources(ContractId, RoleIds, UnitId, GroupId, CustomerId);
        }
        #endregion
        #region ContractPaymentPlan
        public void DeleteContractPaymentPlan(ContractPaymentPlan item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _contractPaymentPlanRepository.Delete(item);
            _eventPublisher.EntityDeleted(item);
        }

        public IList<ContractPaymentPlan> GetAllContractPaymentPlans(int ContractId)
        {
            var query = _contractPaymentPlanRepository.Table;
            if (ContractId > 0)
            {
                query = query.Where(c => c.ContractId == ContractId);
            }
            return query.OrderBy(c => c.PayDate).ToList();
        }
        public IList<ContractPaymentPlan> GetAllContractPaymentPlanByPaymentPlanIds(IList<int> paymentPlanId)
        {
            var query = _contractPaymentPlanRepository.Table.Where(c => paymentPlanId.Contains(c.Id)).ToList();
            return query;
        }
        public ContractPaymentPlan GetContractPaymentPlanById(int itemId)
        {
            return _contractPaymentPlanRepository.GetById(itemId);
        }

        public void InsertContractPaymentPlan(ContractPaymentPlan item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            item.CreatedDate = DateTime.Now;
            _contractPaymentPlanRepository.Insert(item);
            _eventPublisher.EntityInserted(item);
        }

        public void UpdateContractPaymentPlan(ContractPaymentPlan item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _contractPaymentPlanRepository.Update(item);
            _eventPublisher.EntityUpdated(item);
        }
        #endregion
        #region ContractAcceptance
        public IList<ContractAcceptance> getAllContractAcceptance(int ContractId = 0, int TaskId = 0, int PaymentAcceptanceId = 0, int TypeId = 0, bool isFilter = false)
        {
            var query = _contractAcceptanceRepository.Table.Where(c => c.StatusId != (int)ContractAcceptanceStatus.Destroy);
            if (TypeId > 0)
            {
                query = query.Where(c => c.TypeId == TypeId);
            }
            if (ContractId > 0)
            {
                query = query.Where(c => c.ContractId == ContractId);
            }
            if (TaskId > 0)
            {
                query = query.Join(_contractAcceptanceTaskMappingRepository.Table, x => x.Id, y => y.ContractAcceptanceId,
                       (x, y) => new { ContractAcceptance = x, Mapping = y })
                   .Where(z => z.Mapping.TaskId == TaskId)
                   .Select(z => z.ContractAcceptance);
            }
            if (isFilter)
            {
                if (PaymentAcceptanceId == 0)
                {
                    query = query.Where(c => c.PaymentAcceptanceId == null || c.PaymentAcceptanceId == 0);
                }
                else
                {
                    query = query.Where(c => c.PaymentAcceptanceId == PaymentAcceptanceId);
                }
            }
            return query.OrderBy(c => c.Id).ToList();
        }

        public ContractAcceptance getContractAcceptancebyId(int Id)
        {
            return _contractAcceptanceRepository.GetById(Id);
        }
        public ContractAcceptance GetContractAcceptanceByGuid(Guid acceptanceGuid)
        {
            if (acceptanceGuid == Guid.Empty)
                return null;
            var query = _contractAcceptanceRepository.Table.Where(c => c.AcceptanceGuid == acceptanceGuid).FirstOrDefault();
            return query;
        }
        public void InsertContractAcceptance(ContractAcceptance item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            item.CreatedDate = DateTime.Now;
            _contractAcceptanceRepository.Insert(item);
            _eventPublisher.EntityInserted(item);
        }

        public void DeleteContractAcceptance(ContractAcceptance item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            item.StatusId = (int)ContractAcceptanceStatus.Destroy;
            _contractAcceptanceRepository.Update(item);
            _eventPublisher.EntityUpdated(item);
        }

        public void UpdateContractAcceptance(ContractAcceptance item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _contractAcceptanceRepository.Update(item);
            //event notification
            _eventPublisher.EntityUpdated(item);
        }
        public void UpdateContractAcceptancebypayment(int ContractAcceptanceId, int PaymentAcceptanceId)
        {
            var item = getContractAcceptancebyId(ContractAcceptanceId);
            item.PaymentAcceptanceId = PaymentAcceptanceId;
            UpdateContractAcceptance(item);
        }
        public IList<ContractAcceptance> getAllContractAcceptanceByAdvanceId(int AdvanceId, int ContractId = 0)
        {
            var query = _contractAcceptanceRepository.Table.Where(c => c.StatusId != (int)ContractAcceptanceStatus.Destroy && c.TypeId == (int)ContractAcceptancesType.TamUng && c.PaymentAdvanceId == AdvanceId);
            if (ContractId > 0)
            {
                query = query.Where(c => c.ContractId == ContractId);
            }
            return query.ToList();
        }
        public IPagedList<ContractAcceptance> GetAllContractAcceptanceNoiBo(string keySearch, int unitId = 0, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _contractAcceptanceRepository.Table.Where(c => c.StatusId == (int)ContractAcceptanceStatus.Approved
             && c.TypeId == (int)ContractAcceptancesType.NoiBo);
            if (!String.IsNullOrEmpty(keySearch))
            {
                query = query.Where(c => c.Description.Contains(keySearch) || c.Name.Contains(keySearch));
            }
            if (unitId > 0)
            {
                query = query.Where(c => c.UnitId == unitId);
            }
            query = query.OrderByDescending(c => c.CreatedDate);
            return new PagedList<ContractAcceptance>(query, pageIndex, pageSize);
        }
        public ContractAcceptance GetContractAcceptanceNoiBoById(int Id)
        {
            return _contractAcceptanceRepository.Table.Where(c => c.Id == Id && c.StatusId != (int)ContractAcceptanceStatus.Destroy).FirstOrDefault();
        }
        public IList<ContractAcceptance> GetContractAcceptanceByAdvanceId(int AdvanceId)
        {
            var query = _contractAcceptanceRepository.Table.Where(c => c.PaymentAdvanceId == AdvanceId && c.TypeId == 4);
            return query.ToList();
        }
        #endregion
        #region ContractAcceptanceBB
        public IList<ContractAcceptanceBB> getAllContractAcceptanceBB(int ContractId = 0, int ContractIdBB = 0, int TaskId = 0, int TypeId = 1)
        {
            var query = _contractAcceptanceBBRepository.Table.Where(c => c.TypeId == TypeId && c.StatusId == (int)ContractAcceptanceBBType.BB);
            if (ContractId > 0)
            {
                query = query.Where(c => c.ContractId == ContractId);
            }
            if (ContractIdBB > 0)
            {
                query = query.Where(c => c.ContractIdBB == ContractIdBB);
            }
            if (TaskId > 0)
            {
                query = query.Where(c => c.TaskId == TaskId);
            }
            return query.OrderByDescending(c => c.Id).ToList();
        }
        public ContractAcceptanceBB getContractAcceptanceBBbyId(int Id)
        {
            return _contractAcceptanceBBRepository.GetById(Id);
        }

        public void InsertContractAcceptanceBB(ContractAcceptanceBB item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _contractAcceptanceBBRepository.Insert(item);
            //event notification
            _eventPublisher.EntityInserted(item);
        }

        public void DeleteContractAcceptanceBB(ContractAcceptanceBB item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            item.StatusId = (int)ContractAcceptanceBBStatus.Destroy;
            _contractAcceptanceBBRepository.Update(item);
            _eventPublisher.EntityUpdated(item);
        }

        public void UpdateContractAcceptanceBB(ContractAcceptanceBB item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _contractAcceptanceBBRepository.Update(item);
            //event notification
            _eventPublisher.EntityUpdated(item);
        }
        #endregion
        #region ContractAcceptancemapping
        public void InsertContractAcceptanceTaskMapping(ContractAcceptanceTaskMapping item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _contractAcceptanceTaskMappingRepository.Insert(item);
            //event notification
            _eventPublisher.EntityInserted(item);
        }
        public void DeleteContractAcceptanceTaskMapping(ContractAcceptanceTaskMapping item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _contractAcceptanceTaskMappingRepository.Delete(item);
            //event notification
            _eventPublisher.EntityUpdated(item);
        }

        public void DeleteContractAcceptanceTaskMappingbyAcceptanceId(int ContractAcceptanceId)
        {
            var items = _contractAcceptanceTaskMappingRepository.Table.Where(c => c.ContractAcceptanceId == ContractAcceptanceId).ToList();
            _contractAcceptanceTaskMappingRepository.Delete(items);

        }
        public void DeleteContractAcceptanceTaskMappingbyTaskId(int TaskId)
        {
            var items = _contractAcceptanceTaskMappingRepository.Table.Where(c => c.TaskId == TaskId).ToList();
            _contractAcceptanceTaskMappingRepository.Delete(items);
        }
        public List<ContractAcceptanceTaskMapping> GetAllContractAcceptanceTaskMapping(int TaskId = 0, int AcepptanceId = 0)
        {
            var query = _contractAcceptanceTaskMappingRepository.Table;
            if (TaskId > 0)
            {
                query = query.Where(c => c.TaskId == TaskId);
            }
            if (AcepptanceId > 0)
            {
                query = query.Where(c => c.ContractAcceptanceId == AcepptanceId);
            }
            return query.ToList();
        }
        public ContractAcceptanceTaskMapping GetContractAcceptanceTaskMappingByAcceptanceId(int AcceptanceId)
        {
            return _contractAcceptanceTaskMappingRepository.Table.Where(c => c.ContractAcceptanceId == AcceptanceId).FirstOrDefault();
        }
        public List<ContractAcceptanceTaskMapping> GetContractAcceptanceTaskMappingByListAcceptanceId(List<int> ListAcceptanceIds)
        {
            return _contractAcceptanceTaskMappingRepository.Table.Where(c => ListAcceptanceIds.Contains(c.ContractAcceptanceId)).ToList();
        }
        #endregion
        #region  ContractPaymentAcceptance
        public IList<ContractPaymentAcceptance> getAllContractPaymentAcceptance(int ContractId = 0, int TaskId = 0)
        {
            var query = this._contractPaymentAcceptanceRepository.Table.Where(c => c.StatusId != (int)ContractPaymentAcceptanceStatus.Destroy);
            if (ContractId > 0)
            {
                query = query.Where(c => c.ContractId == ContractId);
            }
            return query.OrderBy(c => c.Id).ToList();
        }

        public ContractPaymentAcceptance getContractPaymentAcceptancebyId(int Id)
        {
            return _contractPaymentAcceptanceRepository.GetById(Id);
        }

        public void InsertContractPaymentAcceptance(ContractPaymentAcceptance item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _contractPaymentAcceptanceRepository.Insert(item);
            //event notification
            _eventPublisher.EntityInserted(item);
        }

        public void DeleteContractPaymentAcceptance(ContractPaymentAcceptance item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            var items = _contractAcceptanceRepository.Table.Where(c => c.PaymentAcceptanceId == item.Id).ToList();
            foreach (var i in items)
                i.PaymentAcceptanceId = null;
            if (items.Count > 0)
                _contractAcceptanceRepository.Update(items);

            item.StatusId = (int)ContractPaymentAcceptanceStatus.Destroy;
            _contractPaymentAcceptanceRepository.Update(item);
            //event notification
            _eventPublisher.EntityUpdated(item);
        }

        public void UpdateContractPaymentAcceptance(ContractPaymentAcceptance item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _contractPaymentAcceptanceRepository.Update(item);
            //event notification
            _eventPublisher.EntityUpdated(item);
        }
        public IList<ContractPaymentAcceptance> GetAllContractPaymentAcceptancesByRequestId(int paymentRequestId, int ContractId)
        {
            var items = _contractPaymentAcceptanceRepository.Table.Where(c => c.StatusId != (int)ContractAcceptanceStatus.Destroy
            && c.ContractId == ContractId
            && (c.PaymentRequestId == null || c.PaymentRequestId == paymentRequestId)).ToList();
            return items;
        }
        public IList<int> GetAllContractPaymenAcceptanceIdByPaymentRequestId(int paymentRequestId)
        {
            return _contractPaymentAcceptanceRepository.Table.Where(c => c.PaymentRequestId == paymentRequestId).Select(c => c.Id).ToList();
        }
        public IList<ContractPaymentAcceptance> GetAllContractPaymenAcceptanceByPaymentRequestId(int paymentRequestId)
        {
            return _contractPaymentAcceptanceRepository.Table.Where(c => c.PaymentRequestId == paymentRequestId).ToList();
        }
        public IList<ContractPaymentAcceptance> GetAllContractPaymenAcceptanceByContractId(int contractId)
        {
            return _contractPaymentAcceptanceRepository.Table
                .Where(c => c.ContractId == contractId
                && c.StatusId != (int)GS.Core.Domain.Contracts.ContractPaymentAcceptanceStatus.Destroy).ToList();
        }
        public IList<ContractPaymentAcceptance> GetPaymentAcceptancebyListAcceptanceIds(List<int> ListAcceptanceIds)
        {
            var query = _contractPaymentAcceptanceRepository.Table;
            if (ListAcceptanceIds.Count > 0)
            {
                query = query.Where(c => ListAcceptanceIds.Contains(c.Id));
            }
            return query.ToList();
        }
        public ContractPaymentAcceptance getContractPaymentAcceptanceByPaymentRequestId(int paymentRequestId)
        {
            var query = _contractPaymentAcceptanceRepository.Table.Where(c => c.PaymentRequestId == paymentRequestId).FirstOrDefault();
            return query;
        }
        #endregion
        #region ContractPaymentRequest
        /// <summary>
        /// Delete the review type
        /// </summary>
        /// <param name="item">Contract type</param>
        public void DeleteContractPaymentRequest(ContractPaymentRequest item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            //kiem tra xem co lien ket dem giao dich tien ko ?
            if (_contractPaymentRepository.Table.Any(c => c.PaymentRequestId == item.Id && c.StatusId != (int)ContractPaymentStatus.Destroy))
                return;
            //xoa lien ket nghiem thu thanh toan
            var items = _contractPaymentAcceptanceRepository.Table.Where(c => c.PaymentRequestId == item.Id).ToList();
            foreach (var i in items)
            {
                i.PaymentRequestId = null;
            }
            _contractPaymentAcceptanceRepository.Update(items);
            //xoa cac lien ket giao dich tien lien quan den request, phai thao tac huy giao dich tien truc luc xoa
            //neu con giao dich tien thi ko cho xoa
            var itempayments = _contractPaymentRepository.Table.Where(c => c.PaymentRequestId == item.Id && c.StatusId == (int)ContractPaymentStatus.Destroy).ToList();
            foreach (var i in itempayments)
            {
                i.PaymentRequestId = null;
            }
            _contractPaymentRepository.Update(itempayments);
            //xoa request
            _contractPaymentRequestRepository.Delete(item);
            _eventPublisher.EntityDeleted(item);
        }

        /// <summary>
        /// Get all review types
        /// </summary>
        /// <returns>Contract types</returns>
        public IList<ContractPaymentRequest> GetAllContractPaymentRequestByContractId(int contractId)
        {
            var query = _contractPaymentRequestRepository.Table
                .Where(item => item.ContractId == contractId)
                .ToList();
            return query;
        }
        public List<ContractPaymentRequest> GetAllContractPaymentRequestByContractIdAndPlanId(int contractId, int paymentPlanId)
        {
            var query = _contractPaymentRequestRepository.Table
                .Where(item => item.ContractId == contractId && item.PaymentPlanId == paymentPlanId)
                .ToList();
            return query;
        }
        public int GetCountOfRequest(int contractId, int paymentPlanId)
        {
            var query = _contractPaymentRequestRepository.Table
                .Where(item => item.ContractId == contractId && item.PaymentPlanId == paymentPlanId)
                .Count();
            return query;
        }
        public IList<ContractPaymentRequest> GetAllContractPaymentRequest()
        {
            return _contractPaymentRequestRepository.Table
                .OrderBy(c => c.Id)
                .ToList();
        }
        /// <summary>
        /// Get the review type 
        /// </summary>
        /// <param name="itemId">Contract type identifier</param>
        /// <returns>Contract type</returns>
        public ContractPaymentRequest GetContractPaymentRequestById(int itemId)
        {
            return _contractPaymentRequestRepository.GetById(itemId);
        }

        /// <summary>
        /// Insert the review type
        /// </summary>
        /// <param name="item">Contract type</param>
        public void InsertContractPaymentRequest(ContractPaymentRequest item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            item.CreatedDate = DateTime.Now;
            _contractPaymentRequestRepository.Insert(item);
            _eventPublisher.EntityInserted(item);
        }
        /// <summary>
        /// Update the review type
        /// </summary>
        /// <param name="item">Contract type</param>
        public void UpdateContractPaymentRequest(ContractPaymentRequest item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _contractPaymentRequestRepository.Update(item);
            _eventPublisher.EntityUpdated(item);
        }
        #endregion
        #region ContractPaymentTask
        public IList<ContractPaymentTask> getallContractPaymentTask(int TaskId = 0, int PaymentId = 0)
        {
            var query = _contractPaymentTaskRepository.Table;
            if (TaskId > 0)
            {
                query = query.Where(c => c.TaskId == TaskId);
            }
            if (PaymentId > 0)
            {
                query = query.Where(c => c.PaymentId == PaymentId);
            }
            return query.ToList();
        }
        public void InsertContractPaymentTask(ContractPaymentTask item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _contractPaymentTaskRepository.Insert(item);
            _eventPublisher.EntityInserted(item);
        }
        public void DeleteContractPaymentTask(ContractPaymentTask item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _contractPaymentTaskRepository.Delete(item);
            _eventPublisher.EntityDeleted(item);
        }
        public void DeleteListContractPaymentTask(int TaskId = 0, int PaymentId = 0)
        {
            var list = getallContractPaymentTask(TaskId: TaskId, PaymentId: PaymentId);
            _contractPaymentTaskRepository.Delete(list);
        }
        #endregion
        #region ContractAcceptionSub
        public List<ContractAcceptanceSub> GetallContractAcceptanceSubs(int AcceptanceId = 0, int ContractId = 0, int TaskId = 0, int TypeId = (int)ContractAcceptancesType.KhoiLuong)
        {
            var query = _contractAcceptanceSubRepository.Table.Where(c => c.StatusId != (int)ContractAcceptanceSubStatus.Destroy && c.contractAcceptance.TypeId == TypeId);
            if (AcceptanceId > 0)
            {
                query = query.Where(c => c.AcceptanceId == AcceptanceId);
            }
            if (ContractId > 0)
            {
                query = query.Where(c => c.ContractId == ContractId);
            }
            if (TaskId > 0)
            {
                query = query.Where(c => c.TaskId == TaskId);

            }
            return query.OrderBy(c => c.Id).ToList();
        }

        public ContractAcceptanceSub GetContractAcceptanceSubById(int Id)
        {
            return _contractAcceptanceSubRepository.GetById(Id);
        }

        public void UpdateContractAcceptanceSub(ContractAcceptanceSub item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _contractAcceptanceSubRepository.Update(item);
            //event notification
            _eventPublisher.EntityUpdated(item);
        }

        public void DeleteContractAcceptanceSub(ContractAcceptanceSub item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _contractAcceptanceSubRepository.Delete(item);
            _eventPublisher.EntityDeleted(item);
        }
        public void InsertContractAcceptionSub(ContractAcceptanceSub item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _contractAcceptanceSubRepository.Insert(item);
            _eventPublisher.EntityInserted(item);
        }

        public void DeleteContractAcceptanceSubByAcceptanceId(int AccepId, int TypeId)
        {
            var ContractAcceptanceSub = GetallContractAcceptanceSubs(AcceptanceId: AccepId,TypeId: TypeId);
            foreach (ContractAcceptanceSub sub in ContractAcceptanceSub)
            {
                DeleteContractAcceptanceSub(sub);
            }
        }
        public List<ContractAcceptanceSub> GetAllContractAcceptanceSubByAcceptanceNoiBoId(int acceptanceNoiBoId)
        {
            var query = _contractAcceptanceSubRepository.Table
                .Where(c => c.AcceptanceId == acceptanceNoiBoId
                && c.StatusId != (int)ContractAcceptanceSubStatus.Destroy)
                .ToList();
            return query;
        }
        public IList<ContractAcceptanceSub> GetAllContractAcceptanceSubByListAcceptanceId(IList<int> acceptanceId)
        {
            var query = _contractAcceptanceSubRepository.Table
                .Where(c => acceptanceId.Contains(c.AcceptanceId) && c.StatusId != (int)ContractAcceptanceSubStatus.Destroy).ToList();
            return query;
        }
        public List<ContractAcceptanceSub> GetAllContractAcceptanceNoiBoSub(int ContractId, int unitId, int acceptanceId = 0)
        {
            var query = _contractAcceptanceSubRepository.Table
                .Where(c => c.AcceptanceId == acceptanceId
                && c.StatusId != (int)ContractAcceptanceSubStatus.Destroy
                && c.UnitId == unitId
                && c.ContractId == ContractId)
                .ToList();
            return query;
        }
        #endregion
        #region ContractPaymentAcceptionSub
        public void InsertContractPaymentAcceptionSub(ContractPaymentAcceptanceSub item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _contractPaymentAcceptanceSubRepository.Insert(item);
            _eventPublisher.EntityInserted(item);
        }

        public List<ContractPaymentAcceptanceSub> GetallPaymentAcceptanceSubs(int AcceptanceId = 0, int ContractId = 0, int PaymentAcceptanceId = 0, int TaskId = 0)
        {
            var query = _contractPaymentAcceptanceSubRepository.Table.Where(c => c.StatusId != (int)PaymentAcceptanceSubStatus.Destroy);
            if (PaymentAcceptanceId > 0)
            {
                query = query.Where(c => c.PaymentAcceptanceId == PaymentAcceptanceId);
            }
            if (TaskId > 0)
            {
                query = query.Where(c => c.TaskId == TaskId);
            }
            if (ContractId > 0)
            {
                query = query.Where(c => c.ContractId == ContractId);
            }
            if (AcceptanceId > 0)
            {
                query = query.Where(c => c.AcceptanceId == AcceptanceId);
            }
            return query.OrderBy(c => c.Id).ToList();
        }

        public ContractPaymentAcceptanceSub GetContractPaymentAcceptanceSubById(int Id)
        {
            return _contractPaymentAcceptanceSubRepository.GetById(Id);
        }
        public IList<ContractPaymentAcceptanceSub> GetContractPaymentAcceptanceSubByAcceptanceId(int acceptanceId)
        {
            var query = _contractPaymentAcceptanceSubRepository.Table
                .Where(c => c.StatusId != (int)PaymentAcceptanceSubStatus.Destroy && c.AcceptanceId == acceptanceId);
            return query.ToList();
        }
        public IList<ContractPaymentAcceptanceSub> GetAllContractPaymentAcceptanceSubByApprovalDate(int taskId, string approvaldateString)
        {
            var fromDate = new DateTime();
            var approvaldate = DateTime.ParseExact(approvaldateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var year = approvaldate.Year;

            if (approvaldate.Month >= 1 && approvaldate.Month <= 3)
            {
                fromDate = DateTime.ParseExact(year.ToString() + "0101", "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            if (approvaldate.Month > 3 && approvaldate.Month <= 6)
            {
                fromDate = DateTime.ParseExact(year.ToString() + "0401", "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            if (approvaldate.Month > 6 && approvaldate.Month <= 9)
            {
                fromDate = DateTime.ParseExact(year.ToString() + "0701", "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            if (approvaldate.Month > 9)
            {
                fromDate = DateTime.ParseExact(year.ToString() + "1001", "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            return GetallPaymentAcceptanceSubs(TaskId: taskId).Where(c => c.contractPaymentAcceptance.ApprovalDate >= fromDate && c.contractPaymentAcceptance.ApprovalDate <= approvaldate).ToList();
           
        }
        public void UpdateContractPaymentAcceptanceSub(ContractPaymentAcceptanceSub item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _contractPaymentAcceptanceSubRepository.Update(item);
            //event notification
            _eventPublisher.EntityUpdated(item);
        }

        public void DeleteContractPaymentAcceptanceSub(ContractPaymentAcceptanceSub item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _contractPaymentAcceptanceSubRepository.Delete(item);           
            _eventPublisher.EntityDeleted(item);
        }

        public void DeletePaymentAcceptanceSubByPaymentAcceptanceId(int Id)
        {
            var ContractAcceptanceSub = GetallPaymentAcceptanceSubs(PaymentAcceptanceId: Id);
            foreach (ContractPaymentAcceptanceSub sub in ContractAcceptanceSub)
            {
                DeleteContractPaymentAcceptanceSub(sub);
            }
        }
        #endregion
        #region ContractMonitor
        public virtual String GetStatusContractMonior(int contractId)
        {
            var query = _contractMonitorRepository.Table;
            var ls = query.Where(c => c.ContractId == contractId)
                .Select(c => c.StatusIds).ToString();
            return ls;
        }
        public virtual String GetAcceptanceContractMonitor(int contractId)
        {
            var query = _contractMonitorRepository.Table;
            var ls = query.Where(c => c.ContractId == contractId)
                .Select(c => c.AcceptanceSummary).ToString();
            return ls;
        }
        public virtual String GetPaymentContractMonitor(int contractId)
        {
            var query = _contractMonitorRepository.Table;
            var ls = query.Where(c => c.ContractId == contractId)
                .Select(c => c.PaymentSummary).ToString();
            return ls;
        }
        public virtual ContractMonitor GetContractMonitor(int contractId = 0)
        {
            var query = _contractMonitorRepository.Table;
            if (contractId > 0)
            {
                query = query.Where(c => c.ContractId == contractId);
            }
            return query.ToList().FirstOrDefault();
        }


        #endregion
        #region PaymentPlanContract_map
        public void InsertPaymentPlanContract(PaymentPlanContract item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            if (_paymentPlanContractRepository.Table.Any(c => c.ContractId == item.ContractId && c.PaymentPlanId == item.PaymentPlanId))
            {
                return;
            }
            _paymentPlanContractRepository.Insert(item);
            _eventPublisher.EntityInserted(item);
        }
        public void DeletePaymentPlanContract(PaymentPlanContract item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            _paymentPlanContractRepository.Delete(item);
            _eventPublisher.EntityInserted(item);
        }
        public IList<PaymentPlanContract> GetPaymentPlanContracts(int contractId = 0, int paymentPlanId = 0)
        {
            var query = _paymentPlanContractRepository.Table;
            if (contractId > 0)
            {
                query = query.Where(c => c.ContractId == contractId);
            }
            if (paymentPlanId > 0)
            {
                query = query.Where(c => c.PaymentPlanId == paymentPlanId);
            }
            return query.ToList();
        }
        public IList<PaymentPlanContract> GetPaymentPlanContractsByPaymentPlanId(int paymentPlanId = 0)
        {
            var query = _paymentPlanContractRepository.Table;
            if (paymentPlanId > 0)
            {
                query = query.Where(c => c.PaymentPlanId == paymentPlanId);
            }
            return query.ToList();
        }
        public IList<PaymentPlanContract> GetPaymentPlanContractsByContractId(int contractId = 0)
        {
            var query = _paymentPlanContractRepository.Table;
            if (contractId > 0)
            {
                query = query.Where(c => c.ContractId == contractId);
            }
            return query.ToList();
        }
        #endregion
        #region ContractSettlement
        public IList<ContractSettlement> getAllContractSettlement(int ContractId = 0)
        {
            var query = _contractSettlementRepository.Table.Where(c => c.StatusId != (int)ContractSettlementStatusId.Delete);
            if (ContractId > 0)
            {
                query = query.Where(c => c.ContractId == ContractId);
            }
            return query.OrderBy(c => c.Id).ToList();
        }
        public ContractSettlement getContractSettlementbyId(int Id)
        {
            return _contractSettlementRepository.GetById(Id);
        }
        public void InsertContractSettlement(ContractSettlement item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            item.CreatedDate = DateTime.Now;
            _contractSettlementRepository.Insert(item);
            _eventPublisher.EntityInserted(item);
        }
        public void DeleteContractSettlement(ContractSettlement item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            item.StatusId = (int)ContractSettlementStatusId.Delete;
            _contractSettlementRepository.Update(item);
            _eventPublisher.EntityUpdated(item);
        }
        public void UpdateContractSettlement(ContractSettlement item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _contractSettlementRepository.Update(item);
            _eventPublisher.EntityUpdated(item);
        }
        #endregion
        #region ContractSettlement_map_Task
        public void InsertContractSettlementTaskMapping(ContractSettlementTaskMapping item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _contractSettlementTaskMappingRepository.Insert(item);
            //event notification
            _eventPublisher.EntityInserted(item);
        }
        public void DeleteContractSettlementTaskMapping(ContractSettlementTaskMapping item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _contractSettlementTaskMappingRepository.Delete(item);
            //event notification
            _eventPublisher.EntityUpdated(item);
        }
        public void DeleteContractSettlementTaskMappingbySettlementId(int ContractSettlementId)
        {
            var items = _contractSettlementTaskMappingRepository.Table.Where(c => c.SettlementId == ContractSettlementId).ToList();
            _contractSettlementTaskMappingRepository.Delete(items);
        }
        public void DeleteContractSettlementTaskMappingbyTaskId(int TaskId)
        {
            var items = _contractSettlementTaskMappingRepository.Table.Where(c => c.TaskId == TaskId).ToList();
            _contractSettlementTaskMappingRepository.Delete(items);
        }
        public List<ContractSettlementTaskMapping> GetAllContractSettlementTaskMapping(int TaskId = 0, int SettlementId = 0)
        {
            var query = _contractSettlementTaskMappingRepository.Table;
            if (TaskId > 0)
            {
                query = query.Where(c => c.TaskId == TaskId);
            }
            if (SettlementId > 0)
            {
                query = query.Where(c => c.SettlementId == SettlementId);
            }
            return query.ToList();
        }
        public ContractSettlementTaskMapping GetContractSettlementTaskMappingBySettlementId(int SettlementId)
        {
            return _contractSettlementTaskMappingRepository.Table.Where(c => c.SettlementId == SettlementId).FirstOrDefault();
        }
        public List<ContractSettlementTaskMapping> GetContractSettlementTaskMappingByListSettlementId(List<int> ListSettlementIds)
        {
            return _contractSettlementTaskMappingRepository.Table.Where(c => ListSettlementIds.Contains(c.SettlementId)).ToList();
        }
        #endregion
        #region ContractSettlementSub
        public List<ContractSettlementSub> GetAllContractSettlementSub(int contractId = 0, int taskId = 0, int contractSettlementId = 0)
        {
            var query = _contractSettlementSubRepository.Table;
            if (contractId > 0)
            {
                query = query.Where(c => c.ContractId == contractId);
            }
            if (taskId > 0)
            {
                query = query.Where(c => c.TaskId == taskId);
            }
            if (contractSettlementId > 0)
            {
                query = query.Where(c => c.ContractSettlementId == contractSettlementId);
            }
            return query.ToList();
        }
        public ContractSettlementSub GetContractSettlementSubById(int id)
        {
            return _contractSettlementSubRepository.GetById(id);
        }
        public void UpdateContractSettlementSub(ContractSettlementSub item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _contractSettlementSubRepository.Update(item);
            //event notification
            _eventPublisher.EntityUpdated(item);
        }
        public void DeleteContractSettlementSub(ContractSettlementSub item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _contractSettlementSubRepository.Delete(item);
            _eventPublisher.EntityDeleted(item);
        }
        public void InsertContractSettlementSub(ContractSettlementSub item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _contractSettlementSubRepository.Insert(item);
            _eventPublisher.EntityInserted(item);
        }
        public void DeleteContractSettlementSubBySettlementId(int id)
        {
            var item = GetAllContractSettlementSub(contractSettlementId: id);
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _contractSettlementSubRepository.Delete(item);           
        }
        public virtual IList<int> GetTaskByContractIdInSettlementSub(int id)
        {
            var item = _contractSettlementSubRepository.Table.Where(c => c.ContractId == id).Select(c => c.TaskId);
            return item.ToList();
        }
        public virtual IList<int> GetListTaskIdBySettlementId(int id)
        {
            var item = _contractSettlementSubRepository.Table.Where(c => c.ContractSettlementId == id).Select(c => c.TaskId);
            return item.ToList();
        }
        public virtual IList<ContractSettlementSub> GetSubBySettlementId(int id)
        {
            var item = _contractSettlementSubRepository.Table.Where(c => c.ContractSettlementId == id);
            return item.ToList();
        }
        #endregion
        #region ContractPaymentExpenditure
        public void InsertPaymentExpenditure(PaymentExpenditure item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _paymentExpenditureRepository.Insert(item);
            _eventPublisher.EntityUpdated(item);
        }
        public void DeletePaymentExpenditure(PaymentExpenditure item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _paymentExpenditureRepository.Delete(item);
            _eventPublisher.EntityUpdated(item);
        }
        public void UpdatePaymentExpenditure(PaymentExpenditure item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _paymentExpenditureRepository.Update(item);
            _eventPublisher.EntityUpdated(item);
        }
        public PaymentExpenditure GetPaymentExpenditureById(int Id)
        {
            return _paymentExpenditureRepository.GetById(Id);
        }
        public IPagedList<PaymentExpenditure> GetAllPaymentExpenditure(int UnitId = 0, string Keysearch = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _paymentExpenditureRepository.Table;
            if (UnitId > 0)
            {
                query = query.Where(c => c.UnitId == UnitId);
            }
            if (!string.IsNullOrEmpty(Keysearch))
            {
                query = query.Where(c => c.Name.Contains(Keysearch) || c.Code.Contains(Keysearch));
            }

            return new PagedList<PaymentExpenditure>(query.OrderByDescending(c => c.Id), pageIndex, pageSize);
        }
        public PaymentExpenditure GetPaymentExpenditureByGuid(Guid guId)
        {
            var query = _paymentExpenditureRepository.Table.Where(c => c.ExpenditureGuid == guId);
            return query.FirstOrDefault();
        }
        // public IList<>
        #endregion
        #region ContractUnfinish
        public IList<ContractUnfinish> getallContractUnfinish(int ContractId = 0,DateTime? CreatedDate = null, int ContractTypeId = 0)
        {
            var query = _contractUnfinishRepository.Table;
            if (ContractId >0)
            {
                query = query.Where(c => c.ContractId == ContractId);
            }
            if (CreatedDate != null)
            {
                query = query.Where(c => c.CreatedDate.Date == CreatedDate.GetValueOrDefault().Date);
            }
            if (ContractTypeId >0)
            {
                query = query.Where(c => c.ContractTypeId == ContractTypeId);
            }
            return query.ToList();
        }
        public IList<ContractUnfinish> GetContractUnfinishNow(int contratcId)
        {
            var query = _contractUnfinishRepository.Table.Where(c => c.ContractId == contratcId);
            if (query.Count() >0)
            {
                var datetime = query.OrderByDescending(c => c.CreatedDate).FirstOrDefault().CreatedDate;
                var ls = query.Where(c => c.CreatedDate.Date == datetime.Date);
                return ls.Where(c => c.TypeId == 1).ToList();
            }
            return null;
        }
        public ContractUnfinish getContractUnfinishById(int Id)
        {
            return _contractUnfinishRepository.GetById(Id);
        }
        public void UpdateContractUnfinish(ContractUnfinish item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            _contractUnfinishRepository.Update(item);
            _eventPublisher.EntityUpdated(item);
        }
        public void DeleteContractUnfinishByContractId(int ContractId)
        {
            var items = _contractUnfinishRepository.Table.Where(c => c.ContractId == ContractId);
            _contractUnfinishRepository.Delete(items);
        }
        #endregion
    }
}
