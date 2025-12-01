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
        public IActionResult on_click(List<IFormFile> files)
        {
            if(files == null || !files.Any(f => f != null && f.Length > 0))
            {
                TempData["msg"] = "Please select atleast one file.";
                return View("files");
            }

            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
                return RedirectToAction("Login", "userLogin");

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            int successCount = 0;
            int failCount = 0;

            foreach (var file in files)
            {
                if (file == null || file.Length == 0)
                    continue;

                string extension = Path.GetExtension(file.FileName);
                string newFileName = Guid.NewGuid().ToString() + extension;
                string fullPath = Path.Combine(uploadsFolder, newFileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                string relativePath = "/uploads/" + newFileName;

                int status = db.insertMedia(
                userId.Value,
                relativePath,
                file.ContentType,
                file.Length
                );

                if (status == 1)
                    successCount++;
                else
                    failCount++;
            }

            if (successCount > 0 && failCount == 0)
            {
                TempData["msg"] = $"{successCount} file(s) uploaded successfully.";
            }
            else if (successCount > 0 && failCount > 0)
            {
                TempData["msg"] = $"{successCount} file(s) uploaded, {failCount} failed to save.";
            }
            else
            {
                TempData["msg"] = "Error uploading files.";
            }
            return View("files");  
        }
    }
}
