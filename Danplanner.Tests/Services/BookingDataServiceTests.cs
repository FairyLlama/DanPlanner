using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Danplanner.Data;
using Danplanner.Data.Entities;
using Danplanner.Services;
using Danplanner.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using ProductTypeEnum = Danplanner.Shared.Models.ProductType;

namespace Danplanner.Tests.Services
{
    public class BookingDataServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AppDbContext(options);

            // Produkt til booking
            var product = new Product
            {
                Id = 1,
                ProductType = ProductTypeEnum.Cottage
            };
            db.Products.Add(product);

            // Addon som kan bruges af booking
            db.Addons.Add(new Addons
            {
                Id = 1,
                Name = "Morgenmad",
                Price = 100m
            });

            db.SaveChanges();
            return db;
        }

        [Fact]
        public async Task CreateAsync_UsesPriceCalculatorAndSavesBooking()
        {
            // Arrange
            var db = GetDbContext();

            var calculatorMock = new Mock<IBookingPriceCalculator>();
            calculatorMock
                .Setup(c => c.CalculateTotalPriceAsync(It.IsAny<BookingDto>()))
                .ReturnsAsync(2000m);

            var service = new BookingDataService(db, calculatorMock.Object);

            var dto = new BookingDto
            {
                ProductId = 1,
                StartDate = new DateTime(2025, 12, 1),
                EndDate = new DateTime(2025, 12, 3), // 2 nætter
                NumberOfPeople = 3,
                Status = "Afventer",
                BookingAddons = new List<BookingAddonDto>
                {
                    new BookingAddonDto { AddonId = 1, Quantity = 2 }
                }
            };

            // Act
            var created = await service.CreateAsync(dto);

            // Assert
            Assert.NotNull(created);
            Assert.Equal(2000m, created.TotalPrice);

            var bookingEntity = await db.Bookings
                .Include(b => b.BookingAddons)
                .FirstOrDefaultAsync(b => b.Id == created.Id);

            Assert.NotNull(bookingEntity);
            Assert.Equal(1, bookingEntity!.ProductId);
            Assert.Equal(3, bookingEntity.NumberOfPeople);
            Assert.Equal("Afventer", bookingEntity.Status);
            Assert.Single(bookingEntity.BookingAddons);
            Assert.Equal(1, bookingEntity.BookingAddons.First().AddonId);
            Assert.Equal(2, bookingEntity.BookingAddons.First().Quantity);

            calculatorMock.Verify(
                c => c.CalculateTotalPriceAsync(It.Is<BookingDto>(d =>
                    d.ProductId == dto.ProductId &&
                    d.StartDate == dto.StartDate &&
                    d.EndDate == dto.EndDate)),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_RemovesBookingFromDatabase()
        {
            // Arrange
            var db = GetDbContext();

            var calculatorMock = new Mock<IBookingPriceCalculator>();
            calculatorMock
                .Setup(c => c.CalculateTotalPriceAsync(It.IsAny<BookingDto>()))
                .ReturnsAsync(1500m);

            var service = new BookingDataService(db, calculatorMock.Object);

            var dto = new BookingDto
            {
                ProductId = 1,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(2),
                NumberOfPeople = 2
            };

            var created = await service.CreateAsync(dto);
            var id = created.Id;

            Assert.True(await db.Bookings.AnyAsync(b => b.Id == id));

            // Act
            await service.DeleteAsync(id);

            // Assert
            Assert.False(await db.Bookings.AnyAsync(b => b.Id == id));
        }

        [Fact]
        public async Task UpdateAsync_ChangesFieldsAndRecalculatesPrice()
        {
            // Arrange
            var db = GetDbContext();

            var calculatorMock = new Mock<IBookingPriceCalculator>();
            calculatorMock
                .SetupSequence(c => c.CalculateTotalPriceAsync(It.IsAny<BookingDto>()))
                .ReturnsAsync(1000m)  // første gang: CreateAsync
                .ReturnsAsync(1500m); // anden gang: UpdateAsync

            var service = new BookingDataService(db, calculatorMock.Object);

            var originalDto = new BookingDto
            {
                ProductId = 1,
                StartDate = new DateTime(2025, 12, 1),
                EndDate = new DateTime(2025, 12, 3),
                NumberOfPeople = 2,
                Status = "Afventer"
            };

            var created = await service.CreateAsync(originalDto);

            // Act – opdater booking
            var updatedDto = new BookingDto
            {
                Id = created.Id,
                ProductId = 1,
                StartDate = new DateTime(2025, 12, 2),
                EndDate = new DateTime(2025, 12, 5),
                NumberOfPeople = 4,
                Status = "Aktiv"
            };

            var updated = await service.UpdateAsync(updatedDto);

            // Assert
            Assert.NotNull(updated);
            Assert.Equal(1500m, updated!.TotalPrice); // fra anden mock-return

            var bookingEntity = await db.Bookings.FirstOrDefaultAsync(b => b.Id == created.Id);
            Assert.NotNull(bookingEntity);
            Assert.Equal(new DateTime(2025, 12, 2), bookingEntity!.StartDate);
            Assert.Equal(new DateTime(2025, 12, 5), bookingEntity.EndDate);
            Assert.Equal(4, bookingEntity.NumberOfPeople);
            Assert.Equal("Aktiv", bookingEntity.Status);

            calculatorMock.Verify(
                c => c.CalculateTotalPriceAsync(It.IsAny<BookingDto>()),
                Times.Exactly(2)); // én for create, én for update
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsBookingWithCorrectData()
        {
            // Arrange
            var db = GetDbContext();

            var calculatorMock = new Mock<IBookingPriceCalculator>();
            calculatorMock
                .Setup(c => c.CalculateTotalPriceAsync(It.IsAny<BookingDto>()))
                .ReturnsAsync(1200m);

            var service = new BookingDataService(db, calculatorMock.Object);

            var dto = new BookingDto
            {
                ProductId = 1,
                StartDate = new DateTime(2025, 12, 1),
                EndDate = new DateTime(2025, 12, 4),
                NumberOfPeople = 3,
                Status = "Afventer"
            };

            var created = await service.CreateAsync(dto);

            // Act
            var result = await service.GetByIdAsync(created.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(created.Id, result!.Id);
            Assert.Equal(1, result.ProductId);
            Assert.Equal(3, result.NumberOfPeople);
            Assert.Equal("Afventer", result.Status);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllBookings()
        {
            // Arrange
            var db = GetDbContext();

            var calculatorMock = new Mock<IBookingPriceCalculator>();
            calculatorMock
                .Setup(c => c.CalculateTotalPriceAsync(It.IsAny<BookingDto>()))
                .ReturnsAsync(1000m);

            var service = new BookingDataService(db, calculatorMock.Object);

            var dto1 = new BookingDto
            {
                ProductId = 1,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
                NumberOfPeople = 2
            };

            var dto2 = new BookingDto
            {
                ProductId = 1,
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(3),
                NumberOfPeople = 4
            };

            await service.CreateAsync(dto1);
            await service.CreateAsync(dto2);

            // Act
            var all = await service.GetAllAsync();

            // Assert
            Assert.NotNull(all);
            Assert.True(all.Count >= 2); // der kan være flere, men mindst de 2 vi lige lavede
            Assert.Contains(all, b => b.NumberOfPeople == 2);
            Assert.Contains(all, b => b.NumberOfPeople == 4);
        }
    }
}
