using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Controllers;

namespace PatientRegistration.Tests
{
    public class PatientControllerTests
    {
        private PatientContext GetInMemoryPatientContext()
        {
            var options = new DbContextOptionsBuilder<PatientContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB each test
                .Options;
            var context = new PatientContext(options);

            // (Optionally) seed data here if needed:
            // context.Patients.Add(new Patient { MedicalRecordNumber = 1, Name = "John", Age = 30, Gender = "Male" });
            // context.SaveChanges();

            return context;
        }

        [Fact]
        public void Ping_ReturnsPong()
        {
            // Arrange
            using var context = GetInMemoryPatientContext();
            var controller = new PatientController(context);

            // Act
            var result = controller.Ping();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result); 
            Assert.Equal("pong", okResult.Value);
        }

        [Fact]
        public async Task GetPatients_ReturnsEmptyList_WhenNoPatients()
        {
            // Arrange
            using var context = GetInMemoryPatientContext();
            var controller = new PatientController(context);

            // Act
            var result = await controller.GetPatients();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var patients = Assert.IsType<System.Collections.Generic.List<Patient>>(okResult.Value);
            Assert.Empty(patients);
        }

        [Fact]
        public async Task RegisterPatient_ReturnsCreatedResult_WhenValidPatient()
        {
            // Arrange
            using var context = GetInMemoryPatientContext();
            var controller = new PatientController(context);

            var newPatient = new Patient
            {
                Name = "Jane Doe",
                Age = 28,
                Gender = "Female",
                Contacts = "N/A"
            };

            // Act
            var actionResult = await controller.RegisterPatient(newPatient);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnedPatient = Assert.IsType<Patient>(createdResult.Value);
            Assert.Equal("Jane Doe", returnedPatient.Name);
        }

        [Fact]
        public async Task RegisterPatient_ReturnsBadRequest_WhenMissingName()
        {
            // Arrange
            using var context = GetInMemoryPatientContext();
            var controller = new PatientController(context);

            var newPatient = new Patient
            {
                // Name is missing
                Gender = "Other"
            };

            // Act
            var actionResult = await controller.RegisterPatient(newPatient);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Name and Gender are required.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetPatientById_ReturnsNotFound_WhenPatientNotExists()
        {
            // Arrange
            using var context = GetInMemoryPatientContext();
            var controller = new PatientController(context);

            // Act
            var actionResult = await controller.GetPatientById(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal("Patient with ID 999 not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetPatientById_ReturnsPatient_WhenPatientExists()
        {
            // Arrange
            using var context = GetInMemoryPatientContext();
            context.Patients.Add(new Patient
            {
                MedicalRecordNumber = 1,
                Name = "John Smith",
                Age = 45,
                Gender = "Male"
            });
            await context.SaveChangesAsync();

            var controller = new PatientController(context);

            // Act
            var actionResult = await controller.GetPatientById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var patient = Assert.IsType<Patient>(okResult.Value);
            Assert.Equal(1, patient.MedicalRecordNumber);
            Assert.Equal("John Smith", patient.Name);
        }

        [Fact]
        public async Task UpdatePatient_UpdatesFields_WhenPatientExists()
        {
            // Arrange
            using var context = GetInMemoryPatientContext();
            context.Patients.Add(new Patient
            {
                MedicalRecordNumber = 10,
                Name = "Old Name",
                Age = 20,
                Gender = "Male",
                Contacts = "123"
            });
            await context.SaveChangesAsync();

            var controller = new PatientController(context);
            var updatedPatient = new Patient
            {
                Name = "New Name",
                Age = 30,
                Gender = "Male",
                Contacts = "456"
            };

            // Act
            var result = await controller.UpdatePatient(10, updatedPatient);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var patient = Assert.IsType<Patient>(okResult.Value);
            Assert.Equal("New Name", patient.Name);
            Assert.Equal(30, patient.Age);
            Assert.Equal("456", patient.Contacts);
        }

        [Fact]
        public async Task UpdatePatient_ReturnsNotFound_WhenPatientDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryPatientContext();
            var controller = new PatientController(context);
            var updatedPatient = new Patient
            {
                Name = "Another Name",
                Age = 25,
                Gender = "Male"
            };

            // Act
            var result = await controller.UpdatePatient(999, updatedPatient);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Patient not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task DeletePatient_RemovesPatient_WhenPatientExists()
        {
            // Arrange
            using var context = GetInMemoryPatientContext();
            context.Patients.Add(new Patient
            {
                MedicalRecordNumber = 2,
                Name = "Test Patient",
                Gender = "Unknown"
            });
            await context.SaveChangesAsync();

            var controller = new PatientController(context);

            // Act
            var deleteResult = await controller.DeletePatient(2);

            // Assert
            Assert.IsType<NoContentResult>(deleteResult);
            Assert.False(context.Patients.Any(p => p.MedicalRecordNumber == 2));
        }

        [Fact]
        public async Task DeletePatient_ReturnsNotFound_WhenPatientDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryPatientContext();
            var controller = new PatientController(context);

            // Act
            var deleteResult = await controller.DeletePatient(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(deleteResult);
            Assert.Equal("Patient not found.", notFoundResult.Value);
        }
    }
}
