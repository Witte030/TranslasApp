using TranslasApp.Backend.Dtos.Receiver;
using TranslasApp.Backend.Models;

namespace TranslasApp.Backend.Mappers
{
    public static class ReceiverMapper
    {
        public static ReceiverDto ToReceiverDto(this ReceiverModel model)
        {
            return new ReceiverDto
            {
                Id = model.Id,
                Name = model.Name
            };
        }

        public static ReceiverModel ToReceiverFromCreateDto(this CreateReceiverRequestDto dto)
        {
            return new ReceiverModel
            {
                Name = dto.Name
            };
        }
    }
}
