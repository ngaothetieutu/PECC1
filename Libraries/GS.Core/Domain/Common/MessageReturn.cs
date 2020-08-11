using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Core.Domain.Common
{
    public class MessageReturn
    {
        public const string SUCCESS_CODE = "00";
        public const string ERROR_CODE = "01";
        public const string NOT_FOUND_CODE = "04";
        public MessageReturn() { }
        public MessageReturn(string _code, string _msg)
        {
            Code = _code;
            Message = _msg;
        }

        public string Code { get; set; }
        public string Message { get; set; }
        public string IdRecord { get; set; }
        /// <summary>
        /// Co the luu struct 1 object, hoac 1 list object
        /// </summary>
        public dynamic ObjectInfo { get; set; }

        public static MessageReturn CreateSuccessMessage(string _msg, dynamic _objectInfo = null)
        {
            var msgret = new MessageReturn(SUCCESS_CODE, _msg);
            msgret.ObjectInfo = _objectInfo;
            return msgret;
        }
        public static MessageReturn CreateErrorMessage(string _msg, dynamic _objectInfo = null)
        {
            var msgret = new MessageReturn(ERROR_CODE, _msg);
            msgret.ObjectInfo = _objectInfo;
            return msgret;
        }
        public static MessageReturn CreateNotFoundMessage(string _msg, dynamic _objectInfo = null)
        {
            var msgret = new MessageReturn(NOT_FOUND_CODE, _msg);
            msgret.ObjectInfo = _objectInfo;
            return msgret;
        }
    }
}
