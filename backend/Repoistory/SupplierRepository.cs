using Microsoft.EntityFrameworkCore;
using TranslasApp.Backend.Data;
using TranslasApp.Backend.Dtos.Supplier;
using TranslasApp.Backend.Interfaces;
using TranslasApp.Backend.Models;

namespace TranslasApp.Backend.Repoistory
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly ApplicationDBContext _context;

        public SupplierRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<SupplierModel> CreateAsync(SupplierModel supplierModel)
        {
            await _context.Suppliers.AddAsync(supplierModel);
            await _context.SaveChangesAsync();
            return supplierModel;
        }

        public async Task<List<SupplierModel>> GetAllAsync()
        {
            return await _context.Suppliers.ToListAsync();
        }

        public async Task<SupplierModel?> GetByIdAsync(int id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public async Task<SupplierModel?> UpdateAsync(int id, UpdateSupplierRequestDto updateSupplierDto)
        {
            var existingSupplier = await _context.Suppliers.FirstOrDefaultAsync(c => c.Id == id);
            if (existingSupplier == null)
            {
                return null;
            }
            existingSupplier.Name = updateSupplierDto.Name;

            await _context.SaveChangesAsync();
            return existingSupplier;
        }

       
    }
}