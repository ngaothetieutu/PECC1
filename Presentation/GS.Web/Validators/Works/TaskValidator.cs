using FluentValidation;
using GS.Services.Localization;
using GS.Web.Factories;
using GS.Web.Framework.Validators;
using GS.Web.Models.Works;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GS.Web.Validators.Works
{
    public class TaskValidator : BaseGSValidator<TaskModel>
    {
        public TaskValidator ( ILocalizationService localizationService,
            ITaskModelFactory taskModelFactory)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("AppWork.Contracts.Task.Fields.Name.Required"));
            RuleFor(x => x.MeasurePrice).GreaterThanOrEqualTo(0).WithMessage(localizationService.GetResource("AppWork.Contracts.Task.Fields.MeasurePrice.Greater0"));
            RuleFor(x => x.MeasureMass).GreaterThanOrEqualTo(0).WithMessage(localizationService.GetResource("AppWork.Contracts.Task.Fields.MeasureMass.Greater0"));
            RuleFor(x => x.TaskGroupValue).GreaterThanOrEqualTo(0).WithMessage(localizationService.GetResource("AppWork.Contracts.Task.Fields.TaskGroupValue.Greater0"));
            RuleFor(x => x.EndDate).Must((model, enddate) =>
            {
                return taskModelFactory.CheckTaskEndDate(model);
            }).WithMessage(localizationService.GetResource("AppWork.Contracts.Task.Fields.Task.GreaterThanEndDate"));
            RuleFor(x => x.StartDate).Must((model, stardate) =>
            {
                return taskModelFactory.CheckTaskStartDate(model);
            }).WithMessage(localizationService.GetResource("AppWork.Contracts.Task.Fields.Task.GreaterThanStarDate"));
        }
    }
}
