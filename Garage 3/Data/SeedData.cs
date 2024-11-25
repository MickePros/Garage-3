using Garage_3.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Garage_3.Data
{
    public class SeedData
    {
        private static ApplicationDbContext context = default!;
        private static RoleManager<IdentityRole> roleManager = default!;
        private static UserManager<ApplicationUser> userManager = default!;

        public static async Task Init(ApplicationDbContext _context, IServiceProvider services)
        {
            context = _context;
            if (context.Roles.Any()) return;

            roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            var roleNames = new[] { "user", "Admin" };
            var adminEmail = "admin@group6.com";
            var userEmail = "user@group6.com";
            var dudeEmail = "dude@group6.com";

            await AddRolesAsync(roleNames);

            var admin = await AddAccountAsync(adminEmail, "Admin", "Adminsson", 9012240011, "Group6!");
            var user = await AddAccountAsync(userEmail, "User", "Usersson", 9012240011, "Group6!");
            var dude = await AddAccountAsync(dudeEmail, "Dude", "Dudesson", 9012240011, "Group6!");

            await AddUserToRoleAsync(admin, "Admin");
            await AddUserToRoleAsync(user, "User");
            await AddUserToRoleAsync(dude, "User");

            var vehicleTypes = GenerateTypes();
            await context.AddRangeAsync(vehicleTypes);

            var vehicles = GenerateVehicles(user, dude, vehicleTypes);
            await context.AddRangeAsync(vehicles);

            var parkingSpots = GenerateSpots(vehicles);
            await context.AddRangeAsync(parkingSpots);

            await context.SaveChangesAsync();
        }

        private static IEnumerable<ParkingSpot> GenerateSpots(IEnumerable<Vehicle> vehicles)
        {
            var parkingSpots = new List<ParkingSpot>();
            foreach (var vehicle in vehicles)
            {
                var status = 0;
                var list = new List<Vehicle>();
                list.Add(vehicle);
                if (vehicle.VehicleType.Type == "Motorcycle") {status = 1;}
                else {status = 2;}
                parkingSpots.Add(new ParkingSpot { Status = status, Vehicles = list.ToList() });
            }
            for (int i = 0; i < 3; i++)
            {
                parkingSpots.Add(new ParkingSpot { Status = 0 });
            }

            return parkingSpots;
        }

        private static IEnumerable<Vehicle> GenerateVehicles(ApplicationUser user, ApplicationUser dude, IEnumerable<VehicleType> vehicleTypes)
        {
            var vehicles = new List<Vehicle>();
            int pk = 123;
            foreach (var vehicleType in vehicleTypes)
            {
                if (pk == 124)
                {
                    vehicles.Add(new Vehicle { RegNr = $"ABC-{pk}", Brand = "Volvo", Model = "EX90", Color = "Red", Wheels = 4, Arrival = DateTime.Now, VehicleType = vehicleType, VehicleTypeId = vehicleType.Id, ApplicationUser = dude, ApplicationUserId = user.Id });
                }
                else
                {
                    vehicles.Add(new Vehicle { RegNr = $"ABC-{pk}", Brand = "Volvo", Model = "EX90", Color = "Red", Wheels = 4, Arrival = DateTime.Now, VehicleType = vehicleType, VehicleTypeId = vehicleType.Id, ApplicationUser = user, ApplicationUserId = user.Id });
                }
                pk++;
            }

            return vehicles;
        }

        private static IEnumerable<VehicleType> GenerateTypes()
        {
            var vehicleTypes = new List<VehicleType>();
            vehicleTypes.Add(new VehicleType { Type = "Car", Size = 1 });
            vehicleTypes.Add(new VehicleType { Type = "Truck", Size = 2 });
            vehicleTypes.Add(new VehicleType { Type = "Motorcycle", Size = 0.3 });
            vehicleTypes.Add(new VehicleType { Type = "Boat", Size = 3 });
            vehicleTypes.Add(new VehicleType { Type = "Airplane", Size = 3 });

            return vehicleTypes;
        }

        private static async Task AddUserToRoleAsync(ApplicationUser user, string roleName)
        {
            if (!await userManager.IsInRoleAsync(user, roleName))
            {
                var result = await userManager.AddToRoleAsync(user, roleName);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }

        private static async Task AddRolesAsync(string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                if (await roleManager.RoleExistsAsync(roleName)) continue;
                var role = new IdentityRole { Name = roleName };
                var result = await roleManager.CreateAsync(role);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }

        private static async Task<ApplicationUser> AddAccountAsync(string accountEmail, string fName, string lName, long ssn, string pw)
        {
            var found = await userManager.FindByEmailAsync(accountEmail);
            if (found != null) return null!;
            var user = new ApplicationUser
            {
                UserName = accountEmail,
                Email = accountEmail,
                FirstName = fName,
                LastName = lName,
                SSN = ssn,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(user, pw);
            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            return user;
        }
    }
}
