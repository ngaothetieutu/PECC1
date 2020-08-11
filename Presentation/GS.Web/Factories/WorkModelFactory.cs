using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GS.Core.Domain.Customers;
using GS.Core.Domain.Media;
using GS.Services.Common;
using GS.Services.Customers;
using GS.Services.Media;
using GS.Services.Works;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Areas.Admin.Models.Customers;
using GS.Web.Models.Works;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GS.Web.Factories
{
    public class WorkModelFactory : IWorkModelFactory
    {
        #region Fields
        private readonly ICustomerService _customerService;
        private readonly IWorkTaskService _workTaskService;       
        private readonly IUnitService _unitService;
        #endregion
        #region Ctor
        public WorkModelFactory(ICustomerService customerService,
            IWorkTaskService workTaskService,
            IUnitService unitService)
        {
            this._unitService = unitService;
            this._workTaskService = workTaskService;
            this._customerService = customerService;
        }
        #endregion
        #region customer
      
        #endregion
        #region TaskResource
        public virtual void PrepareTaskResourceEdit(TaskResourceModel rsmodel)
        {
            var customerroles = _customerService.GetCustomerRolesByPrefix("GSTask");
            var unit = _unitService.GetAllUnits();
            rsmodel.RoleModels = customerroles.Select(c => c.ToModel<CustomerRoleModel>()).ToList();
            rsmodel.AvailbleUnit = unit.Select(C => new SelectListItem
            {
                Text = C.Name,
                Value = C.Id.ToString(),
                Selected = rsmodel.UnitId == C.Id,
            }).ToList();
            rsmodel.AvailbleUnit.Insert(0, new SelectListItem
            {
                Text = "---Chọn phòng ban---",
                Value = "0"
            });
            rsmodel.AvailbleRoles = customerroles.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();
        }

        public void PrepareTaskResource(TaskResourceModel model)
        {
            if (model.Id == 0)
            {
                var task = _workTaskService.GetTaskById (model.Id);
                if (task == null)
                    return;
                model.Id = task.Id;
            }
        }
        #endregion
    }
}
