using CODL.Data;
using CODL.Models;
using Microsoft.EntityFrameworkCore;

namespace CODL.Repositories;

public interface ICommentRepository
{
    public Task AddComment(Comment comment);
    public Task<List<Comment>> GetAllByCodewarsUserId(string id);
}

public class CommentRepository : ICommentRepository
{
    private readonly DataContext _context;

    public CommentRepository(DataContext context)
    {
        _context = context;
    }

    public async Task AddComment(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Comment>> GetAllByCodewarsUserId(string id)
    {
        return _context.Comments
            .Where(c => string.Equals(c.Commentee.Id.ToString(), id))
            .Include(c => c.Commenter)
            .ToList();
    }
}