using Microsoft.EntityFrameworkCore;

namespace Web_Service.Models
{
    public class ParkingmachinesdbMainContext : DbContext
    {
        public ParkingmachinesdbMainContext()
        {
        }

        public DbSet<ParkingmachineInfo> Parkingmachines { get; set; }

        public ParkingmachinesdbMainContext(DbContextOptions<ParkingmachinesdbMainContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();           
        }
    }
}