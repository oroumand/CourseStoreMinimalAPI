using CourseStoreMinimalAPI.DAL;
using CourseStoreMinimalAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseStoreMinimalAPI.ApplicationServices;

public class CategoryService(CourseDbContext ctx)
{
    public async Task<List<Category>> GetCategoryAsync()
    {

        return await ctx.Categories.OrderByDescending(c=>c.Name).ThenByDescending(c=>c.Id).AsNoTrackingWithIdentityResolution().ToListAsync();
    }

    public async Task<Category?> GetCategoryAsync(int id)
    {
        return await ctx.Categories.FirstOrDefaultAsync(ctx => ctx.Id == id);
    }
    public async Task<int> InsertAsync(Category category)
    {
        ctx.Categories.Add(category);
        await ctx.SaveChangesAsync();
        return category.Id;
    }
}
