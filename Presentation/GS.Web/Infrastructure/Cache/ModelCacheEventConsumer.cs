﻿using GS.Core.Caching;
using GS.Core.Domain.Blogs;
using GS.Core.Domain.Catalog;
using GS.Core.Domain.Configuration;
using GS.Core.Domain.Directory;
using GS.Core.Domain.Localization;
using GS.Core.Domain.Media;
using GS.Core.Domain.News;
using GS.Core.Domain.Polls;
using GS.Core.Domain.Topics;
using GS.Core.Domain.Vendors;
using GS.Core.Events;
using GS.Core.Plugins;
using GS.Services.Cms;
using GS.Services.Events;

namespace GS.Web.Infrastructure.Cache
{
    /// <summary>
    /// Model cache event consumer (used for caching of presentation layer models)
    /// </summary>
    public partial class ModelCacheEventConsumer :
        //languages
        IConsumer<EntityInsertedEvent<Language>>,
        IConsumer<EntityUpdatedEvent<Language>>,
        IConsumer<EntityDeletedEvent<Language>>,
        //currencies
        IConsumer<EntityInsertedEvent<Currency>>,
        IConsumer<EntityUpdatedEvent<Currency>>,
        IConsumer<EntityDeletedEvent<Currency>>,
        //settings
        IConsumer<EntityUpdatedEvent<Setting>>,
        //manufacturers
        IConsumer<EntityInsertedEvent<Manufacturer>>,
        IConsumer<EntityUpdatedEvent<Manufacturer>>,
        IConsumer<EntityDeletedEvent<Manufacturer>>,
        //vendors
        IConsumer<EntityInsertedEvent<Vendor>>,
        IConsumer<EntityUpdatedEvent<Vendor>>,
        IConsumer<EntityDeletedEvent<Vendor>>,
        //categories
        IConsumer<EntityInsertedEvent<Category>>,
        IConsumer<EntityUpdatedEvent<Category>>,
        IConsumer<EntityDeletedEvent<Category>>,
        //specification attributes
        IConsumer<EntityUpdatedEvent<SpecificationAttribute>>,
        IConsumer<EntityDeletedEvent<SpecificationAttribute>>,
        //specification attribute options
        IConsumer<EntityUpdatedEvent<SpecificationAttributeOption>>,
        IConsumer<EntityDeletedEvent<SpecificationAttributeOption>>,
      
        //Topics
        IConsumer<EntityInsertedEvent<Topic>>,
        IConsumer<EntityUpdatedEvent<Topic>>,
        IConsumer<EntityDeletedEvent<Topic>>,
      
        //Picture
        IConsumer<EntityInsertedEvent<Picture>>,
        IConsumer<EntityUpdatedEvent<Picture>>,
        IConsumer<EntityDeletedEvent<Picture>>,
      
        //polls
        IConsumer<EntityInsertedEvent<Poll>>,
        IConsumer<EntityUpdatedEvent<Poll>>,
        IConsumer<EntityDeletedEvent<Poll>>,
        //blog posts
        IConsumer<EntityInsertedEvent<BlogPost>>,
        IConsumer<EntityUpdatedEvent<BlogPost>>,
        IConsumer<EntityDeletedEvent<BlogPost>>,
        //blog comments
        IConsumer<EntityDeletedEvent<BlogComment>>,
        //news items
        IConsumer<EntityInsertedEvent<NewsItem>>,
        IConsumer<EntityUpdatedEvent<NewsItem>>,
        IConsumer<EntityDeletedEvent<NewsItem>>,
        //news comments
        IConsumer<EntityDeletedEvent<NewsComment>>,
        //states/province
        IConsumer<EntityInsertedEvent<StateProvince>>,
        IConsumer<EntityUpdatedEvent<StateProvince>>,
        IConsumer<EntityDeletedEvent<StateProvince>>,
      
        //templates
        IConsumer<EntityInsertedEvent<CategoryTemplate>>,
        IConsumer<EntityUpdatedEvent<CategoryTemplate>>,
        IConsumer<EntityDeletedEvent<CategoryTemplate>>,
        IConsumer<EntityInsertedEvent<ManufacturerTemplate>>,
        IConsumer<EntityUpdatedEvent<ManufacturerTemplate>>,
        IConsumer<EntityDeletedEvent<ManufacturerTemplate>>,
       
        IConsumer<EntityInsertedEvent<TopicTemplate>>,
        IConsumer<EntityUpdatedEvent<TopicTemplate>>,
        IConsumer<EntityDeletedEvent<TopicTemplate>>,
      
        //plugins
        IConsumer<PluginUpdatedEvent>
    {
        #region Fields

        private readonly CatalogSettings _catalogSettings;
        private readonly IStaticCacheManager _cacheManager;

        #endregion

        #region Ctor

        public ModelCacheEventConsumer(CatalogSettings catalogSettings, IStaticCacheManager cacheManager)
        {
            this._cacheManager = cacheManager;
            this._catalogSettings = catalogSettings;
        }

        #endregion

        #region Cache keys 

        /// <summary>
        /// Key for categories on the search page
        /// </summary>
        /// <remarks>
        /// {0} : language id
        /// {1} : roles of the current user
        /// {2} : current store ID
        /// </remarks>
        public const string SEARCH_CATEGORIES_MODEL_KEY = "GS.pres.search.categories-{0}-{1}-{2}";
        public const string SEARCH_CATEGORIES_PATTERN_KEY = "GS.pres.search.categories";

        /// <summary>
        /// Key for ManufacturerNavigationModel caching
        /// </summary>
        /// <remarks>
        /// {0} : current manufacturer id
        /// {1} : language id
        /// {2} : roles of the current user
        /// {3} : current store ID
        /// </remarks>
        public const string MANUFACTURER_NAVIGATION_MODEL_KEY = "GS.pres.manufacturer.navigation-{0}-{1}-{2}-{3}";
        public const string MANUFACTURER_NAVIGATION_PATTERN_KEY = "GS.pres.manufacturer.navigation";

        /// <summary>
        /// Key for VendorNavigationModel caching
        /// </summary>
        public const string VENDOR_NAVIGATION_MODEL_KEY = "GS.pres.vendor.navigation";
        public const string VENDOR_NAVIGATION_PATTERN_KEY = "GS.pres.vendor.navigation";

        /// <summary>
        /// Key for caching of a value indicating whether a manufacturer has featured products
        /// </summary>
        /// <remarks>
        /// {0} : manufacturer id
        /// {1} : roles of the current user
        /// {2} : current store ID
        /// </remarks>
        public const string MANUFACTURER_HAS_FEATURED_PRODUCTS_KEY = "GS.pres.manufacturer.hasfeaturedproducts-{0}-{1}-{2}";
        public const string MANUFACTURER_HAS_FEATURED_PRODUCTS_PATTERN_KEY = "GS.pres.manufacturer.hasfeaturedproducts";
        public const string MANUFACTURER_HAS_FEATURED_PRODUCTS_PATTERN_KEY_BY_ID = "GS.pres.manufacturer.hasfeaturedproducts-{0}-";

        /// <summary>
        /// Key for list of CategorySimpleModel caching
        /// </summary>
        /// <remarks>
        /// {0} : language id
        /// {1} : comma separated list of customer roles
        /// {2} : current store ID
        /// </remarks>
        public const string CATEGORY_ALL_MODEL_KEY = "GS.pres.category.all-{0}-{1}-{2}";
        public const string CATEGORY_ALL_PATTERN_KEY = "GS.pres.category.all";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : comma separated list of customer roles
        /// {1} : current store ID
        /// {2} : category ID
        /// </remarks>
        public const string CATEGORY_NUMBER_OF_PRODUCTS_MODEL_KEY = "GS.pres.category.numberofproducts-{0}-{1}-{2}";
        public const string CATEGORY_NUMBER_OF_PRODUCTS_PATTERN_KEY = "GS.pres.category.numberofproducts";

        /// <summary>
        /// Key for caching of a value indicating whether a category has featured products
        /// </summary>
        /// <remarks>
        /// {0} : category id
        /// {1} : roles of the current user
        /// {2} : current store ID
        /// </remarks>
        public const string CATEGORY_HAS_FEATURED_PRODUCTS_KEY = "GS.pres.category.hasfeaturedproducts-{0}-{1}-{2}";
        public const string CATEGORY_HAS_FEATURED_PRODUCTS_PATTERN_KEY = "GS.pres.category.hasfeaturedproducts";
        public const string CATEGORY_HAS_FEATURED_PRODUCTS_PATTERN_KEY_BY_ID = "GS.pres.category.hasfeaturedproducts-{0}-";

        /// <summary>
        /// Key for caching of category breadcrumb
        /// </summary>
        /// <remarks>
        /// {0} : category id
        /// {1} : roles of the current user
        /// {2} : current store ID
        /// {3} : language ID
        /// </remarks>
        public const string CATEGORY_BREADCRUMB_KEY = "GS.pres.category.breadcrumb-{0}-{1}-{2}-{3}";
        public const string CATEGORY_BREADCRUMB_PATTERN_KEY = "GS.pres.category.breadcrumb";

        /// <summary>
        /// Key for caching of subcategories of certain category
        /// </summary>
        /// <remarks>
        /// {0} : category id
        /// {1} : roles of the current user
        /// {2} : current store ID
        /// {3} : language ID
        /// {4} : is connection SSL secured (included in a category picture URL)
        /// </remarks>
        public const string CATEGORY_SUBCATEGORIES_KEY = "GS.pres.category.subcategories-{0}-{1}-{2}-{3}-{4}-{5}";
        public const string CATEGORY_SUBCATEGORIES_PATTERN_KEY = "GS.pres.category.subcategories";

        /// <summary>
        /// Key for caching of categories displayed on home page
        /// </summary>
        /// <remarks>
        /// {0} : roles of the current user
        /// {1} : current store ID
        /// {2} : language ID
        /// {3} : is connection SSL secured (included in a category picture URL)
        /// </remarks>
        public const string CATEGORY_HOMEPAGE_KEY = "GS.pres.category.homepage-{0}-{1}-{2}-{3}-{4}";
        public const string CATEGORY_HOMEPAGE_PATTERN_KEY = "GS.pres.category.homepage";

        /// <summary>
        /// Key for SpecificationAttributeOptionFilter caching
        /// </summary>
        /// <remarks>
        /// {0} : comma separated list of specification attribute option IDs
        /// {1} : language id
        /// </remarks>
        public const string SPECS_FILTER_MODEL_KEY = "GS.pres.filter.specs-{0}-{1}";
        public const string SPECS_FILTER_PATTERN_KEY = "GS.pres.filter.specs";

        /// <summary>
        /// Key for ProductBreadcrumbModel caching
        /// </summary>
        /// <remarks>
        /// {0} : product id
        /// {1} : language id
        /// {2} : comma separated list of customer roles
        /// {3} : current store ID
        /// </remarks>
        public const string PRODUCT_BREADCRUMB_MODEL_KEY = "GS.pres.product.breadcrumb-{0}-{1}-{2}-{3}";
        public const string PRODUCT_BREADCRUMB_PATTERN_KEY = "GS.pres.product.breadcrumb";
        public const string PRODUCT_BREADCRUMB_PATTERN_KEY_BY_ID = "GS.pres.product.breadcrumb-{0}-";

        /// <summary>
        /// Key for ProductTagModel caching
        /// </summary>
        /// <remarks>
        /// {0} : product id
        /// {1} : language id
        /// {2} : current store ID
        /// </remarks>
        public const string PRODUCTTAG_BY_PRODUCT_MODEL_KEY = "GS.pres.producttag.byproduct-{0}-{1}-{2}";
        public const string PRODUCTTAG_BY_PRODUCT_PATTERN_KEY = "GS.pres.producttag.byproduct";

        /// <summary>
        /// Key for PopularProductTagsModel caching
        /// </summary>
        /// <remarks>
        /// {0} : language id
        /// {1} : current store ID
        /// </remarks>
        public const string PRODUCTTAG_POPULAR_MODEL_KEY = "GS.pres.producttag.popular-{0}-{1}";
        public const string PRODUCTTAG_POPULAR_PATTERN_KEY = "GS.pres.producttag.popular";

        /// <summary>
        /// Key for ProductManufacturers model caching
        /// </summary>
        /// <remarks>
        /// {0} : product id
        /// {1} : language id
        /// {2} : roles of the current user
        /// {3} : current store ID
        /// </remarks>
        public const string PRODUCT_MANUFACTURERS_MODEL_KEY = "GS.pres.product.manufacturers-{0}-{1}-{2}-{3}";
        public const string PRODUCT_MANUFACTURERS_PATTERN_KEY = "GS.pres.product.manufacturers";
        public const string PRODUCT_MANUFACTURERS_PATTERN_KEY_BY_ID = "GS.pres.product.manufacturers-{0}-";

        /// <summary>
        /// Key for ProductSpecificationModel caching
        /// </summary>
        /// <remarks>
        /// {0} : product id
        /// {1} : language id
        /// </remarks>
        public const string PRODUCT_SPECS_MODEL_KEY = "GS.pres.product.specs-{0}-{1}";
        public const string PRODUCT_SPECS_PATTERN_KEY = "GS.pres.product.specs";
        public const string PRODUCT_SPECS_PATTERN_KEY_BY_ID = "GS.pres.product.specs-{0}-";

        /// <summary>
        /// Key for caching of a value indicating whether a product has product attributes
        /// </summary>
        /// <remarks>
        /// {0} : product id
        /// </remarks>
        public const string PRODUCT_HAS_PRODUCT_ATTRIBUTES_KEY = "GS.pres.product.hasproductattributes-{0}-";
        public const string PRODUCT_HAS_PRODUCT_ATTRIBUTES_PATTERN_KEY = "GS.pres.product.hasproductattributes";
        public const string PRODUCT_HAS_PRODUCT_ATTRIBUTES_PATTERN_KEY_BY_ID = "GS.pres.product.hasproductattributes-{0}-";

        /// <summary>
        /// Key for TopicModel caching
        /// </summary>
        /// <remarks>
        /// {0} : topic system name
        /// {1} : language id
        /// {2} : store id
        /// {3} : comma separated list of customer roles
        /// </remarks>
        public const string TOPIC_MODEL_BY_SYSTEMNAME_KEY = "GS.pres.topic.details.bysystemname-{0}-{1}-{2}-{3}";
        /// <summary>
        /// Key for TopicModel caching
        /// </summary>
        /// <remarks>
        /// {0} : topic id
        /// {1} : language id
        /// {2} : store id
        /// {3} : comma separated list of customer roles
        /// </remarks>
        public const string TOPIC_MODEL_BY_ID_KEY = "GS.pres.topic.details.byid-{0}-{1}-{2}-{3}";
        /// <summary>
        /// Key for TopicModel caching
        /// </summary>
        /// <remarks>
        /// {0} : topic system name
        /// {1} : language id
        /// {2} : store id
        /// {3} : comma separated list of customer roles
        /// </remarks>
        public const string TOPIC_SENAME_BY_SYSTEMNAME = "GS.pres.topic.sename.bysystemname-{0}-{1}-{2}-{3}";
        /// <summary>
        /// Key for TopicModel caching
        /// </summary>
        /// <remarks>
        /// {0} : topic system name
        /// {1} : language id
        /// {2} : store id
        /// {3} : comma separated list of customer roles
        /// </remarks>
        public const string TOPIC_TITLE_BY_SYSTEMNAME = "GS.pres.topic.title.bysystemname-{0}-{1}-{2}-{3}";
        /// <summary>
        /// Key for TopMenuModel caching
        /// </summary>
        /// <remarks>
        /// {0} : language id
        /// {1} : current store ID
        /// {2} : comma separated list of customer roles
        /// </remarks>
        public const string TOPIC_TOP_MENU_MODEL_KEY = "GS.pres.topic.topmenu-{0}-{1}-{2}";
        /// <summary>
        /// Key for TopMenuModel caching
        /// </summary>
        /// <remarks>
        /// {0} : language id
        /// {1} : current store ID
        /// {2} : comma separated list of customer roles
        /// </remarks>
        public const string TOPIC_FOOTER_MODEL_KEY = "GS.pres.topic.footer-{0}-{1}-{2}";
        public const string TOPIC_PATTERN_KEY = "GS.pres.topic";

        /// <summary>
        /// Key for CategoryTemplate caching
        /// </summary>
        /// <remarks>
        /// {0} : category template id
        /// </remarks>
        public const string CATEGORY_TEMPLATE_MODEL_KEY = "GS.pres.categorytemplate-{0}";
        public const string CATEGORY_TEMPLATE_PATTERN_KEY = "GS.pres.categorytemplate";

        /// <summary>
        /// Key for ManufacturerTemplate caching
        /// </summary>
        /// <remarks>
        /// {0} : manufacturer template id
        /// </remarks>
        public const string MANUFACTURER_TEMPLATE_MODEL_KEY = "GS.pres.manufacturertemplate-{0}";
        public const string MANUFACTURER_TEMPLATE_PATTERN_KEY = "GS.pres.manufacturertemplate";

        /// <summary>
        /// Key for ProductTemplate caching
        /// </summary>
        /// <remarks>
        /// {0} : product template id
        /// </remarks>
        public const string PRODUCT_TEMPLATE_MODEL_KEY = "GS.pres.producttemplate-{0}";
        public const string PRODUCT_TEMPLATE_PATTERN_KEY = "GS.pres.producttemplate";

        /// <summary>
        /// Key for TopicTemplate caching
        /// </summary>
        /// <remarks>
        /// {0} : topic template id
        /// </remarks>
        public const string TOPIC_TEMPLATE_MODEL_KEY = "GS.pres.topictemplate-{0}";
        public const string TOPIC_TEMPLATE_PATTERN_KEY = "GS.pres.topictemplate";

        /// <summary>
        /// Key for bestsellers identifiers displayed on the home page
        /// </summary>
        /// <remarks>
        /// {0} : current store ID
        /// </remarks>
        public const string HOMEPAGE_BESTSELLERS_IDS_KEY = "GS.pres.bestsellers.homepage-{0}";
        public const string HOMEPAGE_BESTSELLERS_IDS_PATTERN_KEY = "GS.pres.bestsellers.homepage";

        /// <summary>
        /// Key for "also purchased" product identifiers displayed on the product details page
        /// </summary>
        /// <remarks>
        /// {0} : current product id
        /// {1} : current store ID
        /// </remarks>
        public const string PRODUCTS_ALSO_PURCHASED_IDS_KEY = "GS.pres.alsopuchased-{0}-{1}";
        public const string PRODUCTS_ALSO_PURCHASED_IDS_PATTERN_KEY = "GS.pres.alsopuchased";

        /// <summary>
        /// Key for "related" product identifiers displayed on the product details page
        /// </summary>
        /// <remarks>
        /// {0} : current product id
        /// {1} : current store ID
        /// </remarks>
        public const string PRODUCTS_RELATED_IDS_KEY = "GS.pres.related-{0}-{1}";
        public const string PRODUCTS_RELATED_IDS_PATTERN_KEY = "GS.pres.related";

        /// <summary>
        /// Key for default product picture caching (all pictures)
        /// </summary>
        /// <remarks>
        /// {0} : product id
        /// {1} : picture size
        /// {2} : isAssociatedProduct?
        /// {3} : language ID ("alt" and "title" can depend on localized product name)
        /// {4} : is connection SSL secured?
        /// {5} : current store ID
        /// </remarks>
        public const string PRODUCT_DEFAULTPICTURE_MODEL_KEY = "GS.pres.product.detailspictures-{0}-{1}-{2}-{3}-{4}-{5}";
        public const string PRODUCT_DEFAULTPICTURE_PATTERN_KEY = "GS.pres.product.detailspictures";
        public const string PRODUCT_DEFAULTPICTURE_PATTERN_KEY_BY_ID = "GS.pres.product.detailspictures-{0}-";

        /// <summary>
        /// Key for product picture caching on the product details page
        /// </summary>
        /// <remarks>
        /// {0} : product id
        /// {1} : picture size
        /// {2} : value indicating whether a default picture is displayed in case if no real picture exists
        /// {3} : language ID ("alt" and "title" can depend on localized product name)
        /// {4} : is connection SSL secured?
        /// {5} : current store ID
        /// </remarks>
        public const string PRODUCT_DETAILS_PICTURES_MODEL_KEY = "GS.pres.product.picture-{0}-{1}-{2}-{3}-{4}-{5}";
        public const string PRODUCT_DETAILS_PICTURES_PATTERN_KEY = "GS.pres.product.picture";
        public const string PRODUCT_DETAILS_PICTURES_PATTERN_KEY_BY_ID = "GS.pres.product.picture-{0}-";

        /// <summary>
        /// Key for product reviews caching
        /// </summary>
        /// <remarks>
        /// {0} : product id
        /// {1} : current store ID
        /// </remarks>
        public const string PRODUCT_REVIEWS_MODEL_KEY = "GS.pres.product.reviews-{0}-{1}";
        public const string PRODUCT_REVIEWS_PATTERN_KEY = "GS.pres.product.reviews";
        public const string PRODUCT_REVIEWS_PATTERN_KEY_BY_ID = "GS.pres.product.reviews-{0}-";

        /// <summary>
        /// Key for product attribute picture caching on the product details page
        /// </summary>
        /// <remarks>
        /// {0} : picture id
        /// {1} : is connection SSL secured?
        /// {2} : current store ID
        /// </remarks>
        public const string PRODUCTATTRIBUTE_PICTURE_MODEL_KEY = "GS.pres.productattribute.picture-{0}-{1}-{2}";
        public const string PRODUCTATTRIBUTE_PICTURE_PATTERN_KEY = "GS.pres.productattribute.picture";

        /// <summary>
        /// Key for product attribute picture caching on the product details page
        /// </summary>
        /// <remarks>
        /// {0} : picture id
        /// {1} : is connection SSL secured?
        /// {2} : current store ID
        /// </remarks>
        public const string PRODUCTATTRIBUTE_IMAGESQUARE_PICTURE_MODEL_KEY = "GS.pres.productattribute.imagesquare.picture-{0}-{1}-{2}";
        public const string PRODUCTATTRIBUTE_IMAGESQUARE_PICTURE_PATTERN_KEY = "GS.pres.productattribute.imagesquare.picture";

        /// <summary>
        /// Key for category picture caching
        /// </summary>
        /// <remarks>
        /// {0} : category id
        /// {1} : picture size
        /// {2} : value indicating whether a default picture is displayed in case if no real picture exists
        /// {3} : language ID ("alt" and "title" can depend on localized category name)
        /// {4} : is connection SSL secured?
        /// {5} : current store ID
        /// </remarks>
        public const string CATEGORY_PICTURE_MODEL_KEY = "GS.pres.category.picture-{0}-{1}-{2}-{3}-{4}-{5}";
        public const string CATEGORY_PICTURE_PATTERN_KEY = "GS.pres.category.picture";
        public const string CATEGORY_PICTURE_PATTERN_KEY_BY_ID = "GS.pres.category.picture-{0}-";

        /// <summary>
        /// Key for manufacturer picture caching
        /// </summary>
        /// <remarks>
        /// {0} : manufacturer id
        /// {1} : picture size
        /// {2} : value indicating whether a default picture is displayed in case if no real picture exists
        /// {3} : language ID ("alt" and "title" can depend on localized manufacturer name)
        /// {4} : is connection SSL secured?
        /// {5} : current store ID
        /// </remarks>
        public const string MANUFACTURER_PICTURE_MODEL_KEY = "GS.pres.manufacturer.picture-{0}-{1}-{2}-{3}-{4}-{5}";
        public const string MANUFACTURER_PICTURE_PATTERN_KEY = "GS.pres.manufacturer.picture";
        public const string MANUFACTURER_PICTURE_PATTERN_KEY_BY_ID = "GS.pres.manufacturer.picture-{0}-";

        /// <summary>
        /// Key for vendor picture caching
        /// </summary>
        /// <remarks>
        /// {0} : vendor id
        /// {1} : picture size
        /// {2} : value indicating whether a default picture is displayed in case if no real picture exists
        /// {3} : language ID ("alt" and "title" can depend on localized category name)
        /// {4} : is connection SSL secured?
        /// {5} : current store ID
        /// </remarks>
        public const string VENDOR_PICTURE_MODEL_KEY = "GS.pres.vendor.picture-{0}-{1}-{2}-{3}-{4}-{5}";
        public const string VENDOR_PICTURE_PATTERN_KEY = "GS.pres.vendor.picture";
        public const string VENDOR_PICTURE_PATTERN_KEY_BY_ID = "GS.pres.vendor.picture-{0}-";

        /// <summary>
        /// Key for cart picture caching
        /// </summary>
        /// <remarks>
        /// {0} : shopping cart item id
        /// P.S. we could cache by product ID. it could increase performance.
        /// but it won't work for product attributes with custom images
        /// {1} : picture size
        /// {2} : value indicating whether a default picture is displayed in case if no real picture exists
        /// {3} : language ID ("alt" and "title" can depend on localized product name)
        /// {4} : is connection SSL secured?
        /// {5} : current store ID
        /// </remarks>
        public const string CART_PICTURE_MODEL_KEY = "GS.pres.cart.picture-{0}-{1}-{2}-{3}-{4}-{5}";
        public const string CART_PICTURE_PATTERN_KEY = "GS.pres.cart.picture";

        /// <summary>
        /// Key for home page polls
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// {1} : current store ID
        /// </remarks>
        public const string HOMEPAGE_POLLS_MODEL_KEY = "GS.pres.poll.homepage-{0}-{1}";
        /// <summary>
        /// Key for polls by system name
        /// </summary>
        /// <remarks>
        /// {0} : poll system name
        /// {1} : language ID
        /// {2} : current store ID
        /// </remarks>
        public const string POLL_BY_SYSTEMNAME_MODEL_KEY = "GS.pres.poll.systemname-{0}-{1}-{2}";
        public const string POLLS_PATTERN_KEY = "GS.pres.poll";

        /// <summary>
        /// Key for blog tag list model
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// {1} : current store ID
        /// </remarks>
        public const string BLOG_TAGS_MODEL_KEY = "GS.pres.blog.tags-{0}-{1}";
        /// <summary>
        /// Key for blog archive (years, months) block model
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// {1} : current store ID
        /// </remarks>
        public const string BLOG_MONTHS_MODEL_KEY = "GS.pres.blog.months-{0}-{1}";
        public const string BLOG_PATTERN_KEY = "GS.pres.blog";
        /// <summary>
        /// Key for number of blog comments
        /// </summary>
        /// <remarks>
        /// {0} : blog post ID
        /// {1} : store ID
        /// {2} : are only approved comments?
        /// </remarks>
        public const string BLOG_COMMENTS_NUMBER_KEY = "GS.pres.blog.comments.number-{0}-{1}-{2}";
        public const string BLOG_COMMENTS_PATTERN_KEY = "GS.pres.blog.comments";

        /// <summary>
        /// Key for home page news
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// {1} : current store ID
        /// </remarks>
        public const string HOMEPAGE_NEWSMODEL_KEY = "GS.pres.news.homepage-{0}-{1}";
        public const string NEWS_PATTERN_KEY = "GS.pres.news";
        /// <summary>
        /// Key for number of news comments
        /// </summary>
        /// <remarks>
        /// {0} : news item ID
        /// {1} : store ID
        /// {2} : are only approved comments?
        /// </remarks>
        public const string NEWS_COMMENTS_NUMBER_KEY = "GS.pres.news.comments.number-{0}-{1}-{2}";
        public const string NEWS_COMMENTS_PATTERN_KEY = "GS.pres.news.comments";

        /// <summary>
        /// Key for states by country id
        /// </summary>
        /// <remarks>
        /// {0} : country ID
        /// {1} : "empty" or "select" item
        /// {2} : language ID
        /// </remarks>
        public const string STATEPROVINCES_BY_COUNTRY_MODEL_KEY = "GS.pres.stateprovinces.bycountry-{0}-{1}-{2}";
        public const string STATEPROVINCES_PATTERN_KEY = "GS.pres.stateprovinces";

        /// <summary>
        /// Key for return request reasons
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        public const string RETURNREQUESTREASONS_MODEL_KEY = "GS.pres.returnrequesreasons-{0}";
        public const string RETURNREQUESTREASONS_PATTERN_KEY = "GS.pres.returnrequesreasons";

        /// <summary>
        /// Key for return request actions
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        public const string RETURNREQUESTACTIONS_MODEL_KEY = "GS.pres.returnrequestactions-{0}";
        public const string RETURNREQUESTACTIONS_PATTERN_KEY = "GS.pres.returnrequestactions";

        /// <summary>
        /// Key for logo
        /// </summary>
        /// <remarks>
        /// {0} : current store ID
        /// {1} : current theme
        /// {2} : is connection SSL secured (included in a picture URL)
        /// </remarks>
        public const string STORE_LOGO_PATH = "GS.pres.logo-{0}-{1}-{2}";
        public const string STORE_LOGO_PATH_PATTERN_KEY = "GS.pres.logo";

        /// <summary>
        /// Key for available languages
        /// </summary>
        /// <remarks>
        /// {0} : current store ID
        /// </remarks>
        public const string AVAILABLE_LANGUAGES_MODEL_KEY = "GS.pres.languages.all-{0}";
        public const string AVAILABLE_LANGUAGES_PATTERN_KEY = "GS.pres.languages";

        /// <summary>
        /// Key for available currencies
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// {1} : current store ID
        /// </remarks>
        public const string AVAILABLE_CURRENCIES_MODEL_KEY = "GS.pres.currencies.all-{0}-{1}";
        public const string AVAILABLE_CURRENCIES_PATTERN_KEY = "GS.pres.currencies";

        /// <summary>
        /// Key for caching of a value indicating whether we have checkout attributes
        /// </summary>
        /// <remarks>
        /// {0} : current store ID
        /// {1} : true - all attributes, false - only shippable attributes
        /// </remarks>
        public const string CHECKOUTATTRIBUTES_EXIST_KEY = "GS.pres.checkoutattributes.exist-{0}-{1}";
        public const string CHECKOUTATTRIBUTES_PATTERN_KEY = "GS.pres.checkoutattributes";

        /// <summary>
        /// Key for sitemap on the sitemap page
        /// </summary>
        /// <remarks>
        /// {0} : language id
        /// {1} : roles of the current user
        /// {2} : current store ID
        /// </remarks>
        public const string SITEMAP_PAGE_MODEL_KEY = "GS.pres.sitemap.page-{0}-{1}-{2}";
        /// <summary>
        /// Key for sitemap on the sitemap SEO page
        /// </summary>
        /// <remarks>
        /// {0} : sitemap identifier
        /// {1} : language id
        /// {2} : roles of the current user
        /// {3} : current store ID
        /// </remarks>
        public const string SITEMAP_SEO_MODEL_KEY = "GS.pres.sitemap.seo-{0}-{1}-{2}-{3}";
        public const string SITEMAP_PATTERN_KEY = "GS.pres.sitemap";

        /// <summary>
        /// Key for widget info
        /// </summary>
        /// <remarks>
        /// {0} : current customer ID
        /// {1} : current store ID
        /// {2} : widget zone
        /// {3} : current theme name
        /// </remarks>
        public const string WIDGET_MODEL_KEY = "GS.pres.widget-{0}-{1}-{2}-{3}";
        public const string WIDGET_PATTERN_KEY = "GS.pres.widget";

        #endregion

        #region Methods

        //languages
        public void HandleEvent(EntityInsertedEvent<Language> eventMessage)
        {
            //clear all localizable models
            _cacheManager.RemoveByPattern(SEARCH_CATEGORIES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(MANUFACTURER_NAVIGATION_PATTERN_KEY);
            _cacheManager.RemoveByPattern(PRODUCT_SPECS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(SPECS_FILTER_PATTERN_KEY);
            _cacheManager.RemoveByPattern(TOPIC_PATTERN_KEY);
            _cacheManager.RemoveByPattern(PRODUCT_BREADCRUMB_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CATEGORY_ALL_PATTERN_KEY);
            _cacheManager.RemoveByPattern(PRODUCT_MANUFACTURERS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(STATEPROVINCES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(AVAILABLE_LANGUAGES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(AVAILABLE_CURRENCIES_PATTERN_KEY);
        }
        public void HandleEvent(EntityUpdatedEvent<Language> eventMessage)
        {
            //clear all localizable models
            _cacheManager.RemoveByPattern(SEARCH_CATEGORIES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(MANUFACTURER_NAVIGATION_PATTERN_KEY);
            _cacheManager.RemoveByPattern(PRODUCT_SPECS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(SPECS_FILTER_PATTERN_KEY);
            _cacheManager.RemoveByPattern(TOPIC_PATTERN_KEY);
            _cacheManager.RemoveByPattern(PRODUCT_BREADCRUMB_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CATEGORY_ALL_PATTERN_KEY);
            _cacheManager.RemoveByPattern(PRODUCT_MANUFACTURERS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(STATEPROVINCES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(AVAILABLE_LANGUAGES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(AVAILABLE_CURRENCIES_PATTERN_KEY);
        }
        public void HandleEvent(EntityDeletedEvent<Language> eventMessage)
        {
            //clear all localizable models
            _cacheManager.RemoveByPattern(SEARCH_CATEGORIES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(MANUFACTURER_NAVIGATION_PATTERN_KEY);
            _cacheManager.RemoveByPattern(PRODUCT_SPECS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(SPECS_FILTER_PATTERN_KEY);
            _cacheManager.RemoveByPattern(TOPIC_PATTERN_KEY);
            _cacheManager.RemoveByPattern(PRODUCT_BREADCRUMB_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CATEGORY_ALL_PATTERN_KEY);
            _cacheManager.RemoveByPattern(PRODUCT_MANUFACTURERS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(STATEPROVINCES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(AVAILABLE_LANGUAGES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(AVAILABLE_CURRENCIES_PATTERN_KEY);
        }

        //currencies
        public void HandleEvent(EntityInsertedEvent<Currency> eventMessage)
        {
            _cacheManager.RemoveByPattern(AVAILABLE_CURRENCIES_PATTERN_KEY);
        }
        public void HandleEvent(EntityUpdatedEvent<Currency> eventMessage)
        {
            _cacheManager.RemoveByPattern(AVAILABLE_CURRENCIES_PATTERN_KEY);
        }
        public void HandleEvent(EntityDeletedEvent<Currency> eventMessage)
        {
            _cacheManager.RemoveByPattern(AVAILABLE_CURRENCIES_PATTERN_KEY);
        }

        public void HandleEvent(EntityUpdatedEvent<Setting> eventMessage)
        {
            //clear models which depend on settings
            _cacheManager.RemoveByPattern(PRODUCTTAG_POPULAR_PATTERN_KEY); //depends on CatalogSettings.NumberOfProductTags
            _cacheManager.RemoveByPattern(MANUFACTURER_NAVIGATION_PATTERN_KEY); //depends on CatalogSettings.ManufacturersBlockItemsToDisplay
            _cacheManager.RemoveByPattern(VENDOR_NAVIGATION_PATTERN_KEY); //depends on VendorSettings.VendorBlockItemsToDisplay
            _cacheManager.RemoveByPattern(CATEGORY_ALL_PATTERN_KEY); //depends on CatalogSettings.ShowCategoryProductNumber and CatalogSettings.ShowCategoryProductNumberIncludingSubcategories
            _cacheManager.RemoveByPattern(CATEGORY_NUMBER_OF_PRODUCTS_PATTERN_KEY); //depends on CatalogSettings.ShowCategoryProductNumberIncludingSubcategories
            _cacheManager.RemoveByPattern(HOMEPAGE_BESTSELLERS_IDS_PATTERN_KEY); //depends on CatalogSettings.NumberOfBestsellersOnHomepage
            _cacheManager.RemoveByPattern(PRODUCTS_ALSO_PURCHASED_IDS_PATTERN_KEY); //depends on CatalogSettings.ProductsAlsoPurchasedNumber
            _cacheManager.RemoveByPattern(PRODUCTS_RELATED_IDS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(BLOG_PATTERN_KEY); //depends on BlogSettings.NumberOfTags
            _cacheManager.RemoveByPattern(NEWS_PATTERN_KEY); //depends on NewsSettings.MainPageNewsCount
            _cacheManager.RemoveByPattern(SITEMAP_PATTERN_KEY); //depends on distinct sitemap settings
            _cacheManager.RemoveByPattern(WIDGET_PATTERN_KEY); //depends on WidgetSettings and certain settings of widgets
            _cacheManager.RemoveByPattern(STORE_LOGO_PATH_PATTERN_KEY); //depends on StoreInformationSettings.LogoPictureId
        }

        //vendors
        public void HandleEvent(EntityInsertedEvent<Vendor> eventMessage)
        {
            _cacheManager.RemoveByPattern(VENDOR_NAVIGATION_PATTERN_KEY);
        }
        public void HandleEvent(EntityUpdatedEvent<Vendor> eventMessage)
        {
            _cacheManager.RemoveByPattern(VENDOR_NAVIGATION_PATTERN_KEY);
            _cacheManager.RemoveByPattern(string.Format(VENDOR_PICTURE_PATTERN_KEY_BY_ID, eventMessage.Entity.Id));
        }
        public void HandleEvent(EntityDeletedEvent<Vendor> eventMessage)
        {
            _cacheManager.RemoveByPattern(VENDOR_NAVIGATION_PATTERN_KEY);
        }

        //manufacturers
        public void HandleEvent(EntityInsertedEvent<Manufacturer> eventMessage)
        {
            _cacheManager.RemoveByPattern(MANUFACTURER_NAVIGATION_PATTERN_KEY);
            _cacheManager.RemoveByPattern(SITEMAP_PATTERN_KEY);

        }
        public void HandleEvent(EntityUpdatedEvent<Manufacturer> eventMessage)
        {
            _cacheManager.RemoveByPattern(MANUFACTURER_NAVIGATION_PATTERN_KEY);
            _cacheManager.RemoveByPattern(PRODUCT_MANUFACTURERS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(SITEMAP_PATTERN_KEY);
            _cacheManager.RemoveByPattern(string.Format(MANUFACTURER_PICTURE_PATTERN_KEY_BY_ID, eventMessage.Entity.Id));
        }
        public void HandleEvent(EntityDeletedEvent<Manufacturer> eventMessage)
        {
            _cacheManager.RemoveByPattern(MANUFACTURER_NAVIGATION_PATTERN_KEY);
            _cacheManager.RemoveByPattern(PRODUCT_MANUFACTURERS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(SITEMAP_PATTERN_KEY);
        }

        

        //categories
        public void HandleEvent(EntityInsertedEvent<Category> eventMessage)
        {
            _cacheManager.RemoveByPattern(SEARCH_CATEGORIES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CATEGORY_ALL_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CATEGORY_SUBCATEGORIES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CATEGORY_HOMEPAGE_PATTERN_KEY);
            _cacheManager.RemoveByPattern(SITEMAP_PATTERN_KEY);
        }
        public void HandleEvent(EntityUpdatedEvent<Category> eventMessage)
        {
            _cacheManager.RemoveByPattern(SEARCH_CATEGORIES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(PRODUCT_BREADCRUMB_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CATEGORY_ALL_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CATEGORY_BREADCRUMB_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CATEGORY_SUBCATEGORIES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CATEGORY_HOMEPAGE_PATTERN_KEY);
            _cacheManager.RemoveByPattern(SITEMAP_PATTERN_KEY);
            _cacheManager.RemoveByPattern(string.Format(CATEGORY_PICTURE_PATTERN_KEY_BY_ID, eventMessage.Entity.Id));
        }
        public void HandleEvent(EntityDeletedEvent<Category> eventMessage)
        {
            _cacheManager.RemoveByPattern(SEARCH_CATEGORIES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(PRODUCT_BREADCRUMB_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CATEGORY_ALL_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CATEGORY_BREADCRUMB_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CATEGORY_SUBCATEGORIES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CATEGORY_HOMEPAGE_PATTERN_KEY);
            _cacheManager.RemoveByPattern(SITEMAP_PATTERN_KEY);
        }

       
        //specification attributes
        public void HandleEvent(EntityUpdatedEvent<SpecificationAttribute> eventMessage)
        {
            _cacheManager.RemoveByPattern(PRODUCT_SPECS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(SPECS_FILTER_PATTERN_KEY);
        }
        public void HandleEvent(EntityDeletedEvent<SpecificationAttribute> eventMessage)
        {
            _cacheManager.RemoveByPattern(PRODUCT_SPECS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(SPECS_FILTER_PATTERN_KEY);
        }

        //specification attribute options
        public void HandleEvent(EntityUpdatedEvent<SpecificationAttributeOption> eventMessage)
        {
            _cacheManager.RemoveByPattern(PRODUCT_SPECS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(SPECS_FILTER_PATTERN_KEY);
        }
        public void HandleEvent(EntityDeletedEvent<SpecificationAttributeOption> eventMessage)
        {
            _cacheManager.RemoveByPattern(PRODUCT_SPECS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(SPECS_FILTER_PATTERN_KEY);
        }

        

        //Topics
        public void HandleEvent(EntityInsertedEvent<Topic> eventMessage)
        {
            _cacheManager.RemoveByPattern(TOPIC_PATTERN_KEY);
            _cacheManager.RemoveByPattern(SITEMAP_PATTERN_KEY);
        }
        public void HandleEvent(EntityUpdatedEvent<Topic> eventMessage)
        {
            _cacheManager.RemoveByPattern(TOPIC_PATTERN_KEY);
            _cacheManager.RemoveByPattern(SITEMAP_PATTERN_KEY);
        }
        public void HandleEvent(EntityDeletedEvent<Topic> eventMessage)
        {
            _cacheManager.RemoveByPattern(TOPIC_PATTERN_KEY);
            _cacheManager.RemoveByPattern(SITEMAP_PATTERN_KEY);
        }

        

        //Pictures
        public void HandleEvent(EntityInsertedEvent<Picture> eventMessage)
        {
            _cacheManager.RemoveByPattern(PRODUCTATTRIBUTE_PICTURE_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CART_PICTURE_PATTERN_KEY);
        }
        public void HandleEvent(EntityUpdatedEvent<Picture> eventMessage)
        {
            _cacheManager.RemoveByPattern(PRODUCTATTRIBUTE_PICTURE_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CART_PICTURE_PATTERN_KEY);
        }
        public void HandleEvent(EntityDeletedEvent<Picture> eventMessage)
        {
            _cacheManager.RemoveByPattern(PRODUCTATTRIBUTE_PICTURE_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CART_PICTURE_PATTERN_KEY);
        }

        

        //Polls
        public void HandleEvent(EntityInsertedEvent<Poll> eventMessage)
        {
            _cacheManager.RemoveByPattern(POLLS_PATTERN_KEY);
        }
        public void HandleEvent(EntityUpdatedEvent<Poll> eventMessage)
        {
            _cacheManager.RemoveByPattern(POLLS_PATTERN_KEY);
        }
        public void HandleEvent(EntityDeletedEvent<Poll> eventMessage)
        {
            _cacheManager.RemoveByPattern(POLLS_PATTERN_KEY);
        }

        //Blog posts
        public void HandleEvent(EntityInsertedEvent<BlogPost> eventMessage)
        {
            _cacheManager.RemoveByPattern(BLOG_PATTERN_KEY);
        }
        public void HandleEvent(EntityUpdatedEvent<BlogPost> eventMessage)
        {
            _cacheManager.RemoveByPattern(BLOG_PATTERN_KEY);
        }
        public void HandleEvent(EntityDeletedEvent<BlogPost> eventMessage)
        {
            _cacheManager.RemoveByPattern(BLOG_PATTERN_KEY);
        }

        //Blog comments
        public void HandleEvent(EntityDeletedEvent<BlogComment> eventMessage)
        {
            _cacheManager.RemoveByPattern(BLOG_COMMENTS_PATTERN_KEY);
        }

        //News items
        public void HandleEvent(EntityInsertedEvent<NewsItem> eventMessage)
        {
            _cacheManager.RemoveByPattern(NEWS_PATTERN_KEY);
        }
        public void HandleEvent(EntityUpdatedEvent<NewsItem> eventMessage)
        {
            _cacheManager.RemoveByPattern(NEWS_PATTERN_KEY);
        }
        public void HandleEvent(EntityDeletedEvent<NewsItem> eventMessage)
        {
            _cacheManager.RemoveByPattern(NEWS_PATTERN_KEY);
        }
        //News comments
        public void HandleEvent(EntityDeletedEvent<NewsComment> eventMessage)
        {
            _cacheManager.RemoveByPattern(NEWS_COMMENTS_PATTERN_KEY);
        }

        //State/province
        public void HandleEvent(EntityInsertedEvent<StateProvince> eventMessage)
        {
            _cacheManager.RemoveByPattern(STATEPROVINCES_PATTERN_KEY);
        }
        public void HandleEvent(EntityUpdatedEvent<StateProvince> eventMessage)
        {
            _cacheManager.RemoveByPattern(STATEPROVINCES_PATTERN_KEY);
        }
        public void HandleEvent(EntityDeletedEvent<StateProvince> eventMessage)
        {
            _cacheManager.RemoveByPattern(STATEPROVINCES_PATTERN_KEY);
        }

        

        //templates
        public void HandleEvent(EntityInsertedEvent<CategoryTemplate> eventMessage)
        {
            _cacheManager.RemoveByPattern(CATEGORY_TEMPLATE_PATTERN_KEY);
        }
        public void HandleEvent(EntityUpdatedEvent<CategoryTemplate> eventMessage)
        {
            _cacheManager.RemoveByPattern(CATEGORY_TEMPLATE_PATTERN_KEY);
        }
        public void HandleEvent(EntityDeletedEvent<CategoryTemplate> eventMessage)
        {
            _cacheManager.RemoveByPattern(CATEGORY_TEMPLATE_PATTERN_KEY);
        }
        public void HandleEvent(EntityInsertedEvent<ManufacturerTemplate> eventMessage)
        {
            _cacheManager.RemoveByPattern(MANUFACTURER_TEMPLATE_PATTERN_KEY);
        }
        public void HandleEvent(EntityUpdatedEvent<ManufacturerTemplate> eventMessage)
        {
            _cacheManager.RemoveByPattern(MANUFACTURER_TEMPLATE_PATTERN_KEY);
        }
        public void HandleEvent(EntityDeletedEvent<ManufacturerTemplate> eventMessage)
        {
            _cacheManager.RemoveByPattern(MANUFACTURER_TEMPLATE_PATTERN_KEY);
        }
        
        public void HandleEvent(EntityInsertedEvent<TopicTemplate> eventMessage)
        {
            _cacheManager.RemoveByPattern(TOPIC_TEMPLATE_PATTERN_KEY);
        }
        public void HandleEvent(EntityUpdatedEvent<TopicTemplate> eventMessage)
        {
            _cacheManager.RemoveByPattern(TOPIC_TEMPLATE_PATTERN_KEY);
        }
        public void HandleEvent(EntityDeletedEvent<TopicTemplate> eventMessage)
        {
            _cacheManager.RemoveByPattern(TOPIC_TEMPLATE_PATTERN_KEY);
        }

        

        /// <summary>
        /// Handle plugin updated event
        /// </summary>
        /// <param name="eventMessage">Event message</param>
        public void HandleEvent(PluginUpdatedEvent eventMessage)
        {
            if (eventMessage.Plugin?.Instance() is IWidgetPlugin)
                _cacheManager.RemoveByPattern(WIDGET_PATTERN_KEY);
        }

        #endregion
    }
}