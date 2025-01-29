using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Patient
    {
        [Key]
        public int MedicalRecordNumber { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty; // ✅ Fix nullable warning

        public int Age { get; set; }

        [Required]
        public string Gender { get; set; } = string.Empty; // ✅ Fix nullable warning

        public string Contacts { get; set; } = string.Empty; // ✅ Fix nullable warning

        public string AdmittingDiagnosis { get; set; } = string.Empty; // ✅ Fix nullable warning

        public string AttendingPhysician { get; set; } = string.Empty; // ✅ Fix nullable warning

        public string Department { get; set; } = string.Empty; // ✅ Fix nullable warning
    }
}
