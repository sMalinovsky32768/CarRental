using CarRental.Data;
using CarRental.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Controllers
{
    public class FinesController : Controller
    {
        private readonly DefaultContext _context;
        private readonly IConfiguration _configuration;

        public FinesController(DefaultContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Fines
        public async Task<IActionResult> Index()
        {
            var defaultContext = _context.Fines.Include(f => f.Rental);
            return View(await defaultContext.ToListAsync());
        }

        // GET: Fines/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fine = await _context.Fines
                .Include(f => f.Rental)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fine == null)
            {
                return NotFound();
            }

            return View(fine);
        }

        // GET: Fines/Create
        public IActionResult Create()
        {
            ViewData["RentalId"] = new SelectList(_context.Rentals, "Id", "ContractNumber");
            return View();
        }

        // POST: Fines/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Fine fine)
        {
            if (ModelState.IsValid)
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultContext")))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("[dbo].[fines_registration]", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@rental_id", fine.RentalId);
                        command.Parameters.AddWithValue("@amount", fine.Amount);
                        command.Parameters.AddWithValue("@description", fine.Description);
                        _ = await command.ExecuteNonQueryAsync();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RentalId"] = new SelectList(_context.Rentals, "Id", "ContractNumber", fine.RentalId);
            return View(fine);
        }

        // GET: Fines/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fine = await _context.Fines.FindAsync(id);
            if (fine == null)
            {
                return NotFound();
            }
            ViewData["RentalId"] = new SelectList(_context.Rentals, "Id", "ContractNumber", fine.RentalId);
            return View(fine);
        }

        // POST: Fines/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Fine fine)
        {
            if (id != fine.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FineExists(fine.Id))
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
            ViewData["RentalId"] = new SelectList(_context.Rentals, "Id", "ContractNumber", fine.RentalId);
            return View(fine);
        }

        // GET: Fines/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fine = await _context.Fines
                .Include(f => f.Rental)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fine == null)
            {
                return NotFound();
            }

            return View(fine);
        }

        // POST: Fines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var fine = await _context.Fines.FindAsync(id);
            _context.Fines.Remove(fine);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FineExists(long id)
        {
            return _context.Fines.Any(e => e.Id == id);
        }
    }
}
