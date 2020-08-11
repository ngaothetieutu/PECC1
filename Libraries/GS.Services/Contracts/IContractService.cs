using System;
using System.Collections.Generic;
using System.Text;
using GS.Core;
using GS.Core.Domain.Contracts;
using GS.Core.Domain.Dashboard;
using GS.Core.Domain.Works;

namespace GS.Services.Contracts
{
    public interface IContractService
    {
        #region Contract

        /// <summary>
        /// Delete the review type
        /// </summary>
        /// <param name="item">Contract type</param>
        void DeleteContract(Contract item);

        /// <summary>
        /// Get all review types
        /// </summary>
        /// <returns>Contract types</returns>
        IList<Contract> GetListContractbylistId(List<int> listId);
        /// <summary>
        /// get contract by pagesize 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        IPagedList<Contract> GetContracts(string keySearch = null, string Code = null, string Name = null
            , DateTime? FromDate = null, DateTime? ToDate = null
            , int CustomerId = 0, ContractStatus contractStatus = ContractStatus.All
            , int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false, int ContractListOrder = 0, List<int> SelectedConstructionIds = null
            , ContractClassification classificationId = ContractClassification.All, int ConstructionTypeId = 0, int contractType = 0
            , ContractMonitorStatus contractMonitorStatus = ContractMonitorStatus.All
            , DateTime? signedateFrom = null, DateTime? signedateTo = null, bool isGetRecently = false, List<int> SelectedProcuringAgencyIds = null, int StartYear = 0, DateTime? acceptanceFrom = null, DateTime? acceptanceTo = null);
        /// <summary>
        /// Get the review type 
        /// </summary>
        /// <param name="itemId">Contract type identifier</param>
        /// <returns>Contract type</returns>
        Contract GetContractById(int itemId);
        /// <summary>
        /// Gets a customer by GUIDs
        /// </summary>
        /// <param name="contractGuid"></param>
        /// <returns></returns>
        Contract GetContractByGuid(Guid contractGuid);
        /// <summary>
        /// Insert the review type
        /// </summary>
        /// <param name="item">Contract type</param>
        void InsertContract(Contract item);
        /// <summary>
        /// Update the review type
        /// </summary>
        /// <param name="item">Contract type</param>
        void UpdateContract(Contract item);
        IList<Contract> getAllContractbyUnitId(int UniId);
        IList<Contract> GetContractRelates(int ContractId, ContractClassification classificationId);
        Decimal GetTotalMoney(int year = 0);
        Decimal GetTotalRequest();
        Decimal GetTotalMoneyAcceptance(int year = 0);
        Decimal GetTotalMoneyContractunfinishByYear(int year);
        Decimal GetSumPaymentAdvance(int year = 0);
        Decimal GetSumPaymentPaid(int year = 0);
        IList<Construction> GetTopConstructionContract();
        Decimal GetUnfinish(int year);
        IList<Contract> GetTopContractDealine(int Top = 5);
        IList<Contract> GetContractDelayedProcessing(int Top = 5);
        PaymentPlanModel GetTotalAdvancePayment(int contractId);
        Decimal GetTotalAmountCollected(int contractId);
        Decimal GetTotalAmountSpent(int contractId);
        Decimal GetTotalAmountReceivable(int ContractId);
        Decimal GetTotalAmountSpend(int contractId);
        Decimal GetTotalAdvancedHasPayment(int contractId);
        Decimal GetTotalAmountContract(int contractId);
        Decimal GetTotalAmountAccepted(int contractId);
        Decimal GetTotalAmountBB(int contractId);
        Decimal GetTotalAmountAcceptanced(int contractId);
        Decimal GetTotalMoneyPaymentAcceptance(int contractId);
        Decimal GetTotalAmountPayment(int contractId);
        Decimal GetTotalAmountAcceptanceNoiBo(int contractId);
        Decimal GetTotalAmountPaymentAdvanceForUnit(int contractId);
        IList<ConstructionTypeResult> GetTotalTypeContract(int constructionType = 0);
        IList<ConstructionTypeResultByYear> GetTotalTypeContractByYear(int year = 0, int constructionType = 0);
        Contract GetContractByCode1(string code1);
        IList<Contract> GetAllContract();
        IList<Contract> GetAllContractInYear(int year = 0);
        IList<Contract> GetAllContractInYearAcceptance(int year = 0);
        IList<ContractPayment> GetAllContractPaymentByContractId(int contractId);
        IList<Contract> GetAllContractInYearPaymentAdvance(int year = 0);
        IList<Contract> GetAllContractInYearPaymentPaid(int year = 0);
        IList<Contract> GetAllContractByConstructionId(int constructionTypeId);
        IList<Contract> GetAllContractByConstructionInYear(int constructionTypeId, int year);
        IList<Contract> GetAllContractInYearPaymentByConstructionTypeId(int constructionTypeId, int year);
        IList<Contract> GetAllContractInYearPaymentAdvanceByConstructionTypeId(int constructionTypeId, int year);
        IList<Contract> GetAllContractInYearPaymentPaidByConstructionTypeId(int constructionTypeId, int year);
        IList<Contract> GetAllContractPaymentAcceptanceByUnitId(int UniId, string approvaldateString);
        PaymentPlanModel GetPaymentPlan(int ContractId);
        Decimal GetTotalAmountContractByTask(int contractId, bool beforeTax = true);
        ContractRelate GetContractRelateByContract2Id(int Contract2Id);
        IList<Contract> GetAllContractBBAproval(int contractId);
        #endregion
        #region Contract joint
        void InsertContractJoint(ContractJoint item);
        IList<ContractJoint> GetContractJointsByContractId(int Id);
        void DeleteContractJoint(int _ContractId, int _ProCuringAgencyId);
        ContractJoint GetContractJointById(int _contractId, int _procuringAgencyId);
        IList<ProcuringAgency> GetallProcuringAgencyContract(int contractId);
        #endregion
        #region  Contract File
        IList<WorkFile> GetContractFiles(Guid ContractGuid);
        IList<WorkFile> GetContractFiles(int ContractId);
        IList<ContractFile> GetallContractFiles(int ContractId, int TypeId, int EntityId = 0);
        IList<ContractFile> GetContractFiles(int TypeId, int ContractId = 0, int EntityId = 0);
        void InsertContractFile(ContractFile item);
        void DeleteContractFile(int FileId);
        void DeleteMultiContractFile(int ContractId, int TypeId = 0, int FileId = 0, string entityId = null);
        #endregion
        #region contract view
        void UpdateContractView(ContractView item);
        void InsertContractView(ContractView item);
        void DeleteContractView(int ContractId);
        IList<ContractView> getallContractView(int CustomerId);
        #endregion
        #region Contract resource
        void InsertContractResource(ContractResource item);
        void DeleteContractResource(int ItemId);
        IList<ContractResource> GetContractResources(int ContractId, int CustomerId = 0);
        ContractResource GetContractResourceById(int ItemId);
        void DeleteListContractResource(int ContractId, int CustomerId = 0);
        void UpdateContractResource(ContractResource item);
        /// <summary>
        /// Cap nhat thong tin role resource cua hop dong, theo don vi, nhom, nguoi dung
        /// </summary>
        /// <param name="ContractId"></param>
        /// <param name="RoleIds"></param>
        /// <param name="UnitId"></param>
        /// <param name="GroupId">Chinh la CustomerRole nhung dung de tao nhom cho 1 so nguoi dung</param>
        /// <param name="CustomerId"></param>
        void UpdateContractResources(int ContractId, int[] RoleIds, int UnitId = 0, int GroupId = 0, int CustomerId = 0);
        /// <summary>
        /// Cap nhat thong tin view hop dong cho ca 1 phong, nhom, nguoi dung
        /// </summary>
        /// <param name="ContractId"></param>
        /// <param name="UnitId"></param>
        /// <param name="GroupId"></param>
        /// <param name="CustomerId"></param>
        void UpdateViewContractResources(int ContractId, int UnitId = 0, int GroupId = 0, int CustomerId = 0);
        #endregion
        #region ContractPaymentPlan
        /// <summary>
        /// Delete the review type
        /// </summary>
        /// <param name="item">Contract type</param>
        void DeleteContractPaymentPlan(ContractPaymentPlan item);

        /// <summary>
        /// Get all review types
        /// </summary>
        /// <returns>Contract types</returns>
        IList<ContractPaymentPlan> GetAllContractPaymentPlans(int ContractId);

        /// <summary>
        /// Get the review type 
        /// </summary>
        /// <param name="itemId">Contract type identifier</param>
        /// <returns>Contract type</returns>
        ContractPaymentPlan GetContractPaymentPlanById(int itemId);

        /// <summary>
        /// Insert the review type
        /// </summary>
        /// <param name="item">Contract type</param>
        void InsertContractPaymentPlan(ContractPaymentPlan item);
        /// <summary>
        /// Update the review type
        /// </summary>
        /// <param name="item">Contract type</param>
        void UpdateContractPaymentPlan(ContractPaymentPlan item);
        IList<ContractPaymentPlan> GetAllContractPaymentPlanByPaymentPlanIds(IList<int> paymentPlanId);
        #endregion
        #region ContractAcceptance
        IList<ContractAcceptance> getAllContractAcceptance(int ContractId = 0, int TaskId = 0, int PaymentAcceptanceId = 0, int TypeId = 0, bool isFilter = false);
        ContractAcceptance getContractAcceptancebyId(int Id);
        IList<ContractAcceptance> getAllContractAcceptanceByAdvanceId(int AdvanceId, int ContractId = 0);
        void InsertContractAcceptance(ContractAcceptance item);
        void DeleteContractAcceptance(ContractAcceptance item);
        void UpdateContractAcceptance(ContractAcceptance item);
        void UpdateContractAcceptancebypayment(int ContractAcceptanceId, int PaymentAcceptanceId);
        IPagedList<ContractAcceptance> GetAllContractAcceptanceNoiBo(string keySearch, int unitId = 0, int pageIndex = 0, int pageSize = int.MaxValue);
        ContractAcceptance GetContractAcceptanceByGuid(Guid acceptanceGuid);
        List<ContractAcceptanceSub> GetAllContractAcceptanceNoiBoSub(int ContractId, int unitId, int acceptanceId = 0);
        ContractAcceptance GetContractAcceptanceNoiBoById(int Id);
        IList<ContractAcceptance> GetContractAcceptanceByAdvanceId(int AdvanceId);
        #endregion
        #region ContractAcceptanceBB
        IList<ContractAcceptanceBB> getAllContractAcceptanceBB(int ContractId = 0, int ContractIdBB = 0, int TaskId = 0, int TypeId = 1);
        ContractAcceptanceBB getContractAcceptanceBBbyId(int Id);
        void InsertContractAcceptanceBB(ContractAcceptanceBB item);
        void DeleteContractAcceptanceBB(ContractAcceptanceBB item);
        void UpdateContractAcceptanceBB(ContractAcceptanceBB item);
        #endregion
        #region ContractAcceptanceTaskMapping
        void InsertContractAcceptanceTaskMapping(ContractAcceptanceTaskMapping item);
        void DeleteContractAcceptanceTaskMapping(ContractAcceptanceTaskMapping item);
        void DeleteContractAcceptanceTaskMappingbyAcceptanceId(int ContractAcceptanceId);
        void DeleteContractAcceptanceTaskMappingbyTaskId(int TaskId);
        List<ContractAcceptanceTaskMapping> GetAllContractAcceptanceTaskMapping(int TaskId = 0, int AcepptanceId = 0);
        ContractAcceptanceTaskMapping GetContractAcceptanceTaskMappingByAcceptanceId(int AcceptanceId);
        List<ContractAcceptanceTaskMapping> GetContractAcceptanceTaskMappingByListAcceptanceId(List<int> ListAcceptanceIds);
        #endregion
        #region ContractPaymentRequest
        /// <summary>
        /// Delete the review type
        /// </summary>
        /// <param name="item">Contract type</param>
        void DeleteContractPaymentRequest(ContractPaymentRequest item);

        /// <summary>
        /// Get all review types
        /// </summary>
        /// <returns>Contract types</returns>
        IList<ContractPaymentRequest> GetAllContractPaymentRequestByContractId(int ContractId);

        /// <summary>
        /// Get the review type 
        /// </summary>
        /// <param name="itemId">Contract type identifier</param>
        /// <returns>Contract type</returns>
        ContractPaymentRequest GetContractPaymentRequestById(int itemId);

        /// <summary>
        /// Insert the review type
        /// </summary>
        /// <param name="item">Contract type</param>
        void InsertContractPaymentRequest(ContractPaymentRequest item);
        /// <summary>
        /// Update the review type
        /// </summary>
        /// <param name="item">Contract type</param>
        void UpdateContractPaymentRequest(ContractPaymentRequest item);
        /// <summary>
        /// Get all review types
        /// </summary>
        /// <returns>Contract types</returns>
        IList<ContractPaymentRequest> GetAllContractPaymentRequest();
        List<ContractPaymentRequest> GetAllContractPaymentRequestByContractIdAndPlanId(int contractId, int paymentPlanId);
        int GetCountOfRequest(int contractId, int paymentPlanId);
        #endregion
        #region ContractPaymentAcceptance            
        IList<ContractPaymentAcceptance> getAllContractPaymentAcceptance(int ContractId = 0, int TaskId = 0);
        ContractPaymentAcceptance getContractPaymentAcceptancebyId(int Id);
        void InsertContractPaymentAcceptance(ContractPaymentAcceptance item);
        void DeleteContractPaymentAcceptance(ContractPaymentAcceptance item);
        void UpdateContractPaymentAcceptance(ContractPaymentAcceptance item);
        IList<ContractPaymentAcceptance> GetAllContractPaymentAcceptancesByRequestId(int paymentRequestId, int ContractId);
        IList<int> GetAllContractPaymenAcceptanceIdByPaymentRequestId(int paymentRequestId);
        IList<ContractPaymentAcceptance> GetAllContractPaymenAcceptanceByPaymentRequestId(int paymentRequestId);
        IList<ContractPaymentAcceptance> GetAllContractPaymenAcceptanceByContractId(int contractId);
        IList<ContractPaymentAcceptance> GetPaymentAcceptancebyListAcceptanceIds(List<int> ListAcceptanceIds);
        ContractPaymentAcceptance getContractPaymentAcceptanceByPaymentRequestId(int paymentRequestId);
        #endregion
        #region ContractPaymentTask
        IList<ContractPaymentTask> getallContractPaymentTask(int TaskId = 0, int PaymentId = 0);
        void InsertContractPaymentTask(ContractPaymentTask item);
        void DeleteContractPaymentTask(ContractPaymentTask item);
        void DeleteListContractPaymentTask(int TaskId = 0, int PaymentId = 0);
        #endregion
        #region ContractAcceptionSub
        void InsertContractAcceptionSub(ContractAcceptanceSub item);
        List<ContractAcceptanceSub> GetallContractAcceptanceSubs(int AcceptanceId = 0, int ContractId = 0, int TaskId = 0, int TypeId = (int)ContractAcceptancesType.KhoiLuong);
        ContractAcceptanceSub GetContractAcceptanceSubById(int Id);
        void UpdateContractAcceptanceSub(ContractAcceptanceSub item);
        void DeleteContractAcceptanceSub(ContractAcceptanceSub item);
        void DeleteContractAcceptanceSubByAcceptanceId(int AccepId, int TypeId);
        List<ContractAcceptanceSub> GetAllContractAcceptanceSubByAcceptanceNoiBoId(int acceptanceNoiBoId);
        IList<ContractAcceptanceSub> GetAllContractAcceptanceSubByListAcceptanceId(IList<int> acceptanceId);
        #endregion
        #region ContractpaymentAcceptionSub
        void InsertContractPaymentAcceptionSub(ContractPaymentAcceptanceSub item);
        List<ContractPaymentAcceptanceSub> GetallPaymentAcceptanceSubs(int AcceptanceId = 0, int ContractId = 0, int PaymentAcceptanceId = 0, int TaskId = 0);
        ContractPaymentAcceptanceSub GetContractPaymentAcceptanceSubById(int Id);
        void UpdateContractPaymentAcceptanceSub(ContractPaymentAcceptanceSub item);
        void DeleteContractPaymentAcceptanceSub(ContractPaymentAcceptanceSub item);
        void DeletePaymentAcceptanceSubByPaymentAcceptanceId(int Id);
        IList<ContractPaymentAcceptanceSub> GetContractPaymentAcceptanceSubByAcceptanceId(int acceptanceId);
        IList<ContractPaymentAcceptanceSub> GetAllContractPaymentAcceptanceSubByApprovalDate(int taskId, string approvaldateString);
        #endregion
        #region ContractMonitor
        String GetStatusContractMonior(int contractId);
        String GetAcceptanceContractMonitor(int contractId);
        String GetPaymentContractMonitor(int contractId);
        ContractMonitor GetContractMonitor(int contractId = 0);
        #endregion
        #region PaymentPlanContract_map
        void InsertPaymentPlanContract(PaymentPlanContract item);
        void DeletePaymentPlanContract(PaymentPlanContract item);
        IList<PaymentPlanContract> GetPaymentPlanContracts(int contractId = 0, int paymentPlanId = 0);
        IList<PaymentPlanContract> GetPaymentPlanContractsByPaymentPlanId(int paymentPlanId = 0);
        IList<PaymentPlanContract> GetPaymentPlanContractsByContractId(int contractId = 0);
        #endregion
        #region ContractSettlement
        IList<ContractSettlement> getAllContractSettlement(int ContractId = 0);
        ContractSettlement getContractSettlementbyId(int Id);
        void InsertContractSettlement(ContractSettlement item);
        void DeleteContractSettlement(ContractSettlement item);
        void UpdateContractSettlement(ContractSettlement item);
        #endregion
        #region ContractSettlement_map_Task
        void InsertContractSettlementTaskMapping(ContractSettlementTaskMapping item);
        void DeleteContractSettlementTaskMapping(ContractSettlementTaskMapping item);
        void DeleteContractSettlementTaskMappingbySettlementId(int ContractSettlementId);
        void DeleteContractSettlementTaskMappingbyTaskId(int TaskId);
        List<ContractSettlementTaskMapping> GetAllContractSettlementTaskMapping(int TaskId = 0, int SettlementId = 0);
        ContractSettlementTaskMapping GetContractSettlementTaskMappingBySettlementId(int SettlementId);
        List<ContractSettlementTaskMapping> GetContractSettlementTaskMappingByListSettlementId(List<int> ListSettlementIds);
        #endregion
        #region ContractSettlementSub
        List<ContractSettlementSub> GetAllContractSettlementSub(int contractId = 0, int taskId = 0, int contractSettlementId = 0);
        ContractSettlementSub GetContractSettlementSubById(int id);
        void UpdateContractSettlementSub(ContractSettlementSub item);
        void DeleteContractSettlementSub(ContractSettlementSub item);
        void InsertContractSettlementSub(ContractSettlementSub item);
        void DeleteContractSettlementSubBySettlementId(int id);
        IList<int> GetTaskByContractIdInSettlementSub(int id);
        IList<int> GetListTaskIdBySettlementId(int id);
        IList<ContractSettlementSub> GetSubBySettlementId(int id);
        #endregion
        #region ContractPaymentExpenditure
        void InsertPaymentExpenditure(PaymentExpenditure item);
        void DeletePaymentExpenditure(PaymentExpenditure item);
        void UpdatePaymentExpenditure(PaymentExpenditure item);
        PaymentExpenditure GetPaymentExpenditureById(int Id);
        IPagedList<PaymentExpenditure> GetAllPaymentExpenditure(int UnitId = 0, string Keysearch = "", int pageIndex = 0, int pageSize = int.MaxValue);
        PaymentExpenditure GetPaymentExpenditureByGuid(Guid guId);
        #endregion
        #region ContractUnfinish
        IList<ContractUnfinish> getallContractUnfinish(int ContractId = 0, DateTime? CreatedDate = null, int ContractTypeId = 0);
        ContractUnfinish getContractUnfinishById(int Id);
        void UpdateContractUnfinish(ContractUnfinish item);
        List<ContractUnfinish> GetAllContractunfinishByYear(int year);
        void DeleteContractUnfinishByContractId(int ContractId);
        IList<ContractUnfinish> GetContractUnfinishNow(int contratcId);
        #endregion
    }
}