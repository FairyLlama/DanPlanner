using Danplanner.Shared.Models;

namespace Danplanner.Services
{
    public interface IReceiptService
    {
        byte[] GenerateReceipt(BookingDto booking);
    }

}
