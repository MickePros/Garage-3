using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage_3.Data;
using Garage_3.Models;
using Microsoft.AspNetCore.Identity;
using Garage_3.Models.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Text.RegularExpressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;

namespace Garage_3.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        [TempData]
        public string Message { get; set; }
        [TempData]
        public string Alert { get; set; }

        public VehiclesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

        // GET: Vehicles/Register
        public IActionResult RegisterVehicle()
        {
            // Fetch distinct VehicleTypes from Vehicles
            var vehicleTypes = _context.Vehicles
                .Select(v => v.VehicleType)
                .Distinct()
                .ToList();

            ViewData["VehicleTypes"] = new SelectList(vehicleTypes, "Type", "Type"); 
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterVehicle(RegisterVehicleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                if (userId == null)
                {
                    return NotFound();
                }

                var vehicleType = await _context.VehicleTypes
                .FirstOrDefaultAsync(vt => vt.Type == viewModel.VehicleType);

                // Map the viewModel to a new Vehicle
                var vehicle = new Vehicle
                {
                    RegNr = viewModel.RegNr,
                    Brand = viewModel.Brand,
                    Model = viewModel.Model,
                    Color = viewModel.Color,
                    Wheels = viewModel.Wheels,
                    VehicleTypeId = vehicleType.Id, 
                    Arrival = null,
                    ApplicationUserId = userId
                };

                _context.Vehicles.Add(vehicle);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // If the model state is invalid, repopulate the VehicleType dropdown
            var vehicleTypesFallback = _context.Vehicles.Select(v => v.VehicleType).Distinct().ToList();
            ViewData["VehicleTypes"] = new SelectList(vehicleTypesFallback, "Type", "Type");
            return View(viewModel);
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

        // GET: Vehicles/EditVehicle
        public async Task<IActionResult> EditVehicle(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .Select(v => new EditVehicleViewModel
                {
                    RegNr = v.RegNr,
                    Brand = v.Brand,
                    Model = v.Model,
                    Color = v.Color,
                    Wheels = v.Wheels,
                    VehicleType = v.VehicleType.Type
                })
                .FirstOrDefaultAsync(v => v.RegNr == id);

            if (vehicle == null)
            {
                return NotFound();
            }

            var vehicleTypes = await _context.VehicleTypes
            .Select(vt => vt.Type)
            .Distinct()
            .ToListAsync();

            ViewBag.VehicleTypes = new SelectList(vehicleTypes);
            return View(vehicle);
        }

        // POST: Vehicles/EditVehicle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVehicle(string id, EditVehicleViewModel editVehicleViewModel)
        {
            if (id != editVehicleViewModel.RegNr)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var vehicle = await _context.Vehicles.FindAsync(id);
                    if (vehicle == null)
                    {
                        return NotFound();
                    }

                    vehicle.Brand = editVehicleViewModel.Brand;
                    vehicle.Model = editVehicleViewModel.Model;
                    vehicle.Color = editVehicleViewModel.Color;
                    vehicle.Wheels = editVehicleViewModel.Wheels;

                    // Find matching VehicleType by its TypeName
                    var vehicleType = await _context.VehicleTypes
                        .FirstOrDefaultAsync(vt => vt.Type == editVehicleViewModel.VehicleType);
                    if (vehicleType == null)
                    {
                        ModelState.AddModelError("VehicleType", "Invalid vehicle type.");
                        ViewBag.VehicleTypes = new SelectList(_context.VehicleTypes, "Type", "Type", editVehicleViewModel.VehicleType);
                        return View(editVehicleViewModel);
                    }
                    vehicle.VehicleTypeId = vehicleType.Id;

                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Vehicles.Any(e => e.RegNr == id))
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

            var vehicleTypes = await _context.VehicleTypes
            .Select(vt => vt.Type)
            .Distinct()
            .ToListAsync();

            ViewBag.VehicleTypes = new SelectList(vehicleTypes);
            return View(editVehicleViewModel);
        }


        public async Task<IActionResult> ParkingspotsOverview(int id = -1, int status = -1)
        {
            var model = _context.ParkingSpot.Select(p => new ParkingSpotOverviewViewModel
            {
                Id = p.Id,
                Status = p.Status,
                Vehicles = p.Vehicles
            });

            model = (id <= 0) ?
                model :
                model.Where(p => p.Id == id);

            model = (status < 0) ?
                model :
                model.Where(p => p.Status == status);

            return View(await model.ToListAsync());
        }

        public async Task<IActionResult> ParkingFilter(int id, int status = -1)
        {
            return RedirectToAction(nameof(ParkingspotsOverview), new { id, status });
        }

        public IActionResult AddParkingSpot()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddParkingSpot([Bind("Status")] ParkingSpot parkingSpot)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parkingSpot);
                await _context.SaveChangesAsync();
                Alert = "success";
                Message = $"Parking spot successfully added.";
                return RedirectToAction(nameof(ParkingspotsOverview));
            }
            return View(parkingSpot);
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

            //if (!query.Any()) return NotFound();

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

        // Remote: Check if RegNr exists in DB
        public ActionResult CheckRegNr(string regNr)
        {
            var vehicle = _context.Vehicles
                .FirstOrDefault(v => v.RegNr == regNr);
            if (vehicle != null)
            {
                return Json($"{regNr} is already parked in the garage.");
            }
            return Json(true);
        }

        public async Task<IActionResult> Users(string? freetext = null)
        {
            var model = _context.Users.Select(u => new UsersViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Vehicles = u.Vehicles.Count(),
                Vehiclelist = u.Vehicles.ToList()
            });

            model = string.IsNullOrWhiteSpace(freetext) ?
                model :
                model.Where(u => u.FirstName.Contains(freetext)
                || u.LastName.Contains(freetext)
                || u.Email.Contains(freetext)
                );

            var userModel = await model.ToListAsync();

            foreach (var user in userModel)
            {
                var parkLength = new TimeSpan(0, 0, 0, 0);
                foreach (var vehicle in user.Vehiclelist)
                {
                    if (vehicle.Arrival != null)
                    {
                        parkLength = parkLength + (DateTime.Now - (DateTime)vehicle.Arrival);
                    }
                }
                user.ParkingFee = (parkLength.Hours + parkLength.Days * 24) * 100;
            }

            return View(nameof(Users), userModel);
        }

        public async Task<IActionResult> UserFilter(string freetext)
        {
            return RedirectToAction(nameof(Users), new { freetext });
        }

        public async Task<IActionResult> UserDetails(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Vehicles)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
    }
}
