using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BloggApp.Data;
using BloggApp.Models;
using System.Security.Claims;

namespace BloggApp.Controllers
{
    [Authorize(Roles = "Admin,Contributor")]
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArticlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Articles
        public async Task<IActionResult> Index()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var isAdmin = User.IsInRole("Admin");

            // Admins can see all articles, contributors see only their own
            if (isAdmin)
            {
                return View(await _context.Articles.ToListAsync());
            }
            else
            {
                return View(await _context.Articles
                    .Where(a => a.ContributorUsername == userEmail)
                    .ToListAsync());
            }
        }

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FirstOrDefaultAsync(m => m.ArticleId == id);
            
            if (article == null)
            {
                return NotFound();
            }

            // Check if current user has access to this article
            if (!User.IsInRole("Admin") && article.ContributorUsername != User.FindFirstValue(ClaimTypes.Email))
            {
                return Forbid();
            }

            return View(article);
        }

        // GET: Articles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Articles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Body,StartDate,EndDate")] Article article)
        {
            article.ContributorUsername = User.FindFirstValue(ClaimTypes.Email);
            article.CreatDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(article);
        }

        // GET: Articles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FindAsync(id);
            
            if (article == null)
            {
                return NotFound();
            }

            // Check if current user can edit this article
            if (!User.IsInRole("Admin") && article.ContributorUsername != User.FindFirstValue(ClaimTypes.Email))
            {
                return Forbid();
            }

            return View(article);
        }

        // POST: Articles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArticleId,Title,Body,StartDate,EndDate,ContributorUsername,CreatDate")] Article article)
        {
            if (id != article.ArticleId)
            {
                return NotFound();
            }

            // Check if current user can edit this article
            if (!User.IsInRole("Admin") && article.ContributorUsername != User.FindFirstValue(ClaimTypes.Email))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.ArticleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(article);
        }

        // GET: Articles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FirstOrDefaultAsync(m => m.ArticleId == id);
            
            if (article == null)
            {
                return NotFound();
            }

            // Check if current user can delete this article
            if (!User.IsInRole("Admin") && article.ContributorUsername != User.FindFirstValue(ClaimTypes.Email))
            {
                return Forbid();
            }

            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            
            if (article == null)
            {
                return NotFound();
            }

            // Check if current user can delete this article
            if (!User.IsInRole("Admin") && article.ContributorUsername != User.FindFirstValue(ClaimTypes.Email))
            {
                return Forbid();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleId == id);
        }
    }
}