using Microsoft.AspNetCore.Mvc;
using photoCloud.Models;

namespace photoCloud.Controllers
{
    public class ProfileController : Controller
    {
        DBConnect db = new DBConnect();

        [HttpGet]
        public IActionResult Profile()
        {
            int? uid = HttpContext.Session.GetInt32("userId");

            if (uid == null)
            {
                return RedirectToAction("Login", "userLogin");
            }

            profile userProfile = db.GetProfile(uid.Value);

            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

        [HttpPost]
        public IActionResult Profile(IFormFile? profileImage, string? bio)
        {
            int? uid = HttpContext.Session.GetInt32("userId");

            if (uid == null)
            {
                return RedirectToAction("Login","userLogin");
            }

            string? imagePath = null;

            if (profileImage != null && profileImage.Length > 0)
            {
                var rootPath = Directory.GetCurrentDirectory();
                var folderPath = Path.Combine(rootPath, "wwwroot", "profile_pics");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var extension = Path.GetExtension(profileImage.FileName);
                var fileName = $"{Guid.NewGuid()}{extension}";
                var fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    profileImage.CopyTo(stream);
                }

                imagePath = $"/profile_pics/{fileName}";
            }
            DBConnect db = new DBConnect();
            db.UpdateProfile(uid.Value, imagePath, bio);

            return RedirectToAction("Profile");
        }
        }
        }
