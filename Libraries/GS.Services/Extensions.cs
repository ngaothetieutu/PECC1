using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using GS.Core;
using GS.Core.Infrastructure;
using GS.Services.Localization;
using GS.Core.Domain.Contracts;
using GS.Core.Data;

namespace GS.Services
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Extensions
    {
        public static string ToAmountSummary(this IList<ContractCurrency> items,int PrimaryExchangeRateCurrencyId)
        {
            return string.Join(", ", items.Select(c => {
                var _s = "";
                //neu la tien te chinh (VNĐ)
                if (c.currency.Id == PrimaryExchangeRateCurrencyId)
                {
                    if(c.TotalAmount< 1000000000m)
                        _s = string.Format("{0} triệu đ", Convert.ToDecimal(c.TotalAmount / 1000000m).ToVNStringNumber());
                    else
                        _s = string.Format("{0} tỉ đ", Convert.ToDecimal(c.TotalAmount / 1000000000m).ToVNStringNumber(true));
                }
                else
                {
                    if (c.currency.Id == GSDataSettingsDefaults.GoldCurrencyId)
                        _s = string.Format("{0} {1}", c.TotalAmount.ToVNStringNumber(), c.currency.CurrencyCode);
                    else
                        _s = string.Format("{0} nghìn {1}", Convert.ToDecimal(c.TotalAmount / 1000m).ToVNStringNumber(), c.currency.CurrencyCode);
                }
                return _s;
            }).ToList());
        }
        /// <summary>
        /// Convert to select list
        /// </summary>
        /// <typeparam name="TEnum">Enum type</typeparam>
        /// <param name="enumObj">Enum</param>
        /// <param name="markCurrentAsSelected">Mark current value as selected</param>
        /// <param name="valuesToExclude">Values to exclude</param>
        /// <param name="useLocalization">Localize</param>
        /// <returns>SelectList</returns>
        public static SelectList ToSelectList<TEnum>(this TEnum enumObj,
           bool markCurrentAsSelected = true, int[] valuesToExclude = null, bool useLocalization = true) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("An Enumeration type is required.", nameof(enumObj));

            var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
            var values = from TEnum enumValue in Enum.GetValues(typeof(TEnum))
                         where valuesToExclude == null || !valuesToExclude.Contains(Convert.ToInt32(enumValue))
                         select new { ID = Convert.ToInt32(enumValue), Name = useLocalization ? localizationService.GetLocalizedEnum(enumValue) : CommonHelper.ConvertEnum(enumValue.ToString()) };
            object selectedValue = null;
            if (markCurrentAsSelected)
                selectedValue = Convert.ToInt32(enumObj);
            return new SelectList(values, "ID", "Name", selectedValue);
        }

        /// <summary>
        /// Convert to select list
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="objList">List of objects</param>
        /// <param name="selector">Selector for name</param>
        /// <returns>SelectList</returns>
        public static SelectList ToSelectList<T>(this T objList, Func<BaseEntity, string> selector) where T : IEnumerable<BaseEntity>
        {
            return new SelectList(objList.Select(p => new { ID = p.Id, Name = selector(p) }), "ID", "Name");
        }
        
    }
}