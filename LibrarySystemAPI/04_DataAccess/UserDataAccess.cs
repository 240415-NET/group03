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

    public async Task<string?> DeleteUserInDBAsync(string userFromServiceClass)
    {
        //in swagger we enter a name
        //with the name, the user object is retrieved
        var user = _context.Users.FirstOrDefault(u => u.userName==userFromServiceClass);

        //the user object is removed
        _context.Users.Remove(user); //_context establishing the connection iwth database , Insert into 
        await _context.SaveChangesAsync();
        return userFromServiceClass;
    }
     public async Task<string> UpdateUserInDBAsync(UsernameUpdateDTO usernamesToSwapFromUserService)
    {
        //We create a nullable user object to hold our database return
        //We will query the database for a user who corresponds to the UsernameUpdateDTO's oldUsername string
        User? userToUpdate = await _context.Users
            .SingleOrDefaultAsync(user => user.userName == usernamesToSwapFromUserService.oldUserName);

        userToUpdate.userName = usernamesToSwapFromUserService.newUserName;
        
        await _context.SaveChangesAsync();

        return usernamesToSwapFromUserService.newUserName;

    }
}