using Microsoft.EntityFrameworkCore;
using PetShop.Application.Interfaces;
using PetShop.Core.Contexts;
using PetShop.Domain.Entities;
using PetShop.Domain.Enums;

namespace PetShop.Core.Repositories ;

    public class CustomerRepository(AppDbContext context):ICustomerRepository
    {
        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            var customer = await context.Customers
                .Include(c => c.Orders).ThenInclude(order => order.OrderItems)
                .FirstOrDefaultAsync(c=>c.Id==id);
            if(customer == null) return null;

            // var actualPaymentDue = customer.Orders.Where(o => o.OrderStatus == EnumOrderStatus.Delivered)
            //     .Sum(o => o.ActualCost);
            //
            // var estimatedPayment = customer.Orders.Where(o => o.OrderStatus != EnumOrderStatus.Delivered)
            //     .Sum(o => o.OrderItems.Sum(oi=>oi.Price));
            //
            return customer;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await context.Customers.AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(Customer customer)
        {
            customer.Id = Guid.NewGuid();
            await context.Customers.AddAsync(customer);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer customer)
        {
            context.Customers.Update(customer);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var customer = await context.Customers.FindAsync(id);
            if (customer != null) 
            context.Customers.Remove(customer);
            await context.SaveChangesAsync();
        }
    }