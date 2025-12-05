using Danplanner.Shared.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Linq;
using System.IO;

namespace Danplanner.Services
{
    public class ReceiptService : IReceiptService
    {
        public byte[] GenerateReceipt(BookingDto booking)
        {
            string produktTypeDa = ToDanish(booking.Product?.ProductType);
            string produktBeskrivelse = produktTypeDa;
            if (booking.CottageId != null && booking.Cottage != null)
                produktBeskrivelse += $" (nummer: {booking.Cottage.Number})";
            else if (booking.GrassFieldId != null && booking.GrassField != null)
                produktBeskrivelse += $" (nummer: {booking.GrassField.Number})";

            decimal? prisPrNat = booking.Cottage?.PricePerNight ?? booking.GrassField?.PricePerNight;
            int antalNaetter = (booking.EndDate - booking.StartDate).Days;
            if (antalNaetter < 1) antalNaetter = 1;

            decimal overnatningSubtotal = prisPrNat.HasValue ? prisPrNat.Value * antalNaetter : 0m;
            decimal tilkoebSubtotal = booking.BookingAddons?.Sum(a => a.Quantity * a.Price) ?? 0m;
            decimal total = booking.TotalPrice > 0 ? booking.TotalPrice : (overnatningSubtotal + tilkoebSubtotal);

            var logoPath = Path.Combine("wwwroot", "img", "danplanner-logo.png");
            var logoBytes = File.Exists(logoPath) ? File.ReadAllBytes(logoPath) : null;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);

                    // Header med logo i kontrolleret størrelse + spacing ned til teksten
                    page.Header().Column(col =>
                    {
                        if (logoBytes != null)
                        {
                            col.Item().Container().PaddingBottom(20)
                                .Width(120)        // maks bredde på logoet
                                .Image(logoBytes);
                        }
                    });

                    // Content
                    page.Content().Column(col =>
                    {
                        col.Spacing(12);

                        col.Item().Text($"Kunde: {booking.User?.Name ?? "Ukendt"}");
                        col.Item().Text($"Email: {booking.User?.Email ?? ""}");
                        col.Item().Text($"Telefon: {booking.User?.Phone ?? ""}");
                        col.Item().Text($"Adresse: {booking.User?.Address ?? ""}");
                        col.Item().Text($"{booking.User?.Country ?? ""} • {booking.User?.Language ?? ""}");

                        col.Item().Container().PaddingVertical(10).LineHorizontal(1).LineColor("#DDDDDD");

                        col.Item().Container().PaddingBottom(5).Text("Overnatning").Bold().FontSize(14);
                        if (prisPrNat is not null)
                        {
                            col.Item().Row(r =>
                            {
                                r.RelativeItem().Text($"{produktBeskrivelse} – {antalNaetter} nætter á {prisPrNat.Value:C}");
                                r.ConstantItem(120).AlignRight().Text($"{overnatningSubtotal:C}");
                            });
                        }

                        col.Item().Container().PaddingVertical(10).LineHorizontal(1).LineColor("#DDDDDD");

                        if (booking.BookingAddons is { Count: > 0 })
                        {
                            col.Item().Container().PaddingBottom(5).Text("Tilkøb").Bold().FontSize(14);

                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.ConstantColumn(80);
                                    columns.ConstantColumn(120);
                                    columns.ConstantColumn(120);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Text("Navn").Bold();
                                    header.Cell().AlignRight().Text("Antal").Bold();
                                    header.Cell().AlignRight().Text("Pris").Bold();
                                    header.Cell().AlignRight().Text("Subtotal").Bold();
                                });

                                foreach (var addon in booking.BookingAddons)
                                {
                                    table.Cell().Text(addon.Addon?.Name ?? "Ukendt");
                                    table.Cell().AlignRight().Text(addon.Quantity.ToString());
                                    table.Cell().AlignRight().Text($"{addon.Price:C}");
                                    table.Cell().AlignRight().Text($"{addon.Quantity * addon.Price:C}");
                                }
                            });

                            col.Item().Row(r =>
                            {
                                r.RelativeItem().Text("");
                                r.ConstantItem(200).AlignRight().Text($"Tilkøb subtotal: {tilkoebSubtotal:C}").Bold();
                            });
                        }

                        col.Item().Container().PaddingVertical(15).LineHorizontal(1).LineColor("#DDDDDD");

                        col.Item().Row(r =>
                        {
                            r.RelativeItem().Text("Total").Bold();
                            r.ConstantItem(120).AlignRight().Text($"{total:C}").Bold();
                        });
                    });

                    page.Footer().AlignCenter().Text("Tak fordi du bruger Danplanner!");
                });
            });

            return document.GeneratePdf();
        }

        private static string ToDanish(ProductType? type)
        {
            return type switch
            {
                ProductType.Cottage => "Hytte",
                ProductType.GrassField => "Græsplads",
                _ => "Produkt"
            };
        }
    }
}