﻿using System;
using System.Collections.Generic;
using System.Linq;
using GS.Core;
using GS.Core.Domain.Customers;
using GS.Core.Domain.Forums;
using GS.Core.Domain.Messages;
using GS.Data;
using GS.Services.Customers;
using GS.Services.Forums;
using GS.Services.Helpers;
using GS.Services.Messages;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Framework.Extensions;
using GS.Web.Models.Common;
using GS.Web.Models.PrivateMessages;

namespace GS.Web.Factories
{
    /// <summary>
    /// Represents the private message model factory
    /// </summary>
    public partial class PrivateMessagesModelFactory : IPrivateMessagesModelFactory
    {
        #region Fields
        private readonly IDbContext _dbContext;
        private readonly CustomerSettings _customerSettings;
        private readonly ForumSettings _forumSettings;
        private readonly ICustomerService _customerService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IForumService _forumService;
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;
        private readonly INotificationService _notificationService;

        #endregion

        #region Ctor

        public PrivateMessagesModelFactory(CustomerSettings customerSettings,
            IDbContext dbContext,
            ForumSettings forumSettings,
            ICustomerService customerService,
            IDateTimeHelper dateTimeHelper,
            IForumService forumService,
            IStoreContext storeContext,
            IWorkContext workContext,
            INotificationService notificationService)
        {
            this._dbContext = dbContext;
            this._customerSettings = customerSettings;
            this._forumSettings = forumSettings;
            this._customerService = customerService;
            this._dateTimeHelper = dateTimeHelper;
            this._forumService = forumService;
            this._storeContext = storeContext;
            this._workContext = workContext;
            this._notificationService = notificationService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare the private message index model
        /// </summary>
        /// <param name="page">Number of items page; pass null to disable paging</param>
        /// <param name="tab">Tab name</param>
        /// <returns>Private message index model</returns>
        public virtual PrivateMessageIndexModel PreparePrivateMessageIndexModel(int? page, string tab)
        {
            var inboxPage = 0;
            var sentItemsPage = 0;
            var sentItemsTabSelected = false;

            switch (tab)
            {
                case "inbox":
                    if (page.HasValue)
                    {
                        inboxPage = page.Value;
                    }
                    break;
                case "sent":
                    if (page.HasValue)
                    {
                        sentItemsPage = page.Value;
                    }
                    sentItemsTabSelected = true;
                    break;
                default:
                    break;
            }

            var model = new PrivateMessageIndexModel
            {
                InboxPage = inboxPage,
                SentItemsPage = sentItemsPage,
                SentItemsTabSelected = sentItemsTabSelected
            };

            return model;
        }

        /// <summary>
        /// Prepare the inbox model
        /// </summary>
        /// <param name="page">Number of items page</param>
        /// <param name="tab">Tab name</param>
        /// <returns>Private message list model</returns>
        public virtual PrivateMessageListModel PrepareInboxModel(int page, string tab)
        {
            if (page > 0)
            {
                page -= 1;
            }

            var pageSize = _forumSettings.PrivateMessagesPageSize;

            var messages = new List<PrivateMessageModel>();

            var list = _forumService.GetAllPrivateMessages(_storeContext.CurrentStore.Id,
                0, _workContext.CurrentCustomer.Id, null, null, false, string.Empty, page, pageSize);
            foreach (var pm in list)
                messages.Add(PreparePrivateMessageModel(pm));

            var pagerModel = new PagerModel
            {
                PageSize = list.PageSize,
                TotalRecords = list.TotalCount,
                PageIndex = list.PageIndex,
                ShowTotalSummary = false,
                RouteActionName = "PrivateMessagesPaged",
                UseRouteLinks = true,
                RouteValues = new PrivateMessageRouteValues { pageNumber = page, tab = tab }
            };

            var model = new PrivateMessageListModel
            {
                Messages = messages,
                PagerModel = pagerModel
            };

            return model;
        }

        /// <summary>
        /// Prepare the sent model
        /// </summary>
        /// <param name="page">Number of items page</param>
        /// <param name="tab">Tab name</param>
        /// <returns>Private message list model</returns>
        public virtual PrivateMessageListModel PrepareSentModel(int page, string tab)
        {
            if (page > 0)
            {
                page -= 1;
            }

            var pageSize = _forumSettings.PrivateMessagesPageSize;

            var messages = new List<PrivateMessageModel>();

            var list = _forumService.GetAllPrivateMessages(_storeContext.CurrentStore.Id,
                _workContext.CurrentCustomer.Id, 0, null, false, null, string.Empty, page, pageSize);
            foreach (var pm in list)
                messages.Add(PreparePrivateMessageModel(pm));

            var pagerModel = new PagerModel
            {
                PageSize = list.PageSize,
                TotalRecords = list.TotalCount,
                PageIndex = list.PageIndex,
                ShowTotalSummary = false,
                RouteActionName = "PrivateMessagesPaged",
                UseRouteLinks = true,
                RouteValues = new PrivateMessageRouteValues { pageNumber = page, tab = tab }
            };

            var model = new PrivateMessageListModel
            {
                Messages = messages,
                PagerModel = pagerModel
            };

            return model;
        }

        /// <summary>
        /// Prepare the send private message model
        /// </summary>
        /// <param name="customerTo">Customer, recipient of the message</param>
        /// <param name="replyToPM">Private message, pass if reply to a previous message is need</param>
        /// <returns>Send private message model</returns>
        public virtual SendPrivateMessageModel PrepareSendPrivateMessageModel(Customer customerTo, PrivateMessage replyToPM)
        {
            if (customerTo == null)
                throw new ArgumentNullException(nameof(customerTo));

            var model = new SendPrivateMessageModel
            {
                ToCustomerId = customerTo.Id,
                CustomerToName = _customerService.FormatUserName(customerTo),
                AllowViewingToProfile = _customerSettings.AllowViewingProfiles && !customerTo.IsGuest()
            };

            if (replyToPM == null)
                return model;

            if (replyToPM.ToCustomerId == _workContext.CurrentCustomer.Id ||
                replyToPM.FromCustomerId == _workContext.CurrentCustomer.Id)
            {
                model.ReplyToMessageId = replyToPM.Id;
                model.Subject = $"Re: {replyToPM.Subject}";
            }

            return model;
        }

        /// <summary>
        /// Prepare the private message model
        /// </summary>
        /// <param name="pm">Private message</param>
        /// <returns>Private message model</returns>
        public virtual PrivateMessageModel PreparePrivateMessageModel(PrivateMessage pm)
        {
            if (pm == null)
                throw new ArgumentNullException(nameof(pm));

            var model = new PrivateMessageModel
            {
                Id = pm.Id,
                FromCustomerId = pm.FromCustomer.Id,
                CustomerFromName = _customerService.FormatUserName(pm.FromCustomer),
                AllowViewingFromProfile = _customerSettings.AllowViewingProfiles && pm.FromCustomer != null && !pm.FromCustomer.IsGuest(),
                ToCustomerId = pm.ToCustomer.Id,
                CustomerToName = _customerService.FormatUserName(pm.ToCustomer),
                AllowViewingToProfile = _customerSettings.AllowViewingProfiles && pm.ToCustomer != null && !pm.ToCustomer.IsGuest(),
                Subject = pm.Subject,
                Message = _forumService.FormatPrivateMessageText(pm),
                CreatedOn = _dateTimeHelper.ConvertToUserTime(pm.CreatedOnUtc, DateTimeKind.Utc),
                IsRead = pm.IsRead,
            };
            return model;
        }
        #endregion
        #region Notification
        public NotificationSearchModel PrepareNotificationSearchModel(NotificationSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }
        public IList<NotificationModel> PrepareNotificationListModel(NotificationSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            //get items
            var items = _notificationService.GetNotifications(searchModel.CustomerId, null, 0, 7);
            var model = items.Select(store => store.ToModel<NotificationModel>()).ToList();
            foreach (NotificationModel nt in model.Where(c=>c.StatusId != 3))
            {
                _notificationService.UpdateStatusNotificationToRead(nt.Id);
            }
            return model;
        }
        #endregion
    }
}