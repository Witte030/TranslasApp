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
            // Handle reference entities
            if (cardModel.Receiver != null)
            {
                cardModel.Receiver = null;
            }
            
            if (cardModel.Supplier != null)
            {
                cardModel.Supplier = null;
            }
            
            if (cardModel.Carrier != null)
            {
                cardModel.Carrier = null;
            }
            
            await _context.Cards.AddAsync(cardModel);
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
