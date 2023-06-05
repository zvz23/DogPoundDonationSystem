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
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;

namespace DogPoundDonationSystem.Controllers
{
    
    public class DonationItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DonationItemsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize(Roles = "Donor,Admin")]
        public async Task<IActionResult> Index(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var donation = await _context.Donations.FindAsync(id);
            if (donation.Type != "Goods")
            {
                return BadRequest();
            }
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id != donation.UserId && User.IsInRole("Donor"))
            {
                return BadRequest();
            }
            var donationItems = await _context.DonationItems.Where(di => di.DonationId == donation.Id).ToListAsync();
            ViewData["DonationId"] = donation.Id;

            return View(donationItems);
        }

        [Authorize(Roles = "Donor,Admin")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.DonationItem == null)
            {
                return NotFound();
            }

            var donationItem = await _context.DonationItem
                .Include(d => d.Donation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (donationItem == null)
            {
                return NotFound();
            }

            return View(donationItem);
        }

        [Authorize(Roles = "Donor")]
        public async Task<IActionResult> Create(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
            {
                return NotFound();
            }
            if (user.Id != donation.UserId || donation.Type == "Cash")
            {
                return BadRequest();
            }

            var tempDonationItem = new DonationItem()
            {
                DonationId = id
            };
            return View(tempDonationItem);
        }

        [Authorize(Roles = "Donor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Quantity,DonationId")] DonationItem donationItem)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var donation = await _context.Donations.FindAsync(donationItem.DonationId);
                if (donation == null)
                {
                    return NotFound();
                }
                if (donation.UserId != user.Id)
                {
                    return BadRequest();
                }

                _context.Add(donationItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = donation.Id });
            }
            return View(donationItem);
        }

        [Authorize(Roles = "Donor,Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.DonationItem == null)
            {
                return NotFound();
            }

            var donationItem = await _context.DonationItem.Include(di => di.Donation).ThenInclude(d => d.User).FirstOrDefaultAsync(di => di.Id == id);
            if (donationItem == null)
            {
                return NotFound();
            }
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id != donationItem.Donation.UserId && !User.IsInRole("Admin"))
            {
                return BadRequest();
            }
            return View(donationItem);
        }

        [Authorize(Roles = "Donor,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Quantity")] DonationItem donationItem)
        {
            if (id != donationItem.Id)
            {
                return NotFound();
            }
            var currentUser = await _userManager.GetUserAsync(User);
            var oldDonationItem = await _context.DonationItems.Include(di => di.Donation).ThenInclude(d => d.User).FirstOrDefaultAsync(di => di.Id == id);
            if (oldDonationItem.Donation.UserId != currentUser.Id && !User.IsInRole("Admin"))
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    oldDonationItem.Name = donationItem.Name;
                    oldDonationItem.Quantity = donationItem.Quantity;
                    _context.Update(oldDonationItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonationItemExists(donationItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = oldDonationItem.DonationId });
            }
            ViewData["DonationId"] = new SelectList(_context.Donations, "Id", "Id", donationItem.DonationId);
            return View(donationItem);
        }

        [Authorize(Roles = "Donor,Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.DonationItem == null)
            {
                return NotFound();
            }

            var donationItem = await _context.DonationItem
                .Include(d => d.Donation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (donationItem == null)
            {
                return NotFound();
            }

            return View(donationItem);
        }

        [Authorize(Roles = "Donor,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.DonationItem == null)
            {
                return Problem("Entity set 'ApplicationDbContext.DonationItem'  is null.");
            }
            var donationItem = await _context.DonationItem.FindAsync(id);
            if (donationItem != null)
            {
                _context.DonationItem.Remove(donationItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = donationItem.DonationId });
        }

        private bool DonationItemExists(string id)
        {
            return (_context.DonationItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
