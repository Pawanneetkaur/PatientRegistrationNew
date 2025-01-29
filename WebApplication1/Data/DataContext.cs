using Microsoft.EntityFrameworkCore;
using WebApplication1.Models; // Ensure correct namespace

namespace WebApplication1.Data
{
    public class DataContext : DbContext // ✅ Ensure this matches what PatientRepo expects
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Patient> Patients { get; set; } = null!;
    }
}
