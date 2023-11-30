using CODL.Data;
using CODL.Models;
using Microsoft.EntityFrameworkCore;

namespace CODL.Repositories;

public interface IAppUserRepository
{
    public Task AddAppUser(AppUser appUser);
    public Task<AppUser?> GetAppUserByUsername(string username);
    public Task<AppUser?> GetAppUserById(string id);
}

public class AppUserRepository : IAppUserRepository
{
    private readonly DataContext _context;

    public AppUserRepository(DataContext context)
    {
        _context = context;
    }

    public async Task AddAppUser(AppUser appUser)
    {
        await _context.AppUsers.AddAsync(appUser);
        await _context.SaveChangesAsync();
    }

    public async Task<AppUser?> GetAppUserByUsername(string username)
    {
        return await _context.AppUsers.FirstOrDefaultAsync(u => string.Equals(u.Username, username));
    }
    
    public async Task<AppUser?> GetAppUserById(string id)
    {
        return await _context.AppUsers.FirstOrDefaultAsync(u => string.Equals(u.Id.ToString(), id));
    }
}