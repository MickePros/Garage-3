using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage_3.Data;
using Garage_3.Models.ViewModels;

namespace Garage_3.Controllers
{
    public class ParkingSpotsOverviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParkingSpotsOverviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ParkingSpotsOverviews
        public async Task<IActionResult> Index()
        {
            return View(await _context.ParkingSpotsOverview.ToListAsync());
        }

        // GET: ParkingSpotsOverviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingSpotsOverview = await _context.ParkingSpotsOverview
                .FirstOrDefaultAsync(m => m.ParkingSpotId == id);
            if (parkingSpotsOverview == null)
            {
                return NotFound();
            }

            return View(parkingSpotsOverview);
        }

        // GET: ParkingSpotsOverviews/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ParkingSpotsOverviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ParkingSpotId,RegNr,IsOccupied,VehicleOwner,VehicleType")] ParkingSpotsOverviewViewModel parkingSpotsOverview)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parkingSpotsOverview);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(parkingSpotsOverview);
        }

        // GET: ParkingSpotsOverviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingSpotsOverview = await _context.ParkingSpotsOverview.FindAsync(id);
            if (parkingSpotsOverview == null)
            {
                return NotFound();
            }
            return View(parkingSpotsOverview);
        }

        // POST: ParkingSpotsOverviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ParkingSpotId,RegNr,IsOccupied,VehicleOwner,VehicleType")] ParkingSpotsOverviewViewModel parkingSpotsOverview)
        {
            if (id != parkingSpotsOverview.ParkingSpotId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parkingSpotsOverview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkingSpotsOverviewExists(parkingSpotsOverview.ParkingSpotId))
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
            return View(parkingSpotsOverview);
        }

        // GET: ParkingSpotsOverviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingSpotsOverview = await _context.ParkingSpotsOverview
                .FirstOrDefaultAsync(m => m.ParkingSpotId == id);
            if (parkingSpotsOverview == null)
            {
                return NotFound();
            }

            return View(parkingSpotsOverview);
        }

        // POST: ParkingSpotsOverviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parkingSpotsOverview = await _context.ParkingSpotsOverview.FindAsync(id);
            if (parkingSpotsOverview != null)
            {
                _context.ParkingSpotsOverview.Remove(parkingSpotsOverview);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParkingSpotsOverviewExists(int id)
        {
            return _context.ParkingSpotsOverview.Any(e => e.ParkingSpotId == id);
        }
    }
}
