using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GS.Web.Factories;
using GS.Web.Framework.Components;

namespace GS.Web.Components
{
    public class ForumActiveDiscussionsSmallViewComponent : GSViewComponent
    {
        private readonly IForumModelFactory _forumModelFactory;

        public ForumActiveDiscussionsSmallViewComponent(IForumModelFactory forumModelFactory)
        {
            this._forumModelFactory = forumModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _forumModelFactory.PrepareActiveDiscussionsModel();
            if (!model.ForumTopics.Any())
                return Content("");

            return View(model);
        }
    }
}
