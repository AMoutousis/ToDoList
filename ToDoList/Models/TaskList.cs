using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public partial class TaskList
    {
        public int Id { get; set; }
        public string ToDoName { get; set; }
        public string ToDoDescription { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime DueDate { get; set; }
        public bool Complete { get; set; }
        public string UserId { get; set; }
        public int Priority { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
