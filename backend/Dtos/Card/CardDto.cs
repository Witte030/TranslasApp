using TranslasApp.Backend.Models;
using TranslasApp.Backend.Emum;

namespace TranslasApp.Backend.Dtos.Card
{
    public class CardDto
    {
        public int Id { get; set; }
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
