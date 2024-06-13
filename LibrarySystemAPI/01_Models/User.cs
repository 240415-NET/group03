using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.API.Models;

public class User
{
   
    [Key]
    public Guid userId {get; set;}
    public string userName {get; set;}

     //public List<Checkout> userCheckouts {get; set;} = new();

    public User() {}

    //This constructor takes two arguments
    public User(string _userName){
        userId = Guid.NewGuid();
        userName = _userName;
    }


}