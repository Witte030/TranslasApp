using TranslasApp.Backend.Dtos.Card;
using TranslasApp.Backend.Models;
using TranslasApp.Backend.Emum;

namespace TranslasApp.Backend.Mappers
{
    public static class CardMapper
    {
        public static CardDto ToCardDto(this CardModel cardModel)
        {
            return new CardDto
            {
                Id = cardModel.Id,
                Date = cardModel.Date,
                Receiver = cardModel.Receiver,
                Supplier = cardModel.Supplier,
                Carrier = cardModel.Carrier,
                NumberOfCollies = cardModel.NumberOfCollies,
                NumberOfPallets = cardModel.NumberOfPallets,
                NumberOfBundels = cardModel.NumberOfBundels,
                IsHandled = cardModel.IsHandled,
                Priority = cardModel.Priority
            };
        }

        public static CardModel ToCardFromCreateDto(this CreateCardRequestDto createCardDto)
        {
            return new CardModel
            {
                Date = createCardDto.Date,
                Receiver = createCardDto.Receiver,
                Supplier = createCardDto.Supplier,
                Carrier = createCardDto.Carrier,
                NumberOfCollies = createCardDto.NumberOfCollies,
                NumberOfPallets = createCardDto.NumberOfPallets,
                NumberOfBundels = createCardDto.NumberOfBundels,
                IsHandled = createCardDto.IsHandled,
                Priority = createCardDto.Priority
            };
        }

        // Fix for the method causing the errors
        public static void UpdateCardFromDto(this CardModel cardModel, UpdateCardRequestDto updateCardDto)
        {
            // Only update fields that are provided in the DTO (not null)
            
            // For nullable integer fields, apply only if they have a value
            if (updateCardDto.NumberOfCollies.HasValue)
                cardModel.NumberOfCollies = updateCardDto.NumberOfCollies.Value;
                
            if (updateCardDto.NumberOfPallets.HasValue)
                cardModel.NumberOfPallets = updateCardDto.NumberOfPallets.Value;
                
            if (updateCardDto.NumberOfBundels.HasValue)
                cardModel.NumberOfBundels = updateCardDto.NumberOfBundels.Value;
                
            if (updateCardDto.IsHandled.HasValue)
                cardModel.IsHandled = updateCardDto.IsHandled.Value;
                
            if (updateCardDto.Priority.HasValue)
                cardModel.Priority = (Emum.Priority)updateCardDto.Priority.Value;
                
            // Handle foreign key updates if needed
            if (updateCardDto.ReceiverId.HasValue)
            if (cardModel.Receiver != null && updateCardDto.ReceiverId.HasValue)
                cardModel.Receiver.Id = updateCardDto.ReceiverId.Value;
            
            if (cardModel.Supplier != null && updateCardDto.SupplierId.HasValue)
                cardModel.Supplier.Id = updateCardDto.SupplierId.Value;
            
            if (cardModel.Carrier != null && updateCardDto.CarrierId.HasValue)
                cardModel.Carrier.Id = updateCardDto.CarrierId.Value;
        }
    }
}