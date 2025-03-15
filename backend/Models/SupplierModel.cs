using TranslasApp.Backend.Emum;

namespace TranslasApp.Backend.Models
{
    public class SupplierModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public Priority Priority { get; set; } = Priority.Low;
    }
}
