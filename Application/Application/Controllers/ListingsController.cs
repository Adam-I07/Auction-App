using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Application.Data;
using Application.Models;
using Application.Data.Services;

namespace Application.Controllers
{
    public class ListingsController : Controller
    {
        private readonly IListingService _listingService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ListingsController(IListingService listingService, IWebHostEnvironment webHostEnvironment)
        {
            _listingService = listingService;
            _webHostEnvironment = webHostEnvironment;   
        }

        // GET: Listings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _listingService.GetAll();
            return View(await applicationDbContext.ToListAsync());
        }


        // GET: Listings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Listings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ListingVM listingVM)
        {
            if (listingVM.Image != null) {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                string fileName = listingVM.Image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    listingVM.Image.CopyTo(fileStream);
                }

                var listObject = new Listing
                {
                    Title = listingVM.Title,
                    Description = listingVM.Description,
                    Price = listingVM.Price,
                    IdentityUserId = listingVM.IdentityUserId,
                    ImagePath = fileName,
                };
                await _listingService.Add(listObject);
                return RedirectToAction("Index");
            }
            return View(listingVM);
        }




        // GET: Listings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listing = await _listingService.GetById(id.Value);

            if (listing == null)
            {
                return NotFound();
            }

            return View(listing);
        }

        /*



                // GET: Listings/Edit/5
                public async Task<IActionResult> Edit(int? id)
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var listing = await _context.Listings.FindAsync(id);
                    if (listing == null)
                    {
                        return NotFound();
                    }
                    ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", listing.IdentityUserId);
                    return View(listing);
                }

                // POST: Listings/Edit/5
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Price,ImagePath,IsSold,IdentityUserId")] Listing listing)
                {
                    if (id != listing.Id)
                    {
                        return NotFound();
                    }

                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Update(listing);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!ListingExists(listing.Id))
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
                    ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", listing.IdentityUserId);
                    return View(listing);
                }

                // GET: Listings/Delete/5
                public async Task<IActionResult> Delete(int? id)
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var listing = await _context.Listings
                        .Include(l => l.User)
                        .FirstOrDefaultAsync(m => m.Id == id);
                    if (listing == null)
                    {
                        return NotFound();
                    }

                    return View(listing);
                }

                // POST: Listings/Delete/5
                [HttpPost, ActionName("Delete")]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> DeleteConfirmed(int id)
                {
                    var listing = await _context.Listings.FindAsync(id);
                    if (listing != null)
                    {
                        _context.Listings.Remove(listing);
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                private bool ListingExists(int id)
                {
                    return _context.Listings.Any(e => e.Id == id);
                }*/
    }
}
