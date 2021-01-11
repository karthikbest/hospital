using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KAPatients.Models;
using Microsoft.AspNetCore.Authorization;

namespace KAPatients.Controllers
{
    //This controller allows CRUD operation in countries table.
    [Authorize(Roles = "members")]
    public class KACountryController : Controller
    {
        private readonly PatientsContext _context;

        public KACountryController(PatientsContext context)
        {
            _context = context;
        }

        [AllowAnonymous] 
        // GET: KACountry
        //This method passes the list of  available rows to view
        public async Task<IActionResult> Index()
        {
            return View(await _context.Country.ToListAsync());
        }

        // GET: KACountry/Details/5
        //This method passes the detail of selected row to the view, so that it can show the details
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country
                .FirstOrDefaultAsync(m => m.CountryCode == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // GET: KACountry/Create
        //This method returns the create view page
        public IActionResult Create()
        {
            return View();
        }

        // POST: KACountry/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Saves the data into database through model & returns the view
        public async Task<IActionResult> Create([Bind("CountryCode,Name,PostalPattern,PhonePattern,FederalSalesTax")] Country country)
        {
            if (ModelState.IsValid)
            {
                _context.Add(country);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        // GET: KACountry/Edit/5
        //Returns the edit view of the page
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: KACountry/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Posts the data in to the database through model & returns the view
        public async Task<IActionResult> Edit(string id, [Bind("CountryCode,Name,PostalPattern,PhonePattern,FederalSalesTax")] Country country)
        {
            if (id != country.CountryCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.CountryCode))
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
            return View(country);
        }

        // GET: KACountry/Delete/5
        //Returns the delete view with selected data
        public async Task<IActionResult> Delete(string id)
        {
            
                if (id == null)
                {
                    return NotFound();
                }

                var country = await _context.Country
                    .FirstOrDefaultAsync(m => m.CountryCode == id);
                if (country == null)
                {
                    return NotFound();
                }

                return View(country);
            

          
            
        }

        // POST: KACountry/Delete/5
        //Deletes the selected data through the model
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var country = await _context.Country.FindAsync(id);
                _context.Country.Remove(country);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            catch (Exception ex)
            {
                @ViewBag.ErrorItem = "Country";

                @ViewBag.ReferencingTable = "Province table";

                return View("~/Views/Shared/Error.cshtml");


            }
        }
        //Checks for any duplicate values and returns true or false
        private bool CountryExists(string id)
        {
            return _context.Country.Any(e => e.CountryCode == id);
        }
    }
}
