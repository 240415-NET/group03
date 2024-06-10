using Microsoft.EntityFrameworkCore;
using LibrarySystem.API.Models;

namespace LibrarySystem.API.Data;

public class UserDataAccess : IUserDataAccess
{
 private readonly LibrarySystemContext _context;

  public UserDataAccess(LibrarySystemContext contextFromBuilder)
    {
        _context = contextFromBuilder;
    }

    public async Task<User?> CreateNewUserInDBAsync(User userFromServiceClass)
    {
        _context.Users.Add(userFromServiceClass); //_context establishing the connection iwth database , Insert into 
        await _context.SaveChangesAsync();
        return userFromServiceClass;
    }
}