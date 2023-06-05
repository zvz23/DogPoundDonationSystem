using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DogPoundDonationSystem.Data;
using DogPoundDonationSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace DogPoundDonationSystem.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Donor,Admin")]
        public async Task<IActionResult> Donations(string? sortbystatus)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            IQueryable<Donation> donationsQuery = null;
            if (User.IsInRole("Donor"))
            {
                donationsQuery = _context.Donations.Where(d => d.UserId == user.Id).Include(d => d.User);

            }
            else if (User.IsInRole("Admin"))
            {
                donationsQuery = _context.Donations.AsQueryable();
                
            }
            if (sortbystatus != null)
            {
                donationsQuery = donationsQuery.Where(d => d.Status == sortbystatus);
            }
            List<Donation> donations = await donationsQuery.ToListAsync();

            return View(donations);
        }

        [Authorize(Roles = "Donor,Admin")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Donations == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }


        [Authorize(Roles = "Donor")]
        public async Task<IActionResult> Create()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            ViewData["UserId"] = user.Id;
            ViewData["DonationTypes"] = new SelectList(new List<string> { "Cash", "Goods" });
            return View();
        }

        [Authorize(Roles = "Donor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Type,Amount,Description,UserId")] Donation donation)
        {
            if (ModelState.IsValid)
            {
                donation.Status = "Pending";
                if (donation.Type == "Goods")
                {
                    donation.Amount = null;
                }
                donation.Date = DateTime.Now;
                await _context.Donations.AddAsync(donation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Donations));
            }
            return View(donation);
        }

        [Authorize(Roles = "Donor,Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Donations == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
            {
                return NotFound();
            }

            ApplicationUser user = await _userManager.GetUserAsync(User);
            ViewData["UserId"] = user.Id;
            ViewData["DonationTypes"] = new SelectList(new List<string> { "Cash", "Goods" });
            return View(donation);
        }

        [Authorize(Roles = "Donor,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Type,Amount,Description")] Donation donation)
        {
            if (id != donation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Donation oldDonation = await _context.Donations.FindAsync(id);
                    oldDonation.Type = donation.Type;
                    oldDonation.Amount = donation.Amount;
                    if (oldDonation.Type == "Goods")
                    {
                        oldDonation.Amount = null;
                    }
                    oldDonation.Description = donation.Description;
                    _context.Donations.Update(oldDonation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonationExists(donation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Donations));
            }
            ViewData["DonationTypes"] = new SelectList(new List<string> { "Cash", "Goods" }) ;
            return View(donation);
        }

        [Authorize(Roles = "Donor,Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Donations == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        [Authorize(Roles = "Donor,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Donations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Donations'  is null.");
            }
            var donation = await _context.Donations.FindAsync(id);
            if (donation != null)
            {
                _context.Donations.Remove(donation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Donations));
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeStatus(string id, string status)
        {
            if (id == null || _context.Donations == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
            {
                return NotFound();
            }

            donation.Status = status;
            _context.Donations.Update(donation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Donations));
        }
        private bool DonationExists(string id)
        {
          return (_context.Donations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
