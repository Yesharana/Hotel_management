using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Data;
using RestaurantManagement.Models;

namespace RestaurantManagement.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult Create()
        {
            User user = new User();
            return View(user);
        }
        

        public IActionResult CreateUser(User usr)
        {
            _context.Users.Add(usr);
            _context.SaveChanges();

            HttpContext.Session.SetString("SUserName", usr.UserName);
            HttpContext.Session.SetString("SUserId", usr.UserId.ToString());
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ViewUsers()
        {
            List<User> users = new List<User>();
            users = _context.Users.ToList();
            return View(users);
        }

        public IActionResult DeleteUser(int userId)
        {
            User usr = _context.Users.Find(userId);
            if (usr != null)
            {
                _context.Users.Remove(usr);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(ViewUsers));
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult LoginUser(string email, string password)
        {
            //var user = _context.Users.;
            return RedirectToAction(nameof(Index));
        }
    }
}
