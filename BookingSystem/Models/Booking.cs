using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Models
{
    public class Booking // Bokningsmodell för tjänster
    {
        public int Id { get; set; }
        [Required]
        public DateTime TimeSlot { get; set; } // Datum och tid för bokningen
        [Required]
        public string ServiceType { get; set; } // Typ av tjänst som bokas
        public string? UserId { get; set; } // ID för användaren som gjort bokningen
        public ApplicationUser? User { get; set; } // Användaren som gjort bokningen
        public bool IsConfirmed { get; set; } // Bokningsstatus, om den är bekräftad eller inte
    }
}
