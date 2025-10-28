using BookingSystem.Data;
using BookingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookingSystem.Pages.Bookings
{
    [Authorize]
    public class EditBookingModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        
        public EditBookingModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [BindProperty]
        public Booking Booking { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            
            Booking = await _context.Bookings.FindAsync(id); // Hämta bokning med angivna id:et

            if (Booking == null)
            {
                return NotFound();
            }
            return Page();
        }

        // Metod för att uppdatera en bokning
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User); // Hämta aktulla användaren
            
            if(user == null)
            {
                ModelState.AddModelError(string.Empty, "Användaren kunde inte hittas.");
                return Page();
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

            if (!ModelState.IsValid) // Kontrollera om modellens status är giltig
            {
                Console.WriteLine("ModelState är ogiltig");
                return Page();
            }
            var bookingInDb = await _context.Bookings.FindAsync(Booking.Id); // Hämta bokningen från databasen

            if (bookingInDb == null)
            {
                ModelState.AddModelError(string.Empty, "Bokningen kunde inte hittas.");
                return Page();
            }

            if (string.IsNullOrEmpty(bookingInDb.UserId))
            {
                bookingInDb.UserId = user?.Id;
            }

            bookingInDb.TimeSlot = Booking.TimeSlot; // Uppdatera datum och tid
            bookingInDb.ServiceType = Booking.ServiceType; // Uppdatera tjänstetyp
            
            try
            {
                await _context.SaveChangesAsync(); // Spara ändringarna i databasen
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fel vid sparande: " + ex.Message);
                ModelState.AddModelError(string.Empty, "Ett fel uppstår vid sparande: " + ex.Message);
                return Page();
            }
            

            return RedirectToPage("MyBookings"); // Omdirigera till mina bokningar-sidan
        }
        
    }
}
