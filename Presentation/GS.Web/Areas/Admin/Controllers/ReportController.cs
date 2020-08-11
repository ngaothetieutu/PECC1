using Microsoft.AspNetCore.Mvc;
using GS.Services.Security;
using GS.Web.Areas.Admin.Factories;
using GS.Web.Areas.Admin.Models.Reports;

namespace GS.Web.Areas.Admin.Controllers
{
    public partial class ReportController : BaseAdminController
    {
        #region Fields

        private readonly IPermissionService _permissionService;
        private readonly IReportModelFactory _reportModelFactory;

        #endregion

        #region Ctor

        public ReportController(
            IPermissionService permissionService,
            IReportModelFactory reportModelFactory)
        {
            this._permissionService = permissionService;
            this._reportModelFactory = reportModelFactory;
        }

        #endregion

        #region Methods

     
        #region Country sales

        public virtual IActionResult CountrySales()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.OrderCountryReport))
                return AccessDeniedView();

            //prepare model
            var model = _reportModelFactory.PrepareCountrySalesSearchModel(new CountryReportSearchModel());

            return View(model);
        }

        

        #endregion

        #region Customer reports

        

        [HttpPost]
        public virtual IActionResult ReportRegisteredCustomersList(RegisteredCustomersReportSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _reportModelFactory.PrepareRegisteredCustomersReportListModel(searchModel);

            return Json(model);
        }        

        #endregion

        #endregion
    }
}
