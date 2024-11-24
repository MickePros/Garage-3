using Garage_3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Garage_3.Models.ViewModels;

namespace Garage_3.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<VehicleType> VehicleTypes { get; set; } = default!;
        public DbSet<Vehicle> Vehicles { get; set; } = default!;
    }
}
