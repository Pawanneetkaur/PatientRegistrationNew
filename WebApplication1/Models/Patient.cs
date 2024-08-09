using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Patient
    {
        public string? Name { get; set; }
        [Key]
        public int MedicalRecordNumber { get; set; }
        public int? Age { get; set; }
        public required string Gender { get; set; }
        public List<string>? Contacts { get; set; }
        public string? AdmittingDiagnosis { get; set; }
        public string? AttendingPhysician { get; set; }
        public string? Department { get; set; }
    }
}
