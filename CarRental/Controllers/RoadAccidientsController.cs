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
    public class RoadAccidientsController : Controller
    {
        private readonly DefaultContext _context;
        private readonly IConfiguration _configuration;

        public RoadAccidientsController(DefaultContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: RoadAccidients
        public async Task<IActionResult> Index()
        {
            var defaultContext = _context.RoadAccidients.Include(r => r.Rental);
            return View(await defaultContext.ToListAsync());
        }

        // GET: RoadAccidients/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roadAccidient = await _context.RoadAccidients
                .Include(r => r.Rental)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roadAccidient == null)
            {
                return NotFound();
            }

            return View(roadAccidient);
        }

        // GET: RoadAccidients/Create
        public IActionResult Create()
        {
            ViewData["RentalId"] = new SelectList(_context.Rentals, "Id", "ContractNumber");
            return View();
        }

        // POST: RoadAccidients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( RoadAccidient roadAccidient)
        {
            if (ModelState.IsValid)
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultContext")))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("[dbo].[road_accidient_registration]", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@rental_id", roadAccidient.RentalId);
                        command.Parameters.AddWithValue("@date", roadAccidient.Date);
                        command.Parameters.AddWithValue("@protocol", roadAccidient.TrafficPoliceProtocolId);
                        _ = await command.ExecuteNonQueryAsync();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RentalId"] = new SelectList(_context.Rentals, "Id", "ContractNumber", roadAccidient.RentalId);
            return View(roadAccidient);
        }

        // GET: RoadAccidients/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roadAccidient = await _context.RoadAccidients.FindAsync(id);
            if (roadAccidient == null)
            {
                return NotFound();
            }
            ViewData["RentalId"] = new SelectList(_context.Rentals, "Id", "ContractNumber", roadAccidient.RentalId);
            return View(roadAccidient);
        }

        // POST: RoadAccidients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, RoadAccidient roadAccidient)
        {
            if (id != roadAccidient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roadAccidient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoadAccidientExists(roadAccidient.Id))
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
            ViewData["RentalId"] = new SelectList(_context.Rentals, "Id", "ContractNumber", roadAccidient.RentalId);
            return View(roadAccidient);
        }

        // GET: RoadAccidients/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roadAccidient = await _context.RoadAccidients
                .Include(r => r.Rental)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roadAccidient == null)
            {
                return NotFound();
            }

            return View(roadAccidient);
        }

        // POST: RoadAccidients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var roadAccidient = await _context.RoadAccidients.FindAsync(id);
            _context.RoadAccidients.Remove(roadAccidient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoadAccidientExists(long id)
        {
            return _context.RoadAccidients.Any(e => e.Id == id);
        }
    }
}
