using System;
using System.Collections.Generic;
using System.Linq;
using GS.Core;
using GS.Core.Data;
using GS.Core.Domain.Customers;
using GS.Core.Domain.Gdpr;
using GS.Data.Extensions;
using GS.Services.Authentication.External;
using GS.Services.Blogs;
using GS.Services.Catalog;
using GS.Services.Common;
using GS.Services.Customers;
using GS.Services.Events;
using GS.Services.Forums;
using GS.Services.Messages;
using GS.Services.News;
using GS.Services.Stores;

namespace GS.Services.Gdpr
{
    /// <summary>
    /// Represents the GDPR service
    /// </summary>
    public partial class GdprService : IGdprService
    {
        #region Fields

        private readonly IAddressService _addressService;
        private readonly IBlogService _blogService;
        private readonly ICustomerService _customerService;
        private readonly IExternalAuthenticationService _externalAuthenticationService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IForumService _forumService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly INewsService _newsService;
        private readonly IRepository<GdprConsent> _gdprConsentRepository;
        private readonly IRepository<GdprLog> _gdprLogRepository;
        private readonly IStoreService _storeService;

        #endregion

        #region Ctor

        public GdprService(IAddressService addressService,
            IBlogService blogService,
            ICustomerService customerService,
            IExternalAuthenticationService externalAuthenticationService,
            IEventPublisher eventPublisher,
            IForumService forumService,
            IGenericAttributeService genericAttributeService,
            INewsService newsService,
            INewsLetterSubscriptionService newsLetterSubscriptionService,
            IRepository<GdprConsent> gdprConsentRepository,
            IRepository<GdprLog> gdprLogRepository,
            IStoreService storeService)
        {
            this._addressService = addressService;
            this._blogService = blogService;
            this._customerService = customerService;
            this._externalAuthenticationService = externalAuthenticationService;
            this._eventPublisher = eventPublisher;
            this._forumService = forumService;
            this._genericAttributeService = genericAttributeService;
            this._newsService = newsService;
            this._newsLetterSubscriptionService = newsLetterSubscriptionService;
            this._gdprConsentRepository = gdprConsentRepository;
            this._gdprLogRepository = gdprLogRepository;
            this._storeService = storeService;
        }

        #endregion

        #region Methods

        #region GDPR consent

        /// <summary>
        /// Get a GDPR consent
        /// </summary>
        /// <param name="gdprConsentId">The GDPR consent identifier</param>
        /// <returns>GDPR consent</returns>
        public virtual GdprConsent GetConsentById(int gdprConsentId)
        {
            if (gdprConsentId == 0)
                return null;

            return _gdprConsentRepository.GetById(gdprConsentId);
        }

        /// <summary>
        /// Get all GDPR consents
        /// </summary>
        /// <returns>GDPR consent</returns>
        public virtual IList<GdprConsent> GetAllConsents()
        {
            var query = from c in _gdprConsentRepository.Table
                        orderby c.DisplayOrder, c.Id
                        select c;
            var gdprConsents = query.ToList();
            return gdprConsents;
        }

        /// <summary>
        /// Insert a GDPR consent
        /// </summary>
        /// <param name="gdprConsent">GDPR consent</param>
        public virtual void InsertConsent(GdprConsent gdprConsent)
        {
            if (gdprConsent == null)
                throw new ArgumentNullException(nameof(gdprConsent));

            _gdprConsentRepository.Insert(gdprConsent);

            //event notification
            _eventPublisher.EntityInserted(gdprConsent);
        }

        /// <summary>
        /// Update the GDPR consent
        /// </summary>
        /// <param name="gdprConsent">GDPR consent</param>
        public virtual void UpdateConsent(GdprConsent gdprConsent)
        {
            if (gdprConsent == null)
                throw new ArgumentNullException(nameof(gdprConsent));

            _gdprConsentRepository.Update(gdprConsent);

            //event notification
            _eventPublisher.EntityUpdated(gdprConsent);
        }

        /// <summary>
        /// Delete a GDPR consent
        /// </summary>
        /// <param name="gdprConsent">GDPR consent</param>
        public virtual void DeleteConsent(GdprConsent gdprConsent)
        {
            if (gdprConsent == null)
                throw new ArgumentNullException(nameof(gdprConsent));

            _gdprConsentRepository.Delete(gdprConsent);

            //event notification
            _eventPublisher.EntityDeleted(gdprConsent);
        }

        /// <summary>
        /// Gets the latest selected value (a consent is accepted or not by a customer)
        /// </summary>
        /// <param name="consentId">Consent identifier</param>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>Result; null if previous a customer hasn't been asked</returns>
        public virtual bool? IsConsentAccepted(int consentId, int customerId)
        {
            //get latest record
            var log = GetAllLog(customerId: customerId, consentId: consentId, pageIndex: 0, pageSize: 1).FirstOrDefault();
            if (log == null)
                return null;

            switch (log.RequestType)
            {
                case GdprRequestType.ConsentAgree:
                    return true;
                case GdprRequestType.ConsentDisagree:
                    return false;
                default:
                    return null;
            }
        }
        #endregion

        #region GDPR log

        /// <summary>
        /// Get a GDPR log
        /// </summary>
        /// <param name="gdprLogId">The GDPR log identifier</param>
        /// <returns>GDPR log</returns>
        public virtual GdprLog GetLogById(int gdprLogId)
        {
            if (gdprLogId == 0)
                return null;

            return _gdprLogRepository.GetById(gdprLogId);
        }

        /// <summary>
        /// Get all GDPR log records
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="consentId">Consent identifier</param>
        /// <param name="customerInfo">Customer info (Exact match)</param>
        /// <param name="requestType">GDPR request type</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>GDPR log records</returns>
        public virtual IPagedList<GdprLog> GetAllLog(int customerId = 0, int consentId = 0,
            string customerInfo = "", GdprRequestType? requestType = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _gdprLogRepository.Table;
            if (customerId > 0)
            {
                query = query.Where(log => log.CustomerId == customerId);
            }

            if (consentId > 0)
            {
                query = query.Where(log => log.ConsentId == consentId);
            }

            if (!string.IsNullOrEmpty(customerInfo))
            {
                query = query.Where(log => log.CustomerInfo == customerInfo);
            }

            if (requestType != null)
            {
                var requestTypeId = (int)requestType;
                query = query.Where(log => log.RequestTypeId == requestTypeId);
            }

            query = query.OrderByDescending(log => log.CreatedOnUtc).ThenByDescending(log => log.Id);
            return new PagedList<GdprLog>(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Insert a GDPR log
        /// </summary>
        /// <param name="gdprLog">GDPR log</param>
        public virtual void InsertLog(GdprLog gdprLog)
        {
            if (gdprLog == null)
                throw new ArgumentNullException(nameof(gdprLog));

            _gdprLogRepository.Insert(gdprLog);

            //event notification
            _eventPublisher.EntityInserted(gdprLog);
        }

        /// <summary>
        /// Insert a GDPR log
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="consentId">Consent identifier</param>
        /// <param name="requestType">Request type</param>
        /// <param name="requestDetails">Request details</param>
        public virtual void InsertLog(Customer customer, int consentId, GdprRequestType requestType, string requestDetails)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            var gdprLog = new GdprLog
            {
                CustomerId = customer.Id,
                ConsentId = consentId,
                CustomerInfo = customer.Email,
                RequestType = requestType,
                RequestDetails = requestDetails,
                CreatedOnUtc = DateTime.UtcNow
            };
            InsertLog(gdprLog);
        }

        /// <summary>
        /// Update the GDPR log
        /// </summary>
        /// <param name="gdprLog">GDPR log</param>
        public virtual void UpdateLog(GdprLog gdprLog)
        {
            if (gdprLog == null)
                throw new ArgumentNullException(nameof(gdprLog));

            _gdprLogRepository.Update(gdprLog);

            //event notification
            _eventPublisher.EntityUpdated(gdprLog);
        }

        /// <summary>
        /// Delete a GDPR log
        /// </summary>
        /// <param name="gdprLog">GDPR log</param>
        public virtual void DeleteLog(GdprLog gdprLog)
        {
            if (gdprLog == null)
                throw new ArgumentNullException(nameof(gdprLog));

            _gdprLogRepository.Delete(gdprLog);

            //event notification
            _eventPublisher.EntityDeleted(gdprLog);
        }

        #endregion

        #region Customer

        /// <summary>
        /// Permanent delete of customer
        /// </summary>
        /// <param name="customer">Customer</param>
        public virtual void PermanentDeleteCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            //blog comments
            var blogComments = _blogService.GetAllComments(customerId: customer.Id);
            _blogService.DeleteBlogComments(blogComments);

            //news comments
            var newsComments = _newsService.GetAllComments(customerId: customer.Id);
            _newsService.DeleteNewsComments(newsComments);

          

            //external authentication record
            foreach (var ear in customer.ExternalAuthenticationRecords)
                _externalAuthenticationService.DeleteExternalAuthenticationRecord(ear);

            //forum subscriptions
            var forumSubscriptions = _forumService.GetAllSubscriptions(customer.Id);
            foreach (var forumSubscription in forumSubscriptions)
                _forumService.DeleteSubscription(forumSubscription);


            //private messages (sent)
            foreach (var pm in _forumService.GetAllPrivateMessages(0, customer.Id, 0, null, null, null, null))
                _forumService.DeletePrivateMessage(pm);

            //private messages (received)
            foreach (var pm in _forumService.GetAllPrivateMessages(0, 0, customer.Id, null, null, null, null))
                _forumService.DeletePrivateMessage(pm);

            //newsletter
            var allStores = _storeService.GetAllStores();
            foreach (var store in allStores)
            {
                var newsletter = _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(customer.Email, store.Id);
                if (newsletter != null)
                    _newsLetterSubscriptionService.DeleteNewsLetterSubscription(newsletter);
            }

            //addresses
            foreach (var address in customer.Addresses)
            {
                _customerService.RemoveCustomerAddress(customer, address);
                _customerService.UpdateCustomer(customer);
                //now delete the address record
                _addressService.DeleteAddress(address);
            }

            //generic attributes
            var keyGroup = customer.GetUnproxiedEntityType().Name;
            var genericAttributes = _genericAttributeService.GetAttributesForEntity(customer.Id, keyGroup);
            _genericAttributeService.DeleteAttributes(genericAttributes);

            //ignore ActivityLog
            //ignore ForumPost, ForumTopic, ignore ForumPostVote
            //ignore Log
            //ignore PollVotingRecord
            //ignore ProductReviewHelpfulness
            //ignore RecurringPayment 
            //ignore ReturnRequest
            //ignore RewardPointsHistory
            //and we do not delete orders

            //remove from Registered role, add to Guest one
            if (customer.IsRegistered())
            {
                var registeredRole = _customerService.GetCustomerRoleBySystemName(GSCustomerDefaults.RegisteredRoleName);
                customer.CustomerCustomerRoleMappings
                    .Remove(customer.CustomerCustomerRoleMappings.FirstOrDefault(mapping => mapping.CustomerRoleId == registeredRole.Id));
            }

            if (!customer.IsGuest())
            {
                var guestRole = _customerService.GetCustomerRoleBySystemName(GSCustomerDefaults.GuestsRoleName);
                customer.CustomerCustomerRoleMappings.Add(new CustomerCustomerRoleMapping { CustomerRole = guestRole });
            }

            var email = customer.Email;

            //clear other information
            customer.Email = string.Empty;
            customer.EmailToRevalidate = string.Empty;
            customer.Username = string.Empty;
            customer.Active = false;
            customer.Deleted = true;
            _customerService.UpdateCustomer(customer);

            //raise event
            _eventPublisher.Publish(new CustomerPermanentlyDeleted(customer.Id, email));
        }

        #endregion

        #endregion
    }
}