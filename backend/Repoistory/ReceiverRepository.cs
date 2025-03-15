using Microsoft.EntityFrameworkCore;
using TranslasApp.Backend.Data;
using TranslasApp.Backend.Dtos.Receiver;
using TranslasApp.Backend.Interfaces;
using TranslasApp.Backend.Models;

namespace TranslasApp.Backend.Repoistory
{
    public class ReceiverRepository : IReceiverRepository
    {
        private readonly ApplicationDBContext _context;

        public ReceiverRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<ReceiverModel> CreateAsync(ReceiverModel receiverModel)
        {
            await _context.Receivers.AddAsync(receiverModel);
            await _context.SaveChangesAsync();
            return receiverModel;
        }

        public async Task<List<ReceiverModel>> GetAllAsync()
        {
            return await _context.Receivers.ToListAsync();
        }

        public async Task<ReceiverModel?> GetByIdAsync(int id)
        {
            return await _context.Receivers.FindAsync(id);
        }

        public async Task<ReceiverModel?> UpdateAsync(int id, UpdateReceiverRequestDto updateReceiverDto)
        {
            var existingReceiver = await _context.Receivers.FirstOrDefaultAsync(c => c.Id == id);
            if (existingReceiver == null)
            {
                return null;
            }
            existingReceiver.Name = updateReceiverDto.Name;

            await _context.SaveChangesAsync();
            return existingReceiver;
        }
    }
}