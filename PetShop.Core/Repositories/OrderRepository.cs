using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PetShop.Application.Dtos;
using PetShop.Application.Interfaces;
using PetShop.Core.Contexts;
using PetShop.Domain.Entities;
using PetShop.Domain.Enums;

namespace PetShop.Core.Repositories ;

    public class OrderRepository(AppDbContext context, IMapper mapper):IOrderRepository
    {
        private const string Prefix = "ORD";
        private const string Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            
        public async Task<Order?> GetByIdAsync(Guid id)
        {

            var dbOrder = await context.Orders
                .Where(o => o.Id == id)
                .Select(o => new Order
                {
                    Id = o.Id,
                    OrderStatus = o.OrderStatus,
                    PickupDate = o.PickupDate,
                    ActualCost = o.ActualCost,
                    CustomerId = o.CustomerId,
                    OrderNumber = o.OrderNumber,
                    OrderItems = o.OrderItems.Select(i => new OrderItem
                    {
                        Name = i.Name,
                        Price = i.Price,
                        PetId = i.PetId,
                        OrderId = i.OrderId,
                    }).ToList()
                })
                .FirstOrDefaultAsync();
                
            
            // var order = mapper.Map<OrderDto>(dbOrder);
            
            //
            // if (order == null) return null;
            //
            // //calculate Estimated Cost  if the Order is not delivered
            // if (order.OrderStatus != EnumOrderStatus.Delivered)
            // {
            //     order.ActualCost = order.OrderItems.Sum(i => i.Price);
            // }

            return dbOrder;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await context.Orders.AsNoTracking().ToListAsync();

        }

        private string GenerateUniqueOrderNumber()
        {
            var length = 8;
            var sb = new StringBuilder();
            using var rng = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[1];
            for (int i = 0; i < length; i++)
            {
                rng.GetBytes(bytes);
                int index = bytes[0] % Chars.Length; // Ensure index is within bounds of Chars
                sb.Append(Chars[index]);
            }
            return Prefix + sb.ToString().ToUpper();
        }
        public async Task AddAsync(Order order)
        {
            order.Id = Guid.NewGuid();
            order.OrderStatus = EnumOrderStatus.Open;
            order.OrderDate = DateTime.Now;
            order.CustomerId = order.CustomerId;
            
            order.OrderNumber = GenerateUniqueOrderNumber();

            foreach (var orderItem in order.OrderItems)
            {
                orderItem.Id = Guid.NewGuid();
                orderItem.OrderId = order.Id;
                orderItem.Quantity = 1;
            }
            
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();
        }
        
        public async Task UpdateOrderItemsAsync(UpdateOrderItemsDto dto)
        {
            var existingOrder = await context.Orders.Include(order => order.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == dto.OrderId);
          
            if(existingOrder == null)
                throw new Exception("Order not found");
            
            if(existingOrder.OrderStatus != EnumOrderStatus.Open)
                throw new Exception("Cannot Update Order Items when Order Status is ot Open");

            var orderItems = dto.OrderItems;
            if (orderItems == null)
            {
                existingOrder.OrderItems.Clear();
            }

            if (orderItems != null)
            {
                var dtoItemsByPetId = orderItems
                    .ToDictionary(i => i.PetId);
            
                
                // Remove items not in the new list
                var existingItems = existingOrder.OrderItems.ToList();
                foreach (var existingItem in existingItems)
                {
                    if (!dtoItemsByPetId.ContainsKey(existingItem.PetId))
                    {
                        existingOrder.OrderItems.Remove(existingItem);
                    }
                }
                
                // Update existing and add new items
                foreach (var dtoItem in dto.OrderItems)
                {
                    var existingItem = existingOrder.OrderItems.FirstOrDefault(i => i.PetId == dtoItem.PetId);
                    if (existingItem != null)
                    {
                        existingItem.Price = dtoItem.Price;
                    }
                    else
                    {
                        // Add new item
                        existingOrder.OrderItems.Add(new OrderItem
                        {
                            PetId = dtoItem.PetId,
                            Quantity =1,
                            Name = dtoItem.Name,
                            Price = dtoItem.Price,
                            OrderId = existingOrder.Id
                        });
                    }
                }
            }
            
            context.Orders.Update(existingOrder);
            await context.SaveChangesAsync();
        }
        public async Task UpdateAsync(UpdateOrderDto dto)
        {
            var existingOrder = await context.Orders.Include(order => order.OrderItems).FirstOrDefaultAsync(o => o.Id == dto.Id);
          
            if(existingOrder == null)
                throw new Exception("Order not found");
            
            // update order header information
            switch (existingOrder.OrderStatus)
            {
                case EnumOrderStatus.Open:
                    existingOrder.PickupDate = dto.PickupDate ?? existingOrder.PickupDate;
                    existingOrder.CustomerId = dto.CustomerId ?? existingOrder.CustomerId;
                    existingOrder.OrderStatus = dto.OrderStatus is EnumOrderStatus.Processing ? EnumOrderStatus.Processing : existingOrder.OrderStatus;
                    break;
                
                case EnumOrderStatus.Processing:
                    existingOrder.PickupDate = dto.PickupDate ?? existingOrder.PickupDate;
                    break;
                
                case EnumOrderStatus.Delivered:
                    throw new InvalidOperationException("Delivered order not supported");
                
                default:
                    throw new Exception("Order Status not supported");
            }
            
            
            if (dto.OrderStatus == EnumOrderStatus.Delivered)
            {
                existingOrder.OrderStatus = EnumOrderStatus.Delivered;
                existingOrder.ActualCost = existingOrder.OrderItems.Sum(i => i.Price);
            }

            context.Orders.Update(existingOrder);
            await context.SaveChangesAsync();
          
        }
        

        public async Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }