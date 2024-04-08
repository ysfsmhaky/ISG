using ISG.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ISG.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ISG.Controllers
{
    public class HomeController : Controller
    {
        protected readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View(_context.Abouts.ToList());
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(string Mail,string Ad,string Soyad,string Mesaj)
        {
            Contact entity = new Contact();
            entity.Ad = Ad;
            entity.Soyad = Soyad;
            entity.Mail = Mail;
            entity.Mesaj = Mesaj;

            //_context.Add(entity);
            _context.Contacts.Add(entity);
            _context.SaveChanges();

            return View();
        }
        [Authorize]
        public IActionResult Form()
        {
            return View(_context.Contacts.ToList());
        }
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var about = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (about == null)
            {
                return NotFound();
            }

            return View(about);
        }
        [Authorize]
        // POST: Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Contacts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Contacts'  is null.");
            }
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Admin()
        {
            return View();
        }
    }
}