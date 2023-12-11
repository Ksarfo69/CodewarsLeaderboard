using System.Security.Cryptography;
using CODL.Exceptions;
using CODL.Models;
using CODL.Repositories;

namespace CODL.Services;

public interface IAppUserService
{
    Task<ServiceResponse<string>> Register(AppUserRegister userReg);
    Task<ServiceResponse<AppUserResponse>> LogIn(AppUserLogin userLogin);
}

public class AppUserService : IAppUserService
{
    private readonly ILogger<AppUserService> _logger;
    private readonly IAppUserRepository _appUserRepository;

    public AppUserService(ILogger<AppUserService> logger, IAppUserRepository appUserRepository)
    {
        _logger = logger;
        _appUserRepository = appUserRepository;
    }
    
    public async Task<ServiceResponse<string>> Register(AppUserRegister userReg)
    {
        if (await UserExists(userReg.Username))
        {
            _logger.LogError($"User with username {userReg.Username} already exists.");
            throw new ResourceAlreadyExistsException($"User with username {userReg.Username} already exists.");
        }

        if (!string.Equals(userReg.Password, userReg.ConfirmPassword))
        {
            _logger.LogError($"User provided passwords do not match.");
            throw new BadRequestException("Passwords do not match.");
        }

        byte[] passwordHash, passwordSalt;
        _logger.LogInformation("Creating password hash and salt.");
        CreatePasswordHash(userReg.Password, out passwordHash, out passwordSalt);
        _logger.LogInformation("Password hash and salt created successfully.");

        AppUser newAppUser = new AppUser
        {
            Username = userReg.Username,
            Email = userReg.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        await _appUserRepository.AddAppUser(newAppUser);
        
        _logger.LogInformation($"User with username: {userReg.Username} registered successfully.");
        
        return new ServiceResponse<string>
        {
            Success = true,
            Message = "User registered successfully.",
            Data = newAppUser.Id.ToString()
        };
    }

    public async Task<ServiceResponse<AppUserResponse>> LogIn(AppUserLogin userLogin)
    {
        AppUser? exists = await _appUserRepository.GetAppUserByUsername(userLogin.Username);

        if (exists == default)
        {
            _logger.LogError($"User with username {userLogin.Username} not found.");
            throw new InvalidCredentialsException("Invalid username or password.");
        }

        if (!VerifyPasswordHash(userLogin.Password, exists.PasswordHash, exists.PasswordSalt))
        {
            _logger.LogError($"Provided password does not match that for user: {userLogin.Username}.");
            throw new InvalidCredentialsException("Invalid username or password.");
        }
        
        _logger.LogInformation($"User with username: {userLogin.Username} logged in successfully.");
        
        return new ServiceResponse<AppUserResponse>
        {
            Success = true,
            Message = "User logged in successfully.",
            Data = new AppUserResponse
            {
                Id = exists.Id.ToString(),
                Email = exists.Email,
                Username = exists.Username
            }
        };
    }
    
    private async Task<bool> UserExists(string username)
    {
        AppUser? exists = await _appUserRepository.GetAppUserByUsername(username);
        return exists != default;
    }
        
    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (HMACSHA512 hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
    
    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (HMACSHA512 hmac = new HMACSHA512(passwordSalt))
        {
            byte[] computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != passwordHash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}