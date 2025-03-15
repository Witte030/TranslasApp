using TranslasApp.Backend.Dtos.Carrier;
using TranslasApp.Backend.Models;

namespace TranslasApp.Backend.Interfaces
{
    public interface ICarrierRepository
    {
        Task<List<CarrierModel>> GetAllAsync();
        Task<CarrierModel?> GetByIdAsync(int id);
        Task<CarrierModel> CreateAsync(CarrierModel carrierModel);
        Task<CarrierModel?> UpdateAsync(int id, UpdateCarrierRequestDto updateCarrierDto);
    }
}

