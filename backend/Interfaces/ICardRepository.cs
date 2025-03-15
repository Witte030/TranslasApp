using TranslasApp.Backend.Dtos.Card;
using TranslasApp.Backend.Models;

namespace TranslasApp.Backend.Interfaces
{
    public interface ICardRepository
    {
        Task<List<CardModel>> GetAllAsync();
        Task<CardModel?> GetByIdAsync(int id);
        Task<CardModel> CreateAsync(CardModel cardModel);
        Task<CardModel?> UpdateAsync(int id, UpdateCardRequestDto updateCardDto);
        Task<Models.CardModel?> ToggleHandledAsync(int id, bool isHandled);
    }
}
