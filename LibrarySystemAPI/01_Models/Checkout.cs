using System.ComponentModel.DataAnnotations;



namespace LibrarySystem.API.Models;

public class Checkout
{
    [Key]
    public Guid checkoutId {get; set;}
    
    public string status {get; set;}

    public DateOnly dueDate {get; set;}

    public User checkoutUser {get; set;} = new();
    public Book checkoutBook {get; set;} = new();
    

    public Checkout() {}

    public Checkout(User _user, Book _book)
    {
        checkoutId = Guid.NewGuid();
        status = "OUT";

        dueDate = DateOnly.FromDateTime(DateTime.Now).AddDays(14);

        checkoutUser = _user;
        checkoutBook = _book;
    }

    //Mapping Constructor
    public Checkout(checkoutDTO bookCheckout, User patron, Book tome)
    {
        checkoutUser = patron;
        checkoutBook = tome;
        checkoutId = Guid.NewGuid();
        status = bookCheckout.status;
        dueDate = DateOnly.Parse(bookCheckout.dueDate);
    }

}