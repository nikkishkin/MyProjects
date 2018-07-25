using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TaskOperator.Web.Models.Tasks
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        [Range(0, 100)]
        public int Percentage { get; set; }
        public byte State { get; set; }

        public string StateString
        {
            get { return State.ToString(); }
            set { State = byte.Parse(value); }
        }

        public IEnumerable<SelectListItem> StateOptions { get; set; }

        public UserModel Worker { get; set; }
        public string Date { get; set; }
        public bool IsAssigned { get; set; }
        public IEnumerable<SelectListItem> WorkerOptions { get; set; }
    }
}