using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TravelSystem.Server.Services;
using TravelSystem.Models.DTOs;

namespace TravelSystem.Server.Tests
{
    [TestClass]
    public class DestinationManagementTests
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
        public void GetAllDestinations_ShouldReturnSuccessResponse()
        {
            // Act
            var result = _service.GetAllDestinations();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void AddDestination_ValidDestination_ShouldReturnSuccess()
        {
            // Arrange
            var destination = new DestinationDto
            {
                TownName = "Madrid",
                CountryName = "Spain",
                StayPriceByDay = 160.0
            };

            // Act
            var result = _service.AddDestination(destination);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.IsNotNull(result.Data.Id);
            Assert.AreEqual("Madrid", result.Data.TownName);
            Assert.AreEqual("Spain", result.Data.CountryName);
            Assert.AreEqual(160.0, result.Data.StayPriceByDay);
        }

        [TestMethod]
        public void AddDestination_InvalidDestination_ShouldReturnFailure()
        {
            // Arrange
            var destination = new DestinationDto
            {
                TownName = "", // Invalid
                CountryName = "Spain",
                StayPriceByDay = -50.0 // Invalid
            };

            // Act
            var result = _service.AddDestination(destination);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.IsNotNull(result.ErrorMessage);
        }

        [TestMethod]
        public void UpdateDestination_ValidUpdate_ShouldReturnSuccess()
        {
            // Arrange
            var destination = new DestinationDto
            {
                TownName = "Madrid",
                CountryName = "Spain",
                StayPriceByDay = 160.0
            };

            var addResult = _service.AddDestination(destination);
            var destinationId = addResult.Data.Id;

            var updatedDestination = new DestinationDto
            {
                Id = destinationId,
                TownName = "Madrid",
                CountryName = "Spain",
                StayPriceByDay = 180.0 // Updated price
            };

            // Act
            var result = _service.UpdateDestination(updatedDestination);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(180.0, result.Data.StayPriceByDay);
        }

        [TestMethod]
        public void GetDestinationById_ExistingId_ShouldReturnDestination()
        {
            // Arrange
            var destination = new DestinationDto
            {
                TownName = "Berlin",
                CountryName = "Germany",
                StayPriceByDay = 140.0
            };

            var addResult = _service.AddDestination(destination);
            var destinationId = addResult.Data.Id;

            // Act
            var result = _service.GetDestinationById(destinationId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(destinationId, result.Data.Id);
            Assert.AreEqual("Berlin", result.Data.TownName);
        }

        [TestMethod]
        public void DeleteDestination_ExistingId_ShouldReturnSuccess()
        {
            // Arrange
            var destination = new DestinationDto
            {
                TownName = "Vienna",
                CountryName = "Austria",
                StayPriceByDay = 130.0
            };

            var addResult = _service.AddDestination(destination);
            var destinationId = addResult.Data.Id;

            // Act
            var result = _service.DeleteDestination(destinationId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.Data);
        }

        [TestMethod]
        public void DeleteDestination_NonExistingId_ShouldReturnFailure()
        {
            // Act
            var result = _service.DeleteDestination("non-existing-id");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.IsFalse(result.Data);
        }

        [TestMethod]
        public void DeleteDestination_UsedInArrangement_ShouldReturnFailure()
        {
            // Arrange - Add destination
            var destination = new DestinationDto
            {
                TownName = "Prague",
                CountryName = "Czech Republic",
                StayPriceByDay = 110.0
            };

            var destinationResult = _service.AddDestination(destination);
            var destinationId = destinationResult.Data.Id;

            // Add arrangement that uses this destination
            var arrangement = new TravelArrangementDto
            {
                AssociatedTransportation = TravelSystem.Models.Enums.ModeOfTransport.Train,
                NumberOfDays = 4,
                Destination = destinationResult.Data,
                Passengers = new System.Collections.Generic.List<PassengerDto>()
            };

            _service.AddArrangement(arrangement);

            // Act - Try to delete destination
            var result = _service.DeleteDestination(destinationId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.ErrorMessage.Contains("assigned to one or more travel arrangements"));
        }
    }
}