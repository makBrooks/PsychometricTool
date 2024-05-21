﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using PsychometricWeb.Filters;
using PsychometricWeb.Models;
using PsychometricWeb.Repository;
using System.Data;
using System.Security.Claims;

namespace PsychometricWeb.Controllers
{
    public class PsychoController : Controller
    {
        private IPsychometricRepo _Psycho;
     
        private readonly IConfiguration _config;
        public PsychoController(IPsychometricRepo Psycho, IConfiguration config)
        {
            _Psycho = Psycho;
            _config = config;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Start()
        {
            return View();
        }
        //[Authorize(Roles = "2")]
        public IActionResult PsychoInsert()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login Log)
        {
            try
            {
                int retVal = 0;
                var loginDetail = _Psycho.Login(Log, out retVal);
               
                if (loginDetail !=null)
                {
                    generateClaim(loginDetail);

                    if (loginDetail.UID == "1")
                    {

                        return Content(new JsonResponse { statuscode = 200, status = "success", msg = $"Welcome '{loginDetail.FULLNAME}'" }.ToJson());
                    }
                    if (loginDetail.UID == "2")
                    {
                        return Content(new JsonResponse { statuscode = 200, status = "success", msg = $"Welcome '{loginDetail.FULLNAME}'" }.ToJson());

                    }
                    else
                    {
                        return Content(new JsonResponse { statuscode = 404, status = "warning", msg = "Invalid Password" }.ToJson());
                    }

                }
                else
                {
                    return Content(new JsonResponse { statuscode = 404, status = "warning", msg = "Invalid Password" }.ToJson());

                }

            }
            catch (Exception ex)
            {
                return Content(new JsonResponse { statuscode = 500, status = "error", msg = ex.Message }.ToJson());
            }
        }
        public async Task generateClaim(UsersDto user)
        {
            var identity = new ClaimsIdentity(SysManageAuthAttribute.SysManageAuthScheme);  // Specify the authentication type
            List<Claim> claims = new List<Claim>(){
                            new Claim(ClaimTypes.Role, user.Role.ToString()),
                            new Claim("UID", user.UID),
                            new Claim("FullName", user.FULLNAME),
                            new Claim("UserName", user.UName),
                            new Claim("MobileNo", user.Phone),
                            new Claim("UserType", user.UserType),
                        };
            var isSystem = false;
            identity.AddClaims(claims);
            identity.AddClaim(new Claim(ClaimTypes.IsPersistent, isSystem.ToString()));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(SysManageAuthAttribute.SysManageAuthScheme, principal);
        }
        [Authorize]
        public IActionResult ManageUser()
        {
            int intROLEID = Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.Role));
            if (intROLEID == (int)CommonHelper.EnRoles.Admin)
            {
                return RedirectToAction("ViewPsycho", "Psycho");
            }
            else
            {
                return RedirectToAction("Start", "Psycho");
            }
        }
        [Authorize(Roles = "1,2")]
        public IActionResult ViewPsycho(Psychometriclist obj)
        {
            if (obj.Name!=null)
            {
                var res = _Psycho.GetPsychometricViewSearch(obj);
                return Json(res);
            }
            else
            {
                var res = _Psycho.GetPsychometricView();
                return View(res);
            }
            

        }
        [HttpPost]
        public IActionResult PsychoInsert(Psychometric objPsycho)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
            dataTable.Columns.Add("Phone", typeof(string));
            dataTable.Columns.Add("A", typeof(string));
            dataTable.Columns.Add("B", typeof(string));
            dataTable.Columns.Add("C", typeof(string));
            dataTable.Columns.Add("D", typeof(string));
            dataTable.Columns.Add("MOST", typeof(string));
            dataTable.Columns.Add("LEAST", typeof(string));
            for (int i = 0; i < objPsycho.Most.Length; i++)
            {
                DataRow row = dataTable.NewRow();
                row["Name"] = objPsycho.Name;
                row["Email"] = objPsycho.Email;
                row["Phone"] = objPsycho.Phone;
                row["A"] =  objPsycho.A[i].ToString();
                row["B"] = objPsycho.B[i].ToString();
                row["C"] = objPsycho.C[i].ToString();
                row["D"] = objPsycho.D[i].ToString();
                row["MOST"] = objPsycho.Most[i].ToString();
                row["LEAST"] = objPsycho.Least[i].ToString();

                dataTable.Rows.Add(row);
            }
            var ins = _Psycho.SubmitPsychometricTool(dataTable);
            return Json(ins);
        }
        [HttpPost]
        public IActionResult SearchName(Psychometriclist obj)
        {
            var res = _Psycho.GetNameSearch(obj);
            return Json(res);
        }
        
    }
}
