using Microsoft.AspNetCore.Mvc;
using PsychometricWeb.Models;
using PsychometricWeb.Repository;
using System.Diagnostics;
using System.Reflection;

namespace PsychometricWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IPsychometricRepo _PsychometricRepo;

        public HomeController(IPsychometricRepo PsychometricRepo, IWebHostEnvironment hostingEnvironment)
        {
            _PsychometricRepo = PsychometricRepo;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult UserRegistration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserRegistration(Registration reg)
        {
            try
            {
                if (reg.ADHARUPLOAD == null || reg.ADHARUPLOAD.Length == 0)
                {
                    ViewBag.Message = "Please select a Image.";
                    return View();
                }

                var uploadsFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");

                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

                var filePath = Path.Combine(uploadsFolderPath, reg.ADHARUPLOAD.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await reg.ADHARUPLOAD.CopyToAsync(stream);
                }
                filePath = "/uploads/" + reg.ADHARUPLOAD.FileName;
                reg.UPLOADPATH = filePath;
                int retval = await _PsychometricRepo.InsertRegistration(reg);
                return Json(retval);
            }

            catch (Exception ex)
            {
                return Json(ex);
            }

        }






        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
