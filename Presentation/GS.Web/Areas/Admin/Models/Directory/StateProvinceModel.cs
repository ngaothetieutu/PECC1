using System.Collections.Generic;
using FluentValidation.Attributes;
using GS.Web.Areas.Admin.Validators.Directory;
using GS.Web.Framework.Models;
using GS.Web.Framework.Mvc.ModelBinding;

namespace GS.Web.Areas.Admin.Models.Directory
{
    /// <summary>
    /// Represents a state and province model
    /// </summary>
    [Validator(typeof(StateProvinceValidator))]
    public partial class StateProvinceModel : BaseGSEntityModel, ILocalizedModel<StateProvinceLocalizedModel>
    {
        #region Ctor

        public StateProvinceModel()
        {
            Locales = new List<StateProvinceLocalizedModel>();
        }

        #endregion

        #region Properties

        public int CountryId { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Countries.States.Fields.Name")]
        public string Name { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Countries.States.Fields.Abbreviation")]
        public string Abbreviation { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Countries.States.Fields.Published")]
        public bool Published { get; set; }

        [GSResourceDisplayName("Admin.Configuration.Countries.States.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<StateProvinceLocalizedModel> Locales { get; set; }

        #endregion
    }

    public partial class StateProvinceLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }
        
        [GSResourceDisplayName("Admin.Configuration.Countries.States.Fields.Name")]
        public string Name { get; set; }
    }
}