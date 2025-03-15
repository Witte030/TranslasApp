using Microsoft.EntityFrameworkCore;
using TranslasApp.Backend.Data;
using TranslasApp.Backend.Dtos.Carrier;
using TranslasApp.Backend.Interfaces;
using TranslasApp.Backend.Models;
namespace TranslasApp.Backend.Repoistory
{
    public class CarrierRepository : ICarrierRepository
    {

        private readonly ApplicationDBContext _context;

        public CarrierRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<CarrierModel> CreateAsync(CarrierModel carrierModel)
        {
            await _context.Carriers.AddAsync(carrierModel);
            await _context.SaveChangesAsync();
            return carrierModel;
        }

        public async Task<List<CarrierModel>> GetAllAsync()
        {
            return await _context.Carriers.ToListAsync();
        }

        public async Task<CarrierModel?> GetByIdAsync(int id)
        {
            return await _context.Carriers.FindAsync(id);
        }

        public async Task<CarrierModel?> UpdateAsync(int id, UpdateCarrierRequestDto updateCarrierDto)
        {
            var existingCarrier = await _context.Carriers.FirstOrDefaultAsync(c => c.Id == id);
            if (existingCarrier == null)
            {
                return null;
            }
            existingCarrier.Name = updateCarrierDto.Name;

            await _context.SaveChangesAsync();
            return existingCarrier;

        }
    }
}

