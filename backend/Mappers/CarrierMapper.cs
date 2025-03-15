using TranslasApp.Backend.Dtos.Carrier;
using TranslasApp.Backend.Models;

namespace TranslasApp.Backend.Mappers
{
    public static class CarrierMapper
    {
        public static CarrierDto ToCarrierDto(this CarrierModel model)
        {
            return new CarrierDto
            {
                Id = model.Id,
                Name = model.Name
            };
        }

        public static CarrierModel ToCarrierFromCreateDto(this CreateCarrierRequestDto dto)
        {
            return new CarrierModel
            {
                Name = dto.Name
            };
        }
    }
}

