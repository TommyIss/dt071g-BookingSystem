using BookingSystem.Data;
using BookingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Pages.Bookings
{
    [Authorize]
    public class MyBookingsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MyBookingsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public List<Booking> MyBookings { get; set; } // Egenskap för att binda användarens bokningar i listform

        // Hämta användarens bokningar från databasen
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                ModelState.AddModelError(string.Empty, "Du måste logga in för att se dina bokningar");
                MyBookings = new List<Booking>(); // Tom lista om användaren inte är inloggad
                return Page();
            }

            MyBookings = await _context.Bookings.Where(Booking => Booking.UserId == user.Id).ToListAsync(); // Hämta bokningar som deras användar-ID matchar den inloggade användaren
            return Page();
        }

        // Metod för att bekräfta en bokning
        public async Task<IActionResult> OnPostConfirmAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);// Hitta bokningen med det angivna ID:et

            if(booking != null)
            {
                booking.IsConfirmed = true; // Bekräfta bokningen
                await _context.SaveChangesAsync(); // Spara ändringarna i databasen
            }

            return RedirectToPage();
        }

        // Metod för att avboka en bokning
        public async Task<IActionResult> OnPostCancelAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking != null)
            {
                _context.Bookings.Remove(booking); // Ta bort bokningen från databaskontexten
                await _context.SaveChangesAsync(); // Spara ändringarna i databasen
            }

            
            return RedirectToPage();
        }
    }
}
