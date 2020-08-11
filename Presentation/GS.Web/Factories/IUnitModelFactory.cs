using GS.Core.Domain.Customers;
using GS.Web.Models.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Factories
{
    public partial interface IUnitModelFactory
    {
        #region Unit
        UnitSearchModel PrepareUnitSearchModel(UnitSearchModel searchModel);

        UnitListModel PrepareUnitListModel(UnitSearchModel searchModel);

        List<UnitModel> PrepareUnitTreeListModel(UnitSearchModel searchModel);

        UnitModel PrepareUnitModel(UnitModel model, Unit item, bool excludeProperties = false);
        #endregion

        #region customer_unit
        CustomerUnitSearchModel PrepareCustomerUnitSearchModel(CustomerUnitSearchModel searchModel);

        CustomerUnitListModel PrepareCustomerUnitListModel(CustomerUnitSearchModel searchModel);
        CustomerUnitSearchModel PrepareCustomerSearchListModel(CustomerUnitSearchModel searchModel);
        CustomerUnitSearchModel PrepareCustomerCheckListModel(CustomerUnitSearchModel searchModel);

        CustomerUnitModel PrepareCustomerUnitModel(CustomerUnitModel model, CustomerUnitMapping item, bool excludeProperties = false);
        #endregion
    }
}
