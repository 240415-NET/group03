using LibrarySystem.API.Models;

namespace LibrarySystem.API.Data;

public interface IUserDataAccess
{
  public Task<User?> CreateNewUserInDBAsync(User userFromServiceClass);
  //public Task<string?> DeleteUserInDBAsync(string userFromServiceClass);
  //public Task<string> UpdateUserInDBAsync(UsernameUpdateDTO usernamesToSwapFromUserService);
  public Task<User?> GetUserFromDBByUsernameAsync(string usernameToFindFromUserService);
  public Task<bool> DoesThisUserExistOnDBAsync(string usernameToFindFromUserService);

}