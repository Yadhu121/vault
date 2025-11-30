using Microsoft.AspNetCore.Mvc;
using photoCloud.Models;

namespace photoCloud.Controllers
{
    public class userRegisterController : Controller
    {
        DBConnect db = new DBConnect();
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult on_click(User user)
        {
            user.passwordHash = passwordHelper.hashPassword(user.passwordHash);

            int status = db.insertUser(user);

            switch (status)
            {
                case -1:
                    TempData["msg"] = "Username already exists.";
                    break;
                case -2:
                    TempData["msg"] = "Email already registered.";
                    break;
                case 1:
                    TempData["msg"] = "Registration successfull";
                    break;
                default:
                    TempData["msg"] = "Something went wrong.";
                    break;
            }
            return View("Register");
        }
    }
}
