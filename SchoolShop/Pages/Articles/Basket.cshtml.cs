using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolShop.Helpers;
using SchoolShop.Models;
using Microsoft.EntityFrameworkCore;

namespace SchoolShop.Pages.Articles
{
    public class BasketModel : PageModel
    {
        private readonly SchoolShop.Data.SchoolShopContext _context;
        public Availability Availability { get; set; }
        public Article Article { get; set; }

        [BindProperty]
        public Basket Basket { get; set; }
        public int ScoreCount { get; set; } = 0;

        public BasketModel(SchoolShop.Data.SchoolShopContext context)
        {
            _context = context;
        }
        public IActionResult OnGet(int? id)
        {
            Availability = new Availability(_context, HttpContext);
            if (id == null)
            {
                return NotFound();
            }
            Article = _context.Article
                .Include(s => s.Brand)
                .Include(s => s.Category)
                .FirstOrDefault(m => m.Id == id);
            if (Article == null)
            {
                return NotFound();
            }
            CalculateNumberOfScores();
            return Page();
        }

        private void CalculateNumberOfScores()
        {
            // bereken het aantal scores voor dit ene artikel
            ScoreCount = _context.Scores.Where(s => s.ArticleId == Article.Id).Count();
        }
        public IActionResult OnPost(int? id)
        {
            Availability = new Availability(_context, HttpContext);
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (id == null)
            {
                return Page();
            }
            Basket.ArticleId = (int)id;
            Basket.UserId = int.Parse(Availability.UserId);


            _context.Basket.Add(Basket);
            _context.SaveChanges();

            return RedirectToPage("./Index");
        }

    }
}

