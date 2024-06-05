using LibrarySystem.Model;
using LibrarySystem.DataAccess;
namespace LibrarySystem.Controller;


public class UserController
{

    public static User currentLoggedUser = new User();

    public static void CreateUser(string userName)
    {
        User newUser = new User(userName);  
        UserStorage.StoreUser(newUser);
        Console.WriteLine($"User Profile {newUser.userName} created sucessfully!");
      
        SetCurrentUser(newUser);
    }

    
    public static bool UserExists(string userName)
    {
        if(UserStorage.FindUser(userName) != null)
        {
            return true;
        }
        return false;
    }

  
    public static User ReturnUser (string userName)
    {
        User existingUser = UserStorage.FindUser(userName);
        SetCurrentUser(existingUser);
        return existingUser;
    }

    public static void SetCurrentUser(User currentUser)
    {
         currentLoggedUser = currentUser;
    }

    public static User GetCurrentUser()
    {
        return currentLoggedUser;
    }

    public static string GetCurrentUserName()
    {
        return currentLoggedUser.userName;
    }

     public static Guid GetCurrentGuid()
    {
        return currentLoggedUser.userId;
    }

    

    

}
