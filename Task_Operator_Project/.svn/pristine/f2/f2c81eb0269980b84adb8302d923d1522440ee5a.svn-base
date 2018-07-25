using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using TaskOperator.Core;

namespace TaskOperator.Web.Controllers
{
    public class TaskOperatorController : Controller
    {
        public const string REQUEST_STORAGE_UNIT_OF_WORK = "UnitOfWork";

        public const string ErrorView = "Error";

        private const string ModelStateTempDataKey = "mstat";

        protected void SaveModelState(ModelStateDictionary modelState)
        {
            TempData[ModelStateTempDataKey] = ModelState;
        }

        protected void RestoreModelState()
        {
            ModelStateDictionary restoredModelState = (ModelStateDictionary) TempData[ModelStateTempDataKey];
            if (restoredModelState != null)
            {
                ModelState.Merge(restoredModelState);
            }
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
