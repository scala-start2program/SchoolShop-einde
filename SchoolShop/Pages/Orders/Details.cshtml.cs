using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolShop.Data;
using SchoolShop.Models;
using SchoolShop.Helpers;

namespace SchoolShop.Pages.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly SchoolShop.Data.SchoolShopContext _context;
        public Availability Availability { get; set; }
        public DetailsModel(SchoolShop.Data.SchoolShopContext context)
        {
            _context = context;
        }

        public Order Order { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

        public IActionResult OnGet(int? id)
        {
            Availability = new Availability(_context, HttpContext);
            if (!Availability.IsAdmin)
            {
                return RedirectToPage("../Articles/Index");
            }
            if (id == null)
            {
                return NotFound();
            }

            Order = _context.Order
                .Include(o => o.User).FirstOrDefault(m => m.Id == id);
            if (Order == null)
            {
                return NotFound();
            }
            IQueryable<OrderDetail> query = _context.OrderDetail
                .Include(b => b.Article);
            query = query.Where(b => b.OrderId.Equals(Order.Id));
            OrderDetails = query.ToList();




            return Page();
        }
    }
}
