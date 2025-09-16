using Microsoft.VisualStudio.TestTools.UnitTesting;
using TravelSystem.Models.Entities;
using TravelSystem.Models.Enums;

namespace TravelSystem.Server.Tests
{
    [TestClass]
    public class ValidationTests
    {
        [TestMethod]
        public void TravelArrangement_ValidData_ShouldPassValidation()
        {
            // Arrange
            var destination = new Destination("1", "Paris", "France", 150.0);
            var arrangement = new TravelArrangement("1", ModeOfTransport.Plane, 7, destination);

            // Act
            var isValid = arrangement.IsValid();

            // Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void TravelArrangement_InvalidNumberOfDays_ShouldFailValidation()
        {
            // Arrange
            var destination = new Destination("1", "Paris", "France", 150.0);
            var arrangement = new TravelArrangement("1", ModeOfTransport.Plane, -1, destination);

            // Act
            var isValid = arrangement.IsValid();
            var errors = arrangement.GetValidationErrors();

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(errors.Contains("Number of days must be greater than 0"));
        }

        [TestMethod]
        public void TravelArrangement_NullDestination_ShouldFailValidation()
        {
            // Arrange
            var arrangement = new TravelArrangement("1", ModeOfTransport.Plane, 7, null);

            // Act
            var isValid = arrangement.IsValid();
            var errors = arrangement.GetValidationErrors();

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(errors.Contains("Destination cannot be null"));
        }

        [TestMethod]
        public void Passenger_ValidData_ShouldPassValidation()
        {
            // Arrange
            var passenger = new Passenger("1", "John", "Doe", "AB123456", 25);

            // Act
            var isValid = passenger.IsValid();

            // Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void Passenger_EmptyFirstName_ShouldFailValidation()
        {
            // Arrange
            var passenger = new Passenger("1", "", "Doe", "AB123456", 25);

            // Act
            var isValid = passenger.IsValid();
            var errors = passenger.GetValidationErrors();

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(errors.Contains("First name cannot be null or empty"));
        }

        [TestMethod]
        public void Passenger_EmptyLastName_ShouldFailValidation()
        {
            // Arrange
            var passenger = new Passenger("1", "John", "", "AB123456", 25);

            // Act
            var isValid = passenger.IsValid();
            var errors = passenger.GetValidationErrors();

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(errors.Contains("Last name cannot be null or empty"));
        }

        [TestMethod]
        public void Passenger_EmptyPassportNumber_ShouldFailValidation()
        {
            // Arrange
            var passenger = new Passenger("1", "John", "Doe", "", 25);

            // Act
            var isValid = passenger.IsValid();
            var errors = passenger.GetValidationErrors();

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(errors.Contains("Passport number cannot be null or empty"));
        }

        [TestMethod]
        public void Passenger_NegativeLuggageWeight_ShouldFailValidation()
        {
            // Arrange
            var passenger = new Passenger("1", "John", "Doe", "AB123456", -5);

            // Act
            var isValid = passenger.IsValid();
            var errors = passenger.GetValidationErrors();

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(errors.Contains("Luggage weight cannot be negative"));
        }

        [TestMethod]
        public void Destination_ValidData_ShouldPassValidation()
        {
            // Arrange
            var destination = new Destination("1", "Paris", "France", 150.0);

            // Act
            var isValid = destination.IsValid();

            // Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void Destination_EmptyTownName_ShouldFailValidation()
        {
            // Arrange
            var destination = new Destination("1", "", "France", 150.0);

            // Act
            var isValid = destination.IsValid();
            var errors = destination.GetValidationErrors();

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(errors.Contains("Town name cannot be null or empty"));
        }

        [TestMethod]
        public void Destination_EmptyCountryName_ShouldFailValidation()
        {
            // Arrange
            var destination = new Destination("1", "Paris", "", 150.0);

            // Act
            var isValid = destination.IsValid();
            var errors = destination.GetValidationErrors();

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(errors.Contains("Country name cannot be null or empty"));
        }

        [TestMethod]
        public void Destination_NegativePrice_ShouldFailValidation()
        {
            // Arrange
            var destination = new Destination("1", "Paris", "France", -50.0);

            // Act
            var isValid = destination.IsValid();
            var errors = destination.GetValidationErrors();

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(errors.Contains("Stay price by day must be greater than 0"));
        }
    }
}