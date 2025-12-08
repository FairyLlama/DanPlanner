using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Danplanner.Controllers;
using Danplanner.Services;
using Danplanner.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Danplanner.Tests.Controllers
{
    public class BookingControllerTests
    {
        [Fact]
        public async Task Confirm_WithValidBookingAndUser_SendsEmailAndReturnsOk()
        {
            // Arrange
            var booking = new BookingDto
            {
                Id = 42,
                UserId = 5,
                ProductId = 1,
                Product = new ProductDto { Id = 1, ProductType = ProductType.Cottage },
                CottageId = 10,
                Cottage = new CottageDto { Id = 10, Number = 6 },
                StartDate = new DateTime(2025, 12, 1),
                EndDate = new DateTime(2025, 12, 3),
                NumberOfPeople = 4,
                TotalPrice = 2200m,
                BookingAddons = new List<BookingAddonDto>
                {
                    new BookingAddonDto
                    {
                        AddonId = 1,
                        Quantity = 2,
                        Addon = new AddonDto
                        {
                            Id = 1,
                            Name = "Morgenmad",
                            Price = 100m
                        }
                    }
                },
                User = new UserDto
                {
                    Id = 5,
                    Name = "Test Bruger",
                    Email = "test@example.com",
                    Address = "Testvej 1",
                    Phone = "12345678",
                    Country = "DK",
                    Language = "da"
                }
            };

            var bookingSvcMock = new Mock<IBookingDataService>();
            bookingSvcMock.Setup(s => s.GetByIdAsync(42)).ReturnsAsync(booking);
            bookingSvcMock.Setup(s => s.ConfirmAsync(42, 5)).ReturnsAsync(true);

            var receiptMock = new Mock<IReceiptService>();
            var fakePdf = new byte[] { 1, 2, 3 };
            receiptMock.Setup(r => r.GenerateReceipt(booking)).Returns(fakePdf);

            var emailMock = new Mock<IEmailService>();
            emailMock
                .Setup(e => e.SendAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<byte[]>(),
                    It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var loggerMock = new Mock<ILogger<BookingController>>();
            var configMock = new Mock<IConfiguration>();

            var controller = new BookingController(
                bookingSvcMock.Object,
                receiptMock.Object,
                emailMock.Object,
                loggerMock.Object,
                configMock.Object);

            // Act
            var result = await controller.Confirm(42, 5);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Booking bekræftet og kvittering sendt!", ok.Value);

            // Kvittering genereres
            receiptMock.Verify(r => r.GenerateReceipt(booking), Times.Once);

            // Mail sendes til rigtige modtager med vedhæftet pdf
            emailMock.Verify(e => e.SendAsync(
                    "test@example.com",
                    It.Is<string>(s => s.Contains("Velkommen til Danplanner")),
                    It.Is<string>(body => body.Contains("Tak for din booking")),
                    fakePdf,
                    "kvittering.pdf"),
                Times.Once);
        }
    }
}
