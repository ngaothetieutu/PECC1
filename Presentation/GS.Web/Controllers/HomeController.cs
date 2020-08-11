using Microsoft.AspNetCore.Mvc;
using GS.Web.Framework.Mvc.Filters;
using GS.Web.Framework.Security;
using GS.Web.Models.Customer;
using GS.Core.Domain.Security;
using GS.Core.Domain.Customers;
using GS.Services.Customers;
using GS.Core.Domain.Localization;
using GS.Services.Authentication;
using GS.Services.Events;
using GS.Services.Localization;
using GS.Services.Logging;
using GS.Web.Factories;
using GS.Web.Framework.Security.Captcha;
using GS.Web.Models.Works;
using System;
using GS.Core.Domain.Messages;
using GS.Services.Messages;
using GS.Core;

namespace GS.Web.Controllers
{
    public partial class HomeController : BasePublicController
    {
        #region fields
        private readonly CaptchaSettings _captchaSettings;
        private readonly CustomerSettings _customerSettings;
        private readonly ICustomerService _customerService;
        private readonly LocalizationSettings _localizationSettings;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ICustomerModelFactory _customerModelFactory;
        private readonly INotificationService _notificationService;
        private readonly IWebHelper _webHelper;
        #endregion
        #region ctor
        public HomeController(
             CaptchaSettings captchaSettings,
             CustomerSettings customerSettings,
             ICustomerService customerService,
             LocalizationSettings localizationSettings,
             ICustomerRegistrationService customerRegistrationService,
             IAuthenticationService authenticationService,
             IEventPublisher eventPublisher,
             ILocalizationService localizationService,
             ICustomerActivityService customerActivityService,
             INotificationService notificationService,
             ICustomerModelFactory customerModelFactory,
             IWebHelper webHelper
            )
        {
            this._webHelper = webHelper;
            this._notificationService = notificationService;
            this._eventPublisher = eventPublisher;
            this._captchaSettings = captchaSettings;
            this._customerSettings = customerSettings;
            this._customerService = customerService;
            this._localizationSettings = localizationSettings;
            this._localizationService = localizationService;
            this._customerRegistrationService = customerRegistrationService;
            this._authenticationService = authenticationService;
            this._customerActivityService = customerActivityService;
            this._customerModelFactory = customerModelFactory;
        }
        #endregion
        #region   Utilities


        [HttpsRequirement(SslRequirement.Yes)]
        //available even when a store is closed
        [CheckAccessClosedStore(true)]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual IActionResult Index(bool? checkoutAsGuest)
        {
            var model = _customerModelFactory.PrepareLoginModel(checkoutAsGuest);
            return View(model);
        }
      
        public virtual IActionResult Test()
        {
            var model = new TestModel();
            model.WorkFileIds = "78,79,96";
            model.WorkDate = DateTime.Now.AddDays(-3);
            model.WorkNumber = 10001.25m;
            model.WorkCurrency = "10000000";
            return View(model);
        }
        [HttpPost]
        public virtual IActionResult Test(TestModel model)
        {
            //var inti = 0;
            var _i = model.WorkCurrency.ToNumber();
            return View(model);
        }
        [HttpPost]
        //[ValidateCaptcha]
        //available even when a store is closed
        //[CheckAccessClosedStore(true)]
        //available even when navigation is not allowed
        //[CheckAccessPublicStore(true)]
        [PublicAntiForgery]
        public virtual IActionResult Index(LoginModel model, string returnUrl, bool captchaValid)
        {
            //validate CAPTCHA
            if (_captchaSettings.Enabled && _captchaSettings.ShowOnLoginPage && !captchaValid)
            {
                ModelState.AddModelError("", _captchaSettings.GetWrongCaptchaMessage(_localizationService));
            }

            if (ModelState.IsValid)
            {
                if (_customerSettings.UsernamesEnabled && model.Username != null)
                {
                    model.Username = model.Username.Trim();
                }
                var loginResult = _customerRegistrationService.ValidateCustomer(_customerSettings.UsernamesEnabled ? model.Username : model.Email, model.Password);
                switch (loginResult)
                {
                    case CustomerLoginResults.Successful:
                        {
                            var customer = _customerSettings.UsernamesEnabled
                                ? _customerService.GetCustomerByUsername(model.Username)
                                : _customerService.GetCustomerByEmail(model.Email);


                            //sign in new customer
                            _authenticationService.SignIn(customer, model.RememberMe);

                            //raise event       
                            _eventPublisher.Publish(new CustomerLoggedinEvent(customer));

                            //activity log
                            _customerActivityService.InsertActivity(customer, "PublicStore.Login",
                                _localizationService.GetResource("ActivityLog.PublicStore.Login"), customer);
                            //tao notification
                            var notifyItem = Notification.CreateSimple(customer.Id
                                , _localizationService.GetResource("Account.Login.Notification.Subject")
                                , string.Format(_localizationService.GetResource("Account.Login.Notification.Body"), DateTime.Now.toDateVNString(true), _webHelper.GetCurrentIpAddress())
                                );
                            _notificationService.InsertNotification(notifyItem);

                            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                                return Redirect("AppWork/Index");

                            return Redirect(returnUrl);
                        }
                    case CustomerLoginResults.CustomerNotExist:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.CustomerNotExist"));
                        model.messageResult = _localizationService.GetResource("Account.Login.WrongCredentials.CustomerNotExist");
                        break;
                    case CustomerLoginResults.Deleted:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.Deleted"));
                        model.messageResult = _localizationService.GetResource("Account.Login.WrongCredentials.Deleted");
                        break;
                    case CustomerLoginResults.NotActive:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.NotActive"));
                        model.messageResult = _localizationService.GetResource("Account.Login.WrongCredentials.NotActive");
                        break;
                    case CustomerLoginResults.NotRegistered:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.NotRegistered"));
                        model.messageResult = _localizationService.GetResource("Account.Login.WrongCredentials.NotActive");
                        break;
                    case CustomerLoginResults.LockedOut:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.LockedOut"));
                        model.messageResult = _localizationService.GetResource("Account.Login.WrongCredentials.LockedOut");
                        break;
                    case CustomerLoginResults.WrongPassword:
                    default:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials"));
                        model.messageResult = _localizationService.GetResource("Account.Login.WrongCredentials.WrongCredentials");
                        break;
                }
            }
            //If we got this far, something failed, redisplay form
            //model = _customerModelFactory.PrepareLoginModel(model.CheckoutAsGuest);
            return View(model);
        }
        #endregion
    }
}