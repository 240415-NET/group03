using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.API.Models;

public class Checkin
{
    [Key]
    public Guid checkoutId {get; set;}
    
    public string status {get; set;}
    
    public DateOnly dueDate {get; set;}

    public int checkoutBookbarcode {get; set;}
    
    public Guid userId {get; set;}

    public Checkin() {}

    public Checkin(Guid _checkoutId, string _status, DateOnly _dueDate, int _checkoutBookbarcode, Guid _userId  )
    {
        checkoutId = _checkoutId;
        status = _status;
        dueDate = _dueDate;
        checkoutBookbarcode = _checkoutBookbarcode;
        userId= _userId;
    }


}