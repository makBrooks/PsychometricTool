using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PsychometricWeb.Models;
using PsychometricWeb.Repository;
using System.Diagnostics;
using System.Reflection;

//using System.Net.Mail;
using System.Net;
using MimeKit;
using MailKit.Net.Smtp;


namespace PsychometricWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IPsychometricRepo _PsychometricRepo;
        private readonly MailSettings _mailSettings;
        public HomeController(IPsychometricRepo PsychometricRepo, IWebHostEnvironment hostingEnvironment, IOptions<MailSettings> mailSettingsOptions)
        {
            _PsychometricRepo = PsychometricRepo;
            _hostingEnvironment = hostingEnvironment;
            _mailSettings = mailSettingsOptions.Value;
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
        public IActionResult SendMail(MailData mailData)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                    emailMessage.To.Add(emailTo);

                    //emailMessage.Cc.Add(new MailboxAddress("Cc Receiver", "cc@example.com"));
                    //emailMessage.Bcc.Add(new MailboxAddress("Bcc Receiver", "bcc@example.com"));

                    emailMessage.Subject = mailData.EmailSubject;

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.HtmlBody = mailData.EmailBody;

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();
                    //this is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
                    using (SmtpClient mailClient = new SmtpClient())
                    {
                        mailClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                        mailClient.Authenticate("mailtomakarun@gmail.com", "uqtl xjix vbmg fnsj");
                        mailClient.Send(emailMessage);
                        mailClient.Disconnect(true);
                    }
                }

                return Json(1);
            }
            catch (Exception ex)
            {
                // Exception Details
                return Json(0);
            }
        }
        //public IActionResult SendMail(MailData mailData)
        //{
        //    // Email details
        //    string senderEmail = "mailtomakarun@gmail.com";
        //    string senderPassword = "uqtl xjix vbmg fnsj";
        //    string smtpServer = "smtp.gmail.com";
        //    int smtpPort = 587;


        //    // Create the email message
        //    MailMessage mailMessage = new MailMessage(senderEmail, mailData.EmailToId, mailData.EmailSubject, mailData.EmailBody);

        //    // Configure the SMTP client
        //    SmtpClient smtpClient = new SmtpClient(smtpServer)
        //    {
        //        Port = smtpPort,
        //        Credentials = new NetworkCredential(senderEmail, senderPassword),
        //        EnableSsl = true,
        //        Timeout = 20000,
        //    };

        //    // Send the email
        //    try
        //    {
        //        smtpClient.Send(mailMessage);
        //        Console.WriteLine("Email sent successfully.");
        //        return Json(1);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Failed to send email. Error message: {ex.Message}");
        //        return Json(ex);
        //    }

        //}



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
