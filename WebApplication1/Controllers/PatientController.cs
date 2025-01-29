using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [Route("api/patient")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly PatientContext _context;

        public PatientController(PatientContext context)
        {
           _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet("ping")]
        public ActionResult<string> Ping()
        {
            return Ok("pong");
        }

        [HttpGet("getAllPatients")]
        public async Task<ActionResult<List<Patient>>> GetPatients()
        {
            var patients = await _context.Patients.ToListAsync();
            return Ok(patients);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatientById(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
                return NotFound($"Patient with ID {id} not found.");

            return Ok(patient);
        }

        [HttpPost("registerNewPatient")]
        public async Task<ActionResult<Patient>> RegisterPatient([FromBody] Patient newPatient)
        {
            if (string.IsNullOrEmpty(newPatient.Name) || string.IsNullOrEmpty(newPatient.Gender))
            {
                return BadRequest("Name and Gender are required.");
            }

            _context.Patients.Add(newPatient);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPatients), new { id = newPatient.MedicalRecordNumber }, newPatient);
        }

        [HttpPut("updatePatient/{id}")]
        public async Task<ActionResult<Patient>> UpdatePatient(int id, [FromBody] Patient updatedPatient)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
                return NotFound("Patient not found.");

            patient.Name = updatedPatient.Name;
            patient.Age = updatedPatient.Age;
            patient.Gender = updatedPatient.Gender;
            patient.Contacts = updatedPatient.Contacts;

            await _context.SaveChangesAsync();
            return Ok(patient);
        }

        [HttpDelete("deletePatient/{id}")]
        public async Task<ActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
                return NotFound("Patient not found.");

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
