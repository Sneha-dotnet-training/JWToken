using JWToken.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;


namespace JWToken.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index() {

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]

        public ActionResult Index (AccountLoginModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View("Index", viewModel);


                string encryptedPwd = viewModel.Password;
                var userPassword = Convert.ToString(ConfigurationManager.AppSettings["config:Password"]);
                var userName = Convert.ToString(ConfigurationManager.AppSettings["config:UserName"]);
                if (encryptedPwd.Equals(userPassword) && viewModel.Email.Equals(userName))
                {
                    var roles = new string[] { "SuperAdmin", "Admin" };
                    var jwtSecurityToken = Authentication.GenerateJwtToken(userName, roles.ToList());
                    Session["LoginedIn"] = userName;
                    var validUserName = Authentication.ValidateToken(jwtSecurityToken);
                    return RedirectToAction("Index", "Home", new { token = jwtSecurityToken });

                }

                ModelState.AddModelError("", "Invalid username or password.");


            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Invalid username or password.");
            }
            return View("Index", viewModel);
        }

    }
    }



  
    