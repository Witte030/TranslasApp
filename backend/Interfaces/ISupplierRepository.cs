using TranslasApp.Backend.Dtos.Supplier;
using TranslasApp.Backend.Models;

namespace TranslasApp.Backend.Interfaces
{
    public interface ISupplierRepository
    {
        Task<List<SupplierModel>> GetAllAsync();
        Task<SupplierModel?> GetByIdAsync(int id);
        Task<SupplierModel> CreateAsync(SupplierModel supplierModel);
        Task<SupplierModel?> UpdateAsync(int id, UpdateSupplierRequestDto updateSupplierDto);
    }
}