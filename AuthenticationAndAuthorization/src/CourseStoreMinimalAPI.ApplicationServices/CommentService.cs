using CourseStoreMinimalAPI.DAL;
using CourseStoreMinimalAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseStoreMinimalAPI.ApplicationServices;
public class CommentService(CourseDbContext ctx)
{
    #region Queries
    public async Task<List<Comment>> GetAllAsync(int courseId)
    {
        return await ctx.Comments.Where(c=>c.CourseId == courseId).AsNoTracking().ToListAsync();
    }
    public async Task<int> GetTotalCountAsync(int courseId)
    {
        return await ctx.Comments.Where(c => c.CourseId == courseId).CountAsync();
    }
    public async Task<Comment?> GetCommentAsync(int id)
    {
        return await ctx.Comments.FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task<bool> Exists(int id)
    {
        return await ctx.Comments.AnyAsync(c => c.Id == id);
    }

    #endregion

    #region Commands
    public async Task<int> Insert(Comment comment)
    {
        await ctx.Comments.AddAsync(comment);
        await ctx.SaveChangesAsync();
        return comment.Id;
    }

    public async Task Update()
    {
        await ctx.SaveChangesAsync();
    }

    public async Task Delete(Comment commentForDelete)
    {

        ctx.Comments.Remove(commentForDelete);
        await ctx.SaveChangesAsync();
    }
    #endregion
}
