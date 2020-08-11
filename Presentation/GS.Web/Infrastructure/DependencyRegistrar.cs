using Autofac;
using GS.Core.Configuration;
using GS.Core.Infrastructure;
using GS.Core.Infrastructure.DependencyManagement;
using GS.Services.Contracts;
using GS.Services.Customers;
using GS.Services.Messages;
using GS.Services.PaymentAdvance;
using GS.Services.Tasks;
using GS.Services.WidgetApps;
using GS.Services.Works;
using GS.Web.Areas.Admin.Factories;
using GS.Web.Framework.Factories;
using GS.Web.Infrastructure.Installation;

namespace GS.Web.Infrastructure
{
    /// <summary>
    /// Dependency registrar
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, GSConfig config)
        {
            //installation localization service
            builder.RegisterType<InstallationLocalizationService>().As<IInstallationLocalizationService>().InstancePerLifetimeScope();

            //app work service            
            builder.RegisterType<ConstructionTypeService>().As<IConstructionTypeService>().InstancePerLifetimeScope();
            builder.RegisterType<ReportService>().As<IReportService>().InstancePerLifetimeScope();
            builder.RegisterType<ConstructionCapitalService>().As<IConstructionCapitalService>().InstancePerLifetimeScope();
            builder.RegisterType<ConstructionService>().As<IConstructionService>().InstancePerLifetimeScope();
            builder.RegisterType<ContractAttributeService>().As<IContractAttributeService>().InstancePerLifetimeScope();
            builder.RegisterType<WidgetAppService>().As<IWidgetAppService>().InstancePerLifetimeScope();
            builder.RegisterType<TaskAttributeService>().As<ITaskAttributeService>().InstancePerLifetimeScope();
            builder.RegisterType<ContractFormService>().As<IContractFormService>().InstancePerLifetimeScope();
            builder.RegisterType<ContractTypeService>().As<IContractTypeService>().InstancePerLifetimeScope();
            builder.RegisterType<ContractPeriodService>().As<IContractPeriodService>().InstancePerLifetimeScope();
            builder.RegisterType<TaskGroupService>().As<ITaskGroupService>().InstancePerLifetimeScope();
            builder.RegisterType<UnitService>().As<IUnitService>().InstancePerLifetimeScope();
            builder.RegisterType<ProcuringAgencyService>().As<IProcuringAgencyService>().InstancePerLifetimeScope();
            builder.RegisterType<NotificationService>().As<INotificationService>().InstancePerLifetimeScope();
            builder.RegisterType<ContractService>().As<IContractService>().InstancePerLifetimeScope();
            builder.RegisterType<WorkFileService>().As<IWorkFileService>().InstancePerLifetimeScope();
            builder.RegisterType<WorkTaskService>().As<IWorkTaskService>().InstancePerLifetimeScope();
            builder.RegisterType<ContractLogService>().As<IContractLogService>().InstancePerLifetimeScope();
            builder.RegisterType<ContractPaymentService>().As<IContractPaymentService>().InstancePerLifetimeScope();
            builder.RegisterType<ContractRelateService>().As<IContractRelateService>().InstancePerLifetimeScope();
            builder.RegisterType<ContractMonitorService>().As<IContractMonitorService>().InstancePerLifetimeScope();
            builder.RegisterType<PaymentAdvanceService>().As<IPaymentAdvanceService>().InstancePerLifetimeScope();

            //common factories
            builder.RegisterType<AclSupportedModelFactory>().As<IAclSupportedModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<LocalizedModelFactory>().As<ILocalizedModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<StoreMappingSupportedModelFactory>().As<IStoreMappingSupportedModelFactory>().InstancePerLifetimeScope();

            //admin factories
            builder.RegisterType<BaseAdminModelFactory>().As<IBaseAdminModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ActivityLogModelFactory>().As<IActivityLogModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<AddressAttributeModelFactory>().As<IAddressAttributeModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<BlogModelFactory>().As<IBlogModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryModelFactory>().As<ICategoryModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<CommonModelFactory>().As<ICommonModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ContractAttributeModelFactory>().As<IContractAttributeModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<TaskAttributeModelFactory>().As<ITaskAttributeModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<CountryModelFactory>().As<ICountryModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<CurrencyModelFactory>().As<ICurrencyModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerAttributeModelFactory>().As<ICustomerAttributeModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerModelFactory>().As<ICustomerModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerRoleModelFactory>().As<ICustomerRoleModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<EmailAccountModelFactory>().As<IEmailAccountModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ExternalAuthenticationMethodModelFactory>().As<IExternalAuthenticationMethodModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ForumModelFactory>().As<IForumModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<HomeModelFactory>().As<IHomeModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<LanguageModelFactory>().As<ILanguageModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<LogModelFactory>().As<ILogModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ManufacturerModelFactory>().As<IManufacturerModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<MeasureModelFactory>().As<IMeasureModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<MessageTemplateModelFactory>().As<IMessageTemplateModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<NewsletterSubscriptionModelFactory>().As<INewsletterSubscriptionModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<NewsModelFactory>().As<INewsModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<PluginModelFactory>().As<IPluginModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<PollModelFactory>().As<IPollModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ReportModelFactory>().As<IReportModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<QueuedEmailModelFactory>().As<IQueuedEmailModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ReviewTypeModelFactory>().As<IReviewTypeModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ScheduleTaskModelFactory>().As<IScheduleTaskModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<SecurityModelFactory>().As<ISecurityModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<SettingModelFactory>().As<ISettingModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<SpecificationAttributeModelFactory>().As<ISpecificationAttributeModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<StoreModelFactory>().As<IStoreModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<TemplateModelFactory>().As<ITemplateModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<TopicModelFactory>().As<ITopicModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<VendorAttributeModelFactory>().As<IVendorAttributeModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<VendorModelFactory>().As<IVendorModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<WidgetModelFactory>().As<IWidgetModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<WidgetAppModelFactory>().As<IWidgetAppModelFactory>().InstancePerLifetimeScope();

            //factories
            builder.RegisterType<Factories.AddressModelFactory>().As<Factories.IAddressModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<Factories.BlogModelFactory>().As<Factories.IBlogModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<Factories.CatalogModelFactory>().As<Factories.ICatalogModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<Factories.CommonModelFactory>().As<Factories.ICommonModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<Factories.CountryModelFactory>().As<Factories.ICountryModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<Factories.CustomerModelFactory>().As<Factories.ICustomerModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<Factories.ForumModelFactory>().As<Factories.IForumModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<Factories.ExternalAuthenticationModelFactory>().As<Factories.IExternalAuthenticationModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<Factories.NewsModelFactory>().As<Factories.INewsModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<Factories.NewsletterModelFactory>().As<Factories.INewsletterModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<Factories.PollModelFactory>().As<Factories.IPollModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<Factories.PrivateMessagesModelFactory>().As<Factories.IPrivateMessagesModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<Factories.ProfileModelFactory>().As<Factories.IProfileModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<Factories.TopicModelFactory>().As<Factories.ITopicModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<Factories.VendorModelFactory>().As<Factories.IVendorModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<Factories.WidgetModelFactory>().As<Factories.IWidgetModelFactory>().InstancePerLifetimeScope();

            //app work
            builder.RegisterType<Factories.ConstructionModelFactory>().As<Factories.IConstructionModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<Factories.TaskModelFactory>().As<Factories.ITaskModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<Factories.ContractModelFactory>().As<Factories.IContractModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<Factories.UnitModelFactory>().As<Factories.IUnitModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<Factories.ProcuringAgencyModelFactory>().As<Factories.IProcuringAgencyModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<Factories.WorkModelFactory>().As<Factories.IWorkModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<Factories.PaymentAdvanceFactory>().As<Factories.IPaymentAdvanceFactory>().InstancePerLifetimeScope();
            
            //

        }

        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        public int Order
        {
            get { return 2; }
        }
    }
}
