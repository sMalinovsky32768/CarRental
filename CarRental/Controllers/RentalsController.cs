using CarRental.Data;
using CarRental.Extensions;
using CarRental.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Controllers
{
    public class RentalsController : Controller
    {
        private readonly DefaultContext _context;
        private readonly IConfiguration _configuration;

        public RentalsController(DefaultContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(RentalFiltersModel filters)
        { 
            var defaultContext = _context.Rentals.Include(r => r.Car).Include(r => r.Client).Select(r => r);
            if (filters.From.HasValue)
                defaultContext = defaultContext.Where(r => r.StartDateTime >= filters.From);

            if (filters.To.HasValue)
                defaultContext = defaultContext.Where(r => r.EndDateTime <= filters.To);

            if (filters.SortOrder == Models.SortOrder.Asc)
                switch (filters.SortBy)
                {
                    case RentalSort.EndDateTime:
                        defaultContext = defaultContext.OrderBy(r => r.EndDateTime);
                        break;
                    case RentalSort.StartDateTime:
                        defaultContext = defaultContext.OrderBy(r => r.StartDateTime);
                        break;
                }
            else
                switch (filters.SortBy)
                {
                    case RentalSort.EndDateTime:
                        defaultContext = defaultContext.OrderByDescending(r => r.EndDateTime);
                        break;
                    case RentalSort.StartDateTime:
                        defaultContext = defaultContext.OrderByDescending(r => r.StartDateTime);
                        break;
                }
            var model = new RentalListModel
            {
                Filters = filters,
                Rentals = defaultContext.ToList()
            };
            ViewBag.SortBy = Enum.GetValues<RentalSort>().Select(r => new SelectListItem(r.GetDescription(), $"{(int)r}"));
            return View(model);
        }

        // GET: Rentals/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals
                .Include(r => r.Car)
                .Include(r => r.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // GET: Rentals/Create
        public IActionResult Create()
        {
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "DisplayName");
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FullName");
            return View();
        }

        // POST: Rentals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rental rental)
        {
            if (ModelState.IsValid)
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultContext")))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("[dbo].[rental_registration]", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@client_id", rental.ClientId);
                        command.Parameters.AddWithValue("@car_id", rental.CarId);
                        command.Parameters.AddWithValue("@contract_number", rental.ContractNumber);
                        command.Parameters.AddWithValue("@start_datetime", rental.StartDateTime);
                        command.Parameters.AddWithValue("@number_of_days", rental.NumberOfDays);
                        _ = await command.ExecuteNonQueryAsync();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "DisplayName", rental.CarId);
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FullName", rental.ClientId);
            return View(rental);
        }

        // GET: Rentals/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "DisplayName", rental.CarId);
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FullName", rental.ClientId);
            return View(rental);
        }

        // POST: Rentals/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Rental rental)
        {
            if (id != rental.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rental);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalExists(rental.Id))
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
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "DisplayName", rental.CarId);
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FullName", rental.ClientId);
            return View(rental);
        }

        // GET: Rentals/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals
                .Include(r => r.Car)
                .Include(r => r.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // POST: Rentals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            _context.Rentals.Remove(rental);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentalExists(long id)
        {
            return _context.Rentals.Any(e => e.Id == id);
        }
    }
}
