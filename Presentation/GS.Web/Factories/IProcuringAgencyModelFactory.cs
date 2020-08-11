using GS.Core.Domain.Contracts;
using GS.Web.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Factories
{
    public partial interface IProcuringAgencyModelFactory
    {
        #region ProcuringAgency
        ProcuringAgencySearchModel PrepareProcuringAgencySearchModel(ProcuringAgencySearchModel searchModel);


        ProcuringAgencyListModel PrepareProcuringAgencyListModel(ProcuringAgencySearchModel searchModel);


        ProcuringAgencyModel PrepareProcuringAgencyModel(ProcuringAgencyModel model, ProcuringAgency item, bool excludeProperties = false);
        #endregion
    }
}
