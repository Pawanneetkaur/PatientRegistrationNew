using Microsoft.EntityFrameworkCore;
namespace WebApplication1.Models
{
    public class PatientContext : DbContext
    {
        public PatientContext(DbContextOptions<PatientContext> options) : base(options) { }
        public DbSet<Patient> Patients { get; set; }
    }
}
