using GS.Core.Domain.Contracts;
using GS.Web.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Factories
{
    public partial interface IConstructionModelFactory
    {
        #region ConstructionType
        ConstructionTypeSearchModel PrepareConstructionTypeSearchModel(ConstructionTypeSearchModel searchModel);


        ConstructionTypeListModel PrepareConstructionTypeListModel(ConstructionTypeSearchModel searchModel);


        ConstructionTypeModel PrepareConstructionTypeModel(ConstructionTypeModel model, ConstructionType item, bool excludeProperties = false);
        #endregion
        #region  ConstructionCapital
        ConstructionCapitalSearchModel PrepareConstructionCapitalSearchModel(ConstructionCapitalSearchModel searchModel);


        ConstructionCapitalListModel PrepareConstructionCapitalListModel(ConstructionCapitalSearchModel searchModel);


        ConstructionCapitalModel PrepareConstructionCapitalModel(ConstructionCapitalModel model, ConstructionCapital item, bool excludeProperties = false);
        #endregion
        #region  Construction
        ConstructionSearchModel PrepareConstructionSearchModel(ConstructionSearchModel searchModel);


        ConstructionListModel PrepareConstructionListModel(ConstructionSearchModel searchModel);


        ConstructionModel PrepareConstructionModel(ConstructionModel model, Construction item, bool excludeProperties = false);
        void PrepareConstruction(ConstructionModel model, Construction item);
        #endregion
    }
}