using CourseStoreMinimalAPI.Dal;
using CourseStoreMinimalAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseStoreMinimalAPI.Services
{
    public class CategoryServices
    {
        private readonly CourseDbContext _courseDb;

        public CategoryServices(CourseDbContext courseDb)
        {
            _courseDb = courseDb;
        }
        public async Task<int> Create(Category category)
        {
            _courseDb.Categories.Add(category);
            await _courseDb.SaveChangesAsync();
            return category.Id;
        }

        public async Task Delete(int id)
        {
            await _courseDb.Categories.Where(c => c.Id == id).ExecuteDeleteAsync();
        }

        public async Task<bool> Exists(int id)
        {
            return await _courseDb.Categories.AnyAsync(c => c.Id == id);
        }

        public async Task<List<Category>> GetAll()
        {
            return await _courseDb.Categories.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<Category?> GetById(int id)
        {
            return _courseDb.Categories.FirstOrDefault(c => c.Id == id);
        }

        public async Task Update(Category category)
        {
            _courseDb.Update(category);
            await _courseDb.SaveChangesAsync();
        }
    }
}
