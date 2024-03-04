using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCLInsurance.Models
{
    public class PolicyModel
    {
        [Key]
        public int PolicyId { get; set; }

        [Required(ErrorMessage = "Type is required.")]
        public string PolicyType { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string PolicyName { get; set; }

        // VehicleRegistration and Pincode are now optional (nullable)
        public string? VehicleRegistration { get; set; }

        public string? Pincode { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(50000, double.MaxValue, ErrorMessage = "Policy amount must be greater than or equal to 50k.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "User is required.")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public UserModel User { get; set; }
    }
}
