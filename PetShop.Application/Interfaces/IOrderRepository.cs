using PetShop.Application.Dtos;
using PetShop.Domain.Entities;

namespace PetShop.Application.Interfaces ;

    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid id);
        Task<List<Order>> GetAllAsync();
        Task AddAsync(Order order);
        Task UpdateAsync(UpdateOrderDto order);

        Task UpdateOrderItemsAsync(UpdateOrderItemsDto dto);
        Task DeleteAsync(Guid id);
    }