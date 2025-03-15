
using TranslasApp.Backend.Dtos.Supplier;
using TranslasApp.Backend.Models;


namespace TranslasApp.Backend.Mappers
{
    public static class SupplierMapper
    {
        public static SupplierDto ToSupplierDto(this SupplierModel model)
        {
            return new SupplierDto
            {
                Id = model.Id,
                Name = model.Name,
                Priority = model.Priority

            };
        }

        public static SupplierModel ToSupplierFromCreateDto(this CreateSupplierRequestDto dto)
        {
            return new SupplierModel
            {
                Name = dto.Name,
                Priority = dto.Priority
            };
        }
    }
}

