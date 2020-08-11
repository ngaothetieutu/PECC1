﻿using System;
using System.Collections.Generic;
using System.Linq;
using GS.Services.Customers;
using GS.Services.Localization;
using GS.Services.Security;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Areas.Admin.Models.Customers;
using GS.Web.Areas.Admin.Models.Security;

namespace GS.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the security model factory implementation
    /// </summary>
    public partial class SecurityModelFactory : ISecurityModelFactory
    {
        #region Fields

        private readonly ICustomerService _customerService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;

        #endregion

        #region Ctor

        public SecurityModelFactory(ICustomerService customerService,
            ILocalizationService localizationService,
            IPermissionService permissionService)
        {
            this._customerService = customerService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare permission mapping model
        /// </summary>
        /// <param name="model">Permission mapping model</param>
        /// <returns>Permission mapping model</returns>
        public virtual PermissionMappingModel PreparePermissionMappingModel(PermissionMappingModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var customerRoles = _customerService.GetAllCustomerRoles(true);
            model.AvailableCustomerRoles = customerRoles.Select(role => role.ToModel<CustomerRoleModel>()).ToList();

            foreach (var permissionRecord in _permissionService.GetAllPermissionRecords())
            {
                model.AvailablePermissions.Add(new PermissionRecordModel
                {
                    Name = _localizationService.GetLocalizedPermissionName(permissionRecord),
                    SystemName = permissionRecord.SystemName
                });

                foreach (var role in customerRoles)
                {
                    if (!model.Allowed.ContainsKey(permissionRecord.SystemName))
                        model.Allowed[permissionRecord.SystemName] = new Dictionary<int, bool>();
                    model.Allowed[permissionRecord.SystemName][role.Id] = permissionRecord.PermissionRecordCustomerRoleMappings
                        .Any(mapping => mapping.CustomerRoleId == role.Id);
                }
            }

            return model;
        }

        #endregion
    }
}