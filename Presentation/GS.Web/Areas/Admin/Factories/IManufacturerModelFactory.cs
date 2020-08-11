using GS.Core.Domain.Catalog;
using GS.Web.Areas.Admin.Models.Catalog;

namespace GS.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the manufacturer model factory
    /// </summary>
    public partial interface IManufacturerModelFactory
    {
        /// <summary>
        /// Prepare manufacturer search model
        /// </summary>
        /// <param name="searchModel">Manufacturer search model</param>
        /// <returns>Manufacturer search model</returns>
        ManufacturerSearchModel PrepareManufacturerSearchModel(ManufacturerSearchModel searchModel);

        /// <summary>
        /// Prepare paged manufacturer list model
        /// </summary>
        /// <param name="searchModel">Manufacturer search model</param>
        /// <returns>Manufacturer list model</returns>
        ManufacturerListModel PrepareManufacturerListModel(ManufacturerSearchModel searchModel);

        /// <summary>
        /// Prepare manufacturer model
        /// </summary>
        /// <param name="model">Manufacturer model</param>
        /// <param name="manufacturer">Manufacturer</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Manufacturer model</returns>
        ManufacturerModel PrepareManufacturerModel(ManufacturerModel model,
            Manufacturer manufacturer, bool excludeProperties = false);

        }
}