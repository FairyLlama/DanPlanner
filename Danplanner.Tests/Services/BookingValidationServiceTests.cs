using System;
using System.Collections.Generic;
using Danplanner.Client.Services;
using Danplanner.Shared.Models;
using Xunit;

namespace Danplanner.Tests.Services
{
    public class BookingValidationServiceTests
    {
        private readonly BookingValidationService _svc = new();

        [Fact]
        public void ValidateNumberOfPeople_ReturnsError_WhenTooManyForCottage()
        {
            // Arrange
            var dto = new BookingDto
            {
                NumberOfPeople = 5,
                CottageId = 10
            };
            var product = new ProductDto { Id = 1, ProductType = ProductType.Cottage };
            var cottages = new List<CottageDto>
            {
                new CottageDto { Id = 10, MaxCapacity = 4 }
            };
            var grass = new List<GrassFieldDto>();

            // Act
            var error = _svc.ValidateNumberOfPeople(dto, product, cottages, grass);

            // Assert
            Assert.Equal("Der kan maks være 4 personer i den valgte hytte.", error);
        }

        [Fact]
        public void ValidateNumberOfPeople_ReturnsEmpty_WhenWithinCapacityForGrassField()
        {
            var dto = new BookingDto
            {
                NumberOfPeople = 6,
                GrassFieldId = 20
            };
            var product = new ProductDto { Id = 2, ProductType = ProductType.GrassField };
            var cottages = new List<CottageDto>();
            var grassFields = new List<GrassFieldDto>
            {
                new GrassFieldDto { Id = 20, MaxCapacity = 6 }
            };

            var error = _svc.ValidateNumberOfPeople(dto, product, cottages, grassFields);

            Assert.Equal(string.Empty, error);
        }

        [Fact]
        public void ValidateAvailability_ReturnsError_WhenOverlapsExistingBooking_SameCottage()
        {
            // Arrange
            var dto = new BookingDto
            {
                CottageId = 10,
                StartDate = new DateTime(2025, 12, 10),
                EndDate = new DateTime(2025, 12, 14),
                Status = "Afventer"
            };

            var product = new ProductDto { Id = 1, ProductType = ProductType.Cottage };

            var existing = new List<BookingDto>
            {
                new BookingDto
                {
                    Id = 1,
                    CottageId = 10,
                    StartDate = new DateTime(2025, 12, 12),
                    EndDate   = new DateTime(2025, 12, 16),
                    Status = "Aktiv"
                }
            };

            // Act
            var error = _svc.ValidateAvailability(dto, product, existing);

            // Assert
            Assert.Contains("overlapper booking #1", error);
        }

        [Fact]
        public void ValidateAvailability_ReturnsEmpty_WhenNoOverlap()
        {
            var dto = new BookingDto
            {
                CottageId = 10,
                StartDate = new DateTime(2025, 12, 10),
                EndDate = new DateTime(2025, 12, 14),
            };

            var product = new ProductDto { Id = 1, ProductType = ProductType.Cottage };

            var existing = new List<BookingDto>
            {
                new BookingDto
                {
                    Id = 2,
                    CottageId = 10,
                    StartDate = new DateTime(2025, 12, 15),
                    EndDate   = new DateTime(2025, 12, 20),
                    Status = "Aktiv"
                }
            };

            var error = _svc.ValidateAvailability(dto, product, existing);

            Assert.Equal(string.Empty, error);
        }
    }
}
