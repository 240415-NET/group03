
using LibrarySystem.API.Models;
using LibrarySystem.API.Data;

namespace LibrarySystem.API.Services;

public interface IUserService
{
 public Task<User> CreateNewUserAsync(User userFromControllerClass);
 //public Task<string> UpdateUsernameAsync(UsernameUpdateDTO usernamesToSwapFromController);

 //public  Task<string> DeleteUserAsync(string userFromControllerClass);
 public Task<User> GetUserByUsernameAsync(string usernameToFindFromController);

 public Task<bool> UserExistsAsync(string usernameToFindFromController);

}
