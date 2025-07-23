using Microsoft.EntityFrameworkCore;
using PetShop.Application.Interfaces;
using PetShop.Core.Contexts;
using PetShop.Domain.Entities;

namespace PetShop.Core.Repositories ;

    public class PetRepository(AppDbContext context):IPetRepository
    {
        public async Task<Pet?> GetByIdAsync(Guid id)
        {
            return await context.Pets.FindAsync(id);
        }

        public async Task<List<Pet>> GetAllAsync()
        {
            return await context.Pets.AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(Pet pet)
        {
            pet.id = Guid.NewGuid();
            await context.Pets.AddAsync(pet);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Pet pet)
        {
            context.Pets.Update(pet);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var pet = await context.Pets.FindAsync(id);
            if (pet != null) 
                context.Pets.Remove(pet);
            await context.SaveChangesAsync();
        }
    }