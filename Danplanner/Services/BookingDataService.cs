using Danplanner.Data;
using Danplanner.Data.Entities;
using Danplanner.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Danplanner.Services
{
    public class BookingDataService(AppDbContext db) : IBookingDataService
    {
        private readonly AppDbContext _db = db;

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
                    UserId = b.UserId,
                    ProductId = b.ProductId,
                    CottageId = b.CottageId,
                    GrassFieldId = b.GrassFieldId,
                    Product = new ProductDto
                    {
                        Id = b.Product.Id,
                        ProductType = (ProductType)b.Product.ProductType
                    },
                    Cottage = b.Cottage == null ? null : new CottageDto
                    {
                        Id = b.Cottage.Id,
                        ProductId = b.Cottage.ProductId,
                        MaxCapacity = b.Cottage.MaxCapacity,
                        Number = b.Cottage.Number,
                        PricePerNight = b.Cottage.PricePerNight
                    },
                    GrassField = b.GrassField == null ? null : new GrassFieldDto
                    {
                        Id = b.GrassField.Id,
                        ProductId = b.GrassField.ProductId,
                        Size = b.GrassField.Size,
                        Number = b.GrassField.Number,
                        PricePerNight = b.GrassField.PricePerNight
                    },
                    Status = b.Status,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    NumberOfPeople = b.NumberOfPeople,
                    TotalPrice = b.TotalPrice,
                    BookingAddons = b.BookingAddons.Select(bt => new BookingAddonDto
                    {
                        Id = bt.Id,
                        BookingId = bt.BookingId,
                        AddonId = bt.AddonId,
                        Quantity = bt.Quantity,
                        Price = bt.Price
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
                UserId = b.UserId,
                ProductId = b.ProductId,
                CottageId = b.CottageId,
                GrassFieldId = b.GrassFieldId,
                Product = new ProductDto
                {
                    Id = b.Product.Id,
                    ProductType = (ProductType)b.Product.ProductType
                },
                Cottage = b.Cottage == null ? null : new CottageDto
                {
                    Id = b.Cottage.Id,
                    ProductId = b.Cottage.ProductId,
                    MaxCapacity = b.Cottage.MaxCapacity,
                    Number = b.Cottage.Number,
                    PricePerNight = b.Cottage.PricePerNight
                },
                GrassField = b.GrassField == null ? null : new GrassFieldDto
                {
                    Id = b.GrassField.Id,
                    ProductId = b.GrassField.ProductId,
                    Size = b.GrassField.Size,
                    Number = b.GrassField.Number,
                    PricePerNight = b.GrassField.PricePerNight
                },
                Status = b.Status,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                NumberOfPeople = b.NumberOfPeople,
                TotalPrice = b.TotalPrice,
                BookingAddons = b.BookingAddons.Select(bt => new BookingAddonDto
                {
                    Id = bt.Id,
                    BookingId = bt.BookingId,
                    AddonId = bt.AddonId,
                    Quantity = bt.Quantity,
                    Price = bt.Price
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
                UserId = dto.UserId ?? 0,
                ProductId = dto.ProductId,
                CottageId = dto.CottageId,
                GrassFieldId = dto.GrassFieldId,
                Status = dto.Status ?? "Aktiv",
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Product = product,
                NumberOfPeople = dto.NumberOfPeople,
                BookingAddons = new List<BookingAddon>()   // 👈 sikrer at listen er klar
            };

            decimal basePrice = 0m;
            if (dto.CottageId.HasValue)
            {
                var cottage = await _db.Cottages.FindAsync(dto.CottageId.Value)
                              ?? throw new InvalidOperationException("Cottage findes ikke");
                basePrice = cottage.PricePerNight * days;
            }
            else if (dto.GrassFieldId.HasValue)
            {
                var grass = await _db.GrassFields.FindAsync(dto.GrassFieldId.Value)
                             ?? throw new InvalidOperationException("GrassField findes ikke");
                basePrice = grass.PricePerNight * days;
            }

            var addonsPrice = 0m;
            if (dto.BookingAddons != null && dto.BookingAddons.Any())
            {
                foreach (var ba in dto.BookingAddons)
                {
                    var addonEntity = await _db.Addons.FindAsync(ba.AddonId)
                                      ?? throw new InvalidOperationException($"Addon {ba.AddonId} findes ikke");

                    var addonTotal = addonEntity.Price * ba.Quantity;
                    addonsPrice += addonTotal;

                    booking.BookingAddons.Add(new BookingAddon
                    {
                        BookingId = booking.Id,   // 👈 sæt FK eksplicit
                        AddonId = ba.AddonId,
                        Quantity = ba.Quantity,
                        Price = addonEntity.Price,
                        Addons = addonEntity
                    });
                }
            }

            // Rabat for senior og studerende
            if (dto.DiscountType!.Equals("senior"))
            {
                basePrice -= 15 / 100 * basePrice;
            }
            else if (dto.DiscountType!.Equals("student"))
            {
                basePrice -= 10 / 100 * basePrice;
            }

            booking.TotalPrice = basePrice + addonsPrice;

            _db.Bookings.Add(booking);
            await _db.SaveChangesAsync();

            return new BookingDto
            {
                Id = booking.Id,
                UserId = booking.UserId,
                ProductId = booking.ProductId,
                CottageId = booking.CottageId,
                GrassFieldId = booking.GrassFieldId,
                Product = new ProductDto
                {
                    Id = product.Id,
                    ProductType = (ProductType)product.ProductType
                },
                Status = booking.Status,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                NumberOfPeople = booking.NumberOfPeople,
                TotalPrice = booking.TotalPrice,
                BookingAddons = booking.BookingAddons.Select(ba => new BookingAddonDto
                {
                    Id = ba.Id,
                    BookingId = ba.BookingId,
                    AddonId = ba.AddonId,
                    Quantity = ba.Quantity,
                    Price = ba.Price
                }).ToList()
            };
        }


        public async Task<BookingDto?> UpdateAsync(BookingDto dto)
        {
            var booking = await _db.Bookings
                .Include(b => b.BookingAddons)
                .FirstOrDefaultAsync(b => b.Id == dto.Id);

            if (booking == null) return null;

            // Opdater felter
            booking.ProductId = dto.ProductId;
            booking.CottageId = dto.CottageId;
            booking.GrassFieldId = dto.GrassFieldId;
            booking.StartDate = dto.StartDate;
            booking.EndDate = dto.EndDate;
            booking.NumberOfPeople = dto.NumberOfPeople;
            booking.Status = dto.Status ?? booking.Status;

            // Opdater tilkøb
            booking.BookingAddons.Clear();
            if (dto.BookingAddons != null && dto.BookingAddons.Any())
            {
                foreach (var ba in dto.BookingAddons)
                {
                    var addonEntity = await _db.Addons.FindAsync(ba.AddonId)
                                      ?? throw new InvalidOperationException($"Addon {ba.AddonId} findes ikke");

                    booking.BookingAddons.Add(new BookingAddon
                    {
                        BookingId = booking.Id,
                        AddonId = ba.AddonId,
                        Quantity = ba.Quantity,
                        Price = addonEntity.Price,
                        Addons = addonEntity
                    });
                }
            }

            // Beregn pris igen
            var days = (dto.EndDate - dto.StartDate).Days;
            if (days <= 0) days = 1;

            decimal basePrice = 0m;
            if (dto.CottageId.HasValue)
            {
                var cottage = await _db.Cottages.FindAsync(dto.CottageId.Value);
                if (cottage != null) basePrice = cottage.PricePerNight * days;
            }
            else if (dto.GrassFieldId.HasValue)
            {
                var grass = await _db.GrassFields.FindAsync(dto.GrassFieldId.Value);
                if (grass != null) basePrice = grass.PricePerNight * days;
            }

            decimal addonsPrice = booking.BookingAddons.Sum(a => a.Price * a.Quantity);
            booking.TotalPrice = basePrice + addonsPrice;

            await _db.SaveChangesAsync();

            // Returnér DTO
            return new BookingDto
            {
                Id = booking.Id,
                UserId = booking.UserId,
                ProductId = booking.ProductId,
                CottageId = booking.CottageId,
                GrassFieldId = booking.GrassFieldId,
                Status = booking.Status,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                NumberOfPeople = booking.NumberOfPeople,
                TotalPrice = booking.TotalPrice,
                BookingAddons = booking.BookingAddons.Select(ba => new BookingAddonDto
                {
                    Id = ba.Id,
                    BookingId = ba.BookingId,
                    AddonId = ba.AddonId,
                    Quantity = ba.Quantity,
                    Price = ba.Price
                }).ToList()
            };
        }

        public async Task<bool> ConfirmAsync(int bookingId, int userId)
        {
            var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId);
            if (booking == null) return false;

            booking.UserId = userId;
            booking.Status = "Aktiv";

            await _db.SaveChangesAsync();
            return true;
        }


        public async Task DeleteAsync(int id)
        {
            var booking = await _db.Bookings.FindAsync(id);
            if (booking == null)
                throw new KeyNotFoundException("Booking ikke fundet");
            _db.Bookings.Remove(booking);
            await _db.SaveChangesAsync();
        }
    }
}