using Danplanner.Data;
using Danplanner.Data.Entities;
using Danplanner.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Danplanner.Services
{
    public class BookingDataService : IBookingDataService
    {
        private readonly AppDbContext _db;

        public BookingDataService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<BookingDto>> GetAllAsync()
        {
            return await _db.Bookings
                .Include(b => b.Product)
                .Select(b => new BookingDto
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    ProductId = b.ProductId,
                    Product = new ProductDto
                    {
                        Id = b.Product.Id,
                        ProductType = b.Product.ProductType,
                        SeasonalPrice = b.Product.SeasonalPrice,
                        ServicePrice = b.Product.ServicePrice,
                        NumberOfGuests = b.Product.NumberOfGuests,
                        AdditionalPurchases = b.Product.AdditionalPurchases
                    },
                    CancelBooking = b.CancelBooking,
                    Rebook = b.Rebook,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate
                })
                .ToListAsync();
        }

        public async Task<BookingDto?> GetByIdAsync(int id)
        {
            var b = await _db.Bookings
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (b is null) return null;

            return new BookingDto
            {
                Id = b.Id,
                UserId = b.UserId,
                ProductId = b.ProductId,
                Product = new ProductDto
                {
                    Id = b.Product.Id,
                    ProductType = b.Product.ProductType,
                    SeasonalPrice = b.Product.SeasonalPrice,
                    ServicePrice = b.Product.ServicePrice,
                    NumberOfGuests = b.Product.NumberOfGuests,
                    AdditionalPurchases = b.Product.AdditionalPurchases
                },
                CancelBooking = b.CancelBooking,
                Rebook = b.Rebook,
                StartDate = b.StartDate,
                EndDate = b.EndDate
            };
        }

        public async Task<BookingDto> CreateAsync(BookingDto dto)
        {
            var product = await _db.Products.FindAsync(dto.ProductId)
                          ?? throw new InvalidOperationException("Produkt findes ikke");

            var days = (dto.EndDate - dto.StartDate).Days;
            if (days <= 0) throw new InvalidOperationException("Ugyldig periode");

            var booking = new Booking
            {
                UserId = dto.UserId,
                ProductId = dto.ProductId,
                CancelBooking = dto.CancelBooking,
                Rebook = dto.Rebook,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Product = product

            };

            _db.Bookings.Add(booking);
            await _db.SaveChangesAsync();

            return new BookingDto
            {
                Id = booking.Id,
                UserId = booking.UserId,
                ProductId = booking.ProductId,
                Product = new ProductDto
                {
                    Id = product.Id,
                    ProductType = product.ProductType,
                    SeasonalPrice = product.SeasonalPrice,
                    ServicePrice = product.ServicePrice,
                    NumberOfGuests = product.NumberOfGuests,
                    AdditionalPurchases = product.AdditionalPurchases
                },
                CancelBooking = booking.CancelBooking,
                Rebook = booking.Rebook,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate
            };
        }
    }
}