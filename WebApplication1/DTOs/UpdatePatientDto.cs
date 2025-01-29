using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PatientRegistrationService.DTOs
{
    public class UpdatePatientDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name should be between 2 and 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Medical Record Number is required.")]
        public string MedicalRecordNumber { get; set; }

        [Range(0, 120, ErrorMessage = "Age must be between 0 and 120.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        [MinLength(1, ErrorMessage = "At least one contact is required.")]
        public List<string> Contacts { get; set; } = new List<string>();
    }
}