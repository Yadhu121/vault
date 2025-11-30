using Microsoft.AspNetCore.Mvc;
using photoCloud.Models;
using System.Collections.Generic;

namespace photoCloud.Controllers
{
    public class userHomeController : Controller
    {
        DBConnect db = new DBConnect();
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
            return RedirectToAction("Login", "userLogin");

            List<fileModel> files = db.getFilesByUser(userId.Value);

            return View(files);
        }
    }
}
