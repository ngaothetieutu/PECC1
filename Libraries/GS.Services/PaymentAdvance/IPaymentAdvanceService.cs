using GS.Core;
using GS.Core.Domain.Advance;
using GS.Core.Domain.Contracts;
using GS.Core.Domain.Works;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Services.PaymentAdvance
{
    public interface IPaymentAdvanceService
    {        
        #region PaymentAdvance        
        void UpdatePaymentAdvance(ContractPaymentAdvance item);
        void InsertPaymentAdvance(ContractPaymentAdvance item);
        void deletePaymentAdvance(ContractPaymentAdvance item);
        IPagedList<ContractPaymentAdvance> getAllPaymentAdvance(int UnitId = 0, string Keysearch = "", int pageIndex = 0, int pageSize = int.MaxValue, DateTime? FromDate = null, DateTime? ToDate = null);
        ContractPaymentAdvance GetPaymentAdvanceById(int ItemId);
        ContractPaymentAdvance GetContractAdvanceByGuid(Guid AdvanceGuid);
        #endregion
    }
}
