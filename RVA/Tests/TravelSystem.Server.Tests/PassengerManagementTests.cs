using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TravelSystem.Server.Services;
using TravelSystem.Models.DTOs;

namespace TravelSystem.Server.Tests
{
    [TestClass]
    public class PassengerManagementTests
    {
        private TravelManagementService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new TravelManagementService();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _service = null;
        }

        [TestMethod]
        public void GetAllPassengers_ShouldReturnSuccessResponse()
        {
            // Act
            var result = _service.GetAllPassengers();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void AddPassenger_ValidPassenger_ShouldReturnSuccess()
        {
            // Arrange
            var passenger = new PassengerDto
            {
                FirstName = "John",
                LastName = "Doe",
                PassportNumber = "AB123456",
                LuggageWeight = 25
            };

            // Act
            var result = _service.AddPassenger(passenger);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.IsNotNull(result.Data.Id);
            Assert.AreEqual("John", result.Data.FirstName);
            Assert.AreEqual("Doe", result.Data.LastName);
        }

        [TestMethod]
        public void AddPassenger_InvalidPassenger_ShouldReturnFailure()
        {
            // Arrange
            var passenger = new PassengerDto
            {
                FirstName = "", // Invalid
                LastName = "Doe",
                PassportNumber = "AB123456",
                LuggageWeight = -10 // Invalid
            };

            // Act
            var result = _service.AddPassenger(passenger);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.IsNotNull(result.ErrorMessage);
        }

        [TestMethod]
        public void AddPassenger_DuplicatePassportNumber_ShouldReturnFailure()
        {
            // Arrange
            var passenger1 = new PassengerDto
            {
                FirstName = "John",
                LastName = "Doe",
                PassportNumber = "AB123456",
                LuggageWeight = 25
            };

            var passenger2 = new PassengerDto
            {
                FirstName = "Jane",
                LastName = "Smith",
                PassportNumber = "AB123456", // Same passport number
                LuggageWeight = 20
            };

            // Act
            var result1 = _service.AddPassenger(passenger1);
            var result2 = _service.AddPassenger(passenger2);

            // Assert
            Assert.IsTrue(result1.IsSuccess);
            Assert.IsFalse(result2.IsSuccess);
            Assert.IsTrue(result2.ErrorMessage.Contains("passport number already exists"));
        }

        [TestMethod]
        public void UpdatePassenger_ValidUpdate_ShouldReturnSuccess()
        {
            // Arrange
            var passenger = new PassengerDto
            {
                FirstName = "John",
                LastName = "Doe",
                PassportNumber = "AB123456",
                LuggageWeight = 25
            };

            var addResult = _service.AddPassenger(passenger);
            var passengerId = addResult.Data.Id;

            var updatedPassenger = new PassengerDto
            {
                Id = passengerId,
                FirstName = "Johnny",
                LastName = "Doe",
                PassportNumber = "AB123456",
                LuggageWeight = 30
            };

            // Act
            var result = _service.UpdatePassenger(updatedPassenger);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("Johnny", result.Data.FirstName);
            Assert.AreEqual(30, result.Data.LuggageWeight);
        }

        [TestMethod]
        public void GetPassengerById_ExistingId_ShouldReturnPassenger()
        {
            // Arrange
            var passenger = new PassengerDto
            {
                FirstName = "John",
                LastName = "Doe",
                PassportNumber = "AB123456",
                LuggageWeight = 25
            };

            var addResult = _service.AddPassenger(passenger);
            var passengerId = addResult.Data.Id;

            // Act
            var result = _service.GetPassengerById(passengerId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(passengerId, result.Data.Id);
        }

        [TestMethod]
        public void DeletePassenger_ExistingId_ShouldReturnSuccess()
        {
            // Arrange
            var passenger = new PassengerDto
            {
                FirstName = "John",
                LastName = "Doe",
                PassportNumber = "AB123456",
                LuggageWeight = 25
            };

            var addResult = _service.AddPassenger(passenger);
            var passengerId = addResult.Data.Id;

            // Act
            var result = _service.DeletePassenger(passengerId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.Data);
        }

        [TestMethod]
        public void DeletePassenger_NonExistingId_ShouldReturnFailure()
        {
            // Act
            var result = _service.DeletePassenger("non-existing-id");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.IsFalse(result.Data);
        }
    }
}