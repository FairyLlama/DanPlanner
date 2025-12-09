using Danplanner.Shared.Models;

namespace Danplanner.Services
{
    public interface IReceiptService
    {
        // Generere en kvittering i PDF format for en booking
        byte[] GenerateReceipt(BookingDto booking);
    }

}
