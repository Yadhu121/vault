using Microsoft.AspNetCore.Mvc;
using photoCloud.Models;
namespace photoCloud.Controllers
{
    public class userLoginController : Controller
    {
        int userId = 0;
        DBConnect db = new DBConnect();
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult on_click(string loginInput, string pass)
        {
            string passHash = passwordHelper.hashPassword(pass);
            int Status = db.loginUser(loginInput, passHash);

            if (Status == -1)
            {
                TempData["msg"] = "Invalid username/email or password.";
                return View("Login");
            }
            userId = Status;
            HttpContext.Session.SetInt32("userId", userId);
            return RedirectToAction("Index", "userHome");
        }
    }
}
