using GS.Core.Domain.Tasks;
using GS.Web.Areas.Admin.Models.Tasks;

namespace GS.Web.Areas.Admin.Factories
{
    public partial interface ITaskAttributeModelFactory
    {
        TaskAttributeSearchModel PrepareTaskAttributeSearchModel(TaskAttributeSearchModel searchModel);

        TaskAttributeListModel PrepareTaskAttributeListModel(TaskAttributeSearchModel searchModel);

        TaskAttributeModel PrepareTaskAttributeModel(TaskAttributeModel model,
            TaskAttribute taskAttribute, bool excludeProperties = false);

        TaskAttributeValueListModel PrepareTaskAttributeValueListModel(TaskAttributeValueSearchModel searchModel,
            TaskAttribute taskAttribute);

        TaskAttributeValueModel PrepareTaskAttributeValueModel(TaskAttributeValueModel model,
            TaskAttribute taskAttribute, TaskAttributeValue taskAttributeValue, bool excludeProperties = false);
    }
}
