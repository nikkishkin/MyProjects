using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using WorkWithJson.DAL;
using WorkWithJson.Models.Home;

namespace WorkWithJson.Controllers
{
    public class ExhibitionController : Controller
    {
        public const string REQUEST_STORAGE_DBCONTEXT = "DbContext";

        private const string ModelStateTempDataKey = "ModelStateKey";

        protected void SaveModelState(ModelStateDictionary modelState)
        {
            TempData[ModelStateTempDataKey] = ModelState;
        }

        protected void RestoreModelState()
        {
            ModelStateDictionary restoredModelState = (ModelStateDictionary)TempData[ModelStateTempDataKey];
            if (restoredModelState != null)
            {
                ModelState.Merge(restoredModelState);
            }
        }

        protected ArtGalleryEntities GetContext()
        {
            return
                (ArtGalleryEntities)
                    ControllerContext.HttpContext.Items[REQUEST_STORAGE_DBCONTEXT];
        }

        public static AjaxOptions GetAjaxOptions(string updateTargetId, string url, string onSuccess = null)
        {
            AjaxOptions options = new AjaxOptions
            {
                UpdateTargetId = updateTargetId,
                Url = url,
                HttpMethod = "Post",
                InsertionMode = InsertionMode.Replace,
                LoadingElementId = "loading"
            };
            if (onSuccess != null)
            {
                options.OnSuccess = onSuccess;
            }
            return options;
        }
    }
}
