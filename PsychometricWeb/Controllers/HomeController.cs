using Microsoft.AspNetCore.Mvc;
using PsychometricWeb.Models;
using PsychometricWeb.Repository;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
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
        [HttpPost]
        public IActionResult SendOtpEmail(string recipientEmail, string otp)
        {
            // Email details
            string senderEmail = "satapathy862000@gmail.com";
            string senderPassword = "satapathy@862000";
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 465;
            

            // Create the email message
            MailMessage mailMessage = new MailMessage(senderEmail, recipientEmail, "Your OTP", $"Your OTP is: {otp}");

            // Configure the SMTP client
            SmtpClient smtpClient = new SmtpClient(smtpServer)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
                Timeout = 20000,
        };

            // Send the email
            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email sent successfully.");
                return Json(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email. Error message: {ex.Message}");
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
