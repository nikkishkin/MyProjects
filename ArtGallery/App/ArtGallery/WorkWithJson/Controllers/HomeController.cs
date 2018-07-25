using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WorkWithJson.Auth;
using WorkWithJson.Models.Home;

namespace WorkWithJson.Controllers
{
    public class HomeController : ExhibitionController
    {
        public const string ControllerName = "Home";

        public const string IndexAction = "Index";
        public const string GetExhibitsAction = "GetExhibits";

        private const int PageSize = 3;

        private const string CacheStorageJsonArray = "JsonArray";
        private const string FilePath = @"~\App_Data\pictures.json";

        //
        // GET: /Home/
        public ViewResult Index(int pageNumber = 0)
        {
            if (HttpContext.Cache[CacheStorageJsonArray] == null)
            {
                string path = Server.MapPath(FilePath);

                string jsonArrayString;
                try
                {
                    jsonArrayString = System.IO.File.ReadAllText(path);
                }
                catch (FileNotFoundException)
                {
                    return View("Error");
                }                

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                object[] jsonArray = serializer.Deserialize<object[]>(jsonArrayString);

                HttpContext.Cache.Insert(CacheStorageJsonArray, jsonArray, new CacheDependency(path));
            }

            ViewBag.IsAuthorized = UserPrincipal.CurrentPrincipal != UserPrincipal.Empty;
            return View(GetExhibitListModel(PageSize, pageNumber));       
        }

        private ExhibitModel ToExhibitModel(object jsonObj, int index)
        {
            Dictionary<string, object> dict = (Dictionary<string, object>)jsonObj;
            ExhibitModel result = new ExhibitModel
            {
                Source = (string)dict["source"],
                Description = (string)dict["description"],
                Index = index
            };
            return result;
        }

        private IEnumerable<ExhibitModel> GetExhibitModels(int pageNumber, int pageSize)
        {
            List<ExhibitModel> result = new List<ExhibitModel>();

            object[] jsonArray = (object[]) HttpContext.Cache[CacheStorageJsonArray];
            for (int i = pageNumber * pageSize; i < jsonArray.Length && i < (pageNumber + 1) * pageSize; i++)
            {
                result.Add(ToExhibitModel(jsonArray[i], i));
            }

            return result;
        } 

        private int GetPagesCount(int pageSize)
        {
            int itemsCount = ((object[])HttpContext.Cache[CacheStorageJsonArray]).Count();
            int result = itemsCount / pageSize;
            if (itemsCount % pageSize != 0)
            {
                result++;
            }
            return result;
        }

        private ExhibitListModel GetExhibitListModel(int pageSize, int pageNumber)
        {
            return new ExhibitListModel
            {
                Exhibits = GetExhibitModels(pageNumber, pageSize),
                NextPageExists = pageNumber < (GetPagesCount(pageSize) - 1),
                PageNumber = pageNumber,
                PagesCount = GetPagesCount(pageSize)
            };
        }

        private int GetCorrectPageNumber(int pageNumber, int pageSize)
        {
            if (pageNumber >= GetPagesCount(pageSize))
            {
                pageNumber = GetPagesCount(pageSize) - 1;
            }
            return pageNumber;
        }

        public ActionResult GetExhibits(int pageNumber, int pageSize = PageSize)
        {
            if (Request.IsAjaxRequest())
            {
                pageNumber = GetCorrectPageNumber(pageNumber, pageSize);

                ViewBag.IsAuthorized = UserPrincipal.CurrentPrincipal != UserPrincipal.Empty;
                return PartialView("_ExhibitList", GetExhibitListModel(pageSize, pageNumber));
            }

            pageNumber = GetCorrectPageNumber(pageNumber, PageSize);

            if (Request.HttpMethod == "POST")
            {
                return RedirectToAction("Index", new { pageNumber });
            }

            return View("Index", GetExhibitListModel(PageSize, pageNumber));
        }

        public JsonResult GetExhibitsJson(int pageNumber, int pageSize)
        {
            if (pageNumber >= GetPagesCount(pageSize))
            {
                pageNumber = GetPagesCount(pageSize) - 1;
            }

            IEnumerable<object> exhibits =
                ((object[])HttpContext.Cache["JsonArray"]).Skip(pageNumber * pageSize).Take(pageSize);

            object paging = new
            {
                pagesCount = GetPagesCount(pageSize),
                nextPageExists = pageNumber < (GetPagesCount(pageSize) - 1),
                pageNumber
            };

            return Json(new
            {
                exhibits, 
                paging, 
                startIndex = pageNumber * pageSize, 
                isAuthorized = UserPrincipal.CurrentPrincipal != UserPrincipal.Empty
            }, JsonRequestBehavior.AllowGet);
        }

        //private IEnumerable<object> GetModifiedJsonExhibits(int pageNumber, int pageSize)
        //{
        //    List<object> result = new List<object>();
        //    for (int i = pageNumber * pageSize; i < (pageNumber + 1) * pageSize; i++)
        //    {
        //        Dictionary<string, object> current 
        //        result.Add();
        //    }
        //}
    }
}
