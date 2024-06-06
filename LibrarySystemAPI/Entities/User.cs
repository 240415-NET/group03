namespace LibrarySystemAPIAPI.Model;

public class User
{
   
    public Guid userId {get; set;}
    public string userName {get; set;}

    public User() {}

    //This constructor takes two arguments
    public User(string _userName){
        userId = Guid.NewGuid();
        userName = _userName;
    }


}
