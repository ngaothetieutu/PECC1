using System;
using System.Linq;
using GS.Core.Domain.Tasks;
using GS.Services.Tasks;
using GS.Services.Localization;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Areas.Admin.Models.Tasks;
using GS.Web.Framework.Extensions;
using GS.Web.Framework.Factories;

namespace GS.Web.Areas.Admin.Factories
{
    public partial class TaskAttributeModelFactory : ITaskAttributeModelFactory
    {
        #region Fields

        private readonly ITaskAttributeService _taskAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedModelFactory _localizedModelFactory;

        #endregion

        #region Ctor

        public TaskAttributeModelFactory(ITaskAttributeService taskAttributeService,
            ILocalizationService localizationService,
            ILocalizedModelFactory localizedModelFactory)
        {
            this._taskAttributeService = taskAttributeService;
            this._localizationService = localizationService;
            this._localizedModelFactory = localizedModelFactory;
        }
        #endregion

        #region Utilities
        protected virtual TaskAttributeValueSearchModel PrepareTaskAttributeValueSearchModel(TaskAttributeValueSearchModel searchModel,
            TaskAttribute taskAttribute)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (taskAttribute == null)
                throw new ArgumentNullException(nameof(taskAttribute));

            searchModel.TaskAttributeId = taskAttribute.Id;

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }
        #endregion

        #region Methods
        public virtual TaskAttributeListModel PrepareTaskAttributeListModel(TaskAttributeSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            var taskAttributes = _taskAttributeService.GetAllTaskAttributes();

            var model = new TaskAttributeListModel
            {
                Data = taskAttributes.PaginationByRequestModel(searchModel).Select(attribute =>
                {
                    //fill in model values from the entity
                    var attributeModel = attribute.ToModel<TaskAttributeModel>();

                    //fill in additional values (not existing in the entity)
                    attributeModel.AttributeControlTypeName = _localizationService.GetLocalizedEnum(attribute.AttributeControlType);

                    return attributeModel;
                }),
                Total = taskAttributes.Count
            };
            return model;
        }

        public virtual TaskAttributeModel PrepareTaskAttributeModel(TaskAttributeModel model, 
            TaskAttribute taskAttribute, bool excludeProperties = false)
        {
            Action<TaskAttributeLocalizedModel, int> localizedModelConfiguration = null;

            if (taskAttribute != null)
            {
                //fill in model values from the entity
                model = model ?? taskAttribute.ToModel<TaskAttributeModel>();

                //prepare nested search model
                PrepareTaskAttributeValueSearchModel(model.TaskAttributeValueSearchModel, taskAttribute);

                //define localized model configuration action
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Name = _localizationService.GetLocalized(taskAttribute, entity => entity.Name, languageId, false, false);
                };
            }

            if (!excludeProperties)
                model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return model;
        }

        public virtual TaskAttributeSearchModel PrepareTaskAttributeSearchModel(TaskAttributeSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        public virtual TaskAttributeValueListModel PrepareTaskAttributeValueListModel(TaskAttributeValueSearchModel searchModel, TaskAttribute taskAttribute)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (taskAttribute == null)
                throw new ArgumentNullException(nameof(taskAttribute));

            var taskAttributeValues = _taskAttributeService.GetTaskAttributeValues(taskAttribute.Id);

            var model = new TaskAttributeValueListModel
            {
                //fill in model values from the entity
                Data = taskAttributeValues.PaginationByRequestModel(searchModel)
                    .Select(value => value.ToModel<TaskAttributeValueModel>()),
                Total = taskAttributeValues.Count
            };

            return model;
        }

        public virtual TaskAttributeValueModel PrepareTaskAttributeValueModel(TaskAttributeValueModel model, 
            TaskAttribute taskAttribute, TaskAttributeValue taskAttributeValue, bool excludeProperties = false)
        {
            if (taskAttribute == null)
                throw new ArgumentNullException(nameof(taskAttribute));

            Action<TaskAttributeValueLocalizedModel, int> localizedModelConfiguration = null;

            if (taskAttributeValue != null)
            {
                //fill in model values from the entity
                model = model ?? taskAttributeValue.ToModel<TaskAttributeValueModel>();

                //define localized model configuration action
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Name = _localizationService.GetLocalized(taskAttributeValue, entity => entity.Name, languageId, false, false);
                };                
            }

            model.TaskAttributeId = taskAttribute.Id;

            if (!excludeProperties)
                model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return model;
        }
        #endregion
    }
}
