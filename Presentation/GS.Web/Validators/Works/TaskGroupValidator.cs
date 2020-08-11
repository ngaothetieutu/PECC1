using GS.Web.Models.Works;
using FluentValidation;
using GS.Services.Localization;
using GS.Web.Framework.Validators;

namespace GS.Web.Validators.Works
{
    public class TaskGroupValidator: BaseGSValidator<TaskGroupModel>
    {
        public TaskGroupValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("AppWork.Works.TaskGroup.Fields.Name.Required"));
        }
    }
}
