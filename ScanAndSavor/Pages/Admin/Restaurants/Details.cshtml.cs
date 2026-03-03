using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScanAndSavor.Data;
using ScanAndSavor.Models;

namespace ScanAndSavor.Pages.Admin.Restaurants
{
    public class DetailsModel : PageModel
    {
        private readonly ScanAndSavor.Data.ApplicationDbContext _context;

        public DetailsModel(ScanAndSavor.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Restaurant Restaurant { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(m => m.Id == id);

            if (restaurant is not null)
            {
                Restaurant = restaurant;

                return Page();
            }

            return NotFound();
        }
    }
}
