using HCLInsurance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HCLInsurance.Controllers
{
    public class AdminController : Controller
    {
        HCLInsuranceContext dbc = new HCLInsuranceContext();
        public AdminController(HCLInsuranceContext dbx)
        {
            this.dbc = dbx;
        }

        public IActionResult Dashboard()
        {
            var adminID = HttpContext.Session.GetInt32("UserId");
            var adminDetails= dbc.userModels.FirstOrDefault(x=>x.UserId == adminID);
            return View(adminDetails);
        }

        [HttpGet]
        public IActionResult ViewAllClaims()
        {
            var allClaims = dbc.claimModels.Include(x => x.Policy).Include(x => x.Policy.User);

            return View(allClaims);
        }

        //claim details
        public IActionResult ClaimDetail(int ClaimId)
        {
            var claim = dbc.claimModels.Include(x => x.Policy).Include(x => x.Policy.User).FirstOrDefault(x => x.ClaimId == ClaimId);
            return View(claim);
        }

        [HttpGet]
        public IActionResult EditClaim(int ClaimId)
        {
            var claim = dbc.claimModels
                        .FirstOrDefault(x => x.ClaimId == ClaimId);
            return View(claim);
        }

        [HttpPost]
        public IActionResult EditClaim(ClaimModel claim)
        {
            var oldClaim = dbc.claimModels.FirstOrDefault(x => x.ClaimId == claim.ClaimId);
            if (oldClaim != null)
            {
                oldClaim.Status = claim.Status;
                oldClaim.ApprovedAmount = claim.ApprovedAmount;
                oldClaim.Feedback= claim.Feedback;
                dbc.SaveChanges();
                return RedirectToAction("ViewAllClaims");
            }
            else
            {
                return RedirectToAction("ViewAllClaims");
            }
        }
        [HttpGet]
        public IActionResult ViewAllPolicies()
        {
            var allClaims = dbc.policyModels.Include(x => x.User);

            return View(allClaims);
        }
        
    }
}
