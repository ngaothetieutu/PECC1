using System;
using Microsoft.AspNetCore.Mvc;
using GS.Core.Domain.Contracts;
using GS.Services.Contracts;
using GS.Services.Localization;
using GS.Services.Logging;
using GS.Services.Security;
using GS.Web.Areas.Admin.Factories;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Areas.Admin.Models.Contract;
using GS.Web.Framework.Mvc;
using GS.Web.Framework.Mvc.Filters;

namespace GS.Web.Areas.Admin.Controllers
{
    public partial class ContractAttributeController : BaseAdminController
    {
        #region Fields
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IContractAttributeModelFactory _contractAttributeModelFactory;
        private readonly IContractAttributeService _contractAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IPermissionService _permissionService;
        #endregion

        #region Ctor
        public ContractAttributeController(ICustomerActivityService customerActivityService,
            IContractAttributeModelFactory contractAttributeModelFactory,
            IContractAttributeService contractAttributeService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            IPermissionService permissionService)
        {
            this._customerActivityService = customerActivityService;
            this._contractAttributeModelFactory = contractAttributeModelFactory;
            this._contractAttributeService = contractAttributeService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._permissionService = permissionService;
        }
        #endregion

        #region Utilities
        protected virtual void UpdateAttributeLocales(ContractAttribute contractAttribute, ContractAttributeModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(contractAttribute,
                   x => x.Name,
                   localized.Name,
                   localized.LanguageId);
            }
        }

        protected virtual void UpdateValueLocales(ContractAttributeValue contractAttributeValue, ContractAttributeValueModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(contractAttributeValue,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);
            }
        }
        #endregion

        #region Customer attributes
        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //select "customer form fields" tab
            //SaveSelectedTabName("tab-contractformfields");

            //we just redirect a user to the customer settings page
            return RedirectToAction("ContractAttribute", "Setting");
        }

        [HttpPost]
        public virtual IActionResult List(ContractAttributeSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _contractAttributeModelFactory.PrepareContractAttributeListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //prepare model
            var model = _contractAttributeModelFactory.PrepareContractAttributeModel(new ContractAttributeModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(ContractAttributeModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var contractAttribute = model.ToEntity<ContractAttribute>();
                _contractAttributeService.InsertContractAttribute(contractAttribute);

                //activity log
                _customerActivityService.InsertActivity("AddNewContractAttribute",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewContractAttribute"), contractAttribute.Id),
                    contractAttribute);

                //locales
                UpdateAttributeLocales(contractAttribute, model);

                SuccessNotification(_localizationService.GetResource("Admin.Contract.ContractAttribute.Added"));

                if (!continueEditing)
                    return RedirectToAction("List");

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = contractAttribute.Id });
            }

            //prepare model
            model = _contractAttributeModelFactory.PrepareContractAttributeModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //try to get a customer attribute with the specified id
            var contractAttribute = _contractAttributeService.GetContractAttributeById(id);
            if (contractAttribute == null)
                return RedirectToAction("List");

            //prepare model
            var model = _contractAttributeModelFactory.PrepareContractAttributeModel(null, contractAttribute);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(ContractAttributeModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            var contractAttribute = _contractAttributeService.GetContractAttributeById(model.Id);
            if (contractAttribute == null)
                //no customer attribute found with the specified id
                return RedirectToAction("List");

            if (!ModelState.IsValid)
                //if we got this far, something failed, redisplay form
                return View(model);

            contractAttribute = model.ToEntity(contractAttribute);
            _contractAttributeService.UpdateContractAttribute(contractAttribute);

            //activity log
            _customerActivityService.InsertActivity("EditContractAttribute",
                string.Format(_localizationService.GetResource("ActivityLog.EditContractAttribute"), contractAttribute.Id),
                contractAttribute);

            //locales
            UpdateAttributeLocales(contractAttribute, model);

            SuccessNotification(_localizationService.GetResource("Admin.Contract.ContractAttributes.Updated"));

            if (!continueEditing)
                return RedirectToAction("List");

            //selected tab
            SaveSelectedTabName();

            return RedirectToAction("Edit", new { id = contractAttribute.Id });
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            var contractAttribute = _contractAttributeService.GetContractAttributeById(id);
            _contractAttributeService.DeleteContractAttribute(contractAttribute);

            //activity log
            _customerActivityService.InsertActivity("DeleteContractAttribute",
                string.Format(_localizationService.GetResource("ActivityLog.DeleteContractAttribute"), contractAttribute.Id),
                contractAttribute);

            SuccessNotification(_localizationService.GetResource("Admin.Contract.ContractAttributes.Deleted"));
            return RedirectToAction("List");
        }
        #endregion

        #region Customer attribute values
        [HttpPost]
        public virtual IActionResult ValueList(ContractAttributeValueSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedKendoGridJson();

            //try to get a customer attribute with the specified id
            var contractAttribute = _contractAttributeService.GetContractAttributeById(searchModel.ContractAttributeId)
                ?? throw new ArgumentException("No contract attribute found with the specified id");

            //prepare model
            var model = _contractAttributeModelFactory.PrepareContractAttributeValueListModel(searchModel, contractAttribute);

            return Json(model);
        }

        public virtual IActionResult ValueCreatePopup(int contractAttributeId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //try to get a customer attribute with the specified id
            var contractAttribute = _contractAttributeService.GetContractAttributeById(contractAttributeId);
            if (contractAttribute == null)
                return RedirectToAction("List");

            //prepare model
            var model = _contractAttributeModelFactory
                .PrepareContractAttributeValueModel(new ContractAttributeValueModel(), contractAttribute, null);

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult ValueCreatePopup(ContractAttributeValueModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //try to get a customer attribute with the specified id
            var contractAttribute = _contractAttributeService.GetContractAttributeById(model.ContractAttributeId);
            if (contractAttribute == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var cav = model.ToEntity<ContractAttributeValue>();
                _contractAttributeService.InsertContractAttributeValue(cav);

                //activity log
                _customerActivityService.InsertActivity("AddNewContractAttributeValue",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewContractAttributeValue"), cav.Id), cav);

                UpdateValueLocales(cav, model);

                ViewBag.RefreshPage = true;

                return View(model);
            }

            //prepare model
            model = _contractAttributeModelFactory.PrepareContractAttributeValueModel(model, contractAttribute, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult ValueEditPopup(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            var contractAttributeValue = _contractAttributeService.GetContractAttributeValueById(id);
            if (contractAttributeValue == null)
                return RedirectToAction("List");

            //try to get a contract attribute with the specified id
            var contractAttribute = _contractAttributeService.GetContractAttributeById(contractAttributeValue.ContractAttributeId);
            if (contractAttribute == null)
                return RedirectToAction("List");

            //prepare model
            var model = _contractAttributeModelFactory.PrepareContractAttributeValueModel(null, contractAttribute, contractAttributeValue);

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult ValueEditPopup(ContractAttributeValueModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //try to get a customer attribute value with the specified id
            var contractAttributeValue = _contractAttributeService.GetContractAttributeValueById(model.Id);
            if (contractAttributeValue == null)
                return RedirectToAction("List");

            //try to get a customer attribute with the specified id
            var contractAttribute = _contractAttributeService.GetContractAttributeById(contractAttributeValue.ContractAttributeId);
            if (contractAttribute == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                contractAttributeValue = model.ToEntity(contractAttributeValue);
                _contractAttributeService.UpdateContractAttributeValue(contractAttributeValue);

                //activity log
                _customerActivityService.InsertActivity("EditContractAttributeValue",
                    string.Format(_localizationService.GetResource("ActivityLog.EditContractAttributeValue"), contractAttributeValue.Id),
                    contractAttributeValue);

                UpdateValueLocales(contractAttributeValue, model);

                ViewBag.RefreshPage = true;

                return View(model);
            }

            //prepare model
            model = _contractAttributeModelFactory.PrepareContractAttributeValueModel(model, contractAttribute, contractAttributeValue, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult ValueDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //try to get a contract attribute value with the specified id
            var contractAttributeValue = _contractAttributeService.GetContractAttributeValueById(id)
                ?? throw new ArgumentException("No contract attribute value found with the specified id", nameof(id));

            _contractAttributeService.DeleteContractAttributeValue(contractAttributeValue);

            //activity log
            _customerActivityService.InsertActivity("DeleteContractAttributeValue",
                string.Format(_localizationService.GetResource("ActivityLog.DeleteContractAttributeValue"), contractAttributeValue.Id),
                contractAttributeValue);

            return new NullJsonResult();
        }
        #endregion
    }
}
