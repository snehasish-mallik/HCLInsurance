using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HCLInsurance.Models
{
    public class ClaimModel
    {
        [Key]
        public int ClaimId { get; set; }

        [Required]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Reason is required.")]
        public string Reason { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; } // You might want to use an enum for Status

        [Required(ErrorMessage = "Claim Percentage is required.")]
        public decimal? ClaimPercentage { get; set; }

        [Required(ErrorMessage = "Claim Amount is required.")]
        public decimal? ClaimAmount { get; set; }


        [MaxLength(500)] // Adjust the length as needed
        public string? Feedback { get; set; }

        [Range(10000, double.MaxValue, ErrorMessage = "Approved amount must be greater than or equal to 10k.")]
        public decimal? ApprovedAmount { get; set; }


        [ForeignKey("Policy")]
        public int PolicyId { get; set; }

        public PolicyModel Policy { get; set; }
    }
}
