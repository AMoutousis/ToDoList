using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        //this line is a pointer to database
        private readonly ToDoListIdentityDbContext _todolistdb;

        public HomeController(ToDoListIdentityDbContext todolistdb)
        {
            _todolistdb = todolistdb;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            //getting the id of the user currently logged in
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //get a list of tasks assigned to the person
            var todoList = _todolistdb.ToDoList.Where(x => x.UserId == id).ToList();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
