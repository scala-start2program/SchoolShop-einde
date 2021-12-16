using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolShop.Data;
using SchoolShop.Helpers;
using SchoolShop.Models;

namespace SchoolShop.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly SchoolShop.Data.SchoolShopContext _context;
        public Availability Availability { get; set; }
        public IndexModel(SchoolShop.Data.SchoolShopContext context)
        {
            _context = context;
        }

        public IList<User> User { get;set; }

        public async Task OnGetAsync()
        {
            if (!Availability.IsAdmin)
            {
                RedirectToPage("../Articles/Index");
            }
            Availability = new Availability(_context, HttpContext);
            User = await _context.User.ToListAsync();
        }
    }
}
