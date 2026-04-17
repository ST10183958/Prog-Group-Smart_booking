using Microsoft.AspNetCore.Mvc;
using BookingSystem.Data;
using BookingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Controllers
{
    public class MedicineStockController : Controller
    {
        private readonly BookingSystemContext _context;

        public MedicineStockController(BookingSystemContext context)
        {
            _context = context;
        }

        // GET: MedicineStock/Index
        public async Task<IActionResult> Index()
        {
            var medicines = await _context.Medicines.ToListAsync(); 
            return View(medicines);
        }

        // GET: MedicineStock/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MedicineStock/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Medicine medicine) 
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicine);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Medicine added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(medicine);
        }

        // GET: MedicineStock/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _context.Medicines.FindAsync(id); 
            if (medicine == null)
            {
                return NotFound();
            }
            return View(medicine);
        }

        // POST: MedicineStock/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Medicine medicine) 
        {
            if (id != medicine.MedicineId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicine);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Medicine updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicineExists(medicine.MedicineId))
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
            return View(medicine);
        }

        // GET: MedicineStock/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _context.Medicines 
                .FirstOrDefaultAsync(m => m.MedicineId == id);
            if (medicine == null)
            {
                return NotFound();
            }

            return View(medicine);
        }

        // POST: MedicineStock/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id); 
            if (medicine != null)
            {
                _context.Medicines.Remove(medicine); 
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Medicine deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MedicineExists(int id)
        {
            return _context.Medicines.Any(e => e.MedicineId == id); 
        }
    }
}