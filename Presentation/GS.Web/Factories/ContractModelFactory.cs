using System;
using System.Collections.Generic;
using System.Linq;
using GS.Core;
using GS.Core.Caching;
using GS.Services.Directory;
using GS.Services.Localization;
using GS.Web.Infrastructure.Cache;
using GS.Web.Models.Directory;
using GS.Web.Models.Contracts;
using GS.Services.Contracts;
using GS.Web.Framework.Extensions;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Core.Domain.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using GS.Services.Customers;
using GS.Services.Stores;
using GS.Core.Domain.Works;
using GS.Web.Models.Works;
using GS.Services.Works;
using GS.Services;
using GS.Web.Areas.Admin.Models.Customers;
using GS.Services.Media;
using GS.Core.Domain.Media;
using GS.Services.Common;
using GS.Core.Domain.Customers;
using GS.Services.Catalog;
using GS.Core.Domain.Common;
using System.Globalization;

namespace GS.Web.Factories
{
    public class ContractModelFactory : IContractModelFactory
    {
        #region Fields

        private readonly ICountryService _countryService;
        private readonly ILocalizationService _localizationService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IWorkContext _workContext;
        private readonly ICustomerService _customerService;
        private readonly IWorkTaskService _workTaskService;
        private readonly IContractLogService _contractLogService;
        private readonly IContractPaymentService _contractPaymentService;
        private readonly IContractFormService _contractFormService;
        private readonly IContractTypeService _contractTypeService;
        private readonly IContractRelateService _contractRelateService;
        private readonly IContractPeriodService _contractPeriodService;
        private readonly IContractService _contractService;
        private readonly IConstructionService _constructionService;
        private readonly IConstructionTypeService _constructionTypeService;
        private readonly ICurrencyService _currencyService;
        private readonly IProcuringAgencyService _procuringAgencyService;

        private readonly IPictureService _pictureService;
        private readonly MediaSettings _mediaSettings;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly ITaskGroupService _taskGroupService;
        private readonly IUnitService _unitService;
        #endregion
        #region Ctor

        public ContractModelFactory(IProcuringAgencyService procuringAgencyService,
            IContractLogService contractLogService,
            IContractPaymentService contractPaymentService,
            IContractFormService contractFormService,
            IContractTypeService contractTypeService,
            IContractPeriodService contractPeriodService,
            IContractRelateService contractRelateService,
            IContractService contractService,
            ICountryService countryService,
            ILocalizationService localizationService,
            IStateProvinceService stateProvinceService,
            IStaticCacheManager cacheManager,
            IConstructionService constructionService,
            IConstructionTypeService constructionTypeService,
            ICustomerService customerService,
            ICurrencyService currencyService,
            IWorkContext workContext,
            IWorkTaskService workTaskService,
            IPictureService pictureService,
            MediaSettings mediaSettings,
            IGenericAttributeService genericAttributeService,
             IPriceFormatter priceFormatter,
             ITaskGroupService taskGroupService,
             IUnitService unitService)
        {
            this._pictureService = pictureService;
            this._mediaSettings = mediaSettings;
            this._genericAttributeService = genericAttributeService;

            this._procuringAgencyService = procuringAgencyService;
            this._contractLogService = contractLogService;
            this._contractPaymentService = contractPaymentService;
            this._contractFormService = contractFormService;
            this._contractRelateService = contractRelateService;
            this._contractService = contractService;
            this._countryService = countryService;
            this._localizationService = localizationService;
            this._stateProvinceService = stateProvinceService;
            this._cacheManager = cacheManager;
            this._workContext = workContext;
            this._contractTypeService = contractTypeService;
            this._contractPeriodService = contractPeriodService;
            this._constructionService = constructionService;
            this._constructionTypeService = constructionTypeService;
            this._customerService = customerService;
            this._currencyService = currencyService;
            this._workTaskService = workTaskService;
            this._priceFormatter = priceFormatter;
            this._taskGroupService = taskGroupService;
            this._unitService = unitService;
        }

        #endregion
        #region Customer
        string GetCustomerAvatarUrl(Customer customer)
        {
            return _pictureService.GetPictureUrl(
        _genericAttributeService.GetAttribute<int>(customer, GSCustomerDefaults.AvatarPictureIdAttribute),
        _mediaSettings.AvatarPictureSize, defaultPictureType: PictureType.Avatar);
        }
        #endregion

        #region Contract 
        public ContractSearchModel PrepareContractSearchModel(ContractSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            //prepare page parameters
            searchModel.DdlTypeDisplay = ((ContractDisplayType)searchModel.TypeDisplay).ToSelectList();
            searchModel.DdlTypeOrder = ((ContractListOrderType)searchModel.TypeOrder).ToSelectList();
            var Classificationlist = ((ContractClassification)searchModel.ClassificationId).ToSelectList();
            searchModel.DdlClassification = new SelectList(Classificationlist.Where(x => x.Value != "0").ToList(), "Value", "Text");
            searchModel.AvaiableContractStatus = ((ContractStatus)searchModel.ContractStatus).ToSelectList();
            searchModel.AvaiableContractMonitorStatus = ((ContractMonitorStatus)searchModel.contractMonitorStatus).ToSelectList();
            searchModel.DdlprocuringAgency = PrepareprocuringAgencyddl();
            searchModel.DdlConstruction = PrepareContructtionddl();
            var ConstructionTypes = _constructionTypeService.GetAllConstructionTypes();
            searchModel.AvailableConstructionTypeId = ConstructionTypes.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
                Selected = searchModel.ConstructionTypeId == c.Id,
            }).ToList();
            searchModel.AvailableConstructionTypeId.Insert(0, new SelectListItem
            {
                Text = "--Chọn loại công trình--",
                Value = "0",
            });
            var ContractTypes = _contractFormService.GetAllContractForms();
            searchModel.AvaiableContractForms = ContractTypes.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
                Selected = searchModel.contractForm == c.Id,
            }).ToList();
            searchModel.AvaiableContractForms.Insert(0, new SelectListItem
            {
                Text = "--Chọn hình thức hợp đồng--",
                Value = "0",
            });
            return searchModel;
        }
        public ContractListModel PrepareContractListModel(ContractSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            //get items
            var items = _contractService.GetContracts(CustomerId: _workContext.CurrentCustomer.Id, keySearch: searchModel.Keysearch, classificationId: searchModel.classification,
                pageIndex: searchModel.Page, pageSize: searchModel.PageSize, FromDate: searchModel.StartDateFrom,
                ToDate: searchModel.StartDateTo, ContractListOrder: searchModel.TypeOrder, SelectedProcuringAgencyIds: searchModel.SelectedProcuringAgencyIds.ToList(),
                ConstructionTypeId: searchModel.ConstructionTypeId, contractType: searchModel.contractForm,
                contractStatus: searchModel.ContractStatus, signedateFrom: searchModel.SignedateFrom, signedateTo: searchModel.SignedateTo,
                contractMonitorStatus: searchModel.ContractMonitorStatus, SelectedConstructionIds: searchModel.SelectedConstructionIds.ToList(), StartYear: searchModel.StartYear,
                acceptanceFrom: searchModel.AcceptanceDateFrom, acceptanceTo: searchModel.AcceptanceDateTo);
            //prepare list model            
            var model = new ContractListModel();
            model.Data = items.Select(c =>
            {
                var m = c.ToModel<ContractModel>();
                //PrepareContractUnfinish(m);
                m.DisplayType = searchModel.Display;
                var contractMonitor = _contractService.GetContractMonitor(c.Id);
                if (contractMonitor != null)
                {
                    m.contractMonitor = contractMonitor.ToModel<ContractMonitorModel>();
                }
                return m;
            }).ToList();
            model.Total = items.TotalCount;
            model.TotalPage = items.TotalPages;
            return model;
        }
        public List<ContractModel> PrepareRecentlyContracts(ContractSearchModel searchModel)
        {
            //var contractviews = 
            var items = _contractService.GetContracts(CustomerId: _workContext.CurrentCustomer.Id, isGetRecently: true, pageSize: searchModel.PageSize).Select(c =>
                {
                    var m = c.ToModel<ContractModel>();
                    m.DisplayType = searchModel.Display;
                    //PrepareContractResource(m);
                    return m;
                }).ToList();
            return items;
        }
        public ContractModel PrepareContractModel(ContractModel model, Contract item, bool isDetail = false)
        {
            //model.procuringAgencyNameB = new List<string>();

            if (item != null)
            {
                //fill in model values from the entity
                model = item.ToModel<ContractModel>();
                model.contractFormModels = item.ContractForms.Select(c => c.ToModel<ContractFormModel>()).ToList();
                model.SelectedContractFormIds = model.contractFormModels.Select(c => c.Id).ToList();
                model.contractTypeModels = item.ContractTypes.Select(c => PrepareContractTypeModel(item, c)).ToList();
                model.SelectedContractTypeIds = model.contractTypeModels.Select(c => c.Id).ToList();
                model.contractPeriodModels = item.ContractPeriods.Select(c => c.ToModel<ContractPeriodModel>()).ToList();
                model.SelectedContractPeriodIds = model.contractPeriodModels.Select(c => c.Id).ToList();
                model.classificationText = _localizationService.GetLocalizedEnum(item.classification);
                //model.Unfinished1 = _priceFormatter.FormatPrice((decimal)item.Unfinished1.GetValueOrDefault(0), true, item.currency);
                //model.Unfinished2 = _priceFormatter.FormatPrice((decimal)item.Unfinished2.GetValueOrDefault(0), true, item.currency);
                // tong gia tri BB
                var lstBB = _contractService.GetAllContractBBAproval(item.Id);
                if (lstBB.Count > 0)
                {
                    var _totalAmountBB = lstBB.Sum(c => (decimal)c.TotalAmount.GetValueOrDefault(0) * c.currency.Rate);
                    model.totalAmountBB = _priceFormatter.FormatPrice(_totalAmountBB, true, item.currency);
                }
                // tong gia tri lien danh cua hop dong
                var taskContact = _workTaskService.GetAllTaskContact(item.Id);
                var _totalTaskContract = taskContact.Sum(c => (Decimal)c.TotalMoney);
                model.totalAmountContact = _priceFormatter.FormatPrice(_totalTaskContract, true, item.currency);
                var _allTotalAmount = model.TotalAmount + _totalTaskContract;
                model.AllTotalAmountText = _priceFormatter.FormatPrice(_allTotalAmount, true, item.currency);
                foreach (var contractPeriods in model.SelectedPeriodIds)
                {
                    var contractPeriod = _contractPeriodService.GetContractPeriodById(contractPeriods);
                    model.contractPeriodModels.Add(contractPeriod.ToModel<ContractPeriodModel>());
                }
                if (model.classification == ContractClassification.BB)
                {
                    var ContractBBId = _contractService.GetContractByGuid(model.ContractGuid).Id;
                    var ContractBBRelate = _contractRelateService.GetContractRelateByContract2Id(ContractBBId);
                    model.ContractAB = _contractService.GetContractById(ContractBBRelate.Contract1Id).ToModel<ContractModel>();
                }
                if (model.classification == ContractClassification.Appendix)
                {
                    var ContractAppendixId = _contractService.GetContractByGuid(model.ContractGuid).Id;
                    var ContractAppendixIdRelate = _contractRelateService.GetContractRelateByContract2Id(ContractAppendixId);
                    model.ContractAB = _contractService.GetContractById(ContractAppendixIdRelate.Contract1Id).ToModel<ContractModel>();
                }
                //prepare PrepareprocuringAgencyName A , B of Contract
                PrepareprocuringAgencyName(model);
                //model.CompareDate = DateTime.Compare(DateTime.Now, model.EndDate);
                //model.interval = DateTime.Now.Subtract(model.EndDate).Days;
                PrepareContractListUnfinish(model);
                // chuan bi lstContractAcceptanceSub
                PrepareContractUnfinish(model);
                var acceptanceSubs = _contractService.GetallContractAcceptanceSubs(ContractId: model.Id, TypeId: (int)ContractAcceptancesType.NoiBo);
                model.lstContractAcceptanceSub = acceptanceSubs.Select(c => c.ToModel<ContractAcceptanceSubModel>()).ToList();
                if (item.ClassificationId == (int)ContractClassification.Appendix)
                {
                    var contractRelate = _contractService.GetContractRelateByContract2Id(item.Id);
                    var contract = _contractService.GetContractById(contractRelate.Contract1Id);
                    if (contract != null && contract.ClassificationId == (int)ContractClassification.AB)
                    {
                        model.AppendixAB = true;
                    }
                }
            }

            if (!isDetail)
            {
                //luc tao moi hoac sua
                if (model.ContractRelateId > 0)
                {
                    //lay thong tin tu hop dong AB
                    var contract = _contractService.GetContractById(model.ContractRelateId);
                    model.ConstructionId = (int)contract.ConstructionId;
                }
                model.AvailableContractType = _contractTypeService.GetAllContractTypes().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Selected = model.SelectedContractTypeIds.Contains(c.Id)
                }).ToList();
                model.AvailableContractForm = _contractFormService.GetAllContractForms().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Selected = model.SelectedContractFormIds.Contains(c.Id)
                }).ToList();
                model.AvailableContruction = _constructionService.GetAllConstructions().Select(c =>
                {
                    var itemConstruct = new SelectListItem();
                    var constructionType = _constructionTypeService.GetConstructionTypeById(c.TypeId);
                    itemConstruct.Text = string.Format("{0} ({1})", c.Name, constructionType != null ? constructionType.Name : "");
                    itemConstruct.Value = c.Id.ToString();
                    itemConstruct.Selected = c.Id == model.ConstructionId;
                    return itemConstruct;
                }).ToList();
                model.AvailableContractPeriod = _contractPeriodService.GetAllContractPeriods().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Selected = model.SelectedContractPeriodIds.Contains(c.Id)
                }).ToList();

                if (model.classification == ContractClassification.BB && model.TaskIdBB > 0)
                {
                    var task = _workTaskService.GetTaskById(model.TaskIdBB);
                    model.CurrencyId = (int)task.CurrencyId;
                    if (task != null)
                    {
                        model.TotalAmountNumber = (task.TotalMoney * task.TaskGroupValue.GetValueOrDefault(0)).ToVNStringNumber();
                    }
                    model.suggestionCreatBB = "Đang tạo hợp đồng B-B' trên công việc " + task.Name + "(" + task.ToModel<TaskModel>().TotalAmountText + ")" + ", với tỷ lệ khoán là: " + (task.TaskGroupValue).ToPercentString();
                    model.AvailableCurrency = _currencyService.GetAllCurrencies().Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString(),
                        Selected = c.Id == model.CurrencyId
                    }).ToList();
                    model.TotalAmountNumber = task.TotalMoney.ToVNStringNumber();
                }
                else
                {
                    model.AvailableCurrency = _currencyService.GetAllCurrencies().Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString(),
                        Selected = c.Id == model.CurrencyId
                    }).ToList();
                }
                //check tao hop dong phu luc co là phu luc AB 
                if (model.ClassificationId == (int)ContractClassification.Appendix)
                {
                    var contractAB = _contractService.GetContractById(model.ContractRelateId);
                    if (contractAB != null && contractAB.ClassificationId == (int)ContractClassification.AB)
                    {
                        model.AppendixAB = true;
                    }
                }
            }
            else
            {
                //hien thi chi tiet thi load them cac thong tin khac
                PrepareContractFile(model);
                //Customer, role
                PrepareContractResource(model);
                // Prepare ContractRelate cho Detail
                PrepareContractRelate(model);
                // Prepare ContractAppendix cho Detail
                PrepareContractAppendix(model);
                //preoare ContractAcceptance cho detail
                PrepareContractAcceptance(model);
                //prepare ContractSettlement cho detail
                PrepareContractSettlement(model);
                PrepareContractAcceptanceNoiBo(model);
                model.lstContractPaymentPlan = PrepareContractPaymentPlanSearch(new ContractPaymentPlanSearchModel { ContractId = model.Id }).Data.ToList();
                PrepareContractPaymentAcceptanceList(model);
            }
            return model;
        }
        ///gia tri do dang cua hop dong
        public void PrepareContractUnfinish(ContractModel model)
        {
            DateTime Createdate = DateTime.Now;
            if (model.EndDate.Date < DateTime.Now.Date)
            {
                Createdate = model.EndDate;
            }
            var unfinishs = _contractService.getallContractUnfinish(ContractId: model.Id, CreatedDate: Createdate);
            model.Unfinished1 = unfinishs.Where(c =>c.TypeId == 1).Sum(c => c.OptionValue.GetValueOrDefault(0)).ToVNStringNumber();
            model.Unfinished2 = unfinishs.Where(c => c.TypeId == 2).Sum(c => c.OptionValue.GetValueOrDefault(0)).ToVNStringNumber();
            var contractUnfinishNow = _contractService.GetContractUnfinishNow(model.Id);
            model.Unfinished3 = contractUnfinishNow != null ? contractUnfinishNow.Sum(c => c.OptionValue.GetValueOrDefault(0)).ToVNStringNumber() : "";
        }
        public void PrepareContractListUnfinish(ContractModel model)
        {
            DateTime Createdate = DateTime.Now;
            if (model.EndDate.Date < DateTime.Now.Date)
            {
                Createdate = model.EndDate;
            }
            var lstContractUnfinishModel = _contractService.getallContractUnfinish(ContractId: model.Id, CreatedDate: Createdate).Where(c => c.ContractTypeId != 1000 && c.TypeId == 1);
            model.lstContractUnfinishModel = lstContractUnfinishModel.Select(c => c.ToModel<ContractUnfinishModel>()).ToList();
            // list do dang chua thuc hien 
            var _lstContractUnfinish2Model = _contractService.getallContractUnfinish(ContractId: model.Id, CreatedDate: Createdate).Where(c => c.ContractTypeId != 1000 && c.TypeId == 2);
            model.lstContractUnfinish2Model = _lstContractUnfinish2Model.Select(c => c.ToModel<ContractUnfinishModel>()).ToList();
            var lstContractUnfinishModel2 = _contractService.GetContractUnfinishNow(model.Id);
            if (lstContractUnfinishModel2 != null)
                model.lstContractUnfinishModel2 = lstContractUnfinishModel2.Select(c => c.ToModel<ContractUnfinishModel>()).ToList();
        }
        public ContractTypeModel PrepareContractTypeModel(Contract Contract, ContractType contractType)
        {
            var model = contractType.ToModel<ContractTypeModel>();
            var tasks = _workTaskService.getTasksByConTractId(ContractId: Contract.Id, ContractTypeId: model.Id, isAll: false).Where(c => c.TreeLevel == 1);
            model.totalMoneyText = _priceFormatter.FormatPrice(tasks.Sum(c => c.TotalMoney).GetValueOrDefault(0), true, Contract.currency);
            return model;
        }
        /// <summary>
        /// Hàm nay đề chuyển đổi dữ liệu model hợp đồng sang entity
        /// Không sử dụng mapping
        /// </summary>
        /// <param name="item">Hợp đồng đã được khởi tạo</param>
        /// <param name="model">Model hợp đồng cần chyển</param>
        public void PrepareContract(Contract item, ContractModel model)
        {
            item.classification = model.classification;
            item.Name = model.Name;
            item.Code = model.Code;
            item.Code1 = model.Code1;
            item.PaymentInfo = model.PaymentInfo;
            item.Description = model.Description;
            item.PeriodId = model.PeriodId;
            item.SignedDate = model.SignedDate;
            item.StartDate = model.StartDate;
            item.EndDate = model.EndDate;
            item.ConstructionId = model.ConstructionId;
            item.SignedDate = model.SignedDate;
            if (model.ClassificationId == (int)ContractClassification.Appendix)
            {
                var contract = _contractService.GetContractById(model.ContractRelateId);
                if (contract.ClassificationId == (int)ContractClassification.AB)
                {
                    item.CurrencyId = contract.CurrencyId;
                }
                else
                {
                    item.CurrencyId = model.CurrencyId;
                }
            }
            else
            {
                item.CurrencyId = model.CurrencyId;
            }
            item.TotalAmount = model.TotalAmountNumber.ToNumber();
            if (model.classification == ContractClassification.BB)
            {
                var contractRelate = _contractService.GetContractById(model.ContractRelateId);
                if (contractRelate != null)
                {
                    item.PeriodId = contractRelate.PeriodId;
                    item.ConstructionId = contractRelate.ConstructionId;
                    model.SelectedContractFormIds = contractRelate.ContractForms.Select(c => c.Id).ToList();
                    model.SelectedContractTypeIds = contractRelate.ContractTypes.Select(c => c.Id).ToList();
                    model.SelectedContractPeriodIds = contractRelate.ContractPeriods.Select(c => c.Id).ToList();
                }
            }

        }
        public ContractAppendixModel PrepareIndexContractAppendix(Contract contract, ContractAppendixModel model)
        {
            var lstTaskContact = _workTaskService.GetListTaskContractByContractId(contract.Id);
            var contractABId = _contractRelateService.GetContractRelateByContract2Id(contract.Id);
            var contractAB = _contractService.GetContractById(contractABId.Contract1Id);
            model.ContractGuidAB = contractAB.ContractGuid;
            var lstTask = _workTaskService.GetTaskbyListTaskIds(lstTaskContact.Select(c => c.TaskId).ToList());
            model.taskNew = lstTask.Select(c => c.ToModel<TaskModel>()).ToList();
            var lstNote = lstTaskContact.Select(m => m.Note).ToList();
            foreach (var note in lstNote)
            {
                if (note != "")
                {
                    var _note = note.toEntity<TaskNoteModel>();
                    model.taskContractNote.Add(_note);
                }
            }
            model.ContractAppendix = contract.ToModel<ContractModel>();
            return model;
        }
        public AppendixEditPaymenPlanModel PrepareIndexAppendixEditPaymentPlan(Contract contract, AppendixEditPaymenPlanModel model)
        {
            var lstPaymentPlanContract = _contractService.GetPaymentPlanContractsByContractId(contract.Id);
            // lay contractGuidAB tu contractAppendixId
            var contractABId = _contractRelateService.GetContractRelateByContract2Id(contract.Id);
            var contractAB = _contractService.GetContractById(contractABId.Contract1Id);
            model.ContractGuidAB = contractAB.ContractGuid;
            var lstPaymentPlanid = lstPaymentPlanContract.Select(c => c.PaymentPlanId).ToList();
            var lstContractPaymentPlan = _contractService.GetAllContractPaymentPlanByPaymentPlanIds(lstPaymentPlanid);
            model.contractPaymentPlanNews = lstContractPaymentPlan.Select(c => c.ToModel<ContractPaymentPlanModel>()).ToList();
            var lstNote = lstPaymentPlanContract.Select(m => m.Note).ToList();
            foreach (var note in lstNote)
            {
                if (note != "")
                {
                    var _note = note.toEntity<PaymentPlanNoteModel>();
                    model.AppendixEditPaymentPlanNote.Add(_note);
                }
            }
            model.ContractAppendix = contract.ToModel<ContractModel>();
            return model;
        }
        /// <summary>
        /// danh sach hop dong BB
        /// </summary>
        /// <param name="model"></param>
        public void PrepareContractRelate(ContractModel model)
        {
            var items = _contractService.GetContractRelates(model.Id, ContractClassification.BB);
            model.contractRelateModels = items.Select(c =>
              {
                  var m = c.ToModel<ContractModel>();
                  return m;
              }).ToList();
            model.classificationTextContractRelate = _localizationService.GetLocalizedEnum(ContractClassification.BB);

        }
        /// <summary>
        /// danh sach hop dong phu luc
        /// </summary>
        /// <param name="model"></param>
        public void PrepareContractAppendix(ContractModel model)
        {
            var items = _contractService.GetContractRelates(model.Id, ContractClassification.Appendix);
            model.contractAppendixModels = items.Select(c =>
            {
                var m = c.ToModel<ContractModel>();
                return m;
            }).ToList();
            model.classificationTextContractAppendix = _localizationService.GetLocalizedEnum(ContractClassification.Appendix);
        }
        /// <summary>
        /// chuẩn bị danh sách bên A, bên B của hợp đồng
        /// </summary>
        /// <param name="model"></param>

        public void PrepareprocuringAgencyName(ContractModel model)
        {
            var datas = _contractService.GetContractJointsByContractId(model.Id);
            model.contractJoinModels = datas.Select(c =>
             {
                 var m = c.ToModel<ContractJointModel>();
                 if (string.IsNullOrEmpty(m.ProcuringAgencyData))
                     m.procuringAgency = c.curProcuringAgency.ToModel<ProcuringAgencyModel>();
                 else
                     m.procuringAgency = m.ProcuringAgencyData.toEntity<ProcuringAgencyModel>();
                 return m;
             }).ToList();
            model.procuringAgencyModelSideA = model.contractJoinModels.Where(c => c.IsSideA).Select(p => p.procuringAgency).FirstOrDefault();
            model.procuringAgencyModelSideB = model.contractJoinModels.Where(c => !c.IsSideA).Select(p => p.procuringAgency).ToList();
        }
        public List<SelectListItem> PrepareprocuringAgencyddl()
        {
            var datas = _procuringAgencyService.GetAllProcuringAgencys();
            return datas.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
        }
        public List<SelectListItem> PrepareContructtionddl()
        {
            var datas = _constructionService.GetAllConstructions();
            return datas.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
        }
        public void PrepareContractAcceptance(ContractModel model)
        {
            var pmacs = _contractService.getAllContractAcceptance(ContractId: model.Id, TypeId: (int)ContractAcceptancesType.KhoiLuong);
            model.lstContractAcceptance = pmacs.Select(c => PrepareTotalAcceptanceKL(c)).ToList();
        }
        public void PrepareContractAcceptanceNoiBo(ContractModel model)
        {
            var pmacs = _contractService.getAllContractAcceptance(ContractId: model.Id, TypeId: (int)ContractAcceptancesType.NoiBo);
            model.lstContractAcceptanceNoiBo = pmacs.Select(c => c.ToModel<ContractAcceptanceModel>()).ToList();
        }
        public void PrepareContractAcceptanceSub(ContractModel model)
        {
            var acceptanceSubs = _contractService.GetallContractAcceptanceSubs(ContractId: model.Id);
            model.lstContractAcceptanceSub = acceptanceSubs.Select(c => c.ToModel<ContractAcceptanceSubModel>()).ToList();
        }
        public void PrepareContractPaymentAcceptanceList(ContractModel model)
        {
            var ContractPmacs = _contractService.getAllContractPaymentAcceptance(model.Id).ToList();
            model.lstContractPaymentAcceptance = ContractPmacs.Select(c => c.ToModel<ContractPaymentAcceptanceModel>()).ToList();
        }
        public bool CheckCodeP4Contract(int Id, string Code)
        {
            var contract = _contractService.GetContractByCode1(Code);
            if (contract == null)
                return true;
            if (contract.Id == Id)
                return true;
            return false;
        }
        public bool CheckEndDate(ContractModel model)
        {
            if ((model.EndDate == null) || (model.StartDate == null))
                return true;
            if (model.EndDate < model.StartDate)
                return false;
            return true;
        }
        public ContractModel PrepareDashBoardThongKe(Contract item, ContractModel model, int year)
        {
            if (model == null)
            {
                model = new ContractModel();
                model = item.ToModel<ContractModel>();
            }
            //prepare Total NTKL cua hop dong
            var lstContractAcceptanceId = _contractService.getAllContractAcceptance(ContractId: item.Id, TypeId: (int)ContractAcceptancesType.KhoiLuong).Where(c => Convert.ToDateTime(c.ApprovalDate).Year == year).Select(c => c.Id).ToList();
            var lstContractAcceptanceSub = _contractService.GetAllContractAcceptanceSubByListAcceptanceId(lstContractAcceptanceId);
            var totalContractAcceptanceKL = lstContractAcceptanceSub.Sum(c => c.TotalAmount);
            model.TotalContractAcceptanceKLText = _priceFormatter.FormatPrice((Decimal)totalContractAcceptanceKL, true, item.currency);
            // List payment
            var _lstPayment = _contractService.GetAllContractPaymentByContractId(item.Id);
            //prepare tong da tam ung cua hop dong
            var _lstPaymentAdvanceInYear = _lstPayment.Where(c => c.IsReceipts == true
                                        && c.TypeId == (int)ContractPaymentType.Advance
                                        && Convert.ToDateTime(c.PaymentDate).Year == year).ToList();
            var _totalPaymentAdvance = _lstPaymentAdvanceInYear.Sum(c => c.Amount * c.CurrencyRatio);
            model.TotalPaymentAdvanceContractText = _priceFormatter.FormatPrice(_totalPaymentAdvance, true, item.currency);
            // prepare tong da thanh toan cua hop dong 
            var _lstPaymentpaidInYear = _lstPayment.Where(c => c.IsReceipts == true
                                        && c.TypeId == (int)ContractPaymentType.Payment
                                        && Convert.ToDateTime(c.PaymentDate).Year == year).ToList();
            var _totalPaymentPaid = _lstPaymentpaidInYear.Sum(c => c.Amount * c.CurrencyRatio);
            model.TotalPaymentPaidContractText = _priceFormatter.FormatPrice(_totalPaymentPaid, true, item.currency);
            return model;
        }
        #endregion
        #region  ContractJoint
        public void PrepareContractJoint(ContractJoint item, ContractJointModel model)
        {

            var procuringAgency = _procuringAgencyService.GetProcuringAgencyById(model.ProcuringAgencyId);
            item.ContractId = model.ContractId;
            item.ProcuringAgencyId = model.ProcuringAgencyId;
            item.ProcuringAgencyData = procuringAgency.ToModel<ProcuringAgencyModel>().toStringJson();
            //var m = item.ProcuringAgencyData.toEntity<ProcuringAgencyModel>();

            item.IsSideA = model.IsSideA;
            item.DisplayOrder = model.DisplayOrder;
            item.IsMain = model.IsMain;
        }
        #endregion
        #region Contract Form
        public ContractFormSearchModel PrepareContractFormSearchModel(ContractFormSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        public ContractFormListModel PrepareContractFormListModel(ContractFormSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get items
            var items = _contractFormService.GetAllContractFormsByName(searchModel.Name);

            //prepare list model
            var model = new ContractFormListModel
            {
                //fill in model values from the entity
                Data = items.PaginationByRequestModel(searchModel).Select(store => store.ToModel<ContractFormModel>()),
                Total = items.Count
            };

            return model;
        }

        public ContractFormModel PrepareContractFormModel(ContractFormModel model, ContractForm item, bool excludeProperties = false)
        {
            if (item != null)
            {
                //fill in model values from the entity
                model = model ?? item.ToModel<ContractFormModel>();
            }
            return model;
        }
        #endregion

        #region Contract Type
        public ContractTypeSearchModel PrepareContractTypeSearchModel(ContractTypeSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        public ContractTypeListModel PrepareContractTypeListModel(ContractTypeSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get items
            var items = _contractTypeService.GetAllContractTypeByName(searchModel.Name);

            //prepare list model
            var model = new ContractTypeListModel
            {
                //fill in model values from the entity
                Data = items.PaginationByRequestModel(searchModel).Select(store => store.ToModel<ContractTypeModel>()),
                Total = items.Count
            };

            return model;
        }

        public ContractTypeModel PrepareContractTypeModel(ContractTypeModel model, ContractType item, bool excludeProperties = false)
        {
            if (item != null)
            {
                //fill in model values from the entity
                model = model ?? item.ToModel<ContractTypeModel>();
            }
            return model;
        }
        #endregion

        #region Contract Period
        public ContractPeriodSearchModel PrepareContractPeriodSearchModel(ContractPeriodSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        public ContractPeriodListModel PrepareContractPeriodListModel(ContractPeriodSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get items
            var items = _contractPeriodService.GetAllContractPeriodsByName(searchModel.Name);

            //prepare list model
            var model = new ContractPeriodListModel
            {
                //fill in model values from the entity
                Data = items.PaginationByRequestModel(searchModel).Select(store => store.ToModel<ContractPeriodModel>()),
                Total = items.Count
            };

            return model;
        }

        public ContractPeriodModel PrepareContractPeriodModel(ContractPeriodModel model, ContractPeriod item, bool excludeProperties = false)
        {
            if (item != null)
            {
                //fill in model values from the entity
                model = model ?? item.ToModel<ContractPeriodModel>();
            }
            return model;
        }
        #endregion

        #region  Contract files
        /// <summary>
        /// Chuẩn bị danh sách file cho hợp đồng
        /// </summary>
        /// <param name="model"></param>
        public void PrepareContractFile(ContractModel model)
        {
            if (model.Id == 0)
                return;
            var workFiles = _contractService.GetContractFiles(model.Id);
            model.workFileModels = workFiles.Select(c => c.ToModel<WorkFileModel>()).ToList();
            //model.workFileIds= string.Join(",", workFiles.Select(c => c.Id).ToArray());
        }
        public void PrepareContractResource(ContractModel model)
        {
            if (model.Id == 0)
            {
                var contract = _contractService.GetContractByGuid(model.ContractGuid);
                if (contract == null)
                    return;
                model.Id = contract.Id;
            }

            var contractResources = _contractService.GetContractResources(model.Id);
            //lay danh sach nguoi dung tren hop dong
            model.contractResourceModels = contractResources.GroupBy(c => c.CustomerId)
  .Select(group => group.First()).Select(c =>
            {
                var m = new ContractResourceModel
                {
                    ContractId = c.ContractId,
                    CustomerId = c.CustomerId.GetValueOrDefault(0),
                    CustomerFullName = c.customer != null ? _customerService.GetCustomerFullName(c.customer) : "",
                    CustomerAvatarUrl = c.customer != null ? GetCustomerAvatarUrl(c.customer) : "",
                    UnitId = c.UnitId,
                    UnitName = c.unitInfo != null ? c.unitInfo.Name : "",
                    GroupId = c.GroupId,
                    GroupName = c.groupRole != null ? c.groupRole.Name : ""
                };
                return m;
            }).Distinct().ToList();
            //tao thong tin role id cho resource model
            //1 user co the co nhieu role 
            foreach (var m in model.contractResourceModels)
            {
                m.RoleModels = contractResources.Where(c => c.ContractId == m.ContractId
                    && c.CustomerId == m.CustomerId).Select(x => x.contractRole.ToModel<CustomerRoleModel>()).ToList();
                m.RoleIds = m.RoleModels.Select(c => c.Id).ToArray();
            }
        }
        #endregion

        #region ContractLog
        public ContractLogSearchModel PrepareContractLogSearchModel(ContractLogSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        public ContractLogListModel PrepareContractLogListModel(ContractLogSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get items
            var items = _contractLogService.GetContractLogByContractIdorName(searchModel.ContractId, searchModel.Note, searchModel.Page - 1, searchModel.PageSize);

            //prepare list model
            var model = new ContractLogListModel
            {
                //fill in model values from the entity
                Data = items.Select(c =>
                {
                    var md = c.ToModel<ContractLogModel>();
                    md.UserFullName = _customerService.GetCustomerFullName(c.creator);
                    return md;
                }).ToList(),
                Total = items.TotalCount
            };
            return model;
        }

        public ContractLogModel PrepareContractLogModel(ContractLogModel model, ContractLog item, bool excludeProperties = false)
        {
            if (item != null)
            {
                //fill in model values from the entity
                model = model ?? item.ToModel<ContractLogModel>();
            }
            return model;
        }
        public ContractLog PrepareContractLog(Contract contract)
        {
            var _contractData = _contractService.GetContractById(contract.Id);
            var contractlog = new ContractLog();
            contractlog.ContractId = contract.Id;
            contractlog.CreatedDate = DateTime.Now;
            contractlog.CreatorId = _workContext.CurrentCustomer.Id;
            contractlog.ContractData = _contractData.ToModel<ContractModel>().toStringJson();
            //contractlog.PeriodId = contract.PeriodId;
            return contractlog;
        }
        public void PrepareContractLogActivity(ContractModel model)
        {
            var datasLog = _contractLogService.GetAllContractLogsByContractId(model.Id);
            foreach (var item in datasLog)
            {
                var dataLogModel = item.ToModel<ContractLogModel>();
                dataLogModel.UserName = item.creator.Username;
                dataLogModel.CustomerAvatar = GetCustomerAvatarUrl(item.creator);
                model.contractlogModels.Add(dataLogModel);
            }
        }
        #endregion
        #region Contract Relate
        public ContractRelateSearchModel PrepareContractRelateSearchModel(ContractRelateSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        public ContractRelateListModel PrepareContractRelateListModel(ContractRelateSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get items
            var items = _contractRelateService.GetAllContractRelates();

            //prepare list model
            var model = new ContractRelateListModel
            {
                //fill in model values from the entity
                Data = items.PaginationByRequestModel(searchModel).Select(store => store.ToModel<ContractRelateModel>()),
                Total = items.Count
            };

            return model;
        }

        public ContractRelateModel PrepareContractRelateModel(ContractRelateModel model, ContractRelate item, bool excludeProperties = false)
        {
            if (item != null)
            {
                //fill in model values from the entity
                model = model ?? item.ToModel<ContractRelateModel>();
            }
            return model;
        }
        public ContractRelate PrepareContractRelate(Contract contract, int ContractRelateId)
        {
            var contractRelate = new ContractRelate();
            contractRelate.Contract1Id = ContractRelateId;//Id hop dong AB
            contractRelate.Contract2Id = contract.Id;
            contractRelate.CreatedDate = DateTime.Now;
            contractRelate.DisplayOrder = 1;
            return contractRelate;
        }
        //public void PrepareContractLogActivity(ContractModel model)
        //{
        //    var datasLog = _contractLogService.GetAllContractLogsByContractId(model.Id);
        //    foreach (var item in datasLog)
        //    {
        //        var dataLogModel = item.ToModel<ContractLogModel>();
        //        dataLogModel.UserName = item.creator.Username;
        //        dataLogModel.CustomerAvatar = GetCustomerAvatarUrl(item.creator);
        //        model.contractlogModels.Add(dataLogModel);
        //    }
        //}
        #endregion
        #region Contract Payment

        public ContractPaymentModel PrepareContractPaymentModel(ContractPaymentModel model, ContractPayment item, bool excludeProperties = false)
        {
            if (item != null)
            {
                //fill in model values from the entity
                model = item.ToModel<ContractPaymentModel>();
                if (item.AcceptanceId == null)
                {
                    model.AcceptanceId = 0;
                }
                if (item.PaymentRequestId == null)
                {
                    model.PaymentRequestId = 0;
                }
                if (item.PaymentRequestId > 0)
                {
                    var paymentRequest = _contractService.GetContractPaymentRequestById((int)item.PaymentRequestId);
                    model.PaymentPlanId = paymentRequest.PaymentPlanId;
                }
            }
            else
            {
                var acceptance = _contractService.getContractAcceptancebyId(model.AcceptanceId.GetValueOrDefault(0));
                if (acceptance != null)
                {
                    model.Amount = ((Int64)(acceptance.TotalAmount.GetValueOrDefault(0) * acceptance.Ratio.GetValueOrDefault(0))).ToString();
                    model.CurrencyId = acceptance.CurrencyId.GetValueOrDefault(0);
                    if (acceptance.currency != null)
                        model.CurrencyName = acceptance.currency.Name;
                }
                var Task = _workTaskService.GetTaskById(model.TaskId);
                if (Task != null && Task.currency != null)
                {
                    model.CurrencyId = Task.CurrencyId.GetValueOrDefault(0);
                    model.CurrencyName = Task.currency.Name;
                    model.TaskName = Task.Name;
                }
                if (model.TypeId == (int)ContractPaymentType.Payment && model.TaskId > 0)
                {
                    var payments = _contractPaymentService.GetAllContractsPayment(TaskId: model.TaskId, IsReceipts: 2);
                    var acceptancesubs = _contractService.GetallContractAcceptanceSubs(TaskId: model.TaskId);
                    model.ContractAcceptanced = _priceFormatter.FormatPrice((Int64)acceptancesubs.Sum(c => c.TotalAmount.GetValueOrDefault(0)), true, Task.currency);
                    model.ContractPaymented = _priceFormatter.FormatPrice((Int64)payments.Sum(c => c.Amount), true, Task.currency);
                    var SumRecieve = payments.Sum(c => c.Amount);
                }

            }
            var contract = _contractService.GetContractById(model.ContractId);
            //model.CurrencyId = (int)contract.CurrencyId;
            if (item == null && model.PaymentRequestId > 0)
            {
                var paymentRequest = _contractService.GetContractPaymentRequestById((int)model.PaymentRequestId);
                var contractAcceptance = _contractService.getContractPaymentAcceptanceByPaymentRequestId(paymentRequest.Id);
                model.PaymentPlanId = paymentRequest.PaymentPlanId;
                if (model.TypeId == (int)ContractPaymentType.Advance)
                {
                    model.CurrencyId = contract.currency.Id;
                }
                else
                {
                    model.CurrencyId = (int)contractAcceptance.CurrencyId;
                }
            }

            model.lstType = ((ContractPaymentType)model.TypeId).ToSelectList();

            return model;
        }
        public ContractPaymentModel PrepareContractPaymentBBModel(ContractPaymentModel model, ContractPayment item)
        {
            if (item != null)
            {
                //fill in model values from the entity
                model = item.ToModel<ContractPaymentModel>();
            }
            else
            {
                var Task = _workTaskService.GetTaskById(model.TaskId);
                if (Task != null)
                {
                    model.TaskName = Task.Name;
                }
                //if (model.TypeId == (int)ContractPaymentType.Payment && model.TaskId > 0)
                //{
                //    var payments = _contractPaymentService.GetAllContractsPayment(TaskId: model.TaskId, IsReceipts: 2);
                //    var acceptancesubs = _contractService.GetallContractAcceptanceSubs(TaskId: model.TaskId);
                //    model.ContractAcceptanced = _priceFormatter.FormatPrice((Int64)acceptancesubs.Sum(c => c.TotalAmount.GetValueOrDefault(0)), true, Task.currency);
                //    model.ContractPaymented = _priceFormatter.FormatPrice((Int64)payments.Sum(c => c.Amount), true, Task.currency);
                //    var SumRecieve = payments.Sum(c => c.Amount);
                //}
            }

            var contract = _contractService.GetContractById(model.ContractId);
            //model.CurrencyId = (int)contract.CurrencyId;           
            model.lstType = ((ContractPaymentType)model.TypeId).ToSelectList();
            return model;
        }
        public ContractPaymentSearchModel PrepareContractPaymentSearchModel(ContractPaymentSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            //prepare page parameters
            searchModel.SetGridPageSize();
            var Tasks = _workTaskService.getTasksByConTractId(ContractId: searchModel.ContractId);
            searchModel.AvailableTasks = Tasks.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
            searchModel.lsType = ((ContractPaymentType)searchModel.TypeId).ToSelectList();
            return searchModel;
        }
        public ContractPaymentListModel PrepareContractPaymentListModel(ContractPaymentSearchModel searchModel)
        {
            var items = _contractPaymentService.GetContractPayments(ContractId: searchModel.ContractId, IsReceipts: searchModel.IsReceipts, TypeId: searchModel.TypeId, SelectedListTaskIds: searchModel.SelectListTaskId.ToList(), KeySearch: searchModel.Keysearch, pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);
            var model = new ContractPaymentListModel
            {
                Data = items.Select(c =>
                {
                    var m = c.ToModel<ContractPaymentModel>();
                    m.TypeText = _localizationService.GetLocalizedEnum(c.paymentType);
                    return m;
                }
                ),
                Total = items.TotalCount
            };
            return model;
        }
        public void PrepareContractPayment(ContractPayment item, ContractPaymentModel model)
        {
            if (model.Id == 0)
            {
                item.CreatorId = _workContext.CurrentCustomer.Id;
                item.CreatedDate = DateTime.Now;
            }
            if (model.TaskId > 0)
                item.TaskId = model.TaskId;
            item.Code = model.Code;
            item.Description = model.Description;
            item.ContractId = model.ContractId;
            item.Amount = model.Amount.ToNumber();
            item.CurrencyId = model.CurrencyId;
            item.CurrencyRatio = _currencyService.GetCurrencyById(model.CurrencyId).Rate;
            item.TypeId = model.TypeId;
            item.paymentStatus = ContractPaymentStatus.Approved;
            item.UpdatedDate = DateTime.Now;
            item.PaymentDate = model.PaymentDate;
            item.IsReceipts = model.IsReceipts;
            if (model.PaymentRequestId > 0)
            {
                item.PaymentRequestId = model.PaymentRequestId;
            }
            if (model.AcceptanceId > 0)
            {
                item.AcceptanceId = model.AcceptanceId;
            }

        }

        public void PrepareContractPaymentBB(ContractPayment item, ContractPaymentModel model)
        {
            if (model.Id == 0)
            {
                item.CreatorId = _workContext.CurrentCustomer.Id;
                item.CreatedDate = DateTime.Now;
            }
            if (model.TaskId > 0)
                item.TaskId = model.TaskId;
            var task = _workTaskService.GetTaskById(model.TaskId);
            if (task != null)
            {
                item.ContractIdBB = task.TaskContracts.Where(c => c.contract.ClassificationId == (int)ContractClassification.BB && c.contract.StatusId != (int)ContractStatus.Destroy).FirstOrDefault().ContractId;
                item.CurrencyId = task.CurrencyId.GetValueOrDefault();
            }
            item.Code = model.Code;
            item.Description = model.Description;
            item.ContractId = model.ContractId;
            item.Amount = model.Amount.ToNumber();
            item.TypeId = model.TypeId;
            item.paymentStatus = ContractPaymentStatus.Approved;
            item.UpdatedDate = DateTime.Now;
            item.PaymentDate = model.PaymentDate;
            item.IsReceipts = false;
        }
        #endregion
        #region contractresource
        public void prepareContractResourceModel(ContractResourceModel model)
        {
            if (model.CustomerId > 0)
            {
                var customer = _customerService.GetCustomerById(model.CustomerId);
                model.CustomerFullName = _customerService.GetCustomerFullName(customer);
            }
            if (model.CustomerId > 0)
            {
                var lstContractResoure = _contractService.GetContractResources(ContractId: model.ContractId, CustomerId: model.CustomerId);
                model.SelectedCustomerRoleIds = lstContractResoure.Select(c => c.RoleId).ToList();
            }
            var customerroles = _customerService.GetCustomerRolesByPrefix("GS");
            model.RoleModels = customerroles.Select(c => c.ToModel<CustomerRoleModel>()).ToList();
            model.AvailbleRoles = customerroles.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
                Selected = model.SelectedCustomerRoleIds.Contains(c.Id)
            }).ToList();
        }
        #endregion
        #region ContractPaymentPlan
        //public void prepareContractPaymentPlanModel(ContractPaymentPlanModel model, ContractPaymentPlan item = null)
        //{
        //    var tasks = _workTaskService.getTasksByConTractId(ContractId: model.ContractId);
        //    var workTask = _workTaskService.GetAllTasks();
        //    model.SelectedTaskIds = workTask.Select(c => c.Id).ToList();
        //    model.Availbletasks = tasks.Select(c => new SelectListItem
        //    {
        //        Value = c.Id.ToString(),
        //        Text = c.Name,
        //        Selected = model.SelectedTaskIds.Contains(c.Id)
        //    }).ToList();
        //    if (model.Id != 0)
        //    {
        //        var contractPaymentPlan = _contractService.GetContractPaymentPlanById(model.Id);
        //        var TaskId = contractPaymentPlan.PayRule;
        //        if (TaskId != null)
        //        {
        //            var lst = TaskId.toEntity<ContractPaymentPlanRule>();
        //            model.SelectedTaskIds = lst.lstTaskId;
        //        }
        //    }


        //}
        public ContractPaymentPlanListModel PrepareContractPaymentPlanSearch(ContractPaymentPlanSearchModel searchModel)
        {
            var items = _contractService.GetAllContractPaymentPlans(searchModel.ContractId);
            var model = new ContractPaymentPlanListModel
            {
                Data = items.Select(c =>
                {
                    var m = c.ToModel<ContractPaymentPlanModel>();
                    m.RequestCount = _contractService.GetCountOfRequest(c.ContractId, c.Id);
                    m.lstPaymentPlanContract = _contractService.GetPaymentPlanContractsByPaymentPlanId(c.Id);
                    return m;
                }).ToList(),
                Total = items.Count
            };
            return model;
        }
        public void prepareContractPaymentPlan(ContractPaymentPlanModel model, ContractPaymentPlan item)
        {
            if (model.Id == 0)
            {
                item.CreatorId = _workContext.CurrentCustomer.Id;
                item.CreatedDate = DateTime.Now;
            }
            item.TypeId = model.TypeId;
            item.ContractId = model.ContractId;
            item.PayDate = model.PayDate;
            item.PayRule = model.PayRule;
            item.Description = model.Description;
            item.Amount = model.AmountAdvance.ToNumber();
            item.PercentNum = (model.PercentNum / 100);
            item.PayRule = model.PayRule;
            item.AmountSummary = model.AmountSummary;
        }
        public void prepareContractPaymentPlanModel(ContractPaymentPlanModel model, ContractPaymentPlan item = null)
        {
            var tasks = _workTaskService.getTasksByConTractId(ContractId: model.ContractId);
            var contract = _contractService.GetContractById(model.ContractId);
            model.totalAmountContract = (Int64)contract.TotalAmount;
            model.PayDate = contract.EndDate;
            if (model.Id != 0)
            {
                var contractPaymentPlan = _contractService.GetContractPaymentPlanById(model.Id);
                var TaskId = contractPaymentPlan.PayRule;
                if (TaskId != null)
                {
                    var lst = TaskId.toEntity<ContractPaymentPlanRule>();
                    model.SelectedTaskIds = lst.lstTaskId;
                }
                model.PercentNum = (contractPaymentPlan.PercentNum * 100);
            }
            model.Availbletasks = tasks.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = model.SelectedTaskIds.Contains(c.Id)
            }).ToList();

        }
        #endregion
        #region ContractAcceptance
        public ContractAcceptanceModel PrepareContractAcceptanceKhoiLuong(ContractAcceptanceModel model, ContractAcceptance item = null)
        {
            if (item.Id > 0)
            {
                model = item.ToModel<ContractAcceptanceModel>();
                //var taskIds = _workTaskService.getallTaskHasContractAcceptance(model.ContractId, model.Id).ToList();
                //if (taskIds != null)
                //{
                //    model.SelectedListTaskId = taskIds.Select(c => c.Id).ToList();
                //}
                var subs = _contractService.GetallContractAcceptanceSubs(AcceptanceId: model.Id, TypeId: (int)ContractAcceptancesType.KhoiLuong);
                model.SelectedListTaskId = subs.Select(c => (int)c.TaskId).ToList();
                model.listAcceptanceSub = subs.Select(c => PrepareContractAcceptanceKhoiLuongSubModel(null, c, item.Id)).ToList();
            }
            //cac nghiem thu hop dong da thuc hien
            var contractAcceptances = _contractService.getAllContractAcceptance(ContractId: model.ContractId, TypeId: (int)ContractAcceptancesType.KhoiLuong);

            //tru nghiem thu khoi luong hien tai trong truong hop edit
            if (item.Id > 0)
            {
                contractAcceptances = contractAcceptances.Where(c => c.Id != item.Id).ToList();
            }
            //tim kiem danh sach mapping
            //var Accepmappings = _contractService.GetContractAcceptanceTaskMappingByListAcceptanceId(contractAcceptances.Select(c => c.Id).ToList());
            //danh sach may da chon 
            var tasks = _workTaskService.getTasksByConTractId(ContractId: model.ContractId).Where(c => c.TaskProcuringAgencyId == null);
            var lstAccepted = new List<int>();
            if (model.Id > 0)
            {
                lstAccepted = _contractService.GetallContractAcceptanceSubs(ContractId: model.ContractId, TypeId: (int)ContractAcceptancesType.KhoiLuong).Where(c => c.AcceptanceId != model.Id && c.task.StatusId == (int)TaskStatus.Acceptance).Select(c => c.TaskId.GetValueOrDefault()).ToList();
            }
            else
            {
                lstAccepted = tasks.Where(c => c.StatusId == (int)TaskStatus.Acceptance).Select(c => c.Id).ToList();
            }
            tasks = tasks.Where(c => !lstAccepted.Contains(c.Id));
            model.AvailableTaskList = tasks.Select(
            c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
            }).ToList();
            var Task = _workTaskService.GetTaskById(model.TaskId.GetValueOrDefault(0));
            var existsCPAS = _contractService.GetallPaymentAcceptanceSubs(AcceptanceId: item.Id);
            if (existsCPAS != null && existsCPAS.Count() > 0 && item.TypeId == (int)ContractAcceptancesType.KhoiLuong)
            {
                model.hasPaymentAcceptance = true;
            }
            return model;
        }
        public ContractAcceptanceModel PrepareContractAcceptanceNoiBo(ContractAcceptanceModel model, ContractAcceptance item = null)
        {
            if (item.Id > 0)
            {
                model = item.ToModel<ContractAcceptanceModel>();
                var taskIds = _workTaskService.getallTaskHasContractAcceptance(model.ContractId, model.Id).ToList();
                if (taskIds != null)
                {
                    model.SelectedListTaskId = taskIds.Select(c => c.Id).ToList();
                }
            }
            //cac nghiem thu hop dong da thuc hien
            var contractAcceptances = _contractService.getAllContractAcceptance(ContractId: model.ContractId, TypeId: (int)ContractAcceptancesType.KhoiLuong);
            //tru nghiem thu khoi luong hien tai
            if (item.Id > 0)
            {
                contractAcceptances.Where(c => c.Id != item.Id);
            }
            //tim kiem danh sach mapping
            var Accepmappings = _contractService.GetContractAcceptanceTaskMappingByListAcceptanceId(contractAcceptances.Select(c => c.Id).ToList());
            //danh sach may da chon 
            var taskIdKhoiLuong = Accepmappings.Select(c => c.TaskId).Distinct().ToList();
            var tasks = _workTaskService.getTasksByConTractId(ContractId: model.ContractId);
            model.AvailableTaskList = tasks.Where(c => !taskIdKhoiLuong.Contains(c.Id)).Select(
            c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
            }).ToList();
            var Task = _workTaskService.GetTaskById(model.TaskId.GetValueOrDefault(0));
            return model;
        }
        public ContractAcceptanceModel PrepareContractAcceptanceBB(ContractAcceptanceModel model, ContractAcceptance item = null)
        {
            if (item.Id > 0)
            {
                model = item.ToModel<ContractAcceptanceModel>();
                var taskIds = _workTaskService.getallTaskHasContractAcceptance(model.ContractId, model.Id).ToList();
                if (taskIds != null)
                {
                    model.SelectedListTaskId = taskIds.Select(c => c.Id).ToList();
                }
            }
            //cac nghiem thu hop dong da thuc hien
            var contractAcceptances = _contractService.getAllContractAcceptance(ContractId: model.ContractId, TypeId: (int)ContractAcceptancesType.KhoiLuong);
            //tru nghiem thu khoi luong hien tai
            if (item.Id > 0)
            {
                contractAcceptances.Where(c => c.Id != item.Id);
            }
            //tim kiem danh sach mapping
            var Accepmappings = _contractService.GetContractAcceptanceTaskMappingByListAcceptanceId(contractAcceptances.Select(c => c.Id).ToList());
            //danh sach may da chon 
            var taskIdKhoiLuong = Accepmappings.Select(c => c.TaskId).Distinct().ToList();
            var tasks = _workTaskService.getTasksByConTractId(ContractId: model.ContractId);
            model.AvailableTaskList = tasks.Where(c => !taskIdKhoiLuong.Contains(c.Id)).Select(
            c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
            }).ToList();
            var Task = _workTaskService.GetTaskById(model.TaskId.GetValueOrDefault(0));
            return model;
        }
        public ContractAcceptanceModel PrepareTotalAcceptanceKL(ContractAcceptance item)
        {
            var model = item.ToModel<ContractAcceptanceModel>();
            var lstContractAcceptanceSub = _contractService.GetallContractAcceptanceSubs(AcceptanceId: item.Id);
            model.TotalAmountAcceptance = lstContractAcceptanceSub.Sum(c => c.TotalAmount.GetValueOrDefault(0));
            return model;
        }
        public ContractAcceptanceModel PrepareContractAcceptanceModel(ContractAcceptanceModel model, ContractAcceptance item = null, bool isDetail = false)
        {
            if (item != null)
            {
                model = item.ToModel<ContractAcceptanceModel>();
                var taskIds = _workTaskService.getallTaskHasContractAcceptance(model.ContractId, model.Id).ToList();
                if (taskIds != null)
                {
                    model.SelectedListTaskId = taskIds.Select(c => c.Id).ToList();
                }
                if (item.Ratio != null)
                {
                    model.Ratio = Math.Round((decimal)(item.Ratio * 100), 2);
                }
                if (isDetail)
                {
                    var ListFileId = _contractService.GetallContractFiles(ContractId: item.ContractId, TypeId: (int)ContractFileType.TaskAcceptance, EntityId: item.Id);
                    model.WorkFileIds = String.Join(",", ListFileId.Select(c => c.FileId).ToList());
                }
                var totalamount = _contractPaymentService.GetAllContractsPaymentAmountByAcceptanceId(item.Id);
                if (item.currency != null)
                {
                    model.AmountPaymentAcceptance = _priceFormatter.FormatPrice(totalamount, true, item.currency);
                }
                else
                {
                    model.AmountPaymentAcceptance = _priceFormatter.FormatPrice(totalamount, true, item.contract.currency);
                }
                if (item.TypeId == (int)ContractAcceptancesType.TamUng)
                {
                    var mapping = _contractService.GetAllContractAcceptanceTaskMapping(AcepptanceId: item.Id).FirstOrDefault();
                    if (mapping != null)
                    {
                        model.TaskId = mapping.TaskId;
                    }
                }
                if (
                model.TypeId == (int)ContractAcceptancesType.TamUng)
                {
                    model.TotalAmountMaxReceived = _priceFormatter.FormatPrice((Int64)(item.TotalAmount * item.Ratio), true, item.currency);
                    var contractPayment = _contractPaymentService.GetAllContractsPayment(item.ContractId, item.PaymentAcceptanceId.GetValueOrDefault(0), IsReceipts: 2, TaskId: model.TaskId.GetValueOrDefault(0));
                    if (contractPayment != null)
                    {
                        model.TotalAmountReceived = _priceFormatter.FormatPrice((Int64)(contractPayment.Sum(c => c.Amount)), true, item.currency);
                    }
                }
            }
            var tasks = _workTaskService.getTasksByConTractId(ContractId: model.ContractId);
            model.AvailableTaskList = tasks.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = c.Id == model.TaskId
            }).ToList();
            var Task = _workTaskService.GetTaskById(model.TaskId.GetValueOrDefault(0));
            if (Task != null && Task.currency != null)
            {
                model.CurrencyName = Task.currency.Name;
                model.CurrencyId = Task.CurrencyId;
            }
            if (model.TypeId == (int)ContractAcceptancesType.TamUng && Task != null && Task.currency != null)
            {
                var searchmodel = new ContractAcceptanceSearchModel
                {
                    TaskId = Task.Id,
                    ContractId = Task.ContractId,
                    TypeId = (int)ContractAcceptancesType.TamUng,
                };
                var acceps = _contractService.getAllContractAcceptance(ContractId: Task.ContractId, TaskId: Task.Id, TypeId: (int)ContractAcceptancesType.TamUng).ToList();
                model.ToltalAmountAdvanced = acceps.Sum(c => (Int64)c.TotalAmount);
                model.SumToltalAmount = (Int64)Task.TotalMoney;
                model.ToltalAmountAdvancedText = _priceFormatter.FormatPrice(model.ToltalAmountAdvanced, true, Task.currency);
                model.SumToltalAmountText = _priceFormatter.FormatPrice(model.SumToltalAmount, true, Task.currency);
                if (item == null)
                {
                    model.Ratio = Task.TaskGroupValue.GetValueOrDefault(0) * 100;
                }

            }
            //model.AvailableTaskGroup = _taskGroupService.GetAllTaskGroups().Select(c => new SelectListItem
            //{
            //    Text = c.Name,
            //    Value = c.Id.ToString(),
            //}).ToList();
            //model.AvailableTaskGroup.Insert(0, (new SelectListItem
            //{
            //    Text = "Chọn tỷ lệ khoán",
            //    Value = "0",
            //}));            
            return (model);
        }
        public void PrepareContractAcceptance(ContractAcceptanceModel model, ContractAcceptance item)
        {
            if (model.Id == 0)
            {
                item.CreatorId = _workContext.CurrentCustomer.Id;
                item.ContractId = model.ContractId;
            }
            if (model.PaymentAcceptanceId > 0)
            {
                item.PaymentAcceptanceId = model.PaymentAcceptanceId;
            }
            item.Code = model.Code;
            item.Name = model.Name;
            item.TypeId = model.TypeId;
            item.Description = model.Description;
            item.Ratio = model.Ratio / 100;
            item.ApprovalDate = model.ApprovalDate;
            item.StatusId = model.StatusId;
            item.TotalAmount = model.TotalAmount.ToNumber();
            if (model.CurrencyId > 0)
            {
                item.CurrencyId = model.CurrencyId;
            }
            if (model.ResponsibleId > 0)
            {
                item.ResponsibleId = model.ResponsibleId;
            }
            if (model.UnitId > 0)
            {
                item.UnitId = model.UnitId;
            }
            if (model.PaymentAdvanceId > 0)
            {
                item.PaymentAdvanceId = model.PaymentAdvanceId;
            }
        }

        public void PrepareContractPaymentAcceptance(ContractPaymentAcceptanceModel model, ContractPaymentAcceptance item)
        {
            if (model.Id == 0)
            {
                item.CreatorId = _workContext.CurrentCustomer.Id;
                item.CreatedDate = DateTime.Now;
            }
            item.Code = model.Code;
            item.Name = model.Name;
            item.Description = model.Description;
            item.ApprovalDate = model.ApprovalDate;
            item.ContractId = model.ContractId;
            item.CurrencyId = model.CurrencyId;
            item.TotalAmount = model.TotalAmount;
            item.ReduceAdvance = model.ReduceAdvance;
            item.ReduceKeep = model.ReduceKeep;
            item.ReduceOther = model.ReduceOther;
            item.Depreciation = model.Depreciation;
            if (model.ApprovalId > 0)
            {
                item.ApprovalId = model.ApprovalId;
            }
        }
        public ContractPaymentAcceptanceModel PrepareContractPaymentAcceptanceModel(ContractPaymentAcceptanceModel model, ContractPaymentAcceptance item = null)
        {
            var contract = _contractService.GetContractById(model.ContractId).ToModel<ContractModel>();
            model.CurrencyId = contract.CurrencyId;
            model.CurrencyName = contract.CurrencyName;
            model.ApprovalDate = contract.EndDate;
            if (item.Id > 0)
            {
                model = item.ToModel<ContractPaymentAcceptanceModel>();
                //var items = _contractService.getAllContractAcceptance(ContractId: item.ContractId, TypeId: (int)ContractAcceptancesType.KhoiLuong, PaymentAcceptanceId: item.Id, isFilter: true);
                //model.SelectedContractAcceptanceIds = items.Select(c => c.Id).ToList();

                var subs = _contractService.GetallPaymentAcceptanceSubs(PaymentAcceptanceId: item.Id);
                model.SelectedContractAcceptanceIds = subs.Select(c => (int)c.AcceptanceId).ToList();
                model.listPaymentAcceptanceSub = subs.Select(c => PrepareContractPaymentAcceptanceSubModel(null, c)).ToList();
                foreach (var a in model.listPaymentAcceptanceSub)
                {
                    a.TotalPaymented = (_contractService.GetContractPaymentAcceptanceSubByAcceptanceId(a.AcceptanceId)
                        .Where(c => c.TaskId == a.TaskId).Sum(c => c.TotalMoney) -
                        _contractService.GetContractPaymentAcceptanceSubByAcceptanceId(a.AcceptanceId)
                        .Where(c => c.TaskId == a.TaskId && c.PaymentAcceptanceId == a.PaymentAcceptanceId).Sum(c => c.TotalMoney)).ToString();
                    a.SumTotalMoney = _contractService.GetallContractAcceptanceSubs(AcceptanceId: a.AcceptanceId,
                        ContractId: a.ContractId).Sum(c => c.TotalAmount).ToString();
                }
                //xu ly file
                var ListFileId = _contractService.GetallContractFiles(ContractId: item.ContractId, TypeId: (int)ContractFileType.PaymentAcceptance, EntityId: item.Id);
                model.WorkFileIds = String.Join(",", ListFileId.Select(c => c.FileId).ToList());
            }
            var contractacceptances = _contractService.getAllContractAcceptance(ContractId: model.ContractId, TypeId: (int)ContractAcceptancesType.KhoiLuong);
            var listTemp = _contractService.getAllContractAcceptance(ContractId: model.ContractId, TypeId: (int)ContractAcceptancesType.KhoiLuong);
            foreach (var ca in listTemp)
            {
                var totalMoneyAcceptance = ca.TotalAmount;
                var listPaymentSub = _contractService.GetContractPaymentAcceptanceSubByAcceptanceId(ca.Id);
                var totalMoneyPaymentSub = listPaymentSub.Sum(c => c.TotalMoney);
                if (totalMoneyPaymentSub >= totalMoneyAcceptance && !model.SelectedContractAcceptanceIds.Contains(ca.Id))
                {
                    contractacceptances.Remove(ca);
                }
            }
            model.AvailableContractAcceptances = contractacceptances.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
            }).ToList();

            return (model);
        }
        // Prepare ContractAcceptanceNoibo (cho don vi)
        public void PrepareContractAcceptanceNoiboForUnit(ContractAcceptanceModel model, ContractAcceptance item)
        {
            item.Id = model.Id;
            //item.AcceptanceGuid = model.AcceptanceGuid;
            item.Name = model.Name;
            item.Description = model.Description;
            if (model.Id == 0)
            {
                item.CreatorId = _workContext.CurrentCustomer.Id;
                item.CreatedDate = DateTime.Now;
            }
            item.Code = model.Code;
            item.ApprovalDate = DateTime.Now;
            item.Ratio = model.Ratio;
            item.TotalAmount = model.TotalAmount.ToNumber();
            item.UnitId = model.UnitId;
            item.ContractId = model.ContractId;
            item.TypeId = (int)ContractAcceptancesType.NoiBo;
            item.StatusId = (int)ContractAcceptanceStatus.Approved;
        }
        public ContractAcceptanceListModel PrepareContractAcceptanceListModel(ContractAcceptanceSearchModel searchmodel)
        {
            var items = _contractService.getAllContractAcceptance(ContractId: searchmodel.ContractId, TaskId: searchmodel.TaskId, TypeId: searchmodel.TypeId);
            return new ContractAcceptanceListModel
            {
                Data = items.Select(c => PrepareContractAcceptanceModel(null, c)).ToList(),
                Total = items.Count(),
            };
        }
        public void PrepareContracAcceptanceNoiBoModel(ContractAcceptanceModel model, ContractAcceptance item)
        {
            var lstUnit = _unitService.GetAllUnits();
            string treeTam = "";
            model.AvailableUnit = lstUnit.Select(c => new SelectListItem
            {
                Text = treeTam.PadLeft((int)c.TreeLevel - 1, '-') + c.Name,
                Value = c.Id.ToString(),
                Selected = c.Id == model.UnitId
            }).ToList();
            model.AvailableUnit.Insert(0,
                new SelectListItem
                {
                    Value = "0",
                    Text = "--Chọn đơn vị--"
                });
        }
        #endregion
        #region ContractAcceptanceBB
        public ContractAcceptanceBBModel PrepareContractAcceptanceBBModel(ContractAcceptanceBBModel model, ContractAcceptanceBB item = null)
        {
            if (item.Id > 0)
            {
                model = item.ToModel<ContractAcceptanceBBModel>();
                //xu ly file
                var ListFileId = _contractService.GetallContractFiles(ContractId: item.ContractId, TypeId: (int)ContractFileType.BBAcceaptance, EntityId: item.Id);
                model.WorkFileIds = String.Join(",", ListFileId.Select(c => c.FileId).ToList());
            }
            var task = _workTaskService.GetTaskById(model.TaskId.GetValueOrDefault(0));
            if (task != null)
            {
                model.TaskName = task.Name;
                if (task.UnitId > 0)
                    model.UnitId = (int)task.UnitId;
            }
            var units = _unitService.GetAllUnits();
            model.AvailableUnit = units.Select(c => new SelectListItem()
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = model.UnitId == c.Id,
            }).ToList();
            model.AvailableUnit.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = "---Chọn đơn vị nghiệm thu---"
            });
            return model;
        }
        public void PrepareContractAcceptanceBB(ContractAcceptanceBBModel model, ContractAcceptanceBB item = null)
        {
            item.Name = model.Name;
            item.Code = model.Code;
            if (model.Id == 0)
            {
                item.CreatorId = _workContext.CurrentCustomer.Id;
                item.CreatedDate = DateTime.Now;
                item.ApprovalId = _workContext.CurrentCustomer.Id;
            }
            item.ApprovalDate = model.ApprovalDate.Value;
            item.TaskId = (int)model.TaskId;
            item.Description = model.Description;
            item.ContractId = model.ContractId;
            var task = _workTaskService.GetTaskById(model.TaskId.GetValueOrDefault());
            if (task != null)
            {
                item.ContractIdBB = task.TaskContracts.Where(c => c.contract.ClassificationId == (int)ContractClassification.BB && c.contract.StatusId != (int)ContractStatus.Destroy).FirstOrDefault().ContractId;
            }
            if (model.UnitId > 0)
            {
                item.UnitId = model.UnitId;
            }
            else
            {
                if (task.UnitId > 0)
                {
                    item.UnitId = task.UnitId;
                }
                else
                {
                    item.UnitId = null;
                }
            }
        }
        #endregion
        #region ContractPaymentRequest
        public ContractPaymentRequestSearchModel PrepareContractPaymentRequestSearchModel(ContractPaymentRequestSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        public ContractPaymentRequestListModel PrepareContractPaymentRequestListModel(ContractPaymentRequestSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get items
            var items = _contractService.GetAllContractPaymentRequestByContractId(searchModel.ContractId);

            //prepare list model
            var model = new ContractPaymentRequestListModel
            {
                //fill in model values from the entity
                Data = items.PaginationByRequestModel(searchModel).Select(store => store.ToModel<ContractPaymentRequestModel>()),
                Total = items.Count
            };

            return model;
        }
        public ContractPaymentRequestModel PrepareContractPaymentRequestModel(ContractPaymentRequestModel model, ContractPaymentRequest item, bool excludeProperties = false)
        {
            if (item != null)
            {
                //fill in model values from the entity
                model = model ?? item.ToModel<ContractPaymentRequestModel>();
                var totalamount = _contractPaymentService.GetAllContractsPaymentAmountByPaymentRequestId(item.Id);
                if (item.TotalAmount == totalamount)
                {
                    model.IsPaymented = true;
                }
                model.AmountPayment = _priceFormatter.FormatPrice(totalamount, true, item.contract.currency);
            }
            return model;
        }
        public void prepareContractPaymentRequestModelAdd(ContractPaymentRequestModel model, int PaymentPlanId)
        {
            model.PaymentPlanId = PaymentPlanId;
            var contractPaymentPlan = _contractService.GetContractPaymentPlanById(PaymentPlanId);
            model.ContractId = contractPaymentPlan.ContractId;
            if (model.Id > 0)
            {
                model.SelectedContractPaymentAcceptence = _contractService.GetAllContractPaymenAcceptanceByContractId(model.ContractId).Select(c => c.Id).ToList();
            }
            model.AvailableContractPaymentAcceptence = _contractService.GetAllContractPaymenAcceptanceByContractId(model.ContractId).Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
            }).ToList();
            model.TaxRatio = model.TaxRatio * 100;
        }
        public void prepareContractPaymentRequest(ContractPaymentRequestModel model, ContractPaymentRequest item)
        {
            item.Id = model.Id;
            item.TypeId = model.TypeId;
            item.CreatedDate = DateTime.Now;
            item.ContractId = model.ContractId;
            item.Code = model.Code;
            item.Name = model.Name;
            item.ContractId = model.ContractId;
            item.CreatorId = _workContext.CurrentCustomer.Id;
            item.PaymentPlanId = model.PaymentPlanId;
            item.Description = model.Description;
            item.TotalAmount = model.TotalAmount.ToNumber();
            item.RequestDate = model.RequestDate;
            item.EndDate = model.EndDate;
            item.DataInfo = model.DataInfo;
            item.TaxRatio = (model.TaxRatio / 100);
        }
        #endregion
        #region ContractAcceptanceSub
        public ContractAcceptanceSubModel PrepareContractAcceptanceSubModel(ContractAcceptanceSubModel model, ContractAcceptanceSub item)
        {
            var taskId = model.TaskId;
            if (model != null)
            {
                model = new ContractAcceptanceSubModel();
            }
            if (item != null)
            {
                //fill in model values from the entity
                model = model ?? item.ToModel<ContractAcceptanceSubModel>();
            }
            if (item != null && item.Ratio != null)
            {
                model.Ratio = item.Ratio.GetValueOrDefault(0) * 100;
            }
            var unit = _unitService.GetAllUnits();
            if (taskId > 0)
            {
                var tasks = _workTaskService.getTasksByConTractId(ContractId: 0, ParentId: (int)taskId, includParent: true);
                model.AvailableTasks = tasks.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                }).ToList();
                var task = _workTaskService.GetTaskById(taskId.GetValueOrDefault(0));
                model.TaskName = task.Name;
                if (task.unitInfo != null)
                {
                    model.UnitName = task.unitInfo.Name;
                }
            }
            return model;
        }
        public ContractAcceptanceSubModel PrepareContractAcceptanceKhoiLuongSubModel(ContractAcceptanceSubModel model, ContractAcceptanceSub item, int ContractAcceptanceId = 0)
        {
            if (item != null)
            {
                //fill in model values from the entity
                model = model ?? item.ToModel<ContractAcceptanceSubModel>();
            }
            var taskId = model.TaskId;
            if (item != null && item.Ratio != null)
            {
                model.Ratio = item.Ratio.GetValueOrDefault(0) * 100;
            }
            var unit = _unitService.GetAllUnits();
            if (taskId > 0)
            {
                var tasks = _workTaskService.getTasksByConTractId(ContractId: 0, ParentId: (int)taskId, includParent: true);
                model.AvailableTasks = tasks.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                }).ToList();
                var task = _workTaskService.GetTaskById(taskId.GetValueOrDefault(0));
                if (task != null)
                {
                    model.TaskValue = task.TotalMoney.GetValueOrDefault(0);
                    model.TaskValueText = _priceFormatter.FormatPrice(task.TotalMoney.GetValueOrDefault(0), true, task.currency);
                }
                model.TaskName = task.Name;
                if (task.unitInfo != null)
                {
                    model.UnitName = task.unitInfo.Name;
                }
                var acceptances = _contractService.GetallContractAcceptanceSubs(TaskId: taskId.GetValueOrDefault(0), TypeId: (int)ContractAcceptancesType.KhoiLuong);
                if (ContractAcceptanceId > 0)
                {
                    var PresentAcceptance = _contractService.GetallContractAcceptanceSubs(TaskId: taskId.GetValueOrDefault(0), TypeId: (int)ContractAcceptancesType.KhoiLuong, AcceptanceId: ContractAcceptanceId).Select(c => c.Id).ToList();
                    acceptances = acceptances.Where(c => !PresentAcceptance.Contains(c.Id)).ToList();
                }
                model.TaskAcceptanced = (Int64)(acceptances.Sum(c => c.TotalAmount.GetValueOrDefault(0)));
                if (ContractAcceptanceId == 0)
                {
                    model.TotalAmount = (model.TaskValue - model.TaskAcceptanced).ToVNStringNumber();
                }
                model.TaskAcceptancedText = acceptances.Sum(c => c.TotalAmount.GetValueOrDefault(0)).ToVNStringNumber();
                model.TaskId = taskId;
            }
            return model;
        }
        public void PrepareContractAcceptanceSub(ContractAcceptanceSub item, ContractAcceptanceSubModel model)
        {
            if (model.Id == 0)
            {
                item.CreatorId = _workContext.CurrentCustomer.Id;
                item.CreatedDate = DateTime.Now;
            }
            if (model.CurrencyId > 0)
            {
                item.CurrencyId = model.CurrencyId;
            }
            item.AcceptanceId = model.AcceptanceId;
            item.ContractId = model.ContractId;
            item.TotalAmount = model.TotalAmount.ToNumber();
            item.Ratio = model.Ratio.GetValueOrDefault(0) / 100;
            item.UnitId = model.UnitId;
            item.TotalMoney = model.TotalMoney.ToNumber();
            item.TaskId = (int)model.TaskId;
        }
        public ContractAcceptanceSub PrepareContractAcceptanceSubByTask(ContractAcceptanceSubModel model, ContractAcceptanceSub item)
        {
            if (item == null)
            {
                item = new ContractAcceptanceSub();
            }
            var _task = _workTaskService.GetTaskById((int)model.TaskId);
            item.UnitId = (int)_task.UnitId;
            item.ContractId = _task.ContractId;
            item.TaskId = _task.Id;
            item.Ratio = model.Ratio / 100;
            item.TotalAmount = model.TotalAmount.ToNumber();
            item.CurrencyId = _task.currency.Id;
            item.StatusId = (int)ContractAcceptanceSubStatus.Use;
            item.CreatorId = _workContext.CurrentCustomer.Id;
            item.CreatedDate = DateTime.Now;
            item.Description = model.Description + " - " + _task.Name;
            return item;
        }
        public ContractAcceptanceSubModel PrepareAcceptanceSubModelbyPaymentSub(ContractPaymentAcceptanceSub paymentSub)
        {
            var model = new ContractAcceptanceSubModel();
            model.TaskName = paymentSub.task.Name;
            model.TotalMoney = ((Int64)paymentSub.TotalMoney).ToString();
            model.TaskId = paymentSub.TaskId;
            if (paymentSub.unit != null)
            {
                model.UnitId = (int)paymentSub.UnitId;
            }
            else
            {
                return null;
            }
            if (paymentSub.currency != null)
            {
                model.CurrencyId = (int)paymentSub.CurrencyId;
            }
            return model;
        }
        public ContractAcceptanceSubModel PrepareAcceptanceSubModelbyTask(Task taskmodel, ContractPaymentAcceptanceSubModel paysubmodel, int ContractAcceptanceId = 0)
        {
            var model = new ContractAcceptanceSubModel();

            if (ContractAcceptanceId > 0)
            {
                var ContractAcceptanceSub = _contractService.GetallContractAcceptanceSubs(AcceptanceId: ContractAcceptanceId, TaskId: taskmodel.Id).FirstOrDefault();
                if (ContractAcceptanceSub != null)
                    return ContractAcceptanceSub.ToModel<ContractAcceptanceSubModel>();
            }
            model.TaskName = taskmodel.Name;
            var taskcontract = _workTaskService.getTaskContract(TaskId: taskmodel.Id);
            model.hasBB = taskcontract.Any(c => c.contract.classification == ContractClassification.BB);
            if (taskmodel.unitInfo != null)
            {
                model.UnitName = taskmodel.unitInfo.Name;
                model.UnitId = (int)taskmodel.UnitId;
            }
            if (taskmodel.CurrencyId != null)
            {
                model.CurrencyId = taskmodel.CurrencyId;
            }
            if (taskmodel.parentTask != null && taskmodel.parentTask.TotalMoney > 0)
            {
                model.TotalMoney = (Convert.ToInt64(paysubmodel.TotalMoney.ToNumber() * (taskmodel.TotalMoney / taskmodel.parentTask.TotalMoney) / 1000) * 1000).ToString();
            }
            else
            {
                model.TotalMoney = paysubmodel.TotalMoney;
            }
            if (model.hasBB && taskmodel.UnitId > 0)
            {
                model.Type = (int)ContractAcceptanceSubType.BB_DON_VI;
                model.UnitName = model.UnitName + "  thuê BB'";
                model.TotalAmount = taskcontract.FirstOrDefault().contract.TotalAmount.ToVNStringNumber();
            }
            if (model.hasBB && taskmodel.UnitId == null)
            {
                model.Type = (int)ContractAcceptanceSubType.BB_CONG_TY;
                model.UnitName = "PECC1 thuê BB'";
            }
            if (!model.hasBB)
            {
                model.Type = (int)ContractAcceptanceSubType.THI_CONG;
                if (string.IsNullOrEmpty(model.UnitName))
                {
                    model.UnitName = "Chưa thiết lập đơn vị";
                }
                //model.Ratio = taskmodel.TaskGroupValue;
            }
            return model;
        }
        public void PrepareContractAcceptanceKhoiLuongSub(ContractAcceptanceSub item)
        {
            var task = _workTaskService.GetTaskById((int)item.TaskId);
            if (task.UnitId > 0)
            {
                item.UnitId = (int)task.UnitId;
            }
            else
            {
                item.UnitId = null;
            }
            if (task.CurrencyId > 0)
            {
                item.CurrencyId = (int)task.CurrencyId;
            }
            else
            {
                item.CurrencyId = null;
            }
            if (task.ContractId > 0)
            {
                item.ContractId = (int)task.ContractId;
            }
        }
        public ContractAcceptanceSubModel PrepareContractAcceptanceSubByTaskModel(int taskId, string approvalDateString)
        {
            var model = new ContractAcceptanceSubModel();

            var _task = _workTaskService.GetTaskById(taskId);
            model.TaskId = _task.Id;
            model.TaskName = _task.Name;
            var hasbb = _task.TaskContracts.Any(c => c.contract.ClassificationId == (int)ContractClassification.BB && c.contract.StatusId != (int)ContractStatus.Destroy);
            model.HasBB = hasbb;
            if (hasbb)
            {
                model.ContractBBValue = _task.TaskContracts.Where(c => c.contract.ClassificationId == (int)ContractClassification.BB && c.contract.StatusId != (int)ContractStatus.Destroy).Select(x => x.contract).ToList().Sum(c => (Int64)c.TotalAmount.GetValueOrDefault(0));
                model.ContractBBValueText = model.ContractBBValue.ToVNStringNumber();
            }
            model.TotalAmountTask = _task.TotalMoney.ToVNStringNumber();
            model.UnitId = (int)_task.UnitId;
            var _unit = _unitService.GetUnitById(model.UnitId);
            model.UnitName = _unit.Name;
            model.Ratio = _task.TaskGroupValue * 100;
            //tong nghiem thu thanh toan sau thue cua cong viec trong khoang thoi gian approvalDate
            var lstpmas = _contractService.GetAllContractPaymentAcceptanceSubByApprovalDate(_task.Id, approvalDateString);
            model.TotalPaymentAcceptanceTask = lstpmas.Sum(c => c.TotalMoney).ToVNStringNumber();
            // gia tri nghiem thu thanh toan truoc thue
            model.TotalPaymentAcceptanceTaskTruocThue = (lstpmas.Sum(c => c.TotalMoney) / (1 + _task.TaxPercent)).ToVNStringNumber();
            model.TotalAmount = (model.TotalPaymentAcceptanceTaskTruocThue.ToNumber() * model.Ratio / 100).ToVNStringNumber();
            return model;
        }
        public ContractAcceptanceSubModel PrepareContarctAcceptanceSubModelByAcceptanceSub(ContractAcceptanceSub item)
        {
            var model = new ContractAcceptanceSubModel();
            var _task = _workTaskService.GetTaskById((int)item.TaskId);
            model.TaskId = item.TaskId;
            model.TaskName = _task.Name;
            model.TotalAmountTask = _task.TotalMoney.ToVNStringNumber();
            var _unit = _unitService.GetUnitById((int)item.UnitId);
            model.UnitName = _unit.Name;
            model.Ratio = item.Ratio * 100;
            model.TotalAmount = item.TotalAmount.ToVNStringNumber();
            return model;
        }
        public ContractAcceptanceSearchModel PrepareContractAcceptanceSearchModel(ContractAcceptanceSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            var lstUnit = _unitService.GetAllUnits();
            string treeTam = "";
            searchModel.AvailableUnit = lstUnit.Select(c => new SelectListItem
            {
                Text = treeTam.PadLeft((int)c.TreeLevel - 1, '-') + c.Name,
                Value = c.Id.ToString(),
                Selected = c.Id == searchModel.UnitId
            }).ToList();
            searchModel.AvailableUnit.Insert(0,
                new SelectListItem
                {
                    Value = "0",
                    Text = "--Chọn đơn vị--"
                });
            searchModel.SetGridPageSize();
            return searchModel;
        }

        public ContractAcceptanceListModel PrepareContractAcceptanceNoiBoListModel(ContractAcceptanceSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get items
            var items = _contractService.GetAllContractAcceptanceNoiBo(searchModel.Keysearch, searchModel.UnitId, searchModel.Page - 1, searchModel.PageSize);

            //prepare list model
            var model = new ContractAcceptanceListModel
            {
                //fill in model values from the entity
                Data = items.Select(c =>
                {
                    var m = c.ToModel<ContractAcceptanceModel>();
                    var _unit = _unitService.GetUnitById((int)c.UnitId);
                    m.UnitName = _unit.Name;
                    m.DateApproval = c.ApprovalDate.toDateVNString();
                    m.TotalAmount = c.TotalAmount.ToVNStringNumber();
                    return m;
                }
                ),
                Total = items.TotalCount
            };
            return model;
        }
        #endregion

        #region ContractPaymentAcceptanceSub

        public ContractPaymentAcceptanceSubModel PrepareContractPaymentAcceptanceSubModel(ContractPaymentAcceptanceSubModel model, ContractPaymentAcceptanceSub item)
        {
            model = model ?? item.ToModel<ContractPaymentAcceptanceSubModel>();
            return model;
        }
        public void PrepareContractPaymentAcceptanceSub(ContractPaymentAcceptanceSub item, ContractPaymentAcceptanceSubModel model)
        {
            var Task = _workTaskService.GetTaskById((int)model.TaskId);
            //item.PaymentAcceptanceId = model.PaymentAcceptanceId;
            item.CreatorId = _workContext.CurrentCustomer.Id;
            item.CreatedDate = DateTime.Now;
            item.TaskId = (int)model.TaskId;
            if (model.AcceptanceId > 0)
            {
                item.AcceptanceId = model.AcceptanceId;
            }
            else
            {
                item.AcceptanceId = null;
            }
            item.TotalAmount = model.TotalAmount;
            item.TotalMoney = model.TotalMoney.ToNumber();
            item.ReduceAdvance = model.ReduceAdvance.ToNumber();
            item.ReduceKeep = model.ReduceKeep.ToNumber();
            item.ReduceOther = model.ReduceOther.ToNumber();
            item.Depreciation = model.Depreciation.ToNumber();
            if (Task != null && Task.UnitId != null)
            {
                item.UnitId = (int)Task.UnitId;
            }
            else
            {
                item.UnitId = null;
            }
            if (Task != null && Task.CurrencyId != null)
            {
                item.CurrencyId = (int)Task.CurrencyId;
            }
            else
            {
                item.CurrencyId = null;
            }
        }
        public ContractPaymentAcceptanceSubModel prepareContractPaymentAcceptancebyTaskMap(ContractAcceptanceSub AcceptanceSub)
        {
            var task = _workTaskService.GetTaskById((int)AcceptanceSub.TaskId);
            var PaymentAcceptanceSubModel = new ContractPaymentAcceptanceSubModel
            {
                TaskId = task.Id,
                ContractId = task.ContractId,
                CurrencyId = task.CurrencyId,
                AcceptanceId = AcceptanceSub.AcceptanceId,
                TaskName = task.Name,
                TotalMoney = (AcceptanceSub.TotalAmount - _contractService.GetContractPaymentAcceptanceSubByAcceptanceId(AcceptanceSub.AcceptanceId)
                    .Where(c => c.TaskId == task.Id).Sum(c => c.TotalMoney)).ToVNStringNumber(),
                SumTotalMoney = ((Int64)AcceptanceSub.TotalAmount).ToString(),
                TotalPaymented = _contractService.GetContractPaymentAcceptanceSubByAcceptanceId(AcceptanceSub.AcceptanceId)
                    .Where(c => c.TaskId == task.Id).Sum(c => c.TotalMoney).ToString()
            };
            if (task.UnitId != null)
            {
                PaymentAcceptanceSubModel.UnitId = (int)task.UnitId;
                PaymentAcceptanceSubModel.UnitName = task.unitInfo.Name;
            }
            return PaymentAcceptanceSubModel;
        }
        #endregion
        #region ContractSettlement
        public ContractSettlementModel prepareTotalAmountContractSettle(ContractSettlement item)
        {
            var model = item.ToModel<ContractSettlementModel>();
            var listContractSettlementSub = _contractService.GetAllContractSettlementSub(contractSettlementId: item.Id);
            model.TotalAmountSettlement = listContractSettlementSub.Sum(c => c.TotalAmount);
            return model;
        }
        public void PrepareContractSettlement(ContractModel model)
        {
            var m = _contractService.getAllContractSettlement(ContractId: model.Id);
            model.lsContractSettlement = m.Select(c => prepareTotalAmountContractSettle(c)).ToList();
        }
        public void PrepareContractSettlement(ContractSettlementModel model, ContractSettlement item)
        {
            if (model.Id == 0)
            {
                item.CreatorId = _workContext.CurrentCustomer.Id;
                item.CreatedDate = DateTime.Now;
                item.ContractId = model.ContractId;
                item.ApproverId = _workContext.CurrentCustomer.Id;
            }
            item.Code = model.Code;
            item.Name = model.Name;
            item.Description = model.Description;
            item.ApprovalDate = model.ApprovalDate;
            item.StatusId = model.StatusId;
        }
        #endregion
        #region PaymentExpenditure
        public void PreparePaymentExpenditure(PaymentExpenditureModel model, PaymentExpenditure item)
        {
            if (model.Id == 0)
            {
                item.CreatorId = _workContext.CurrentCustomer.Id;
                item.CreatedDate = DateTime.Now;
                item.UnitId = model.UnitId;
            }
            item.Name = model.Name;
            item.Code = model.Code;
            item.Description = model.Description;
            if (model.PaymentAdvanceId != 0)
            {
                item.TypeId = (int)PaymentExpenditureType.Advance;
            }
            if (model.AcceptanceId != 0)
            {
                item.TypeId = (int)PaymentExpenditureType.Acceptance;
            }
            item.TotalAmount = model.lstContractPayment.Sum(c => c.Amount.ToNumber());
        }
        public PaymentExpenditureListModel PreparePaymentExpenditureListModel(PaymentExpenditureSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            var item = _contractService.GetAllPaymentExpenditure(Keysearch: searchModel.Keysearch, UnitId: (int)searchModel.UnitId, pageSize: searchModel.PageSize, pageIndex: searchModel.Page - 1);
            var model = new PaymentExpenditureListModel
            {
                Data = item.Select(c =>
                {
                    var m = c.ToModel<PaymentExpenditureModel>();
                    m.CreatedDates = c.CreatedDate.toDateVNString();
                    m.TotalAmounts = c.TotalAmount.ToVNStringNumber();
                    return m;
                }),
                Total = item.TotalCount
            };
            return model;
        }
        #endregion
    }
}
