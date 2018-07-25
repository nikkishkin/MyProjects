using System.ComponentModel.DataAnnotations;

namespace TaskOperator.Web.Models.Tasks
{
    public class AddTaskModel
    {
        [Required]
        public string Name { get; set; }

        //[Required]
        //public string Content { get; set; }
    }
}