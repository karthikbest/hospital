using KAPatients.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KAPatients.Controllers
{
    //This controller displays the details of Concentration Unit tables & Allows CRUD operation
    public class KAConcentrationUnitController : Controller
    {
        private readonly PatientsContext _context;

        public KAConcentrationUnitController(PatientsContext context)
        {
            _context = context;
        }

        // GET: KAConcentrationUnits
        //Calls the view to list the data available in the concentration unit table
        public async Task<IActionResult> Index()
        {
            return View(await _context.ConcentrationUnit.ToListAsync());
        }

        // GET: KAConcentrationUnits/Details/5
        //Provides the details of the selected row in concentration unit table
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concentrationUnit = await _context.ConcentrationUnit
                .FirstOrDefaultAsync(m => m.ConcentrationCode == id);
            if (concentrationUnit == null)
            {
                return NotFound();
            }

            return View(concentrationUnit);
        }

        // GET: KAConcentrationUnits/Create
        //Call the view - views/create.cshtml
        public IActionResult Create()
        {
            return View();
        }

        // POST: KAConcentrationUnits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Accepts the data from the view
        public async Task<IActionResult> Create([Bind("ConcentrationCode")] ConcentrationUnit concentrationUnit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(concentrationUnit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(concentrationUnit);
        }

        // GET: KAConcentrationUnits/Edit/5
        // Shows the selected data of selected row in the edit page
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concentrationUnit = await _context.ConcentrationUnit.FindAsync(id);
            if (concentrationUnit == null)
            {
                return NotFound();
            }
            return View(concentrationUnit);
        }

        // POST: KAConcentrationUnits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Accepts the data & saves it to the database
        public async Task<IActionResult> Edit(string id, [Bind("ConcentrationCode")] ConcentrationUnit concentrationUnit)
        {
            if (id != concentrationUnit.ConcentrationCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(concentrationUnit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConcentrationUnitExists(concentrationUnit.ConcentrationCode))
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
            return View(concentrationUnit);
        }

        // GET: KAConcentrationUnits/Delete/5
        //Shows the selected row in the delete page
        public async Task<IActionResult> Delete(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var concentrationUnit = await _context.ConcentrationUnit
                .FirstOrDefaultAsync(m => m.ConcentrationCode == id);
            if (concentrationUnit == null)
            {
                return NotFound();
            }

            return View(concentrationUnit);



        }

       
        // POST: KAConcentrationUnits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // Deletes the data from the database
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var concentrationUnit = await _context.ConcentrationUnit.FindAsync(id);
                _context.ConcentrationUnit.Remove(concentrationUnit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

            catch (Exception ex)
            {
                ViewBag.ErrorItem = "Concentration Unit";
                ViewBag.ReferencingTable = "Medication table";                
                return View("~/Views/Shared/Error.cshtml");
               

            }
        
        }
        //Checks for any duplicate values
        private bool ConcentrationUnitExists(string id)
        {
            return _context.ConcentrationUnit.Any(e => e.ConcentrationCode == id);
        }



         
    }
}
