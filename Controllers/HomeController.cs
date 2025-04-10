using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BloggApp.Models;
using BloggApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BloggApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var currentDate = DateTime.Now;
        var articles = await _context.Articles
            .Where(a => a.StartDate <= currentDate && a.EndDate >= currentDate)
            .OrderByDescending(a => a.CreatDate)
            .ToListAsync();
        
        return View(articles);
    }

    public async Task<IActionResult> Article(int id)
    {
        var article = await _context.Articles.FindAsync(id);
        
        if (article == null)
        {
            return NotFound();
        }
        
        return View(article);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}