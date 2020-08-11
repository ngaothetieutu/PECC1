using System;
using System.Collections.Generic;
using System.Linq;
using GS.Core;
using GS.Core.Domain.Catalog;
using GS.Core.Domain.Customers;
using GS.Services.Catalog;
using GS.Services.Customers;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Areas.Admin.Models.Catalog;
using GS.Web.Areas.Admin.Models.Customers;
using GS.Web.Framework.Extensions;

namespace GS.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the customer role model factory implementation
    /// </summary>
    public partial class CustomerRoleModelFactory : ICustomerRoleModelFactory
    {
        #region Fields

        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly ICustomerService _customerService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public CustomerRoleModelFactory(IBaseAdminModelFactory baseAdminModelFactory,
            ICustomerService customerService,
            IWorkContext workContext)
        {
            this._baseAdminModelFactory = baseAdminModelFactory;
            this._customerService = customerService;
            this._workContext = workContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare customer role search model
        /// </summary>
        /// <param name="searchModel">Customer role search model</param>
        /// <returns>Customer role search model</returns>
        public virtual CustomerRoleSearchModel PrepareCustomerRoleSearchModel(CustomerRoleSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged customer role list model
        /// </summary>
        /// <param name="searchModel">Customer role search model</param>
        /// <returns>Customer role list model</returns>
        public virtual CustomerRoleListModel PrepareCustomerRoleListModel(CustomerRoleSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get customer roles
            var customerRoles = _customerService.GetAllCustomerRoles(true);

            //prepare grid model
            var model = new CustomerRoleListModel
            {
                Data = customerRoles.PaginationByRequestModel(searchModel).Select(role =>
                {
                    //fill in model values from the entity
                    var customerRoleModel = role.ToModel<CustomerRoleModel>();

                    //fill in additional values (not existing in the entity)

                    return customerRoleModel;
                }),
                Total = customerRoles.Count
            };

            return model;
        }

        /// <summary>
        /// Prepare customer role model
        /// </summary>
        /// <param name="model">Customer role model</param>
        /// <param name="customerRole">Customer role</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Customer role model</returns>
        public virtual CustomerRoleModel PrepareCustomerRoleModel(CustomerRoleModel model, CustomerRole customerRole, bool excludeProperties = false)
        {
            if (customerRole != null)
            {
                //fill in model values from the entity
                model = model ?? customerRole.ToModel<CustomerRoleModel>();
            }

            //set default values for the new model
            if (customerRole == null)
                model.Active = true;


            return model;
        }

        #endregion
    }
}