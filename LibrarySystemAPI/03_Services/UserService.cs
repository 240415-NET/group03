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
/*
    public async Task<string> UpdateUsernameAsync(UsernameUpdateDTO usernamesToSwapFromController)
    {
        return await _userDataAccess.UpdateUserInDBAsync(usernamesToSwapFromController);
    }

     public async Task<string> DeleteUserAsync(string userFromControllerClass)
    {
       await _userDataAccess.DeleteUserInDBAsync(userFromControllerClass);
       return userFromControllerClass; //do not need ok, this is a Task User 
       //Task action result this when we need the Ok();
    }
   */ 
    public async Task<User> GetUserByUsernameAsync(string usernameToFindFromController)
    {
        if(String.IsNullOrEmpty(usernameToFindFromController))
        {
            throw new Exception("Cannot pass in a null or empty string!");
        }
        
        try
        {   

            
            User? foundUser =  await _userDataAccess.GetUserFromDBByUsernameAsync(usernameToFindFromController);


           
            if(foundUser == null)
            {
                throw new Exception("User not found in DB?");
            }

            return foundUser;


        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }

    }
    public async Task<bool> UserExistsAsync(string usernameToFindFromController)
    {
        return await _userDataAccess.DoesThisUserExistOnDBAsync(usernameToFindFromController);
    } 

    
}
