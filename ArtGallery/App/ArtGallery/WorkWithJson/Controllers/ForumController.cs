using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WorkWithJson.Auth;
using WorkWithJson.Models.Forum;

namespace WorkWithJson.Controllers
{
    public class ForumController : ExhibitionController
    {
        public const string ControllerName = "Forum";

        public const string GetCommentsAction = "GetComments";
        public const string AddCommentAction = "AddComment";
        public const string DeleteCommentAction = "DeleteComment";

        private const string CacheStorageComments = "Comments";

        private List<CommentModel> GetAllComments()
        {
            List<CommentModel> allComments = (List<CommentModel>)HttpContext.Cache[CacheStorageComments];
            if (allComments == null)
            {
                allComments = new List<CommentModel>();
                HttpContext.Cache[CacheStorageComments] = allComments;
            }
            return allComments;
        }

        [HttpPost]
        public PartialViewResult GetComments(int pictureIndex)
        {
            List<CommentModel> allComments = GetAllComments();

            // Index is assigned now, before showing comments, in order to indexed comment may be deleted in fufure
            List<CommentModel> pictureComments = new List<CommentModel>();
            for (int i = 0; i < allComments.Count; i++)
            {
                if (allComments[i].PictureIndex == pictureIndex)
                {
                    allComments[i].Index = i;
                    pictureComments.Add(allComments[i]);
                }
            }

            //IEnumerable<CommentModel> pictureComments =
            //    allComments.Where(c => c.PictureIndex == pictureIndex).OrderByDescending(c => c.CreationDate);

            ViewBag.IsAdmin = UserPrincipal.CurrentPrincipal.IsInRole("Admin");
            return PartialView("_Forum",
                new ForumModel
                {
                    Comments = pictureComments,
                    PictureIndex = pictureIndex
                });
        }

        public PartialViewResult AddComment(CommentModel comment)
        {
            if (ModelState.IsValid)
            {
                ICollection<CommentModel> comments = GetAllComments();

                comment.CreationDate = DateTime.Now;
                comment.UserId = UserPrincipal.CurrentPrincipal.UserId;
                comment.Username = UserPrincipal.CurrentPrincipal.Identity.Name;
                comments.Add(comment);
            }

            return PartialView("_AddComment");
        }

        public PartialViewResult DeleteComment(int commentIndex, int pictureIndex)
        {
            List<CommentModel> comments = GetAllComments();
            comments.RemoveAt(commentIndex);
            return GetComments(pictureIndex);
        }
    }
}
