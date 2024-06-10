using LibrarySystem.API.Models;
using LibrarySystem.API.Data;

namespace LibrarySystem.API.Services;

public class UserService : IUserService
{
 private readonly IUserDataAccess _userDataAccess;

  public UserService(IUserDataAccess userStorageFromBuilder)
    {
        _userDataAccess = userStorageFromBuilder;
    }

     public async Task<User> CreateNewUserAsync(User userFromControllerClass)
    {
       await _userDataAccess.CreateNewUserInDBAsync(userFromControllerClass);
       return userFromControllerClass; //do not need ok, this is a Task User 
       //Task action result this when we need the Ok();
    }

     public async Task<string> DeleteUserAsync(string userFromControllerClass)
    {
       await _userDataAccess.DeleteUserInDBAsync(userFromControllerClass);
       return userFromControllerClass; //do not need ok, this is a Task User 
       //Task action result this when we need the Ok();
    }
}
