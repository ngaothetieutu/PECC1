using GS.Core.Domain.Customers;
using GS.Core.Domain.Forums;
using GS.Web.Models.PrivateMessages;
using System.Collections.Generic;

namespace GS.Web.Factories
{
    /// <summary>
    /// Represents the interface of the private message model factory
    /// </summary>
    public partial interface IPrivateMessagesModelFactory
    {
        #region privateMessage
        /// <summary>
        /// Prepare the private message index model
        /// </summary>
        /// <param name="page">Number of items page; pass null to disable paging</param>
        /// <param name="tab">Tab name</param>
        /// <returns>Private message index model</returns>
        PrivateMessageIndexModel PreparePrivateMessageIndexModel(int? page, string tab);

        /// <summary>
        /// Prepare the inbox model
        /// </summary>
        /// <param name="page">Number of items page</param>
        /// <param name="tab">Tab name</param>
        /// <returns>Private message list model</returns>
        PrivateMessageListModel PrepareInboxModel(int page, string tab);

        /// <summary>
        /// Prepare the sent model
        /// </summary>
        /// <param name="page">Number of items page</param>
        /// <param name="tab">Tab name</param>
        /// <returns>Private message list model</returns>
        PrivateMessageListModel PrepareSentModel(int page, string tab);

        /// <summary>
        /// Prepare the send private message model
        /// </summary>
        /// <param name="customerTo">Customer, recipient of the message</param>
        /// <param name="replyToPM">Private message, pass if reply to a previous message is need</param>
        /// <returns>Send private message model</returns>
        SendPrivateMessageModel PrepareSendPrivateMessageModel(Customer customerTo,
            PrivateMessage replyToPM);

        /// <summary>
        /// Prepare the private message model
        /// </summary>
        /// <param name="pm">Private message</param>
        /// <returns>Private message model</returns>
        PrivateMessageModel PreparePrivateMessageModel(PrivateMessage pm);
        #endregion
        #region notification
        NotificationSearchModel PrepareNotificationSearchModel(NotificationSearchModel searchModel);
        IList<NotificationModel> PrepareNotificationListModel(NotificationSearchModel searchModel);       
        #endregion
    }
}
