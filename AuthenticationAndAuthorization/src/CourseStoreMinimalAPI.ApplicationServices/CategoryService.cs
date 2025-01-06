using CourseStoreMinimalAPI.DAL;
using CourseStoreMinimalAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseStoreMinimalAPI.ApplicationServices;

public class CategoryService(CourseDbContext ctx)
{
    public async Task<List<Category>> GetCategoryAsync()
    {

        return await ctx.Categories.OrderByDescending(c => c.Name).ThenByDescending(c => c.Id).AsNoTrackingWithIdentityResolution().ToListAsync();
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

    //public async Task Update(Category category)
    //{
    //    var categoryForUpdate = ctx.Categories.FirstOrDefault(c => c.Id == category.Id);
    //    var detachedCategoryState = ctx.Entry(category); 
    //    var state = ctx.Entry(categoryForUpdate).State;
    //    categoryForUpdate.Name = category.Name;
    //    var stateAfter = ctx.Entry(categoryForUpdate).State;
    //    await ctx.SaveChangesAsync();
    //    var stateAfterSaveChage = ctx.Entry(categoryForUpdate).State;
    //}

    //public async Task Update(Category category)
    //{
    //    var categoryForUpdate = ctx.Categories.FirstOrDefault(c => c.Id == category.Id);
    //    if (categoryForUpdate != null)
    //    {
    //        var detachedCategoryState = ctx.Entry(category);
    //        var state = ctx.Entry(categoryForUpdate).State;
    //        categoryForUpdate.Name = category.Name;
    //        var stateAfter = ctx.Entry(categoryForUpdate).State;
    //        await ctx.SaveChangesAsync();
    //        var stateAfterSaveChage = ctx.Entry(categoryForUpdate).State;
    //    }

    //}
    public async Task<bool> Exists(int id)
    {
        return await ctx.Categories.AnyAsync(c => c.Id == id);
    }
    public async Task Update(Category category)
    {
        ctx.Update(category);
        await ctx.SaveChangesAsync();
    }

    //public async Task Delete(int id)
    //{

    //    var entityForSave = await GetCategoryAsync(id);

    //    ctx.Categories.Remove(entityForSave);
    //    var state = ctx.Entry(entityForSave).State;
    //    await ctx.SaveChangesAsync();
    //}
    public async Task Delete(int id)
    {
        var entityForSave = new Category { Id = id };
        ctx.Categories.Remove(entityForSave);
        await ctx.SaveChangesAsync();
    }

    //public async Task Delete(int id)
    //{

    //    await ctx.Categories.Where(c => c.Id == id).ExecuteDeleteAsync();
    //}

    public async Task<bool> IsRepeated(int id, string name)
    {
        return await ctx.Categories.AnyAsync(c=> c.Id!= id && c.Name == name);
    }
}
