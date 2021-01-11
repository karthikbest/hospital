using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KAPatients.Models;
using Microsoft.AspNetCore.Http;

namespace KAPatients.Controllers
{
    public class KAPatientTreatmentController : Controller
    {
        private readonly PatientsContext _context;

        public KAPatientTreatmentController(PatientsContext context)
        {
            _context = context;
        }

        // GET: KAPatientTreatment
        public async Task<IActionResult> Index(int? id, int? did, string pName, string dName)

        {
            if (id == null)//URL EMPTY
            {
                if (String.IsNullOrWhiteSpace(HttpContext.Session.GetString("sessionId")))//SESSION VRB IS EMPTY
                {
                    TempData["noDiagnosisID"] = "Please select a diagnosis first";
                    return RedirectToAction("index", "KAPatientDiagnosis");
                }

                else //SESSION VRB NOT EMPTY
                {

                    id = Convert.ToInt32(HttpContext.Session.GetString("sessionId"));
                }

            }

            else //URL NOT EMPTY
            {


                HttpContext.Session.SetString("sessionId", id.ToString());

            }
            var patientsContext = _context.PatientTreatment.Include(p => p.PatientDiagnosis).Include(p => p.Treatment).Where(p => p.PatientDiagnosisId == id).OrderByDescending(p => p.DatePrescribed);

            if (did != null)
            {


                HttpContext.Session.SetString("sessionDid", did.ToString());

            }

            if (!String.IsNullOrWhiteSpace(pName))
            {
                HttpContext.Session.SetString("sessionPName", pName);

            }


            if (!String.IsNullOrWhiteSpace(dName))
            {
                HttpContext.Session.SetString("sessionDName", dName);

            }

            //var patientsContext = _context.PatientTreatment.Include(p => p.PatientDiagnosis).Include(p => p.Treatment);
            return View(await patientsContext.ToListAsync());
        }

        // GET: KAPatientTreatment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientTreatment = await _context.PatientTreatment
                .Include(p => p.PatientDiagnosis)
                .Include(p => p.Treatment)
                .FirstOrDefaultAsync(m => m.PatientTreatmentId == id);
            if (patientTreatment == null)
            {
                return NotFound();
            }

            return View(patientTreatment);
        }

        // GET: KAPatientTreatment/Create
        public IActionResult Create()
        {

            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId");
            ViewData["TreatmentId"] = new SelectList(_context.Treatment.Where(x => x.DiagnosisId == Convert.ToInt32(HttpContext.Session.GetString("sessionDid"))), "TreatmentId", "Name");
            return View();
        }

        // POST: KAPatientTreatment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientTreatmentId,TreatmentId,DatePrescribed,Comments,PatientDiagnosisId")] PatientTreatment patientTreatment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patientTreatment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId", patientTreatment.PatientDiagnosisId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatment.Where(x=>x.DiagnosisId == Convert.ToInt32(HttpContext.Session.GetString("sessionDid"))), "TreatmentId", "Name", patientTreatment.TreatmentId);
            return View(patientTreatment);
        }

        // GET: KAPatientTreatment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientTreatment = await _context.PatientTreatment.FindAsync(id);
            if (patientTreatment == null)
            {
                return NotFound();
            }
            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId", patientTreatment.PatientDiagnosisId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatment.Where(x => x.DiagnosisId == Convert.ToInt32(HttpContext.Session.GetString("sessionDid"))), "TreatmentId", "Name", patientTreatment.TreatmentId);
            return View(patientTreatment);
        }

        // POST: KAPatientTreatment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientTreatmentId,TreatmentId,DatePrescribed,Comments,PatientDiagnosisId")] PatientTreatment patientTreatment)
        {
            if (id != patientTreatment.PatientTreatmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientTreatment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientTreatmentExists(patientTreatment.PatientTreatmentId))
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
            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId", patientTreatment.PatientDiagnosisId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatment.Where(x => x.DiagnosisId == Convert.ToInt32(HttpContext.Session.GetString("sessionDid"))), "TreatmentId", "Name", patientTreatment.TreatmentId);
            return View(patientTreatment);
        }

        // GET: KAPatientTreatment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientTreatment = await _context.PatientTreatment
                .Include(p => p.PatientDiagnosis)
                .Include(p => p.Treatment)
                .FirstOrDefaultAsync(m => m.PatientTreatmentId == id);
            if (patientTreatment == null)
            {
                return NotFound();
            }

            return View(patientTreatment);
        }

        // POST: KAPatientTreatment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)

        {
            try
            {
                var patientTreatment = await _context.PatientTreatment.FindAsync(id);
                _context.PatientTreatment.Remove(patientTreatment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

            catch
            {
                ViewBag.ReferencingTable = "Patient Medication";
                return View("~/Views/Shared/Error.cshtml");


            }
        }

        private bool PatientTreatmentExists(int id)
        {
            return _context.PatientTreatment.Any(e => e.PatientTreatmentId == id);
        }
    }
}
