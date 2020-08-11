using GS.Core;
using GS.Core.Domain.Contracts;
using GS.Services;
using GS.Services.Contracts;
using GS.Services.Customers;
using GS.Services.ExportImport;
using GS.Services.Security;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Factories;
using GS.Web.Framework.Controllers;
using GS.Web.Models.Contracts;
using GS.Web.Models.Report;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Controllers
{
    public partial class ReportController : BaseWorksController
    {
        #region Fields
        private readonly IContractService _contractService;
        private readonly IPermissionService _permissionService;
        private readonly IConstructionModelFactory _constructionModelFactory;
        private readonly IConstructionService _constructionService;
        private readonly IReportService _reportService;
        private readonly IContractFormService _contractFormService;
        private readonly IUnitService _unitService;
        private readonly IExportManager _exportManager;
        private readonly IProcuringAgencyService _procuringAgencyService;
        private readonly IContractTypeService _contractTypeService;
        private readonly IConstructionTypeService _constructionTypeService;
        private readonly ICustomerService _customerService;
        #endregion
        #region Ctor
        public ReportController(IContractService contractService,
            ICustomerService customerService,
            IUnitService unitService,
            IProcuringAgencyService procuringAgencyService,
            IConstructionModelFactory constructionModelFactory,
            IReportService reportService,
            IExportManager exportManager,
            IConstructionService constructionService,
            IPermissionService permissionService,
            IContractFormService contractFormService,
            IContractTypeService contractTypeService,
            IConstructionTypeService constructionTypeService
            )
        {
            this._customerService = customerService;
            this._constructionTypeService = constructionTypeService;
            this._contractTypeService = contractTypeService;
            this._procuringAgencyService = procuringAgencyService;
            this._unitService = unitService;
            this._exportManager = exportManager;
            this._contractService = contractService;
            this._permissionService = permissionService;
            this._reportService = reportService;
            this._constructionModelFactory = constructionModelFactory;
            this._constructionService = constructionService;
            this._contractFormService = contractFormService;
        }
        #endregion
        #region Methods
        [HttpPost]
        public virtual IActionResult _Report(ReportSearchModel SearchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstruction))
                return AccessDeniedKendoGridJson();

            var model = _reportService.GetReport(constructionCode: SearchModel.ConstructionCode,
                contractNameCode: SearchModel.ContractNameCode, startDateFrom: SearchModel.StartDateFrom, startDateTo: SearchModel.StartDateTo,
                constructionType: SearchModel.ConstructionType, contractFormId: SearchModel.ContractFormId,
                SelectConstructionName: SearchModel.SelectConstructionIds.ToList(),
                contractStatus: SearchModel.contractStatus, SelectConstructionType: SearchModel.SelectedConstructionType).ToList();

            return PartialView(model);
        }
        public virtual IActionResult Index()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstruction))
                return AccessDeniedKendoGridJson();
            var model = new ReportSearchModel();
            model.ConstructionName = ConstructionNameddl();
            var contractforms = _contractFormService.GetAllContractForms();
            model.AvailableContractForm = contractforms.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = model.ContractFormId == c.Id,
            }).ToList();
            model.AvailableContractForm.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = "--Chọn hình thức hợp đồng--"
            });
            var constructionType = _constructionTypeService.GetAllConstructionTypes();
            model.ConstructionTypeSLI = constructionType.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
            }).ToList();
            model.AvailableContractStatus = ((ContractStatus)model.contractStatus).ToSelectList();
            model.StartDateFrom = new DateTime(DateTime.Now.Year, 1, 1);
            model.StartDateTo = DateTime.Now;
            return View(model);
        }
        public virtual IActionResult ReportContractPaymentAcceptance()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstruction))
                return AccessDeniedKendoGridJson();

            var model = new ReportContractPaymentAcceptanceSearchModel();
            model.SelectConstructionIds = ConstructionNameddl();
            var unitCode = _unitService.GetAllUnits();
            model.AvailableunitCode = unitCode.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = model.unitCode == c.Id,
            }).ToList();
            model.AvailableunitCode.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = "--Chọn đơn vị--"
            });

            model.AvailableQuarter.Insert(0, new SelectListItem
            {
                Value = "0",
                Text = "Quý 1"
            });

            model.AvailableQuarter.Insert(1, new SelectListItem
            {
                Value = "1",
                Text = "Quý 2"
            });

            model.AvailableQuarter.Insert(2, new SelectListItem
            {
                Value = "2",
                Text = "Quý 3"
            });

            model.AvailableQuarter.Insert(3, new SelectListItem
            {
                Value = "3",
                Text = "Quý 4"
            });

            for(int i = DateTime.Now.Year + 1; i > 1990; i--)
            {
                model.AvailableYear.Insert(DateTime.Now.Year + 1 - i, new SelectListItem
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                });
            }

            return View(model);
        }
        public virtual IActionResult _ReportContractPaymentAcceptance(ReportContractPaymentAcceptanceSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstruction))
                return AccessDeniedKendoGridJson();

            var model = _reportService.GetReportContractPaymentAcceptance(SelectConstructionIds: searchModel.ConstructionIds.ToList(),
                contractCodeName: searchModel.contractCodeName, dateTimeFrom: searchModel.dateTimeFrom, datetimeTo: searchModel.datetimeTo, 
                unitCode: searchModel.unitCode, quarterId: searchModel.quarterId, yearId: searchModel.yearId);

            return PartialView(model);
        }
        public virtual IActionResult ReportContractBB()
        {
            var model = new ReportContractBBSearchModel();
            model.ConstructionIds = ConstructionNameddl();
            model.datetimeFrom = new DateTime(DateTime.Now.Year, 1, 1);
            model.datetimeTo = DateTime.Now;
            return View(model);
        }
        public virtual IActionResult _ReportContractBB(ReportContractBBSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstruction))
                return AccessDeniedKendoGridJson();

            var model = _reportService.GetContractReportBB(SelectedConstructionIds: searchModel.SelectedConstructionIds, contractCodeName: searchModel.contractCodeName,
                datetimeFrom: searchModel.datetimeFrom, datetimeTo: searchModel.datetimeTo, constructionType: searchModel.constructionType);

            return PartialView(model);
        }
        public virtual IActionResult ReportContractAB()
        {
            var model = new ReportContractABSearchModel();
            model.ConstructionIds = ConstructionNameddl();
            model.datetimeFrom = new DateTime(DateTime.Now.Year, 1, 1);
            model.datetimeTo = DateTime.Now;
            return View(model);
        }
        public virtual IActionResult _ReportContractAB(ReportContractABSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstruction))
                return AccessDeniedKendoGridJson();

            var model = _reportService.GetContractReportAB(SelectedConstructionIds: searchModel.SelectedConstructionIds, contractCodeName: searchModel.contractCodeName,
                datetimeFrom: searchModel.datetimeFrom, datetimeTo: searchModel.datetimeTo, constructionType: searchModel.constructionType);

            return PartialView(model);
        }
        public virtual IActionResult ReportProcuringAgency()
        {
            var model = new ReportProcuringAgencySearchModel();
            model.ProcuringAgencyCode = PrepareprocuringAgencyddl();
            model.datetimeFrom = new DateTime(DateTime.Now.Year, 1, 1);
            model.datetimeTo = DateTime.Now;
            return View(model);
        }
        public virtual IActionResult _ReportProcuringAgency(ReportProcuringAgencySearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstruction))
                return AccessDeniedKendoGridJson();

            var model = _reportService.GetReportProcuringAgency(SelectedProcuringAgencyIds: searchModel.SelectedProcuringAgencyIds.ToList(), datetimeFrom: searchModel.datetimeFrom, datetimeTo: searchModel.datetimeTo);
            return PartialView(model);
        }
        public virtual IActionResult ReportContractDealine()
        {
            var model = new ReportContractDealineSearchModel();
            model.ConstructionIds = ConstructionNameddl();
            model.datetimeFrom = new DateTime(DateTime.Now.Year, 1, 1);
            model.datetimeTo = DateTime.Now;
            return View(model);
        }
        public virtual IActionResult _ReportContractDealine(ReportContractDealineSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstruction))
                return AccessDeniedKendoGridJson();

            var model = _reportService.ReportContractDealine(SelectedConstructionIds: searchModel.SelectedConstructionIds, datetimeFrom: searchModel.datetimeFrom, datetimeTo: searchModel.datetimeTo);

            return PartialView(model);
        }
        public virtual IActionResult ReportTaskByUnit()
        {
            var model = new ReportTaskByUniySearchModel();
            model.ConstructionIds = ConstructionNameddl();
            model.UnitIds = Unitddl();
            model.dateFrom = new DateTime(DateTime.Now.Year, 1, 1);
            model.dateTo = DateTime.Now;
            return View(model);
        }
        public virtual IActionResult _ReportTaskByUnit(ReportTaskByUniySearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstruction))
                return AccessDeniedKendoGridJson();

            var query = _reportService.GetReportTaskByUnit(
                SelectedConstructionIds: searchModel.SelectedConstructionIds,
                SelectedUnitIds: searchModel.SelectedUnitIds,
                dateFrom: searchModel.dateFrom,
                dateTo: searchModel.dateTo);
            return PartialView(query);
        }
        public virtual IActionResult ReportContractUnfinish()
        {
            var model = new ReportContractUnfinishSearchModel();
            model.ConstructionIds = ConstructionNameddl();
            model.time = DateTime.Now;
            return View(model);
        }
        public virtual IActionResult _ReportContractUnfinish(ReportContractUnfinishSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageConstruction))
                return AccessDeniedKendoGridJson();

            var query = _reportService.GetReportContractUnfinishes(
                ConstructionIds: searchModel.SelectedConstructionIds,
                time: searchModel.time);
            return PartialView(query);
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
        public List<SelectListItem> ConstructionNameddl()
        {
            var datas = _constructionService.GetAllConstructions();
            return datas.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
        }
        public List<SelectListItem> Unitddl()
        {
            var datas = _unitService.GetAllUnits();
            return datas.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
            }).ToList();
        }
        #endregion
        #region Export Excel
        [HttpPost]
        public virtual IActionResult ExportExcelIndexs(ReportSearchModel searchModel)
        {
            if (!string.IsNullOrEmpty(searchModel.StringConstructionIds))
            {
                var listConstruction = searchModel.StringConstructionIds.Split(',').ToList();
                searchModel.SelectConstructionIds = listConstruction.Select(c => (int)c.ToNumber()).ToList();
            }
            try
            {
                var report = _reportService.GetReport(constructionCode: searchModel.ConstructionCode,
                    SelectConstructionName: searchModel.SelectConstructionIds.ToList(),
                    contractNameCode: searchModel.ContractNameCode,
                    startDateFrom: searchModel.StartDateFrom,
                    startDateTo: searchModel.StartDateTo,
                    constructionType: searchModel.ConstructionType,
                    contractFormId: searchModel.ContractFormId,
                    contractStatus: searchModel.contractStatus);
                byte[] bytes;
                using (var stream = new MemoryStream())
                {
                    _exportManager.ExportReportToXlsx(report, stream, Convert.ToDateTime(searchModel.StartDateFrom), Convert.ToDateTime(searchModel.StartDateTo));
                    bytes = stream.ToArray();
                }
                return File(bytes, MimeTypes.TextXlsx, "BaoCaoChung.xlsx");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }
        [HttpPost]
        public virtual IActionResult exportReportContractPaymentAcceptance(ReportContractPaymentAcceptanceSearchModel searchModel)
        {
            if (!string.IsNullOrEmpty(searchModel.StringConstructionIds))
            {
                var listConstruction = searchModel.StringConstructionIds.Split(',').ToList();
                searchModel.ConstructionIds = listConstruction.Select(c => (int)c.ToNumber()).ToList();
            }
            try
            {
                var report = _reportService.GetReportContractPaymentAcceptance(
                    contractCodeName: searchModel.contractCodeName,
                    dateTimeFrom: searchModel.dateTimeFrom,
                    datetimeTo: searchModel.datetimeTo,
                    unitCode: searchModel.unitCode,
                    quarterId: searchModel.quarterId,
                    yearId: searchModel.yearId);
                byte[] bytes;
                using (var stream = new MemoryStream())
                {
                    _exportManager.ExportExcelContractPaymentAcceptance(report, stream);
                    bytes = stream.ToArray();
                }
                return File(bytes, MimeTypes.TextXlsx, "Baocaonghiemthunoibo.xlsx");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }
        [HttpPost]
        public virtual IActionResult ExportReportContractAB(ReportContractABSearchModel searchModel)
        {
            if (!string.IsNullOrEmpty(searchModel.stringConstructionIds))
            {
                var listConstruction = searchModel.stringConstructionIds.Split(',').ToList();
                searchModel.SelectedConstructionIds = listConstruction.Select(c => (int)c.ToNumber()).ToList();
            }
            try
            {
                var report = _reportService.GetContractReportAB(
                    constructionType: searchModel.constructionType,
                    contractCodeName: searchModel.contractCodeName,
                    datetimeFrom: searchModel.datetimeFrom,
                    datetimeTo: searchModel.datetimeTo);
                byte[] bytes;
                using (var stream = new MemoryStream())
                {
                    _exportManager.ExportExcelContractAB(report, stream, Convert.ToDateTime(searchModel.datetimeFrom), Convert.ToDateTime(searchModel.datetimeTo));
                    bytes = stream.ToArray();
                }
                return File(bytes, MimeTypes.TextXlsx, "BaoCaoHopDongAB.xlsx");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }
        [HttpPost]
        public virtual IActionResult ExportReportContractBB(ReportContractBBSearchModel searchModel)
        {
            if (!string.IsNullOrEmpty(searchModel.stringConstructionIds))
            {
                var listConstruction = searchModel.stringConstructionIds.Split(',').ToList();
                searchModel.SelectedConstructionIds = listConstruction.Select(c => (int)c.ToNumber()).ToList();
            }
            try
            {
                var report = _reportService.GetContractReportBB(
                    constructionType: searchModel.constructionType,
                    contractCodeName: searchModel.contractCodeName,
                    datetimeFrom: searchModel.datetimeFrom,
                    datetimeTo: searchModel.datetimeTo);
                byte[] bytes;
                using (var stream = new MemoryStream())
                {
                    _exportManager.ExportReportContractBB(report, stream);
                    bytes = stream.ToArray();
                }
                return File(bytes, MimeTypes.TextXlsx, "BaoCaoHopDongBB.xlsx");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }
        [HttpPost]
        public virtual IActionResult ExportReportProcuringAgency(ReportProcuringAgencySearchModel searchModel)
        {
            if (!string.IsNullOrEmpty(searchModel.StringProcuringAgencyId))
            {
                var listProcuringAgency = searchModel.StringProcuringAgencyId.Split(',').ToList();
                searchModel.SelectedProcuringAgencyIds = listProcuringAgency.Select(c => (int)c.ToNumber()).ToList();
            }
            try
            {
                var report = _reportService.GetReportProcuringAgency(
                    SelectedProcuringAgencyIds: searchModel.SelectedProcuringAgencyIds.ToList(),
                    datetimeFrom: searchModel.datetimeFrom,
                    datetimeTo: searchModel.datetimeTo);
                byte[] bytes;
                using (var stream = new MemoryStream())
                {
                    _exportManager.ExportContractProcuringAgency(report, stream, Convert.ToDateTime(searchModel.datetimeFrom), Convert.ToDateTime(searchModel.datetimeTo));
                    bytes = stream.ToArray();
                }
                return File(bytes, MimeTypes.TextXlsx, "BaoCaoLichSuHDvaCDT.xlsx");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }
        [HttpPost]
        public virtual IActionResult ExportReportContractDealine(ReportContractDealineSearchModel searchModel)
        {
            if (!string.IsNullOrEmpty(searchModel.stringConstructionIds))
            {
                var lstConstruction = searchModel.stringConstructionIds.Split(',').ToList();
                searchModel.SelectedConstructionIds = lstConstruction.Select(c => (int)c.ToNumber()).ToList();
            }
            try
            {
                var report = _reportService.ReportContractDealine(
                    SelectedConstructionIds: searchModel.SelectedConstructionIds,
                    datetimeFrom: searchModel.datetimeFrom,
                    datetimeTo: searchModel.datetimeTo);
                byte[] bytes;
                using (var stream = new MemoryStream())
                {
                    _exportManager.ExportContractDeadline(report, stream, Convert.ToDateTime(searchModel.datetimeFrom), Convert.ToDateTime(searchModel.datetimeTo));
                    bytes = stream.ToArray();
                }
                return File(bytes, MimeTypes.TextXlsx, "BaoCaoDSHopDongSapDenHan.xlsx");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }
        [HttpPost]
        public virtual IActionResult ExportReportTaskByUnit(ReportTaskByUniySearchModel searchModel)
        {
            if (!string.IsNullOrEmpty(searchModel.stringConstructionIds))
            {
                var lstConstruction = searchModel.stringConstructionIds.Split(',').ToList();
                searchModel.SelectedConstructionIds = lstConstruction.Select(c => (int)c.ToNumber()).ToList();
            }

            try
            {
                var report = _reportService.GetReportTaskByUnit(SelectedConstructionIds: searchModel.SelectedConstructionIds,
                    SelectedUnitIds: searchModel.SelectedUnitIds, 
                    dateFrom: searchModel.dateFrom, dateTo: searchModel.dateTo);
                byte[] bytes;
                using (var stream = new MemoryStream())
                {
                     _exportManager.ExportTaskByUnit(report,stream);
                    bytes = stream.ToArray();
                }
                return File(bytes, MimeTypes.TextXlsx, "BaoCaoDSCongViec.xlsx");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("ReportTaskByUnit");
            }
        }
        [HttpPost]
        public virtual IActionResult ExportReportContractUnfinish(ReportContractUnfinishSearchModel searchModel)
        {
            if (!string.IsNullOrEmpty(searchModel.stringConstructionIds))
            {
                var lstConstruction = searchModel.stringConstructionIds.Split(',').ToList();
                searchModel.SelectedConstructionIds = lstConstruction.Select(c => (int)c.ToNumber()).ToList();
            }
            try
            {
                var report = _reportService.GetReportContractUnfinishes(ConstructionIds: searchModel.SelectedConstructionIds, time: searchModel.time);
                byte[] bytes;
                using (var stream = new MemoryStream())
                {
                    _exportManager.ExportContractUnfinish(report, stream, Convert.ToDateTime(searchModel.time));
                    bytes = stream.ToArray();
                }
                return File(bytes, MimeTypes.TextXlsx, "BaoCaoSLDoDang.xlsx");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }
        #endregion
    }
}
