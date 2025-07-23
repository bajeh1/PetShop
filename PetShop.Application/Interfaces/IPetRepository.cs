using PetShop.Domain.Entities;

namespace PetShop.Application.Interfaces ;

    public interface IPetRepository
    {
        Task<Pet?> GetByIdAsync(Guid id);
        Task<List<Pet>> GetAllAsync();
        Task AddAsync(Pet pet);
        Task UpdateAsync(Pet pet);
        Task DeleteAsync(Guid id);
    }