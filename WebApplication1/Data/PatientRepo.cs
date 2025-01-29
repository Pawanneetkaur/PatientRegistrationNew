using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models; // For Patient and PatientContext

namespace WebApplication1.Data
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientContext _context;

        public PatientRepository(PatientContext context)
        {
            _context = context;
        }

        public IEnumerable<Patient> GetAllPatients() 
            => _context.Patients.ToList();

        public Patient GetPatient(int id)
            => _context.Patients.Find(id);

        public void AddPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            _context.SaveChanges();
        }

        public void UpdatePatient(Patient patient)
        {
            _context.Patients.Update(patient);
            _context.SaveChanges();
        }

        public void RemovePatient(int id)
        {
            var patient = _context.Patients.Find(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                _context.SaveChanges();
            }
        }
    }

    public interface IPatientRepository
    {
        IEnumerable<Patient> GetAllPatients();
        Patient GetPatient(int id);
        void AddPatient(Patient patient);
        void UpdatePatient(Patient patient);
        void RemovePatient(int id);
    }
}
