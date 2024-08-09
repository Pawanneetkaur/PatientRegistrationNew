using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly PatientContext _context;

        public PatientController(PatientContext context)
        {
            _context = context;
        }
        [HttpGet("ping")]
        public ActionResult<Patient> Ping()
        {
            return Ok("pong");
        }

//        private static List<Patient> patients = new List<Patient>();
//        private static int nextId = 1;
        // GET api/<PatientController>
        [HttpGet]
        public async Task<ActionResult<List<Patient>>> GetPatients()
        {
            return await _context.Patients.ToListAsync();
        }

        // GET api/<PatientController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatientByID(int id)
        {
            var resultPatient = await _context.Patients.FindAsync(id);
            //Patient resultPatient = patients.FirstOrDefault(predicate: patient => patient.MedicalRecordNumber == id);
            if (resultPatient == null)
            {
                return BadRequest("Patient with MedicalRecordNumber " + id + " not found.");
            }
            return resultPatient;
        }

        // POST api/<PatientController>
        [HttpPost]
        public async Task<ActionResult<Patient>> RegisterPatient([FromBody] Patient newPatient)
        {
            if (newPatient.MedicalRecordNumber != 0)
            {
                return BadRequest("MedicalRecordNumber will be auto generated, it can not be configured manually. Please try again without it.");
            }
            if (newPatient.AdmittingDiagnosis == null){}
            else if (newPatient.AdmittingDiagnosis.Equals("Breast Cancer") || newPatient.AdmittingDiagnosis.Equals("Lung Cancer"))
            {
                newPatient.AttendingPhysician = "Dr. Susan Jones";
                newPatient.Department = "J";
            }
            else if (newPatient.AdmittingDiagnosis.Equals("Prostate Cancer"))
            {
                newPatient.AttendingPhysician = "Dr. Ben Smith";
                newPatient.Department = "S";
            }
            else
            {
                newPatient.AdmittingDiagnosis = "Unspecified Cancer";
                newPatient.AttendingPhysician = "Dr. Ben Smith";
                newPatient.Department = "S";
            }
            if (newPatient.Name == null)
            {
                if (newPatient.Gender.Equals("Male"))
                    newPatient.Name = "John Doe";
                else
                    newPatient.Name = "Jane Doe";
            }

//            newPatient.MedicalRecordNumber = nextId++;
//            patients.Add(newPatient);
            _context.Patients.Add(newPatient);
            await _context.SaveChangesAsync();
            return newPatient;
        }

        // PUT api/<PatientController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Patient>> UpdatePatient(int id, [FromBody] Patient updatedPatient)
        {
            //           Patient patientToUpdate = patients.FirstOrDefault(predicate: patient => patient.MedicalRecordNumber == id);
            var patientToUpdate = await _context.Patients.FindAsync(id);
            if (patientToUpdate == null)
            {
                return BadRequest("Patient with MedicalRecordNumber " + id + " not found.");
            }
            if (updatedPatient.Name == null || updatedPatient.Gender == null)
            {
                return BadRequest("Name and Gender are mandatory");
            }

            patientToUpdate.Name = updatedPatient.Name;
            patientToUpdate.Age = updatedPatient.Age;
            patientToUpdate.Gender = updatedPatient.Gender;
            patientToUpdate.Contacts = updatedPatient.Contacts;

            if (patientToUpdate.AdmittingDiagnosis == null && updatedPatient.AdmittingDiagnosis != null)
            {
                if (updatedPatient.AdmittingDiagnosis.Equals("Breast Cancer") || updatedPatient.AdmittingDiagnosis.Equals("Lung Cancer"))
                {
                    patientToUpdate.AdmittingDiagnosis = updatedPatient.AdmittingDiagnosis;
                    patientToUpdate.AttendingPhysician = "Dr. Susan Jones";
                    patientToUpdate.Department = "J";
                }
                else if (updatedPatient.AdmittingDiagnosis.Equals("Prostate Cancer"))
                {
                    patientToUpdate.AdmittingDiagnosis = patientToUpdate.AdmittingDiagnosis;
                    patientToUpdate.AttendingPhysician = "Dr. Ben Smith";
                    patientToUpdate.Department = "S";
                }
                else
                {
                    patientToUpdate.AdmittingDiagnosis = "Unspecified Cancer";
                    patientToUpdate.AttendingPhysician = "Dr. Ben Smith";
                    patientToUpdate.Department = "S";
                }
            }
            else if (updatedPatient.AdmittingDiagnosis != null && updatedPatient.AdmittingDiagnosis != patientToUpdate.AdmittingDiagnosis)
            {
                return BadRequest("Admitting Diagnosis can not be changed, please make a new entry instead.");
            }

            _context.Entry(patientToUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return patientToUpdate;
        }

        // DELETE api/<PatientController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Patient>> DeletePatient(int id)
        {
            //            Patient patientToDelete = patients.FirstOrDefault(predicate: patient => patient.MedicalRecordNumber == id);
            var patientToDelete = await _context.Patients.FindAsync(id);
            if (patientToDelete == null)
            {
                return BadRequest("Patient with MedicalRecordNumber " + id + " not found.");
            }
            else if (patientToDelete.AdmittingDiagnosis != null)
            {
                return BadRequest("Patient has an admitting diagnosis recorded. Unable to delete by design.");
            }
            else
            {
                //patients.Remove(patientToDelete);
                _context.Patients.Remove(patientToDelete);
                await _context.SaveChangesAsync();
            }
            return Ok("Patient with MedicalRecordNumber " + id + " deleted successfully");
        }
    }
}
