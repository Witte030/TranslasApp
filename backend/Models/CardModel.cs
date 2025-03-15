using TranslasApp.Backend.Emum;

namespace TranslasApp.Backend.Models
{
    public class CardModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public ReceiverModel? Receiver { get; set; }
        public SupplierModel? Supplier { get; set; }
        public CarrierModel? Carrier { get; set; }
        public int NumberOfCollies { get; set; }
        public int NumberOfPallets { get; set; }
        public int NumberOfBundels { get; set; }
        public bool IsHandled { get; set; }
        public Priority Priority { get; set; } = Priority.Low;
    }
}
