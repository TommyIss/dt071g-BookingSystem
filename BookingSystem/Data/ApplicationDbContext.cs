using BookingSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> // Databaskontext som ärver från IdentityDbContext med ApplicationUser
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; } // Databasset för bokningar
    }
}
