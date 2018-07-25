using System.Collections.Generic;
using WorkWithJson.Models.Home;

namespace WorkWithJson.Models.Forum
{
    public class ForumModel
    {
        public IEnumerable<CommentModel> Comments { get; set; }

        public int PictureIndex { get; set; }
    }
}