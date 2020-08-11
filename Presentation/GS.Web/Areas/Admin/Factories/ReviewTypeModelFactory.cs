using System;
using System.Linq;
using GS.Core.Domain.Catalog;
using GS.Services.Catalog;
using GS.Services.Localization;
using GS.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using GS.Web.Areas.Admin.Models.Catalog;
using GS.Web.Framework.Extensions;
using GS.Web.Framework.Factories;

namespace GS.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents a review type model factory implementation
    /// </summary>
    public partial class ReviewTypeModelFactory : IReviewTypeModelFactory
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IReviewTypeService _reviewTypeService;

        #endregion

        #region Ctor

        public ReviewTypeModelFactory(ILocalizationService localizationService,
            ILocalizedModelFactory localizedModelFactory,
            IReviewTypeService reviewTypeService)
        {
            this._localizationService = localizationService;
            this._localizedModelFactory = localizedModelFactory;
            this._reviewTypeService = reviewTypeService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare review type search model
        /// </summary>
        /// <param name="searchModel">Review type search model</param>
        /// <returns>Review type search model</returns>
        public virtual ReviewTypeSearchModel PrepareReviewTypeSearchModel(ReviewTypeSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }
        

        /// <summary>
        /// Prepare review type model
        /// </summary>
        /// <param name="model">Review type model</param>
        /// <param name="reviewType">Review type</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Review type model</returns>
        public virtual ReviewTypeModel PrepareReviewTypeModel(ReviewTypeModel model, 
            ReviewType reviewType, bool excludeProperties = false)
        {
            Action<ReviewTypeLocalizedModel, int> localizedModelConfiguration = null;

            if (reviewType != null)
            {
                //fill in model values from the entity
                model = model ?? reviewType.ToModel<ReviewTypeModel>();

                //define localized model configuration action
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Name = _localizationService.GetLocalized(reviewType, entity => entity.Name, languageId, false, false);
                    locale.Description = _localizationService.GetLocalized(reviewType, entity => entity.Description, languageId, false, false);
                };
            }

            //prepare localized models
            if (!excludeProperties)
                model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return model;
        }

        #endregion
    }
}
