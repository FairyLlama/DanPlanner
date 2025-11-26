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
                .Include(b => b.Cottage)
                .Include(b => b.GrassField)
                .Include(b => b.BookingAddons).ThenInclude(bt => bt.Addons)
                .Include(b => b.Receipt)
                .Select(b => new BookingDto
                {
                    Id = b.Id,
                    CampistId = b.CampistId,
                    ProductId = b.ProductId,
                    CottageId = b.CottageId,
                    GrassFieldId = b.GrassFieldId,
                    Product = new ProductDto
                    {
                        Id = b.Product.Id,
                        ProductType = b.Product.ProductType,
                        PricePerNight = b.Product.PricePerNight
                    },
                    Cottage = b.Cottage == null ? null : new CottageDto
                    {
                        Id = b.Cottage.Id,
                        ProductId = b.Cottage.ProductId,
                        MaxCapacity = b.Cottage.MaxCapacity,
                        Number = b.Cottage.Number
                    },
                    GrassField = b.GrassField == null ? null : new GrassFieldDto
                    {
                        Id = b.GrassField.Id,
                        ProductId = b.GrassField.ProductId,
                        Size = b.GrassField.Size,
                        Number = b.GrassField.Number
                    },
                    Status = b.Status,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    NumberOfPeople = b.NumberOfPeople,
                    Addons = b.BookingAddons.Select(bt => new AddonDto
                    {
                        Id = bt.Addons.Id,
                        Name = bt.Addons.Name,
                        Price = bt.Addons.Price
                    }).ToList(),
                    Receipt = b.Receipt == null ? null : new ReceiptDto
                    {
                        Id = b.Receipt.Id,
                        BookingId = b.Receipt.BookingId,
                        Date = b.Receipt.Date,
                        TotalPrice = b.Receipt.TotalPrice
                    }
                })
                .ToListAsync();
        }

        public async Task<BookingDto?> GetByIdAsync(int id)
        {
            var b = await _db.Bookings
                .Include(x => x.Product)
                .Include(x => x.Cottage)
                .Include(x => x.GrassField)
                .Include(x => x.BookingAddons).ThenInclude(bt => bt.Addons)
                .Include(x => x.Receipt)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (b is null) return null;

            return new BookingDto
            {
                Id = b.Id,
                CampistId = b.CampistId,
                ProductId = b.ProductId,
                CottageId = b.CottageId,
                GrassFieldId = b.GrassFieldId,
                Product = new ProductDto
                {
                    Id = b.Product.Id,
                    ProductType = b.Product.ProductType,
                    PricePerNight = b.Product.PricePerNight
                },
                Cottage = b.Cottage == null ? null : new CottageDto
                {
                    Id = b.Cottage.Id,
                    ProductId = b.Cottage.ProductId,
                    MaxCapacity = b.Cottage.MaxCapacity,
                    Number = b.Cottage.Number
                },
                GrassField = b.GrassField == null ? null : new GrassFieldDto
                {
                    Id = b.GrassField.Id,
                    ProductId = b.GrassField.ProductId,
                    Size = b.GrassField.Size,
                    Number = b.GrassField.Number
                },
                Status = b.Status,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                NumberOfPeople = b.NumberOfPeople,
                Addons = b.BookingAddons.Select(bt => new AddonDto
                {
                    Id = bt.Addons.Id,
                    Name = bt.Addons.Name,
                    Price = bt.Addons.Price
                }).ToList(),
                Receipt = b.Receipt == null ? null : new ReceiptDto
                {
                    Id = b.Receipt.Id,
                    BookingId = b.Receipt.BookingId,
                    Date = b.Receipt.Date,
                    TotalPrice = b.Receipt.TotalPrice
                }
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
                CampistId = dto.CampistId,
                ProductId = dto.ProductId,
                CottageId = dto.CottageId,
                GrassFieldId = dto.GrassFieldId,
                Status = dto.Status ?? "Aktiv",
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Product = product,
                NumberOfPeople = dto.NumberOfPeople
            };

            if (dto.BookingAddons != null)
            {
                foreach (var ba in dto.BookingAddons)
                {
                    var addonEntity = await _db.Addons.FindAsync(ba.AddonId)
                                      ?? throw new InvalidOperationException($"Addon {ba.AddonId} findes ikke");

                    booking.BookingAddons.Add(new BookingAddon
                    {
                        AddonId = ba.AddonId,
                        Quantity = ba.Quantity,
                        Addons = addonEntity,
                        Booking = booking
                    });
                }
            }

            _db.Bookings.Add(booking);
            await _db.SaveChangesAsync();

            return new BookingDto
            {
                Id = booking.Id,
                CampistId = booking.CampistId,
                ProductId = booking.ProductId,
                CottageId = booking.CottageId,
                GrassFieldId = booking.GrassFieldId,
                Product = new ProductDto
                {
                    Id = product.Id,
                    ProductType = product.ProductType,
                    PricePerNight = product.PricePerNight
                },
                Status = booking.Status,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                NumberOfPeople = booking.NumberOfPeople,
                BookingAddons = booking.BookingAddons.Select(ba => new BookingAddonDto
                {
                    Id = ba.Id,
                    BookingId = ba.BookingId,
                    AddonId = ba.AddonId,
                    Quantity = ba.Quantity
                }).ToList()
            };
        }
    }
}