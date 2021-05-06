using Microsoft.EntityFrameworkCore;
using Web_Service.Models;

namespace Client_WPF.Models
{
    public partial class ParkingmachinesdbContext : DbContext
    {
        public ParkingmachinesdbContext()
        {
        }

        public ParkingmachinesdbContext(DbContextOptions<ParkingmachinesdbContext> options) : base(options)
        {
        }

        public virtual DbSet<ParkingmachineInfo> Parkingmachines { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var current_dir = new System.IO.DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
                var application_dir = current_dir.Parent.Parent.Parent.Parent.FullName;
                string db_path = "Data Source=" + application_dir + "\\Web Service" + "\\Parkingmachines.db";

                optionsBuilder.UseSqlite(db_path);
            }
        }

    }
}