using TranslasApp.Backend.Emum;

namespace TranslasApp.Backend.Dtos.Supplier
{
    public class UpdateSupplierRequestDto
    {
        public string? Name { get; set; }
        public Priority Priority { get; set; }
    }
}