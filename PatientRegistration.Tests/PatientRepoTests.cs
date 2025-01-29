using Xunit;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using System;

namespace PatientRegistration.Tests
{
    public class PatientRepositoryTests
    {
        private PatientContext GetInMemoryPatientContext()
        {
            var options = new DbContextOptionsBuilder<PatientContext>()
                // Unique DB name per test run
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new PatientContext(options);
            // Optionally ensure everything is fresh:
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public void GetAllPatients_ReturnsEmpty_WhenNoneInDatabase()
        {
            // Arrange
            using var context = GetInMemoryPatientContext();
            var repo = new PatientRepository(context);

            // Act
            var patients = repo.GetAllPatients(); // Should be an empty collection

            // Assert
            Assert.Empty(patients);
        }

        [Fact]
        public void AddPatient_SavesPatientToDatabase()
        {
            // Arrange
            using var context = GetInMemoryPatientContext();
            var repo = new PatientRepository(context);

            var patient = new Patient
            {
                Name = "TestAdd",
                Gender = "Female"
            };

            // Act
            repo.AddPatient(patient);

            // Assert
            Assert.Single(context.Patients);
            Assert.Equal("TestAdd", context.Patients.First().Name);
        }

        [Fact]
        public void RemovePatient_DeletesPatient_WhenExists()
        {
            // Arrange
            using var context = GetInMemoryPatientContext();
            var repo = new PatientRepository(context);

            // Insert one patient
            var patient = new Patient
            {
                Name = "Removable",
                Gender = "Male"
            };
            context.Patients.Add(patient);
            context.SaveChanges();

            // Confirm that there's exactly 1 patient in DB
            Assert.Single(context.Patients);

            // Now remove that patient by its MedicalRecordNumber
            repo.RemovePatient(patient.MedicalRecordNumber);

            // Assert
            Assert.Empty(context.Patients);
        }

        [Fact]
        public void GetPatient_ReturnsPatient_WhenExists()
        {
            // Arrange
            using var context = GetInMemoryPatientContext();
            var repo = new PatientRepository(context);

            // Insert a patient
            var patient = new Patient
            {
                Name = "Somebody",
                Gender = "Male"
            };
            context.Patients.Add(patient);
            context.SaveChanges();

            int actualId = patient.MedicalRecordNumber;

            // Act
            var fetched = repo.GetPatient(actualId);

            // Assert
            Assert.NotNull(fetched);
            Assert.Equal("Somebody", fetched.Name);
        }
    }
}
