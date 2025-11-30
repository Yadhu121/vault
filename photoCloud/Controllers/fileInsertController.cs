using Microsoft.AspNetCore.Mvc;
using photoCloud.Models;
using System.IO;

namespace photoCloud.Controllers
{
    public class fileInsertController : Controller
    {
        DBConnect db = new DBConnect();
        public IActionResult files()
        {
            return View();
        }

        [HttpPost]
        public IActionResult on_click(IFormFile file)
        {
            if(file == null || file.Length == 0)
            {
                TempData["msg"] = "Please select a file.";
                return View();
            }
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string extension = Path.GetExtension(file.FileName);
            string newFileName = Guid.NewGuid().ToString() + extension;
            string fullPath = Path.Combine(uploadsFolder, newFileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            string relativePath = "/uploads/" + newFileName;

            int? userId = HttpContext.Session.GetInt32("userId");

            if(userId == null)
            return RedirectToAction("Login", "userLogin");

            int status = db.insertMedia(
                userId.Value,
                relativePath,
                file.ContentType,
                file.Length
            );

            if (status == 1)
            {
                TempData["msg"] = "File uploaded successfully.";
            }
            else
            {
                TempData["msg"] = "Error saving file info.";
            }

            return View("files");
        }
    }
}
