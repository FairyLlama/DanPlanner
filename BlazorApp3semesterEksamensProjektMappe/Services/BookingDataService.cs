using BlazorApp3semesterEksamensProjektMappe.Data;
using BlazorApp3semesterEksamensProjektMappe.Data.Entities;
using BlazorApp3semesterEksamensProjektMappe.shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp3semesterEksamensProjektMappe.Services
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
                .Include(b => b.Resource)
                .Include(b => b.Product)
                .Select(b => new BookingDto
                {
                    Id = b.Id,
                    ResourceId = b.ResourceId,
                    ResourceName = b.Resource.Name,
                    ProductId = b.ProductId,
                    ProductName = b.Product.Name,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    TotalPrice = b.TotalPrice
                })
                .ToListAsync();
        }

        public async Task<BookingDto?> GetByIdAsync(int id)
        {
            var b = await _db.Bookings
                .Include(x => x.Resource)
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (b is null) return null;

            return new BookingDto
            {
                Id = b.Id,
                ResourceId = b.ResourceId,
                ResourceName = b.Resource.Name,
                ProductId = b.ProductId,
                ProductName = b.Product.Name,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                TotalPrice = b.TotalPrice
            };
        }

        public async Task<BookingDto> CreateAsync(BookingDto dto)
        {
            var product = await _db.Products.FindAsync(dto.ProductId)
                          ?? throw new InvalidOperationException("Produkt findes ikke");
            var resource = await _db.Resources.FindAsync(dto.ResourceId)
                          ?? throw new InvalidOperationException("Ressource findes ikke");

            var days = (dto.EndDate - dto.StartDate).Days;
            if (days <= 0) throw new InvalidOperationException("Ugyldig periode");

            var booking = new Booking
            {
                ResourceId = dto.ResourceId,
                ProductId = dto.ProductId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                TotalPrice = product.BasePrice * days
            };

            _db.Bookings.Add(booking);
            await _db.SaveChangesAsync();

            return new BookingDto
            {
                Id = booking.Id,
                ResourceId = booking.ResourceId,
                ResourceName = resource.Name,
                ProductId = booking.ProductId,
                ProductName = product.Name,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                TotalPrice = booking.TotalPrice
            };
        }
    }

}
