using GS.Core.Domain.Contracts;
using GS.Core.Domain.Works;
using GS.Web.Models.Contracts;
using GS.Web.Models.Works;
using System;
using System.Collections.Generic;
using System.Linq;



namespace GS.Web.Factories
{
    public partial interface IContractModelFactory
    {
        #region Contract
        ContractSearchModel PrepareContractSearchModel(ContractSearchModel searchModel);
        ContractListModel PrepareContractListModel(ContractSearchModel searchModel);
        void PrepareprocuringAgencyName(ContractModel model);
        void PrepareContractUnfinish(ContractModel model);
        ContractModel PrepareContractModel(ContractModel model, Contract item, bool isDetail = false);
        /// <summary>
        /// Hàm nay đề chuyển đổi dữ liệu model hợp đồng sang entity
        /// Không sử dụng mapping
        /// </summary>
        /// <param name="item"></param>
        /// <param name="model"></param>
        void PrepareContract(Contract item, ContractModel model);
        List<ContractModel> PrepareRecentlyContracts(ContractSearchModel searchModel);
        void PrepareContractFile(ContractModel model);
        void PrepareContractResource(ContractModel model);
        void PrepareContractAppendix(ContractModel model);
        void PrepareContractRelate(ContractModel model);
        ContractAppendixModel PrepareIndexContractAppendix(Contract contract, ContractAppendixModel model);
        bool CheckCodeP4Contract(int Id, string Code);
        bool CheckEndDate(ContractModel model);
        ContractModel PrepareDashBoardThongKe(Contract item, ContractModel model, int year);
        #endregion
        #region Contract Form
        ContractFormSearchModel PrepareContractFormSearchModel(ContractFormSearchModel searchModel);


        ContractFormListModel PrepareContractFormListModel(ContractFormSearchModel searchModel);
        //Chuan bi contractjoint khi them ben A, ben B vao hop dong
        void PrepareContractJoint(ContractJoint item, ContractJointModel model);
        ContractFormModel PrepareContractFormModel(ContractFormModel model, ContractForm item, bool excludeProperties = false);
        #endregion
        #region Contract Payment
        ContractPaymentSearchModel PrepareContractPaymentSearchModel(ContractPaymentSearchModel searchModel);
        ContractPaymentListModel PrepareContractPaymentListModel(ContractPaymentSearchModel searchModel);
        ContractPaymentModel PrepareContractPaymentModel(ContractPaymentModel model, ContractPayment item, bool excludeProperties = false);
        ContractPaymentModel PrepareContractPaymentBBModel(ContractPaymentModel model, ContractPayment item);
        void PrepareContractPayment(ContractPayment item, ContractPaymentModel model);
        void PrepareContractPaymentBB(ContractPayment item, ContractPaymentModel model);
        #endregion
        #region Contract Type
        ContractTypeSearchModel PrepareContractTypeSearchModel(ContractTypeSearchModel searchModel);


        ContractTypeListModel PrepareContractTypeListModel(ContractTypeSearchModel searchModel);


        ContractTypeModel PrepareContractTypeModel(ContractTypeModel model, ContractType item, bool excludeProperties = false);
        #endregion

        #region Contract Period
        ContractPeriodSearchModel PrepareContractPeriodSearchModel(ContractPeriodSearchModel searchModel);


        ContractPeriodListModel PrepareContractPeriodListModel(ContractPeriodSearchModel searchModel);


        ContractPeriodModel PrepareContractPeriodModel(ContractPeriodModel model, ContractPeriod item, bool excludeProperties = false);
        #endregion
        #region Contract Log
        ContractLogSearchModel PrepareContractLogSearchModel(ContractLogSearchModel searchModel);


        ContractLogListModel PrepareContractLogListModel(ContractLogSearchModel searchModel);


        ContractLogModel PrepareContractLogModel(ContractLogModel model, ContractLog item, bool excludeProperties = false);
        ContractLog PrepareContractLog(Contract contract);
        void PrepareContractLogActivity(ContractModel model);

        #endregion
        #region Contractresource
        void prepareContractResourceModel(ContractResourceModel rsmodel);
        #endregion
        #region Contract Relate
        ContractRelate PrepareContractRelate(Contract contract, int ContractRelateId);
        ContractRelateModel PrepareContractRelateModel(ContractRelateModel model, ContractRelate item, bool excludeProperties = false);
        ContractRelateListModel PrepareContractRelateListModel(ContractRelateSearchModel searchModel);
        ContractRelateSearchModel PrepareContractRelateSearchModel(ContractRelateSearchModel searchModel);
        #endregion
        #region ContractPaymentPlan
        ContractPaymentPlanListModel PrepareContractPaymentPlanSearch(ContractPaymentPlanSearchModel searchModel);
        void prepareContractPaymentPlan(ContractPaymentPlanModel model , ContractPaymentPlan item );
        void prepareContractPaymentPlanModel(ContractPaymentPlanModel model, ContractPaymentPlan item =null);
        AppendixEditPaymenPlanModel PrepareIndexAppendixEditPaymentPlan(Contract contract, AppendixEditPaymenPlanModel model);
        #endregion
        #region ContractPaymentRequest
        ContractPaymentRequestSearchModel PrepareContractPaymentRequestSearchModel(ContractPaymentRequestSearchModel searchModel);
        ContractPaymentRequestListModel PrepareContractPaymentRequestListModel(ContractPaymentRequestSearchModel searchModel);
        ContractPaymentRequestModel PrepareContractPaymentRequestModel(ContractPaymentRequestModel model, ContractPaymentRequest item, bool excludeProperties = false);
        void prepareContractPaymentRequest(ContractPaymentRequestModel model, ContractPaymentRequest item);
        void prepareContractPaymentRequestModelAdd(ContractPaymentRequestModel model, int PaymentPlanId);
        #endregion
        #region ContractAcceptance
        ContractAcceptanceModel PrepareContractAcceptanceModel(ContractAcceptanceModel model, ContractAcceptance item =null, bool isDetail = false);
        ContractAcceptanceModel PrepareContractAcceptanceKhoiLuong(ContractAcceptanceModel model, ContractAcceptance item = null);
        ContractAcceptanceModel PrepareContractAcceptanceNoiBo(ContractAcceptanceModel model, ContractAcceptance item = null);
        ContractAcceptanceModel PrepareContractAcceptanceBB(ContractAcceptanceModel model, ContractAcceptance item = null);

        void PrepareContractAcceptance(ContractAcceptanceModel model, ContractAcceptance item);
        void PrepareContractPaymentAcceptance(ContractPaymentAcceptanceModel model, ContractPaymentAcceptance item);
        void PrepareContractAcceptanceNoiBo(ContractModel model);
        ContractPaymentAcceptanceModel PrepareContractPaymentAcceptanceModel(ContractPaymentAcceptanceModel model, ContractPaymentAcceptance item = null);
        ContractAcceptanceListModel PrepareContractAcceptanceListModel(ContractAcceptanceSearchModel searchmodel);
        void PrepareContractAcceptanceNoiboForUnit(ContractAcceptanceModel model, ContractAcceptance item);
        ContractAcceptanceSearchModel PrepareContractAcceptanceSearchModel(ContractAcceptanceSearchModel searchModel);
        ContractAcceptanceListModel PrepareContractAcceptanceNoiBoListModel(ContractAcceptanceSearchModel searchModel);
        void PrepareContracAcceptanceNoiBoModel(ContractAcceptanceModel model, ContractAcceptance item);
        ContractAcceptanceModel PrepareTotalAcceptanceKL(ContractAcceptance item);

        #endregion
        #region ContractAcceptanceBB
        ContractAcceptanceBBModel PrepareContractAcceptanceBBModel(ContractAcceptanceBBModel model, ContractAcceptanceBB item = null);
        void PrepareContractAcceptanceBB(ContractAcceptanceBBModel model, ContractAcceptanceBB item = null);
        #endregion
        #region ContractAcceptanceSub
        ContractAcceptanceSubModel PrepareAcceptanceSubModelbyPaymentSub(ContractPaymentAcceptanceSub paymentSubmodel);
        ContractAcceptanceSubModel PrepareAcceptanceSubModelbyTask(Task taskmodel, ContractPaymentAcceptanceSubModel paysubmodel,int ContractAcceptanceId=0);
        ContractAcceptanceSubModel PrepareContractAcceptanceSubModel(ContractAcceptanceSubModel model, ContractAcceptanceSub item);
        ContractAcceptanceSubModel PrepareContractAcceptanceKhoiLuongSubModel(ContractAcceptanceSubModel model, ContractAcceptanceSub item,int ContractAcceptanceId = 0);
        void PrepareContractAcceptanceSub(ContractAcceptanceSub item, ContractAcceptanceSubModel model);
        ContractAcceptanceSubModel PrepareContractAcceptanceSubByTaskModel(int taskId, string approvalDate);
        void PrepareContractAcceptanceKhoiLuongSub(ContractAcceptanceSub item);
        ContractAcceptanceSubModel PrepareContarctAcceptanceSubModelByAcceptanceSub(ContractAcceptanceSub item);
        void PrepareContractAcceptanceSub(ContractModel model);
        #endregion
        #region ContractPaymentAcceptanceSub
        ContractPaymentAcceptanceSubModel PrepareContractPaymentAcceptanceSubModel(ContractPaymentAcceptanceSubModel model, ContractPaymentAcceptanceSub item);
        void PrepareContractPaymentAcceptanceSub(ContractPaymentAcceptanceSub item, ContractPaymentAcceptanceSubModel model);
        ContractPaymentAcceptanceSubModel prepareContractPaymentAcceptancebyTaskMap(ContractAcceptanceSub AcceptanceSub);
        ContractAcceptanceSub PrepareContractAcceptanceSubByTask(ContractAcceptanceSubModel model, ContractAcceptanceSub item);
        #endregion
        #region ContractSettlement
        void PrepareContractSettlement(ContractSettlementModel model, ContractSettlement item);
        ContractSettlementModel prepareTotalAmountContractSettle(ContractSettlement item);
        #endregion
        #region PaymentExpenditure
        void PreparePaymentExpenditure(PaymentExpenditureModel model, PaymentExpenditure item);
        PaymentExpenditureListModel PreparePaymentExpenditureListModel(PaymentExpenditureSearchModel searchModel);
        #endregion
    }
}
