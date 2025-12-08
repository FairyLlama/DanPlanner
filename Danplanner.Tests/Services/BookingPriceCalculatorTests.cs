using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Danplanner.Data;
using Danplanner.Data.Entities;
using Danplanner.Services;
using Danplanner.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using ProductTypeEnum = Danplanner.Shared.Models.ProductType;


namespace Danplanner.Tests.Services
{
    public class BookingPriceCalculatorTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AppDbContext(options);

            // 👉 Opret et produkt til hytten (kræves pga. "required Product")
            var cottageProduct = new Product
            {
                Id = 1,
                ProductType = ProductTypeEnum.Cottage
            };
            db.Products.Add(cottageProduct);

            // 👉 Cottage, der peger på produktet
            db.Cottages.Add(new Cottage
            {
                Id = 1,
                ProductId = cottageProduct.Id,
                Product = cottageProduct,
                PricePerNight = 500m,
                MaxCapacity = 4
            });

            // 👉 Addon
            db.Addons.Add(new Addons
            {
                Id = 1,
                Name = "WiFi",
                Price = 50m
            });

            db.SaveChanges();
            return db;
        }

        [Fact]
        public async Task CalculateTotalPrice_CottageWithoutDiscount_ReturnsCorrectPrice()
        {
            // Arrange
            var db = GetDbContext();
            var calc = new BookingPriceCalculator(db);

            var dto = new BookingDto
            {
                StartDate = new DateTime(2025, 12, 1),
                EndDate = new DateTime(2025, 12, 4), // 3 nætter
                CottageId = 1,
                BookingAddons = new List<BookingAddonDto>
                {
                    new BookingAddonDto { AddonId = 1, Quantity = 2 }
                }
            };

            // Act
            var total = await calc.CalculateTotalPriceAsync(dto);

            // base = 3 * 500 = 1500
            // addons = 2 * 50 = 100
            // total = 1600
            Assert.Equal(1600m, total);
        }

        [Fact]
        public async Task CalculateTotalPrice_WithSeniorDiscount_Applies15PercentOff()
        {
            // Arrange
            var db = GetDbContext();
            var calc = new BookingPriceCalculator(db);

            var dto = new BookingDto
            {
                StartDate = new DateTime(2025, 12, 1),
                EndDate = new DateTime(2025, 12, 3), // 2 nætter
                CottageId = 1,
                DiscountType = "senior"
            };

            // Act
            var total = await calc.CalculateTotalPriceAsync(dto);

            // base = 2 * 500 = 1000
            // senior = 15% rabat → 1000 * 0.85 = 850
            Assert.Equal(850m, total);
        }
    }
}
