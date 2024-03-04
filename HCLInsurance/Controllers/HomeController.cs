using HCLInsurance.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HCLInsurance.Controllers
{
    public class HomeController : Controller
    {
        HCLInsuranceContext dbc = new HCLInsuranceContext();
        public HomeController(HCLInsuranceContext dbx)
        {
            this.dbc = dbx;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();

        }
        [HttpPost]
        public IActionResult LogIn(UserModel user)
        {
            var userID = HttpContext.Session.GetInt32("UserID");
            if(userID == null) 
            {
                var loginUser = dbc.userModels.FirstOrDefault(x => x.Email == user.Email);
                if (loginUser != null)
                {
                    // Log in the user and store user information in the session
                    if (loginUser.Password == user.Password && loginUser.Role == user.Role)
                    {
                        if (loginUser.Role == "Admin")
                        {
                            HttpContext.Session.SetInt32("UserId", loginUser.UserId);
                            return RedirectToAction("Dashboard", "Admin");
                        }
                        else
                        {
                            HttpContext.Session.SetInt32("UserId", loginUser.UserId);
                            return RedirectToAction("Dashboard", "User");
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Invalid Credentials for Password or Role";
                        return View();
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "User Doesn't Exist. Register First";
                    return View(user);
                }

            }
            else
            {
                var loggedU = dbc.userModels.FirstOrDefault(x => x.UserId == userID);
                if (loggedU.Role == "Admin")
                {
                    HttpContext.Session.SetInt32("UserId", loggedU.UserId);
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    HttpContext.Session.SetInt32("UserId", loggedU.UserId);
                    return RedirectToAction("Dashboard", "User");
                }
            }
            

        }

        public IActionResult Dashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var loggedU= dbc.userModels.FirstOrDefault(y => y.UserId == userId);
            if(loggedU != null)
            {
                if (loggedU.Role == "Admin")
                {
                    HttpContext.Session.SetInt32("UserId", loggedU.UserId);
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    HttpContext.Session.SetInt32("UserId", loggedU.UserId);
                    return RedirectToAction("Dashboard", "User");
                }
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
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

        public IActionResult LogOut()
        {
            // Sign out the user
            HttpContext.Session.Clear();

            // Redirect to the login page or another desired page after logout
            return RedirectToAction("Index");

        }
    }
}