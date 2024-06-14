
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.API.Models;

public class checkoutDTO
{
    public Guid checkoutId {get; set;} = Guid.Empty;
    
    public string status {get; set;}
    public string dueDate {get; set;}

    public Guid userId {get; set;}
    public int bookBarcode {get; set;}

    public checkoutDTO(){}

    public checkoutDTO(Checkout _checkout)
    {
        checkoutId = _checkout.checkoutId;
        status = _checkout.status;
        dueDate = _checkout.dueDate.ToString();
        userId = _checkout.checkoutUser.userId;
        bookBarcode = _checkout.checkoutBook.barcode;
    }
    
}