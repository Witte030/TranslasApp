using TranslasApp.Backend.Emum;

namespace TranslasApp.Backend.Dtos.Supplier
{
    public class CreateSupplierRequestDto
    {
        public string? Name { get; set; }
        public Priority Priority { get; set; } = Priority.Low;
    }
}