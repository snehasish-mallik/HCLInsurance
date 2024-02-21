using HCLInsurance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace HCLInsurance.Controllers
{
    public class UserController : Controller
    {
        HCLInsuranceContext dbc= new HCLInsuranceContext();
        public UserController(HCLInsuranceContext dbc)
        {
            this.dbc = dbc;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(UserModel user)
        {
            if(dbc.userModels.Any(x=>x.Email== user.Email))
            {
                ViewBag.ErrorMessage = "User already exists";
                return RedirectToAction("LogIn", "Admin");
            }
            else
            {
                if(ModelState.IsValid)
                {
                    if (user.Role == "Admin")
                    {
                        ViewBag.ErrorMessage = "Can't register as admin";
                        return View(user);
                    }
                    else
                    {
                        dbc.userModels.Add(user);
                        dbc.SaveChanges();
                        return RedirectToAction("LogIn", "Home");
                    }
                }
                else
                {

                    return View(user);
                }
            }
        }
        //login

        //
        public IActionResult Dashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = dbc.userModels.FirstOrDefault(x => x.UserId == userId);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                ViewBag.ErrorMessage = "Log in First";
                return RedirectToAction("LogIn", "Home");
            }
        }

        //Create Policy
        [HttpGet]
        public IActionResult CreatePolicy()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserId = userId;
            return View();

        }
        [HttpPost]
        public IActionResult CreatePolicy(PolicyModel policy)
        {
            var userID = HttpContext.Session.GetInt32("UserId");
            if (userID != null)
            {
                policy.UserId = (int)userID;
                dbc.policyModels.Add(policy);
                dbc.SaveChanges();
                return RedirectToAction("PolicyList");
            }
            else
            {
                ViewBag.ErrorMessage = "Log in First";
                return RedirectToAction("LogIn");
            }
        }

        //show all policies
        public IActionResult PolicyList()
        {
            var userId= HttpContext.Session.GetInt32("UserId");
            var policyList= dbc.policyModels.Where(x=>x.UserId == userId).ToList();
            return View(policyList);
        }

      
        public IActionResult ViewClaimHistory()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var userD= dbc.userModels.FirstOrDefault(x=>x.UserId==userId);
            var allClaims= dbc.claimModels.Include(x=>x.Policy).Where(x=>x.Policy.UserId==userD.UserId).ToList();
            return View(allClaims);
        }

        [HttpGet]
        public IActionResult CreateClaim(int PolicyId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {
                var existingClaim = dbc.claimModels.FirstOrDefault(x => x.PolicyId == PolicyId);

                if (existingClaim != null)
                {
                    ViewBag.ErrorMessage = "Claim for this Policy already Exists";
                    return View();
                }

                
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        [HttpPost]
        public IActionResult CreateClaim(ClaimModel claim)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {
                // Check if the policy with the given PolicyId exists
                var policy = dbc.policyModels.FirstOrDefault(p => p.PolicyId == claim.PolicyId);

                if (policy == null)
                {
                    ViewBag.ErrorMessage = "Policy not found for the given PolicyId.";
                    return View(claim);
                }

                var existingClaim = dbc.claimModels.FirstOrDefault(x => x.PolicyId == claim.PolicyId);

                if (existingClaim != null)
                {
                    ViewBag.ErrorMessage = "Claim for this Policy already Exists";
                    return View(claim);
                }

                claim.ClaimAmount = policy.Amount * ((claim.ClaimPercentage)/100);
                claim.Status = "Under Review";
                dbc.claimModels.Add(claim);
                dbc.SaveChanges();

                return RedirectToAction("ViewClaimHistory");
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }



        public IActionResult ClaimDetail(int ClaimId)
        {
            var claim = dbc.claimModels.Include(x => x.Policy).Include(x => x.Policy.User).FirstOrDefault(x => x.ClaimId == ClaimId);
            return View(claim);
        }

        [HttpGet]
        public IActionResult DeleteClaim(int ClaimId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {
                var claim = dbc.claimModels
                    .Include(c => c.Policy)
                    .FirstOrDefault(c => c.ClaimId == ClaimId && c.Policy.UserId == userId);

                if (claim != null)
                {
                    return View(claim);
                }
                else
                {
                    ViewBag.ErrorMessage = "Claim not found or you don't have permission to delete it.";
                    return View();
                }
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        [HttpPost]
        public IActionResult DeleteClaim(ClaimModel claim)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {
                var existingClaim = dbc.claimModels.FirstOrDefault(c => c.ClaimId == claim.ClaimId);

                if (existingClaim != null)
                {
                    dbc.claimModels.Remove(existingClaim);
                    dbc.SaveChanges(); // Save changes to the database
                    return RedirectToAction("ViewClaimHistory");
                }
                else
                {
                    ViewBag.ErrorMessage = "Claim not found or you don't have permission to delete it.";
                    return View(claim);
                }
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }



    }
}
