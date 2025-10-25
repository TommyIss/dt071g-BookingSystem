using Microsoft.AspNetCore.Identity;

namespace BookingSystem.Models
{
    public class ApplicationUser : IdentityUser // Användarklass som ärver från IdentityUser
    {
        public ICollection<Booking> Bookings { get; set; } // En lista av bokningar kopplade till användaren
    }
}
