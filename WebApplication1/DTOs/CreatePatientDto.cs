using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class CreatePatientDto
    {
        [Required]
        public string Name { get; set; } = string.Empty; // ✅ Fix nullable warning

        [Required]
        public string MedicalRecordNumber { get; set; } = string.Empty; // ✅ Fix nullable warning

        [Required]
        public string Gender { get; set; } = string.Empty; // ✅ Fix nullable warning
    }
}
