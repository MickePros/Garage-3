﻿using Garage_3.Models;
using Microsoft.AspNetCore.Identity;

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

            await AddRolesAsync(roleNames);

            var admin = await AddAccountAsync(adminEmail, "Admin", "Adminsson", 9012240011, "Group6!");
            var user = await AddAccountAsync(userEmail, "User", "Usersson", 9012240011, "Group6!");

            await AddUserToRoleAsync(admin, "Admin");
            await AddUserToRoleAsync(user, "User");

            var vehicleTypes = GenerateTypes();
            await context.AddRangeAsync(vehicleTypes);

            var vehicles = GenerateVehicles(user, vehicleTypes);
            await context.AddRangeAsync(vehicles);

            var parkingSpots = GenerateSpots();
            await context.AddRangeAsync(parkingSpots);

            await context.SaveChangesAsync();
        }

        private static IEnumerable<ParkingSpot> GenerateSpots()
        {
            var parkingSpots = new List<ParkingSpot>();
            parkingSpots.Add(new ParkingSpot { Status = 2 });
            parkingSpots.Add(new ParkingSpot { Status = 0 });
            parkingSpots.Add(new ParkingSpot { Status = 1 });
            parkingSpots.Add(new ParkingSpot { Status = 0 });
            parkingSpots.Add(new ParkingSpot { Status = 2 });

            return parkingSpots;
        }

        private static IEnumerable<Vehicle> GenerateVehicles(ApplicationUser user, IEnumerable<VehicleType> vehicleTypes)
        {
            var vehicles = new List<Vehicle>();
            int pk = 123;
            foreach (var vehicleType in vehicleTypes)
            {
                vehicles.Add(new Vehicle { RegNr = $"ABC-{pk}", Brand = "Volvo", Model = "EX90", Color = "Red", Wheels = 4, Arrival = DateTime.Now, VehicleType = vehicleType, VehicleTypeId = vehicleType.Id, ApplicationUser = user, ApplicationUserId = user.Id });
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
