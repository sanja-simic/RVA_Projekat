using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using TravelSystem.Server.Services;
using TravelSystem.Models.DTOs;
using TravelSystem.Models.Enums;

namespace TravelSystem.Server.Tests
{
    [TestClass]
    public class TravelManagementServiceTests
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
        public void GetAllArrangements_ShouldReturnSuccessResponse()
        {
            // Act
            var result = _service.GetAllArrangements();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void AddArrangement_ValidArrangement_ShouldReturnSuccess()
        {
            // Arrange
            var arrangement = new TravelArrangementDto
            {
                AssociatedTransportation = ModeOfTransport.Plane,
                NumberOfDays = 7,
                Destination = new DestinationDto
                {
                    TownName = "Paris",
                    CountryName = "France",
                    StayPriceByDay = 150.0
                },
                Passengers = new System.Collections.Generic.List<PassengerDto>()
            };

            // Act
            var result = _service.AddArrangement(arrangement);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.IsNotNull(result.Data.Id);
        }

        [TestMethod]
        public void AddArrangement_InvalidArrangement_ShouldReturnFailure()
        {
            // Arrange
            var arrangement = new TravelArrangementDto
            {
                AssociatedTransportation = ModeOfTransport.Plane,
                NumberOfDays = -1, // Invalid
                Destination = null, // Invalid
                Passengers = new System.Collections.Generic.List<PassengerDto>()
            };

            // Act
            var result = _service.AddArrangement(arrangement);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.IsNotNull(result.ErrorMessage);
        }

        [TestMethod]
        public void GetArrangementById_ExistingId_ShouldReturnArrangement()
        {
            // Arrange
            var arrangement = new TravelArrangementDto
            {
                AssociatedTransportation = ModeOfTransport.Bus,
                NumberOfDays = 5,
                Destination = new DestinationDto
                {
                    TownName = "London",
                    CountryName = "England",
                    StayPriceByDay = 200.0
                },
                Passengers = new System.Collections.Generic.List<PassengerDto>()
            };

            var addResult = _service.AddArrangement(arrangement);
            var arrangementId = addResult.Data.Id;

            // Act
            var result = _service.GetArrangementById(arrangementId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(arrangementId, result.Data.Id);
        }

        [TestMethod]
        public void GetArrangementById_NonExistingId_ShouldReturnFailure()
        {
            // Act
            var result = _service.GetArrangementById("non-existing-id");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.IsNull(result.Data);
        }

        [TestMethod]
        public void DeleteArrangement_ExistingId_ShouldReturnSuccess()
        {
            // Arrange
            var arrangement = new TravelArrangementDto
            {
                AssociatedTransportation = ModeOfTransport.Train,
                NumberOfDays = 3,
                Destination = new DestinationDto
                {
                    TownName = "Rome",
                    CountryName = "Italy",
                    StayPriceByDay = 120.0
                },
                Passengers = new System.Collections.Generic.List<PassengerDto>()
            };

            var addResult = _service.AddArrangement(arrangement);
            var arrangementId = addResult.Data.Id;

            // Act
            var result = _service.DeleteArrangement(arrangementId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.Data);
        }

        [TestMethod]
        public void ChangeArrangementState_ShouldAdvanceState()
        {
            // Arrange
            var arrangement = new TravelArrangementDto
            {
                AssociatedTransportation = ModeOfTransport.Plane,
                NumberOfDays = 7,
                Destination = new DestinationDto
                {
                    TownName = "Barcelona",
                    CountryName = "Spain",
                    StayPriceByDay = 180.0
                },
                Passengers = new System.Collections.Generic.List<PassengerDto>()
            };

            var addResult = _service.AddArrangement(arrangement);
            var arrangementId = addResult.Data.Id;
            var initialState = addResult.Data.State;

            // Act
            var result = _service.ChangeArrangementState(arrangementId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.AreNotEqual(initialState, result.Data.State);
        }
    }
}