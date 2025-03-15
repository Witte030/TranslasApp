using TranslasApp.Backend.Emum;

namespace TranslasApp.Backend.Dtos.Supplier
{
    public class SupplierDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Priority Priority { get; set; }
    }
}
