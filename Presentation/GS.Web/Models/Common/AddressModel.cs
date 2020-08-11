using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using GS.Web.Framework.Mvc.ModelBinding;
using GS.Web.Framework.Models;
using GS.Web.Validators.Common;

namespace GS.Web.Models.Common
{
    [Validator(typeof(AddressValidator))]
    public partial class AddressModel : BaseGSEntityModel
    {
        public AddressModel()
        {
            AvailableCountries = new List<SelectListItem>();
            AvailableStates = new List<SelectListItem>();
            CustomAddressAttributes = new List<AddressAttributeModel>();
        }

        [GSResourceDisplayName("Address.Fields.FirstName")]
        public string FirstName { get; set; }
        [GSResourceDisplayName("Address.Fields.LastName")]
        public string LastName { get; set; }
        [DataType(DataType.EmailAddress)]
        [GSResourceDisplayName("Address.Fields.Email")]
        public string Email { get; set; }


        public bool CompanyEnabled { get; set; }
        public bool CompanyRequired { get; set; }
        [GSResourceDisplayName("Address.Fields.Company")]
        public string Company { get; set; }

        public bool CountryEnabled { get; set; }
        [GSResourceDisplayName("Address.Fields.Country")]
        public int? CountryId { get; set; }
        [GSResourceDisplayName("Address.Fields.Country")]
        public string CountryName { get; set; }

        public bool StateProvinceEnabled { get; set; }
        [GSResourceDisplayName("Address.Fields.StateProvince")]
        public int? StateProvinceId { get; set; }
        [GSResourceDisplayName("Address.Fields.StateProvince")]
        public string StateProvinceName { get; set; }

        public bool CountyEnabled { get; set; }
        public bool CountyRequired { get; set; }
        [GSResourceDisplayName("Address.Fields.County")]
        public string County { get; set; }

        public bool CityEnabled { get; set; }
        public bool CityRequired { get; set; }
        [GSResourceDisplayName("Address.Fields.City")]
        public string City { get; set; }

        public bool StreetAddressEnabled { get; set; }
        public bool StreetAddressRequired { get; set; }
        [GSResourceDisplayName("Address.Fields.Address1")]
        public string Address1 { get; set; }

        public bool StreetAddress2Enabled { get; set; }
        public bool StreetAddress2Required { get; set; }
        [GSResourceDisplayName("Address.Fields.Address2")]
        public string Address2 { get; set; }

        public bool ZipPostalCodeEnabled { get; set; }
        public bool ZipPostalCodeRequired { get; set; }
        [GSResourceDisplayName("Address.Fields.ZipPostalCode")]
        public string ZipPostalCode { get; set; }

        public bool PhoneEnabled { get; set; }
        public bool PhoneRequired { get; set; }
        [DataType(DataType.PhoneNumber)]
        [GSResourceDisplayName("Address.Fields.PhoneNumber")]
        public string PhoneNumber { get; set; }

        public bool FaxEnabled { get; set; }
        public bool FaxRequired { get; set; }
        [GSResourceDisplayName("Address.Fields.FaxNumber")]
        public string FaxNumber { get; set; }
        
        public IList<SelectListItem> AvailableCountries { get; set; }
        public IList<SelectListItem> AvailableStates { get; set; }

        public string FormattedCustomAddressAttributes { get; set; }
        public IList<AddressAttributeModel> CustomAddressAttributes { get; set; }
    }
}