using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TranslasApp.Backend.Data;
using TranslasApp.Backend.Dtos.Card;
using TranslasApp.Backend.Interfaces;
using TranslasApp.Backend.Mappers;
using TranslasApp.Backend.Models;

namespace TranslasApp.Backend.Repoistory
{
    public class CardRepository : ICardRepository
    {
        private readonly ApplicationDBContext _context;

        public CardRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<CardModel> CreateAsync(CardModel cardModel)
{
    // Attach entities properly to avoid tracking issues
    if (cardModel.Receiver != null)
    {
        _context.Entry(cardModel.Receiver).State = EntityState.Unchanged;
    }
    
    if (cardModel.Supplier != null)
    {
        _context.Entry(cardModel.Supplier).State = EntityState.Unchanged;
    }
    
    if (cardModel.Carrier != null)
    {
        _context.Entry(cardModel.Carrier).State = EntityState.Unchanged;
    }
    
    _context.Cards.Add(cardModel);
    await _context.SaveChangesAsync();
    return cardModel;
}

        public async Task<List<CardModel>> GetAllAsync()
        {
            return await _context.Cards
                .Include(c => c.Receiver)
                .Include(c => c.Supplier)
                .Include(c => c.Carrier)
                .ToListAsync();
        }

        public async Task<CardModel?> GetByIdAsync(int id)
        {
            return await _context.Cards
                .Include(c => c.Receiver)
                .Include(c => c.Supplier)
                .Include(c => c.Carrier)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CardModel?> UpdateAsync(int id, UpdateCardRequestDto updateCardDto)
        {
            var card = await _context.Cards
                .Include(c => c.Receiver)
                .Include(c => c.Supplier)
                .Include(c => c.Carrier)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (card == null)
            {
                return null;
            }

            // Use the mapper to update the card
            card.UpdateCardFromDto(updateCardDto);
            
            await _context.SaveChangesAsync();
            return card;
        }

        public async Task<CardModel?> ToggleHandledAsync(int id, bool isHandled)
        {
            var card = await _context.Cards
                .Include(c => c.Receiver)
                .Include(c => c.Supplier)
                .Include(c => c.Carrier)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (card == null)
            {
                return null;
            }

            card.IsHandled = isHandled;
            await _context.SaveChangesAsync();
            return card;
        }

       
    }
}
