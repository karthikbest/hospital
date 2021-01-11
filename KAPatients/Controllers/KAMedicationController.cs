    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KAPatients.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace KAPatients.Controllers
{
    public class KAMedicationController : Controller
    {
        private readonly PatientsContext _context;

        public KAMedicationController(PatientsContext context)
        {
            _context = context;
        }

        // GET: KAMedication
        public async Task<IActionResult> Index(string id, string Name )
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                if (String.IsNullOrWhiteSpace(HttpContext.Session.GetString("SessionMedicationTypeID")))
                {
                    TempData["message"] = "Please select a medication type to see its medications";
                    return RedirectToAction("index", "KAMedicationTypes");

                }
                else
                {
                    id = HttpContext.Session.GetString("SessionMedicationTypeID");

                }

            }

            else
            {
                HttpContext.Session.SetString("SessionMedicationTypeID", id);
            }

            if (!String.IsNullOrWhiteSpace(Name))
            {
                HttpContext.Session.SetString("SessionName", Name);
            }

            if (String.IsNullOrWhiteSpace(Name)) {
                //HttpContext.Session.SetString("SessionName", _context.Medication.Include(m => m.ConcentrationCodeNavigation).Include(m => m.DispensingCodeNavigation)
                //.Include(m => m.MedicationType).Where(m => m.MedicationTypeId == int.Parse(id)).Select(m=>m.Name).FirstOrDefault());
                HttpContext.Session.SetString("SessionName", (_context.MedicationType.Where(m => m.MedicationTypeId == int.Parse(id)).Select(m => m.Name).FirstOrDefault()));

            }



            var patientsContext = _context.Medication.Include(m => m.ConcentrationCodeNavigation).Include(m => m.DispensingCodeNavigation)
            .Include(m => m.MedicationType).Where(m=>m.MedicationTypeId == int.Parse(id)).OrderBy(m=>m.Concentration).OrderBy(m => m.Name);
            ViewBag.session = HttpContext.Session.GetString("SessionName");
            ViewBag.mid = id;
            return View(await patientsContext.ToListAsync());
          

            
        }

        // GET: KAMedication/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medication = await _context.Medication
                .Include(m => m.ConcentrationCodeNavigation)
                .Include(m => m.DispensingCodeNavigation)
                .Include(m => m.MedicationType)
                .FirstOrDefaultAsync(m => m.Din == id);
            if (medication == null)
            {
                return NotFound();
            }

            return View(medication);
        }

        // GET: KAMedication/Create
        public IActionResult Create(string id)
        {
            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnit.OrderBy(m=>m.ConcentrationCode), "ConcentrationCode", "ConcentrationCode");
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnit.OrderBy(m => m.DispensingCode), "DispensingCode", "DispensingCode");
            ViewBag.SelectedMedicationType = id;
            //ViewData["MedicationTypeId"] = new SelectList(_context.MedicationType, "MedicationTypeId", "Name");
            ViewBag.SessionName = HttpContext.Session.GetString("SessionName");
            ViewBag.MedicationTypeForCreate = HttpContext.Session.GetString("SessionMedicationTypeID");
            return View();
        }

        // POST: KAMedication/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Din,Name,Image,MedicationTypeId,DispensingCode,Concentration,ConcentrationCode")] Medication medication)
        {
            if (_context.Medication.Any(x => x.Name == medication.Name))               
            {
                if ((_context.Medication.Any(x => x.Concentration == medication.Concentration)))
                {
                    if (_context.Medication.Any(x => x.ConcentrationCode == medication.ConcentrationCode))
                    {
                        ModelState.AddModelError("ConcentrationCode", "You cannot add another medication with same name, concentration and concentration code as the existing medication");
                        ModelState.AddModelError("Name", "You cannot add another medication with same name, concentration and concentration code as the existing medication");
                        ModelState.AddModelError("Concentration", "You cannot add another medication with same name, concentration and concentration code as the existing medication");
                        ModelState.AddModelError(string.Empty, "You cannot add another medication with same name, concentration and concentration code as the existing medication");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(medication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnit, "ConcentrationCode", "ConcentrationCode", medication.ConcentrationCode);
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnit, "DispensingCode", "DispensingCode", medication.DispensingCode);
            ViewData["MedicationTypeId"] = new SelectList(_context.MedicationType, "MedicationTypeId", "Name", medication.MedicationTypeId);
            return View(medication);
        }

        // GET: KAMedication/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medication = await _context.Medication.FindAsync(id);
            if (medication == null)
            {
                return NotFound();
            }
            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnit.OrderBy(m=>m.ConcentrationCode), "ConcentrationCode", "ConcentrationCode", medication.ConcentrationCode);
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnit.OrderBy(m => m.DispensingCode), "DispensingCode", "DispensingCode", medication.DispensingCode);
            ViewData["MedicationTypeId"] = new SelectList(_context.MedicationType, "MedicationTypeId", "Name", medication.MedicationTypeId);
            ViewBag.MedicationTypeForEdit = HttpContext.Session.GetString("SessionMedicationTypeID");
            return View(medication);
        }

        // POST: KAMedication/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Din,Name,Image,MedicationTypeId,DispensingCode,Concentration,ConcentrationCode")] Medication medication)
        {
            if (id != medication.Din)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicationExists(medication.Din))
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
            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnit, "ConcentrationCode", "ConcentrationCode", medication.ConcentrationCode);
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnit, "DispensingCode", "DispensingCode", medication.DispensingCode);
            ViewData["MedicationTypeId"] = new SelectList(_context.MedicationType, "MedicationTypeId", "Name", medication.MedicationTypeId);
            return View(medication);
        }

        // GET: KAMedication/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medication = await _context.Medication
                .Include(m => m.ConcentrationCodeNavigation)
                .Include(m => m.DispensingCodeNavigation)
                .Include(m => m.MedicationType)
                .FirstOrDefaultAsync(m => m.Din == id);
            if (medication == null)
            {
                return NotFound();
            }

            return View(medication);
        }

        // POST: KAMedication/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {

            try
            {
                var medication = await _context.Medication.FindAsync(id);
                _context.Medication.Remove(medication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           
            catch {
                @ViewBag.ErrorItem = "Medication";

                @ViewBag.ReferencingTable = "Patient medication / Treatment Medication";

                return View("Views/Shared/Error.cshtml");
            }
           

        }

        private bool MedicationExists(string id)
        {
            return _context.Medication.Any(e => e.Din == id);
        }
    }
}
