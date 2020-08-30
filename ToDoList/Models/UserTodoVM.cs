using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class UserTodoVM
    {
        public List<TaskList> Tasks { get; set; }
        public AspNetUsers User { get; set; }

        public UserTodoVM(List<TaskList> tasks, AspNetUsers user)
        {
            this.Tasks = tasks;
            this.User = user;
        }
        public UserTodoVM()
        {

        }
    }
}
