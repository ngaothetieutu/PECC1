using GS.Core.Domain.Advance;
using GS.Core.Domain.Contracts;
using GS.Core.Domain.Works;
using GS.Web.Models.Contracts;
using GS.Web.Models.PaymentAdvance;
using GS.Web.Models.Works;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = GS.Core.Domain.Works.Task;

namespace GS.Web.Factories
{
    public interface IPaymentAdvanceFactory
    {
        #region PaymentAdvance
        void PrepareContractPaymentAdvanceModel(ContractPaymentAdvanceModel model, ContractPaymentAdvance item);
        void PrepareContractPaymentAdvance(ContractPaymentAdvanceModel model, ContractPaymentAdvance item);
        ContractPaymentAdvanceSearchModel PrepareContractPaymentAdvanceSearchModel(ContractPaymentAdvanceSearchModel searchModel);
        ContractPaymentAdvanceListModel PrepareContractPaymentAdvanceListModel(ContractPaymentAdvanceSearchModel searchModel);      
        ContractAcceptanceModel prepareContractAcceptancebyTask(Task task);
        ContractModel PrepareContractAdvanceModel(int UnitId,Contract contract,int AdvanceId = 0);
        #endregion
    }
}
