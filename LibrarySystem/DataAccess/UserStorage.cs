using LibrarySystem.Model;
using System.Text.Json;
namespace LibrarySystem.DataAccess;

public class UserStorage 
{
 
    public static string filePath = "UsersFile.json";
    

    public static void StoreUser(User user)
    {   
       
        if(File.Exists(filePath))
        {
            
            string existingUsersJson = File.ReadAllText(filePath);

            //Once you get the string from the file, THEN you can deserialize it.
            List<User> existingUsersList = JsonSerializer.Deserialize<List<User>>(existingUsersJson);
            

            //Once we deserialize our exisitng JSON text from the file into a new List<User> object
            //We will then simply add it to the list, using the Add() method
            existingUsersList.Add(user);

            //Here we will serialize our list of users, into a JSON text string
            string jsonExistingUsersListString = JsonSerializer.Serialize(existingUsersList);

            //Now we will store our jsonUsersString to our file
            File.WriteAllText(filePath, jsonExistingUsersListString);

        }
        else if (!File.Exists(filePath)) //The first time the program runs, the file probably doesn't exist
        {
            //Creating a blank list to use later
            
            List<User> initialUsersList = new List<User>();

            //Adding our user to our list, PRIOR to serializing it
            initialUsersList.Add(user);

            //Here we will serialize our list of users, into a JSON text string
            string jsonUsersListString = JsonSerializer.Serialize(initialUsersList);

            //Now we will store our jsonUsersString to our file
            File.WriteAllText(filePath, jsonUsersListString);
        }

    }


    public static User FindUser(string usernameToFind)
    {
      
        User foundUser = new User();
        try{

            //First, read the string back from our .json file
            string existingUsersJson = File.ReadAllText(filePath);

            //Then, we need to serialize the string back into a List of User objects
            List<User> existingUsersList = JsonSerializer.Deserialize<List<User>>(existingUsersJson);

            foundUser = existingUsersList.FirstOrDefault(user => user.userName == usernameToFind);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
      
        return foundUser;

    }

}
