using CODL.Data;
using CODL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CODL.Repositories;

public interface ICodewarsUserRepository
{
    public Task AddCodewarsUser(CodewarsUser CodewarsUser);
    public Task<CodewarsUser?> GetCodewarsUserByUsername(string username);
    public Task<CodewarsUser?> GetCodewarsUserById(string id);
    public Task DeleteCodewarsUserByUsername(string username);
    public Task DeleteCodewarsUserByUsernameIn(List<string> usernames);
    public Task<List<string>> GetAllUsernames();
}

public class CodewarsUserRepository : ICodewarsUserRepository
{
    private readonly DataContext _context;

    public CodewarsUserRepository(DataContext context)
    {
        _context = context;
    }

    public async Task AddCodewarsUser(CodewarsUser codewarsUser)
    {
        await _context.CodewarsUsers.AddAsync(codewarsUser);
        await _context.SaveChangesAsync();
    }

    public async Task<CodewarsUser?> GetCodewarsUserByUsername(string username)
    {
        return await _context.CodewarsUsers
            .FirstOrDefaultAsync(u => string.Equals(u.Username, username));
    }

    public async Task<CodewarsUser?> GetCodewarsUserById(string id)
    {
        return await _context.CodewarsUsers
            .FirstOrDefaultAsync(u => string.Equals(u.Username, id));
    }

    public async Task DeleteCodewarsUserByUsername(string username)
    {
        CodewarsUser? savedCodewarsUser = await _context.CodewarsUsers
            .FirstOrDefaultAsync(u => u.Username == username);
        
        _context.CodewarsUsers.Remove(savedCodewarsUser);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCodewarsUserByUsernameIn(List<string> usernames)
    {
        ISet<string> usernameSet = usernames.ToHashSet();
        List<CodewarsUser> toDelete = _context.CodewarsUsers.Where(cu => usernameSet.Contains(cu.Username)).ToList();
        _context.CodewarsUsers.RemoveRange(toDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<List<string>> GetAllUsernames()
    {
        return _context.CodewarsUsers.Select(cu => cu.Username).ToList();
    }
}