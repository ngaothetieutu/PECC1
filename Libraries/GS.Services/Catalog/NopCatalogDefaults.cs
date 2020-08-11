namespace GS.Services.Catalog
{
    /// <summary>
    /// Represents default values related to catalog services
    /// </summary>
    public static partial class GSCatalogDefaults
    {
        #region Categories

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : category ID
        /// </remarks>
        public static string CategoriesByIdCacheKey => "GS.category.id-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : parent category ID
        /// {1} : show hidden records?
        /// {2} : current customer ID
        /// {3} : store ID
        /// </remarks>
        public static string CategoriesByParentCategoryIdCacheKey => "GS.category.byparent-{0}-{1}-{2}-{3}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : current store ID
        /// {1} : comma separated list of customer roles
        /// {2} : show hidden records?
        /// </remarks>
        public static string CategoriesAllCacheKey => "GS.category.all-{0}-{1}-{2}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : parent category id
        /// {1} : comma separated list of customer roles
        /// {2} : current store ID
        /// {3} : show hidden records?
        /// </remarks>
        public static string CategoriesChildIdentifiersCacheKey => "GS.category.childidentifiers-{0}-{1}-{2}-{3}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// {1} : category ID
        /// {2} : page index
        /// {3} : page size
        /// {4} : current customer ID
        /// {5} : store ID
        /// </remarks>
        public static string ProductCategoriesAllByCategoryIdCacheKey => "GS.productcategory.allbycategoryid-{0}-{1}-{2}-{3}-{4}-{5}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// {1} : product ID
        /// {2} : current customer ID
        /// {3} : store ID
        /// </remarks>
        public static string ProductCategoriesAllByProductIdCacheKey => "GS.productcategory.allbyproductid-{0}-{1}-{2}-{3}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string CategoriesPatternCacheKey => "GS.category.";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ProductCategoriesPatternCacheKey => "GS.productcategory.";

        #endregion

        #region Manufacturers

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : manufacturer ID
        /// </remarks>
        public static string ManufacturersByIdCacheKey => "GS.manufacturer.id-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// {1} : manufacturer ID
        /// {2} : page index
        /// {3} : page size
        /// {4} : current customer ID
        /// {5} : store ID
        /// </remarks>
        public static string ProductManufacturersAllByManufacturerIdCacheKey => "GS.productmanufacturer.allbymanufacturerid-{0}-{1}-{2}-{3}-{4}-{5}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// {1} : product ID
        /// {2} : current customer ID
        /// {3} : store ID
        /// </remarks>
        public static string ProductManufacturersAllByProductIdCacheKey => "GS.productmanufacturer.allbyproductid-{0}-{1}-{2}-{3}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ManufacturersPatternCacheKey => "GS.manufacturer.";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ProductManufacturersPatternCacheKey => "GS.productmanufacturer.";

        #endregion

        #region Products

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : product ID
        /// </remarks>
        public static string ProductsByIdCacheKey => "GS.product.id-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ProductsPatternCacheKey => "GS.product.";

        /// <summary>
        /// Gets a key for product prices
        /// </summary>
        /// <remarks>
        /// {0} : product id
        /// {1} : overridden product price
        /// {2} : additional charge
        /// {3} : include discounts (true, false)
        /// {4} : quantity
        /// {5} : roles of the current user
        /// {6} : current store ID
        /// </remarks>
        public static string ProductPriceModelCacheKey => "GS.totals.productprice-{0}-{1}-{2}-{3}-{4}-{5}-{6}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ProductPricePatternCacheKey => "GS.totals.productprice";

        /// <summary>
        /// Gets a key for category IDs of a product
        /// </summary>
        /// <remarks>
        /// {0} : product id
        /// {1} : roles of the current user
        /// {2} : current store ID
        /// </remarks>
        public static string ProductCategoryIdsModelCacheKey => "GS.totals.product.categoryids-{0}-{1}-{2}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ProductCategoryIdsPatternCacheKey => "GS.totals.product.categoryids";

        /// <summary>
        /// Gets a key for manufacturer IDs of a product
        /// </summary>
        /// <remarks>
        /// {0} : product id
        /// {1} : roles of the current user
        /// {2} : current store ID
        /// </remarks>
        public static string ProductManufacturerIdsModelCacheKey => "GS.totals.product.manufacturerids-{0}-{1}-{2}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ProductManufacturerIdsPatternCacheKey => "GS.totals.product.manufacturerids";

        /// <summary>
        /// Gets a template of product name on copying
        /// </summary>
        /// <remarks>
        /// {0} : product name
        /// </remarks>
        public static string ProductCopyNameTemplate => "Copy of {0}";

        #endregion

        #region Product attributes

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : page index
        /// {1} : page size
        /// </remarks>
        public static string ProductAttributesAllCacheKey => "GS.productattribute.all-{0}-{1}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : product attribute ID
        /// </remarks>
        public static string ProductAttributesByIdCacheKey => "GS.productattribute.id-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : product ID
        /// </remarks>
        public static string ProductAttributeMappingsAllCacheKey => "GS.productattributemapping.all-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : product attribute mapping ID
        /// </remarks>
        public static string ProductAttributeMappingsByIdCacheKey => "GS.productattributemapping.id-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : product attribute mapping ID
        /// </remarks>
        public static string ProductAttributeValuesAllCacheKey => "GS.productattributevalue.all-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : product attribute value ID
        /// </remarks>
        public static string ProductAttributeValuesByIdCacheKey => "GS.productattributevalue.id-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : product ID
        /// </remarks>
        public static string ProductAttributeCombinationsAllCacheKey => "GS.productattributecombination.all-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ProductAttributesPatternCacheKey => "GS.productattribute.";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ProductAttributeMappingsPatternCacheKey => "GS.productattributemapping.";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ProductAttributeValuesPatternCacheKey => "GS.productattributevalue.";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ProductAttributeCombinationsPatternCacheKey => "GS.productattributecombination.";

        #endregion

        #region Product tags

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// </remarks>
        public static string ProductTagCountCacheKey => "GS.producttag.count-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : product ID
        /// </remarks>
        public static string ProductTagAllByProductIdCacheKey => "GS.producttag.allbyproductid-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ProductTagPatternCacheKey => "GS.producttag.";

        #endregion

        #region Review type

        /// <summary>
        /// Key for caching all review types
        /// </summary>
        public static string ReviewTypeAllKey => "GS.reviewType.all";

        /// <summary>
        /// Key for caching review type
        /// </summary>
        /// <remarks>
        /// {0} : review type ID
        /// </remarks>
        public static string ReviewTypeByIdKey => "GS.reviewType.id-{0}";

        /// <summary>
        /// Key pattern to clear cache review types
        /// </summary>
        public static string ReviewTypeByPatternKey => "GS.reviewType.";

        /// <summary>
        /// Key for caching product review and review type mapping
        /// </summary>
        /// <remarks>
        /// {0} : product review ID
        /// </remarks>
        public static string ProductReviewReviewTypeMappingAllKey => "GS.productReviewReviewTypeMapping.all-{0}";

        #endregion

        #region Specification attributes

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : product ID
        /// {1} : specification attribute option ID
        /// {2} : allow filtering
        /// {3} : show on product page
        /// </remarks>
        public static string ProductSpecificationAttributeAllByProductIdCacheKey => "GS.productspecificationattribute.allbyproductid-{0}-{1}-{2}-{3}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ProductSpecificationAttributePatternCacheKey => "GS.productspecificationattribute.";

        #endregion
        #region Construction 

        /// <summary>
        /// Key for caching all Construction types
        /// </summary>
        public static string ConstructionAllKey => "GS.Construction.all";

        /// <summary>
        /// Key for caching Construction type
        /// </summary>
        /// <remarks>
        /// {0} : Construction type ID
        /// </remarks>
        public static string ConstructionByIdKey => "GS.Construction.id-{0}";

        /// <summary>
        /// Key pattern to clear cache Construction types
        /// </summary>
        public static string ConstructionByPatternKey => "GS.Construction.";
        #endregion
        #region ProcuringAgency 

        /// <summary>
        /// Key for caching all Construction types
        /// </summary>
        public static string ProcuringAgencyAllKey => "GS.ProcuringAgency.all";

        /// <summary>
        /// Key for caching Construction type
        /// </summary>
        /// <remarks>
        /// {0} : Construction type ID
        /// </remarks>
        public static string ProcuringAgencyByIdKey => "GS.ProcuringAgency.id-{0}";

        /// <summary>
        /// Key pattern to clear cache Construction types
        /// </summary>
        public static string ProcuringAgencyByPatternKey => "GS.ProcuringAgency.";
        #endregion
        #region Construction type

        /// <summary>
        /// Key for caching all Construction types
        /// </summary>
        public static string ConstructionTypeAllKey => "GS.ConstructionType.all";

        /// <summary>
        /// Key for caching Construction type
        /// </summary>
        /// <remarks>
        /// {0} : Construction type ID
        /// </remarks>
        public static string ConstructionTypeByIdKey => "GS.ConstructionType.id-{0}";

        /// <summary>
        /// Key pattern to clear cache Construction types
        /// </summary>
        public static string ConstructionTypeByPatternKey => "GS.ConstructionType.";
        #region Construction Capital
        /// <summary>
        /// Key for caching all Construction types
        /// </summary>
        public static string ConstructionCapitalAllKey => "GS.ConstructionCapital.all";

        /// <summary>
        /// Key for caching Construction type
        /// </summary>
        /// <remarks>
        /// {0} : Construction type ID
        /// </remarks>
        public static string ConstructionCapitalByIdKey => "GS.ConstructionCapital.id-{0}";

        /// <summary>
        /// Key pattern to clear cache Construction types
        /// </summary>
        public static string ConstructionCapitalByPatternKey => "GS.ConstructionCapital.";
        #endregion
        #endregion
        #region Contract Form

        /// <summary>
        /// Key for caching all Contract Forms
        /// </summary>
        public static string ContractFormAllKey => "GS.ContractForm.all";

        /// <summary>
        /// Key for caching ContractForm
        /// </summary>
        /// <remarks>
        /// {0} : Contract Form ID
        /// </remarks>
        public static string ContractFormByIdKey => "GS.ContractForm.id-{0}";

        /// <summary>
        /// Key pattern to clear cache Contract Forms
        /// </summary>
        public static string ContractFormByPatternKey => "GS.ContractForm.";
        #endregion

        #region Contract 

        /// <summary>
        /// Key for caching all Construction types
        /// </summary>
        public static string Contract => "GS.Contract.all";

        /// <summary>
        /// Key for caching Construction type
        /// </summary>
        /// <remarks>
        /// {0} : Construction type ID
        /// </remarks>
        public static string ContractByIdKey => "GS.Contract.id-{0}";

        /// <summary>
        /// Key pattern to clear cache Construction types
        /// </summary>
        public static string ContractByPatternKey => "GS.Contract.";
        #endregion
        #region Contract Relate
        /// <summary>
        /// Key for caching all Contract Relate
        /// </summary>
        public static string ContractRelateAllKey => "GS.ContractRelate.all";

        /// <summary>
        /// Key for caching ContractRelate
        /// </summary>
        /// <remarks>
        /// {0} : Contract Relate ID
        /// </remarks>
        public static string ContractRelateByIdKey => "GS.ContractRelate.id-{0}";

        /// <summary>
        /// Key pattern to clear cache Contract Relate
        /// </summary>
        public static string ContractRelateByPatternKey => "GS.ContractRelate.";
        #endregion
        #region ContractLog

        /// <summary>
        /// Key for caching all ContractLog
        /// </summary>
        public static string ContractLogAllKey => "GS.ContractLog.all";

        /// <summary>
        /// Key for caching ContractLog
        /// </summary>
        /// <remarks>
        /// {0} : Construction type ID
        /// </remarks>
        public static string ContractLogByIdKey => "GS.ContractLog.id-{0}";

        /// <summary>
        /// Key pattern to clear cache ContractLog
        /// </summary>
        public static string ContractLogByPatternKey => "GS.ContractLog.";
        #endregion
        #region Contract Type

        /// <summary>
        /// Key for caching all Contract Types
        /// </summary>
        public static string ContractTypeAllKey => "GS.ContractType.all";

        /// <summary>
        /// Key for caching Contract Type
        /// </summary>
        /// <remarks>
        /// {0} : Contract Type ID
        /// </remarks>
        public static string ContractTypeByIdKey => "GS.ContractType.id-{0}";

        /// <summary>
        /// Key pattern to clear cache Contract Type
        /// </summary>
        public static string ContractTypeByPatternKey => "GS.ContractType.";
        #endregion

        #region Contract Period

        /// <summary>
        /// Key for caching all Contract Periods
        /// </summary>
        public static string ContractPeriodAllKey => "GS.ContractPeriod.all";

        /// <summary>
        /// Key for caching Contract Period
        /// </summary>
        /// <remarks>
        /// {0} : Contract Period ID
        /// </remarks>
        public static string ContractPeriodByIdKey => "GS.ContractPeriod.id-{0}";

        /// <summary>
        /// Key pattern to clear cache Contract Period
        /// </summary>
        public static string ContractPeriodByPatternKey => "GS.ContractPeriod.";
        #endregion

        #region Task Group

        /// <summary>
        /// Key for caching all Task Groups
        /// </summary>
        public static string TaskGroupAllKey => "GS.TaskGroup.all";

        /// <summary>
        /// Key for caching Task Group
        /// </summary>
        /// <remarks>
        /// {0} : Task Group ID
        /// </remarks>
        public static string TaskGroupByIdKey => "GS.TaskGroup.id-{0}";

        /// <summary>
        /// Key pattern to clear cache Task Group
        /// </summary>
        public static string TaskGroupByPatternKey => "GS.TaskGroup.";
        #endregion
    }
}