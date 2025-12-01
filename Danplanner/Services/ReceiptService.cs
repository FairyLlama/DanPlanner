using Danplanner.Shared.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Linq;

namespace Danplanner.Services
{
    public class ReceiptService : IReceiptService
    {
        public byte[] GenerateReceipt(BookingDto booking)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(50);

                    page.Header()
                        .Text("Danplanner Kvittering")
                        .FontSize(20)
                        .Bold();

                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Booking ID: {booking.Id}");
                        col.Item().Text($"Kunde: {booking.User?.Name ?? "Ukendt"}");

                        // Produkt + nummer (hytte eller græsplads)
                        string produktBeskrivelse = booking.Product?.ProductType.ToString() ?? "Ukendt";
                        if (booking.CottageId != null && booking.Cottage != null)
                            produktBeskrivelse += $" #{booking.Cottage.Number}";
                        else if (booking.GrassFieldId != null && booking.GrassField != null)
                            produktBeskrivelse += $" #{booking.GrassField.Number}";

                        col.Item().Text($"Produkt: {produktBeskrivelse}");

                        col.Item().Text($"Periode: {booking.StartDate:dd-MM-yyyy} til {booking.EndDate:dd-MM-yyyy}");
                        col.Item().Text($"Antal personer: {booking.NumberOfPeople}");

                        // Tilkøb
                        if (booking.BookingAddons != null && booking.BookingAddons.Any())
                        {
                            col.Item().Text("Tilkøb:").Bold();
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(200); // navn
                                    columns.RelativeColumn();    // pris
                                });

                                foreach (var addon in booking.BookingAddons)
                                {
                                    table.Cell().Text(addon.Addon?.Name ?? "Ukendt");
                                    table.Cell().Text($"{addon.Price:C}");
                                }
                            });
                        }

                        col.Item().Text($"Totalpris: {booking.TotalPrice:C}").Bold();
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text("Tak fordi du bruger Danplanner!");
                });
            });

            return document.GeneratePdf();
        }
    }
}