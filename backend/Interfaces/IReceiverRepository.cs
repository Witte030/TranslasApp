using TranslasApp.Backend.Dtos.Receiver;
using TranslasApp.Backend.Models;

namespace TranslasApp.Backend.Interfaces
{
    public interface IReceiverRepository
    {
        Task<List<ReceiverModel>> GetAllAsync();
        Task<ReceiverModel?> GetByIdAsync(int id);
        Task<ReceiverModel> CreateAsync(ReceiverModel receiverModel);
        Task<ReceiverModel?> UpdateAsync(int id, UpdateReceiverRequestDto updateReceiverDto);
    }
}