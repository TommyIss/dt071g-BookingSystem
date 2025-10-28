using BookingSystem.Data;
using BookingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookingSystem.Pages.Bookings
{
    [Authorize]
    public class CreateBookingModel : PageModel
    {
        private readonly ApplicationDbContext _context; // Databaskontext
        private readonly UserManager<ApplicationUser> _userManager; // Användarhanterare

        // Konstruktor för att initialisera databaskontext och användarhanterare
        public CreateBookingModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context; 
            _userManager = userManager;
        }

        [BindProperty]
        public Booking Booking { get; set; } // Egenskap för att binda bokningsdata

        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User); // Hämta aktuella användaren
            if(user == null) // Kontrollera om användaren finns
            {
                ModelState.AddModelError(string.Empty, "Användare kunde inte hittas.");
                return Page(); // Returnera sidan med felmeddelande
            }

            if(Booking.TimeSlot < DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "Bokningstid måste vara i framtiden.");
                return Page();
            }
            if(string.IsNullOrWhiteSpace(Booking.ServiceType))
            {
                ModelState.AddModelError(string.Empty, "Tjänstetyp måste anges.");
                return Page();
            }

            Booking.UserId = user.Id; // Sätt användar-ID för bokningen

            Booking.IsConfirmed = false; // Bokningen är inte bekräftad vid skapande

            _context.Bookings.Add(Booking); // Lägg till bokningen i databaskontexten
            await _context.SaveChangesAsync(); // Spara ändringarna i databasen

            return RedirectToPage("MyBookings"); // Omdirigera till sidan med användarens bokningar)
        }
    }
}
