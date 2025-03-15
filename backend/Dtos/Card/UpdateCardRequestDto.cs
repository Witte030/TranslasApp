using TranslasApp.Backend.Emum;
using TranslasApp.Backend.Models;

namespace TranslasApp.Backend.Dtos.Card
{
    public class UpdateCardRequestDto
    {
        
        public int? NumberOfCollies { get; set; }
        public int? NumberOfPallets { get; set; }
        public int? NumberOfBundels { get; set; }
        public bool? IsHandled { get; set; }
        public int? Priority { get; set; }
        public int? ReceiverId { get; set; }
        public int? SupplierId { get; set; }
        public int? CarrierId { get; set; }
    }
}
