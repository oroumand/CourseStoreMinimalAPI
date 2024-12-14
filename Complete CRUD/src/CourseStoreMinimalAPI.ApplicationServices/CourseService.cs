using CourseStoreMinimalAPI.DAL;
using CourseStoreMinimalAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseStoreMinimalAPI.ApplicationServices;
public class CourseService(CourseDbContext ctx)
{
    #region Queries
    public async Task<List<Course>> GetAllAsync(int pageNumber, int itemPerPage)
    {
        int SkipCount = (pageNumber - 1) * itemPerPage;
        return await ctx.Courses.OrderBy(c => c.Title).Skip(SkipCount).Take(itemPerPage).AsNoTracking().ToListAsync();
    }
    public async Task<int> GetTotalCountAsync()
    {
        return await ctx.Courses.CountAsync();
    }
    public async Task<Course?> GetCourseAsync(int id)
    {
        return await ctx.Courses.FirstOrDefaultAsync(c => c.Id == id);
    }
    public async Task<List<Course>> SearchAsync(string title, bool? isOnline)
    {
        var courseQuery = ctx.Courses.AsQueryable();

        if (!string.IsNullOrEmpty(title))
        {
            courseQuery = courseQuery.Where(c => c.Title.Contains(title));
        }
        if (isOnline.HasValue)
        {
            courseQuery = courseQuery.Where(c => c.IsOnline == isOnline.Value);
        }


        courseQuery = courseQuery.OrderBy(c => c.Title).ThenBy(c => c.StartDate);

        return await courseQuery.AsNoTracking().ToListAsync();
    }
    public async Task<bool> Exists(int id)
    {
        return await ctx.Courses.AnyAsync(c => c.Id == id);
    }

    #endregion

    #region Commands
    public async Task<int> Insert(Course course)
    {
        await ctx.Courses.AddAsync(course);
        await ctx.SaveChangesAsync();
        return course.Id;
    }

    public async Task Update()
    {
        await ctx.SaveChangesAsync();
    }

    public async Task Delete(Course courseForDelete)
    {

        ctx.Courses.Remove(courseForDelete);
        await ctx.SaveChangesAsync();
    }
    #endregion

}