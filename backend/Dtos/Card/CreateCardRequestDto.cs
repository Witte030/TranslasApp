using TranslasApp.Backend.Emum;
using TranslasApp.Backend.Models;

namespace TranslasApp.Backend.Dtos.Card
{
    public class CreateCardRequestDto
    {
    public DateTime Date { get; set; }
    public ReceiverModel? Receiver { get; set; }
    public SupplierModel? Supplier { get; set; }
    public CarrierModel? Carrier { get; set; }
    public int NumberOfCollies { get; set; }
    public int NumberOfPallets { get; set; }
    public int NumberOfBundels { get; set; }
    public bool IsHandled { get; set; }
    public Priority Priority { get; set; }
    }
}
