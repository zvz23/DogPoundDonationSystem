using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DogPoundDonationSystem.Models
{
    public class Donation
    {
        public string? Id { get; set; }
        public string? Type { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "The amount must be at least 1.")]
        public double? Amount { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime? Date { get; set; }
        public List<DonationItem>? Items { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

    }

    public class DonationItem
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "The quantity must be at least 1.")]
        public int Quantity { get; set; }
        public string? DonationId { get; set; }
        public Donation? Donation { get; set; }
    }

}
