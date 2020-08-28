using System;
using System.Collections.Generic;

namespace ToDoList.Models
{
    public partial class ToDoList
    {
        public int Id { get; set; }
        public string ToDoName { get; set; }
        public string ToDoDescription { get; set; }
        public DateTime? DueDate { get; set; }
        public bool Complete { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
