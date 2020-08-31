using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ToDoController : Controller
    {
        //this line is a pointer to database
        private readonly ToDoListIdentityDbContext _todolistdb;

        public ToDoController(ToDoListIdentityDbContext todolistdb)
        {
            _todolistdb = todolistdb;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult ToDoList(string sortParameter)
        {
            string id = FindUserId();

            List<TaskList> tasks = _todolistdb.ToDoList.Where(x => x.UserId == id).ToList();

            switch (sortParameter)
            {
                case "DueDate":
                    tasks.Sort((date1, date2) => DateTime.Compare(date1.DueDate, date2.DueDate));
                    break;
                case "DueDateDescending":
                    tasks.Sort((date1, date2) => DateTime.Compare(date2.DueDate, date1.DueDate));
                    break;
                case "Priority":
                    tasks.Sort((p1, p2) => (p1.Priority - p2.Priority));
                    break;
                case "PriorityDescending":
                    tasks.Sort((p1, p2) => (p2.Priority - p1.Priority));
                    break;
                //case "Complete":
                //    tasks.OrderBy(c => c.Complete);
                //    break;
                case "CompleteDescending":
                    tasks.OrderBy(c => !c.Complete);
                    break;
                default:
                    break;
            }

            if (sortParameter == "Complete")
            {
                tasks = (from item in tasks
                         orderby item ascending
                         select item).ToList();
            }

            AspNetUsers currentUser = _todolistdb.AspNetUsers.Find(id);
            UserTodoVM userList = new UserTodoVM(tasks, currentUser);


            return View(userList);
        }

        [HttpGet] //This method is simply displaying info in the view
        public IActionResult AddTask()
        {
            return View();
        }
        [HttpPost] //This method is receiving information from the view
        public IActionResult AddTask(TaskList newTask)
        {
            newTask.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (ModelState.IsValid)
            {
                _todolistdb.ToDoList.Add(newTask);
                _todolistdb.SaveChanges(); //DON'T FORGET THIS
            }
            return RedirectToAction("ToDoList");
        }

        public IActionResult DeleteTask(int id)
        {
            var foundTask = _todolistdb.ToDoList.Find(id);
            if (foundTask != null)
            {
                _todolistdb.ToDoList.Remove(foundTask);
                _todolistdb.SaveChanges();
            }

            return RedirectToAction("ToDoList");
        
        }

        public IActionResult UpdateTask(int id)
        {
            TaskList task = _todolistdb.ToDoList.Find(id);

            if (task == null)
            {
                return RedirectToAction("ToDoList");
            }
            else
            {
                return View(task);
            }

        }

        public IActionResult SaveChanges(TaskList updatedTask)
        {
            TaskList task = _todolistdb.ToDoList.Find(updatedTask.Id);


            //merging updates with existing database
            task.Priority = updatedTask.Priority;
            task.ToDoName = updatedTask.ToDoName;
            task.ToDoDescription = updatedTask.ToDoDescription;
            task.DueDate = updatedTask.DueDate;
            task.Complete = updatedTask.Complete;

            //creates a log of changes for this entry. A way to tracking our work
            _todolistdb.Entry(task).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _todolistdb.Update(task);
            _todolistdb.SaveChanges();

            return RedirectToAction("ToDoList");

        }

        //this method is used so we don't have to pass potentially unsafe user data through the URL
        public string FindUserId()
        {
            //in order to avoid sending any user information through the URL, we will need to find and isolate the user information when applicable

            //lookup user table data based on the username of the user currently logged in and assign that to a var
            var userInfo = _todolistdb.AspNetUsers.Where(s => s.UserName == User.Identity.Name).ToList();

            string id = "";

            //loop through the userInfo, isolate the ID and assign it to a string variable
            foreach (var info in userInfo)
            {
                id = info.Id;
            }

            return (id);

        }
    }
}
