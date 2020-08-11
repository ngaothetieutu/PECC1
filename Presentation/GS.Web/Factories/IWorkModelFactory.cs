using GS.Web.Models.Works;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Factories
{
    public partial interface IWorkModelFactory
    {
        #region Taskresource
        void PrepareTaskResourceEdit(TaskResourceModel rsmodel);

        void PrepareTaskResource(TaskResourceModel model);
        #endregion
    }
}
