using System;
using System.ComponentModel.DataAnnotations;

namespace WorkWithJson.Models.Forum
{
    public class CommentModel
    {
        [Required]
        public string Content { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public DateTime CreationDate { get; set; }

        public int PictureIndex { get; set; }

        public int Index { get; set; }
    }
}