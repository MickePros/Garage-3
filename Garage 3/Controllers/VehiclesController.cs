using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage_3.Data;
using Garage_3.Models;
using Garage_3.Models.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Text.RegularExpressions;

namespace Garage_3.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehiclesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Vehicles.Include(v => v.ApplicationUser).Include(v => v.VehicleType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .Include(v => v.ApplicationUser)
                .Include(v => v.VehicleType)
                .FirstOrDefaultAsync(m => m.RegNr == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["VehicleTypeId"] = new SelectList(_context.Set<VehicleType>(), "Id", "Id");
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RegNr,Brand,Model,Color,Wheels,Registration,Arrival,VehicleTypeId,ApplicationUserId")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", vehicle.ApplicationUserId);
            ViewData["VehicleTypeId"] = new SelectList(_context.Set<VehicleType>(), "Id", "Id", vehicle.VehicleTypeId);
            return View(vehicle);
        }

        public IActionResult AddVehicleType()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVehicleType([Bind("Type,Size")] VehicleType vehicleType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicleType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vehicleType);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", vehicle.ApplicationUserId);
            ViewData["VehicleTypeId"] = new SelectList(_context.Set<VehicleType>(), "Id", "Id", vehicle.VehicleTypeId);
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("RegNr,Brand,Model,Color,Wheels,Registration,Arrival,VehicleTypeId,ApplicationUserId")] Vehicle vehicle)
        {
            if (id != vehicle.RegNr)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.RegNr))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", vehicle.ApplicationUserId);
            ViewData["VehicleTypeId"] = new SelectList(_context.Set<VehicleType>(), "Id", "Id", vehicle.VehicleTypeId);
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .Include(v => v.ApplicationUser)
                .Include(v => v.VehicleType)
                .FirstOrDefaultAsync(m => m.RegNr == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(string id)
        {
            return _context.Vehicles.Any(e => e.RegNr == id);
        }


        public async Task<IActionResult> ParkedVehiclesOverview(string regNr, string? type)
        {
            //Get vehicles with arrival time set
            var query = _context.Vehicles
                .Include(v => v.ApplicationUser)
                .Include(v => v.VehicleType)
                .Include(v => v.ParkingSpots)
                .Where(v => v.Arrival.HasValue);

            if (!query.Any()) return NotFound();

            // Apply filters if provided
            if (!string.IsNullOrWhiteSpace(regNr))
            {
                query = query.Where(v => v.RegNr.Contains(regNr));
            }
            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(v => v.VehicleType.Type == type);
            }

            var vehicles = await query.ToListAsync();

            //Return empty list if no vehicles were found (used in view)
            if (!vehicles.Any()) return View(new List<ParkedVehicleOverwievViewModel>()); 

            var viewModels = vehicles.Select(vehicle => new ParkedVehicleOverwievViewModel
            {
                Owner = vehicle.ApplicationUser.FirstName + " " + vehicle.ApplicationUser.LastName,
                // Membership = vehicle.ApplicationUser.MembershipStatus, // Uncomment if this exists
                Type = vehicle.VehicleType.Type,
                RegNr = vehicle.RegNr,
                ParkingSpotId = vehicle.ParkingSpots
                    .FirstOrDefault(spot => spot.Vehicles.Any(v => v.RegNr == vehicle.RegNr)).Id,
                Arrival = (DateTime)vehicle.Arrival
            }).ToList();

            return View(viewModels);
        }

        public async Task<IActionResult> Filter(string regNr, string? type)
        {
            return RedirectToAction(nameof(ParkedVehiclesOverview), new { regNr, type });
        }
    }
}
