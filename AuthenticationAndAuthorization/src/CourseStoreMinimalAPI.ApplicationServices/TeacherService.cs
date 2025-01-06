using CourseStoreMinimalAPI.DAL;
using CourseStoreMinimalAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace CourseStoreMinimalAPI.ApplicationServices;
public class TeacherService(CourseDbContext ctx)
{
    #region Queries
    public async Task<List<Teacher>> GetAllAsync(int pageNumber, int itemPerPage)
    {
        int SkipCount = (pageNumber - 1) * itemPerPage;
        return await ctx.Teachers.OrderBy(c => c.LastName).Skip(SkipCount).Take(itemPerPage).AsNoTracking().ToListAsync();
    }
    public async Task<int> GetTotalCountAsync()
    {
        return await ctx.Teachers.CountAsync();
    }
    public async Task<Teacher?> GetTeacherAsync(int id)
    {
        return await ctx.Teachers.FirstOrDefaultAsync(c => c.Id == id);
    }
    public async Task<List<Teacher>> SearchAsync(string firatName = "", string lastName = "")
    {
        var teacherQuery = ctx.Teachers.AsQueryable();

        if (!string.IsNullOrEmpty(firatName))
        {
            teacherQuery = teacherQuery.Where(c => c.FirstName.Contains(firatName));
        }

        if (!string.IsNullOrEmpty(lastName))
        {
            teacherQuery = teacherQuery.Where(c => c.LastName.Contains(lastName));
        }
        teacherQuery = teacherQuery.OrderBy(c => c.LastName).ThenBy(c => c.FirstName);

        return await teacherQuery.AsNoTracking().ToListAsync();
    }
    public async Task<bool> Exists(int id)
    {
        return await ctx.Teachers.AnyAsync(c => c.Id == id);
    }

    #endregion

    #region Commands
    public async Task<int> Insert(Teacher teacher)
    {
        await ctx.Teachers.AddAsync(teacher);
        await ctx.SaveChangesAsync();
        return teacher.Id;
    }

    public async Task Update()
    {
        //ctx.Teachers.Update(teacher);
        await ctx.SaveChangesAsync();
    }

    public async Task Delete(Teacher teacherForDelete)
    {

        ctx.Teachers.Remove(teacherForDelete);
        await ctx.SaveChangesAsync();
    }
    #endregion

}
