using Microsoft.EntityFrameworkCore;

namespace HCLInsurance.Models
{
    public class HCLInsuranceContext : DbContext
    {
        public HCLInsuranceContext() 
        {
        
        }
        public HCLInsuranceContext(DbContextOptions options): base(options) 
        { 

        }
        public DbSet<UserModel> userModels { get; set; }
        public DbSet<PolicyModel> policyModels { get; set; }
        public DbSet<ClaimModel> claimModels { get; set; }
    }
}
